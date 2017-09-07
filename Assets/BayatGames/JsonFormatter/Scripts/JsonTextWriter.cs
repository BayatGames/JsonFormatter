using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json text writer.
	/// </summary>
	public class JsonTextWriter : JsonWriter
	{
		
		#region Fields

		/// <summary>
		/// The writer.
		/// </summary>
		protected TextWriter m_Writer;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the writer.
		/// </summary>
		/// <value>The writer.</value>
		public virtual TextWriter writer
		{
			get
			{
				return m_Writer;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public JsonTextWriter ( TextWriter writer ) : this ( writer, null, new StreamingContext ( StreamingContextStates.All ) )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonTextWriter ( TextWriter writer, ISurrogateSelector selector, StreamingContext context ) : base ( selector, context )
		{
			m_Writer = writer;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Write the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public override void Write ( object value )
		{
			if ( value == null )
			{
				m_Writer.Write ( "null" );
			}
			else
			{
				Type type = value.GetType ();
				if ( type == typeof ( string ) || type.IsEnum )
				{
					m_Writer.Write ( "\"{0}\"", value );
				}
				else if ( type == typeof ( short ) || type == typeof ( int ) || type == typeof ( long ) ||
				          type == typeof ( ushort ) || type == typeof ( uint ) || type == typeof ( ulong ) ||
				          type == typeof ( byte ) || type == typeof ( sbyte ) || type == typeof ( decimal ) ||
				          type == typeof ( double ) || type == typeof ( float ) )
				{
					m_Writer.Write ( Convert.ChangeType ( value, typeof ( string ) ) );
				}
				else if ( type.IsSerializable && !( value is ISerializable ) && !type.IsArray )
				{
					WriteObject ( value, type );
				}
				else if ( value is ISerializable )
				{
					SerializationInfo info = new SerializationInfo ( type, new FormatterConverter () );
					ISerializable serializable = value as ISerializable;
					serializable.GetObjectData ( info, m_Context );
					WriteSerializationInfo ( info );
				}
				else if ( type.IsGenericType && type.GetGenericTypeDefinition () == typeof ( KeyValuePair<,> ) )
				{
					m_Writer.Write ( "{" );
					Write ( "Key" );
					m_Writer.Write ( ":" );
					Write ( type.GetProperty ( "Key" ).GetValue ( value, BindingFlags.Default, null, null, null ) );
					m_Writer.Write ( "," );
					Write ( "Value" );
					m_Writer.Write ( ":" );
					Write ( type.GetProperty ( "Value" ).GetValue ( value, BindingFlags.Default, null, null, null ) );
					m_Writer.Write ( "}" );
				}
				else if ( value is IDictionary )
				{
					IDictionary dictionary = value as IDictionary;
					bool isFirst = true;
					m_Writer.Write ( "{" );
					foreach ( var key in dictionary.Keys )
					{
						if ( isFirst )
						{
							isFirst = false;
						}
						else
						{
							m_Writer.Write ( "," );
						}
						Write ( key );
						m_Writer.Write ( ":" );
						Write ( dictionary [ key ] );
					}
					m_Writer.Write ( "}" );
				}
				else if ( value is IEnumerable )
				{
					IEnumerable enumerable = value as IEnumerable;
					IEnumerator e = enumerable.GetEnumerator ();
					bool isFirst = true;
					m_Writer.Write ( "[" );
					while ( e.MoveNext () )
					{
						if ( isFirst )
						{
							isFirst = false;
						}
						else
						{
							m_Writer.Write ( "," );
						}
						Write ( e.Current );
					}
					m_Writer.Write ( "]" );
				}
				else
				{
					if ( m_SurrogateSelector != null )
					{
						ISurrogateSelector selector;
						ISerializationSurrogate surrogate = m_SurrogateSelector.GetSurrogate ( type, m_Context, out selector );
						if ( surrogate != null )
						{
							SerializationInfo info = new SerializationInfo ( type, new FormatterConverter () );
							surrogate.GetObjectData ( value, info, m_Context );
							WriteSerializationInfo ( info );
						}
					}
					else
					{
						WriteObject ( value, type );
					}
				}
			}
		}

		/// <summary>
		/// Writes the object.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="type">Type.</param>
		protected virtual void WriteObject ( object value, Type type )
		{
			FieldInfo [] fields = type.GetFields ();
			PropertyInfo [] properties = type.GetProperties ();
			bool isFirst = true;
			m_Writer.Write ( "{" );
			for ( int i = 0; i < fields.Length; i++ )
			{
				if ( fields [ i ].IsPublic && !fields [ i ].IsStatic && !fields [ i ].IsLiteral && !fields [ i ].IsNotSerialized )
				{
					if ( isFirst )
					{
						isFirst = false;
					}
					else
					{
						m_Writer.Write ( "," );
					}
					Write ( fields [ i ].Name );
					m_Writer.Write ( ":" );
					Write ( fields [ i ].GetValue ( value ) );
				}
			}
			for ( int i = 0; i < properties.Length; i++ )
			{
				if ( properties [ i ].CanRead && properties [ i ].CanWrite )
				{
					if ( isFirst )
					{
						isFirst = false;
					}
					else
					{
						m_Writer.Write ( "," );
					}
					Write ( properties [ i ].Name );
					m_Writer.Write ( ":" );
					Write ( properties [ i ].GetValue ( value, BindingFlags.Default, null, null, null ) );
				}
			}
			m_Writer.Write ( "}" );
		}

		/// <summary>
		/// Writes the serialization info.
		/// </summary>
		/// <param name="info">Info.</param>
		protected virtual void WriteSerializationInfo ( SerializationInfo info )
		{
			var e = info.GetEnumerator ();
			bool isFirst = true;
			m_Writer.Write ( "{" );
			while ( e.MoveNext () )
			{
				if ( isFirst )
				{
					isFirst = false;
				}
				else
				{
					m_Writer.Write ( "," );
				}
				Write ( e.Name );
				m_Writer.Write ( ":" );
				Write ( e.Value );
			}
			m_Writer.Write ( "}" );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> so the garbage collector can reclaim the
		/// memory that the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> was occupying.</remarks>
		public override void Dispose ()
		{
			if ( m_Writer != null )
			{
				m_Writer.Dispose ();
			}
		}

		#endregion

	}

}