using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json reader.
	/// </summary>
	public abstract class JsonReader : IDisposable
	{

		#region Fields

		protected ISurrogateSelector m_SurrogateSelector;
		protected StreamingContext m_Context;
		protected Encoding m_Encoding;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the surrogate selector.
		/// </summary>
		/// <value>The surrogate selector.</value>
		public virtual ISurrogateSelector SurrogateSelector
		{
			get
			{
				return m_SurrogateSelector;
			}
			set
			{
				m_SurrogateSelector = value;
			}
		}

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		public virtual StreamingContext Context
		{
			get
			{
				return m_Context;
			}
			set
			{
				m_Context = value;
			}
		}

		/// <summary>
		/// Gets or sets the encoding.
		/// </summary>
		/// <value>The encoding.</value>
		public virtual Encoding Encoding
		{
			get
			{
				if ( m_Encoding == null )
				{
					m_Encoding = Encoding.UTF8;
				}
				return m_Encoding;
			}
			set
			{
				m_Encoding = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		public JsonReader () : this ( null, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		public JsonReader ( ISurrogateSelector selector ) : this ( selector, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public JsonReader ( StreamingContext context ) : this ( null, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="encoding">Encoding.</param>
		public JsonReader ( Encoding encoding ) : this ( null, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonReader ( ISurrogateSelector selector, StreamingContext context ) : this ( selector, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonReader ( ISurrogateSelector selector, Encoding encoding ) : this ( selector, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonReader ( StreamingContext context, Encoding encoding ) : this ( null, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonReader ( ISurrogateSelector selector, StreamingContext context, Encoding encoding )
		{
			m_SurrogateSelector = selector;
			m_Context = context;
			m_Encoding = encoding;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Read the specified type and json.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="json">Json.</param>
		public virtual object Read ( Type type, string json )
		{
			if ( json == null || json.Length <= 2 )
			{
				return null;
			}
			if ( type.IsPrimitive || type == typeof ( string ) || type == typeof ( decimal ) || json [ 0 ] == '\"' )
			{
				return Convert.ChangeType ( json.Substring ( 1, json.Length - 2 ), type );
			}
			else if ( type.IsArray || json [ 0 ] == '[' )
			{
				Type elementType = type.GetElementType ();
				List<string> elements = Split ( json );
				Array array = Array.CreateInstance ( elementType, elements.Count );
				for ( int i = 0; i < elements.Count; i++ )
				{
					array.SetValue ( Read ( elementType, elements [ i ] ), i );
				}
				return array;
			}
			else if ( type.IsGenericType && type.GetGenericTypeDefinition () == typeof ( IList ) )
			{
				Type listType = type.GetGenericArguments () [ 0 ];
				List<string> elements = Split ( json );
				IList list = ( IList )type.GetConstructor ( new Type[] { typeof ( int ) } ).Invoke ( new object[] { elements.Count } );
				for ( int i = 0; i < elements.Count; i++ )
				{
					list.Add ( Read ( listType, elements [ i ] ) );
				}
				return list;
			}
			else if ( type.IsGenericType && type.GetGenericTypeDefinition () == typeof ( IDictionary ) )
			{
				Type [] genericArgs = type.GetGenericArguments ();
				Type keyType = genericArgs [ 0 ];
				Type valueType = genericArgs [ 1 ];
				List<string> elements = Split ( json );
				if ( elements.Count % 2 != 0 )
				{
					return null;
				}
				IDictionary dictionary = ( IDictionary )type.GetConstructor ( new Type[] { typeof ( int ) } ).Invoke ( new object[] { elements.Count / 2 } );
				for ( int i = 0; i < elements.Count; i += 2 )
				{
					dictionary.Add ( Read ( keyType, elements [ i ] ), Read ( valueType, elements [ i + 1 ] ) );
				}
				return dictionary;
			}
			else
			{
				ISurrogateSelector selector = null;
				ISerializationSurrogate surrogate = null;
				SerializationInfo info = null;
				if ( m_SurrogateSelector != null )
				{
					surrogate = m_SurrogateSelector.GetSurrogate ( type, m_Context, out selector );
					info = new SerializationInfo ( type, new FormatterConverter () );
				}
				object instance = FormatterServices.GetUninitializedObject ( type );
				Dictionary<string, FieldInfo> fields = type.GetFields ().Where ( field => !field.IsLiteral && !field.IsNotSerialized ).ToDictionary ( field => field.Name );
				List<string> elements = Split ( json );
				if ( elements.Count % 2 != 0 )
				{
					return null;
				}
				for ( int i = 0; i < elements.Count; i += 2 )
				{
					string key = ( string )Read ( typeof ( string ), elements [ i ] );
					FieldInfo field = null;
					fields.TryGetValue ( key, out field );
					if ( field != null )
					{
						if ( info == null )
						{
							field.SetValue ( instance, Read ( field.FieldType, elements [ i + 1 ] ) );
						}
						else
						{
							info.AddValue ( key, Read ( field.FieldType, elements [ i + 1 ] ) );
						}
						continue;
					}
				}
				if ( surrogate != null )
				{
					object result = surrogate.SetObjectData ( instance, info, m_Context, selector );
					if ( result != null )
					{
						instance = result;
					}
				}
				return instance;
			}
		}

		/// <summary>
		/// Read the specified type.
		/// </summary>
		/// <param name="type">Type.</param>
		public abstract object Read ( Type type );

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> so the garbage collector can reclaim the memory
		/// that the <see cref="BayatGames.Serialization.Formatters.Json.JsonReader"/> was occupying.</remarks>
		public abstract void Dispose ();

		/// <summary>
		/// Split the specified json.
		/// </summary>
		/// <param name="json">Json.</param>
		public static List<string> Split ( string json )
		{
			List<string> splitArray = new List<string> ();
			StringBuilder builder = new StringBuilder ();
			int parseDepth = 0;
			splitArray.Clear ();
			for ( int i = 1; i < json.Length - 1; i++ )
			{
				switch ( json [ i ] )
				{
					case '[':
					case '{':
						parseDepth++;
						break;
					case ']':
					case '}':
						parseDepth--;
						break;
					case '\"':
						int index = json.IndexOf ( '\"', i + 1 );
						builder.Append ( json.Substring ( i, index - i + 1 ) );
						i = index;
						continue;
					case ',':
					case ':':
						if ( parseDepth == 0 )
						{
							splitArray.Add ( builder.ToString () );
							builder.Length = 0;
							continue;
						}
						break;
				}
				builder.Append ( json [ i ] );
			}
			splitArray.Add ( builder.ToString () );
			return splitArray;
		}

		#endregion

	}

}