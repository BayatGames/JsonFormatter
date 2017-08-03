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
	/// Json writer.
	/// </summary>
	public abstract class JsonWriter : IDisposable
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
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		public JsonWriter () : this ( null, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		public JsonWriter ( ISurrogateSelector selector ) : this ( selector, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public JsonWriter ( StreamingContext context ) : this ( null, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="encoding">Encoding.</param>
		public JsonWriter ( Encoding encoding ) : this ( null, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonWriter ( ISurrogateSelector selector, StreamingContext context ) : this ( selector, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonWriter ( ISurrogateSelector selector, Encoding encoding ) : this ( selector, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonWriter ( StreamingContext context, Encoding encoding ) : this ( null, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonWriter ( ISurrogateSelector selector, StreamingContext context, Encoding encoding )
		{
			m_SurrogateSelector = selector;
			m_Context = context;
			m_Encoding = encoding;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Write the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public virtual void Write ( object value )
		{
			if ( value == null )
			{
				Write ( "null" );
				return;
			}
			Type type = value.GetType ();
			if ( type.IsPrimitive || type == typeof ( string ) || type == typeof ( decimal ) )
			{
				WriteRaw ( string.Format ( "\"{0}\"", Convert.ChangeType ( value, typeof ( string ) ).ToString () ) );
			}
			else if ( type.IsArray )
			{
				WriteRaw ( "[" );
				bool isFirst = true;
				IList list = value as IList;
				for ( int i = 0; i < list.Count; i++ )
				{
					if ( isFirst )
					{
						isFirst = false;
					}
					else
					{
						WriteRaw ( "," );
					}
					Write ( list [ i ] );
				}
				WriteRaw ( "]" );
			}
			else
			{
				WriteRaw ( "{" );
				bool isFirst = true;
				List<FieldInfo> fields = type.GetFields ().Where ( field => !field.IsLiteral && !field.IsNotSerialized && field.IsPublic ).ToList ();
				ISerializationSurrogate surrogate = null;
				if ( m_SurrogateSelector != null )
				{
					ISurrogateSelector selector;
					surrogate = m_SurrogateSelector.GetSurrogate ( type, m_Context, out selector );
				}
				if ( value is ISerializable || ( m_SurrogateSelector != null && surrogate != null ) )
				{
					SerializationInfo info = new SerializationInfo ( type, new FormatterConverter () );
					if ( value is ISerializable )
					{
						ISerializable serializable = value as ISerializable;
						serializable.GetObjectData ( info, m_Context );
					}
					else
					{
						surrogate.GetObjectData ( value, info, m_Context );
					}
					fields.Clear ();
					var e = info.GetEnumerator ();
					while ( e.MoveNext () )
					{
						FieldInfo field = type.GetField ( e.Name );
						if ( field != null )
						{
							fields.Add ( field );
							continue;
						}
					}
				}
				for ( int i = 0; i < fields.Count; i++ )
				{
					if ( isFirst )
					{
						isFirst = false;
					}
					else
					{
						WriteRaw ( "," );
					}
					Write ( fields [ i ].Name );
					WriteRaw ( ":" );
					Write ( fields [ i ].GetValue ( value ) );
				}
				WriteRaw ( "}" );
			}
		}

		/// <summary>
		/// Writes the raw.
		/// </summary>
		/// <param name="value">Value.</param>
		public abstract void WriteRaw ( string value );

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> so the garbage collector can reclaim the memory
		/// that the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> was occupying.</remarks>
		public abstract void Dispose ();

		#endregion

	}

}