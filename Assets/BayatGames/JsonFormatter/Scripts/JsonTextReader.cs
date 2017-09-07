using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json text reader.
	/// </summary>
	public class JsonTextReader : JsonReader
	{
		
		#region Fields

		/// <summary>
		/// The reader.
		/// </summary>
		protected TextReader m_Reader;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the reader.
		/// </summary>
		/// <value>The reader.</value>
		public virtual TextReader reader
		{
			get
			{
				return m_Reader;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public JsonTextReader ( TextReader reader ) : this ( reader, null, new StreamingContext ( StreamingContextStates.All ) )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonTextReader ( TextReader reader, ISurrogateSelector selector, StreamingContext context ) : base ( selector, context )
		{
			m_Reader = reader;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Read the specified type.
		/// </summary>
		/// <param name="type">Type.</param>
		public override object Read ( Type type )
		{
			return Read ( type, m_Reader.ReadToEnd ().RemoveWhitespaceJson () );
		}

		/// <summary>
		/// Read the specified json.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected virtual T Read<T> ( string json )
		{
			return ( T )Read ( typeof ( T ), json );
		}

		/// <summary>
		/// Read the specified type and json.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="json">Json.</param>
		protected virtual object Read ( Type type, string json )
		{
			object result = null;
			if ( type == null || string.IsNullOrEmpty ( json ) )
			{
				result = null;
			}
			else
			{
				if ( type == typeof ( string ) || type.IsEnum )
				{
					result = json.Substring ( 1, json.Length - 2 );
				}
				else if ( type == typeof ( short ) || type == typeof ( int ) || type == typeof ( long ) ||
				          type == typeof ( ushort ) || type == typeof ( uint ) || type == typeof ( ulong ) ||
				          type == typeof ( byte ) || type == typeof ( sbyte ) || type == typeof ( decimal ) ||
				          type == typeof ( double ) || type == typeof ( float ) )
				{
					result = Convert.ChangeType ( json, type );
				}
				else if ( type.IsArray )
				{
					Type elementType = type.GetElementType ();
					string [] items = json.SplitJson ();
					Array array = Array.CreateInstance ( elementType, items.Length );
					for ( int i = 0; i < items.Length; i++ )
					{
						array.SetValue ( Read ( elementType, items [ i ] ), i );
					}
					result = array;
				}
				else if ( type.IsGenericType && type.GetGenericTypeDefinition () == typeof ( KeyValuePair<,> ) )
				{
					Type [] genericArgs = type.GetGenericArguments ();
					string [] items = json.SplitJson ();
					object key = null;
					object value = null;
					if ( items.Length == 1 )
					{
						key = Read ( genericArgs [ 0 ], items [ 0 ] );
						value = Read ( genericArgs [ 1 ], items [ 1 ] );
					}
					else
					{
						for ( int i = 0; i < items.Length; i += 2 )
						{
							if ( items [ i ] == "Key" )
							{
								key = Read ( genericArgs [ 0 ], items [ i + 1 ] );
							}
							else if ( items [ i ] == "Value" )
							{
								value = Read ( genericArgs [ 1 ], items [ i + 1 ] );
							}
						}
					}
					result = type.GetConstructor ( genericArgs ).Invoke ( new object [] { key, value } );
				}
				else if ( type.IsGenericType && type.GetGenericTypeDefinition () == typeof ( List<> ) )
				{
					Type [] genericArgs = type.GetGenericArguments ();
					string [] items = json.SplitJson ();
					IList list = ( IList )type.GetConstructor ( Type.EmptyTypes ).Invoke ( null );
					for ( int i = 0; i < items.Length; i++ )
					{
						list.Add ( Read ( genericArgs [ 0 ], items [ i ] ) );
					}
					result = list;
				}
				else if ( type.IsGenericType && type.GetGenericTypeDefinition () == typeof ( Dictionary<,> ) )
				{
					Type [] genericArgs = type.GetGenericArguments ();
					string [] items = json.SplitJson ();
					if ( items.Length == 8 && Read<string> ( items [ 6 ] ) == "KeyValuePairs" )
					{
						items = items [ 7 ].SplitJson () [ 0 ].SplitJson ();
						List<string> newItems = new List<string> ();
						for ( int i = 0; i < items.Length; i += 4 )
						{
							newItems.Add ( items [ i + 1 ] );
							newItems.Add ( items [ i + 3 ] );
						}
						items = newItems.ToArray ();
					}
					IDictionary dictionary = ( IDictionary )type.GetConstructor ( Type.EmptyTypes ).Invoke ( null );
					for ( int i = 0; i < items.Length; i += 2 )
					{
						dictionary.Add ( Read ( genericArgs [ 0 ], items [ i ] ), Read ( genericArgs [ 1 ], items [ i + 1 ] ) );
					}
					result = dictionary;
				}
				else
				{
					result = ReadObject ( type, json );
				}
			}
			if ( result is IDeserializationCallback )
			{
				( result as IDeserializationCallback ).OnDeserialization ( this );
			}
			return result;
		}

		/// <summary>
		/// Reads the object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="type">Type.</param>
		/// <param name="json">Json.</param>
		protected virtual object ReadObject ( Type type, string json )
		{
			object result = null;
			if ( type.IsValueType )
			{
				result = Activator.CreateInstance ( type );
			}
			else
			{
				result = FormatterServices.GetUninitializedObject ( type );
			}
			ISurrogateSelector selector = null;
			SerializationInfo info = null;
			ISerializationSurrogate surrogate = null;
			if ( m_SurrogateSelector != null )
			{
				surrogate = m_SurrogateSelector.GetSurrogate ( type, m_Context, out selector );
				if ( surrogate != null )
				{
					info = new SerializationInfo ( type, new FormatterConverter () );
				}
			}
			if ( result != null )
			{
				string [] items = json.SplitJson ();
				for ( int i = 0; i < items.Length; i += 2 )
				{
					string name = Read<string> ( items [ i ] );
					FieldInfo field = type.GetField ( name );
					if ( field != null )
					{
						if ( info != null )
						{
							info.AddValue ( name, Read ( field.FieldType, items [ i + 1 ] ) );
						}
						else
						{
							field.SetValue ( result, Read ( field.FieldType, items [ i + 1 ] ) );
						}
						continue;
					}
					PropertyInfo property = type.GetProperty ( name );
					if ( property != null )
					{
						if ( info != null )
						{
							info.AddValue ( name, Read ( property.PropertyType, items [ i + 1 ] ) );
						}
						else
						{
							property.SetValue (
								result,
								Read ( property.PropertyType, items [ i + 1 ] ),
								BindingFlags.Default,
								null,
								null,
								null );
						}
						continue;
					}
				}
				if ( surrogate != null )
				{
					surrogate.SetObjectData ( result, info, m_Context, selector );
				}
			}
			return result;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> so the garbage collector can reclaim the
		/// memory that the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> was occupying.</remarks>
		public override void Dispose ()
		{
			if ( m_Reader != null )
			{
				m_Reader.Dispose ();
			}
		}

		#endregion
		
	}

}