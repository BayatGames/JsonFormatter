using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json text reader.
	/// </summary>
	public class JsonTextReader : JsonReader
	{

		#region Fields

		protected TextReader m_Reader;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the reader.
		/// </summary>
		/// <value>The reader.</value>
		public virtual TextReader Reader
		{
			get
			{
				return m_Reader;
			}
			set
			{
				m_Reader = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public JsonTextReader ( Stream stream ) : this ( new StreamReader ( stream, Encoding.UTF8 ) )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		public JsonTextReader ( Stream stream, ISurrogateSelector selector ) : this ( new StreamReader (
				stream,
				Encoding.UTF8 ), selector )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="context">Context.</param>
		public JsonTextReader ( Stream stream, StreamingContext context ) : this ( new StreamReader ( stream, Encoding.UTF8 ), context )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( Stream stream, Encoding encoding ) : this ( new StreamReader ( stream, encoding ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonTextReader ( Stream stream, ISurrogateSelector selector, StreamingContext context ) : this ( new StreamReader (
				stream,
				Encoding.UTF8 ), selector, context )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( Stream stream, ISurrogateSelector selector, Encoding encoding ) : this ( new StreamReader (
				stream,
				encoding ), selector, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( Stream stream, StreamingContext context, Encoding encoding ) : this ( new StreamReader (
				stream,
				encoding ), context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( Stream stream, ISurrogateSelector selector, StreamingContext context, Encoding encoding ) : this ( new StreamReader (
				stream,
				encoding ), selector, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public JsonTextReader ( TextReader reader ) : this ( reader, null, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="selector">Selector.</param>
		public JsonTextReader ( TextReader reader, ISurrogateSelector selector ) : this ( reader, selector, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="context">Context.</param>
		public JsonTextReader ( TextReader reader, StreamingContext context ) : this ( reader, null, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( TextReader reader, Encoding encoding ) : this ( reader, null, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonTextReader ( TextReader reader, ISurrogateSelector selector, StreamingContext context ) : this ( reader, selector, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( TextReader reader, ISurrogateSelector selector, Encoding encoding ) : this ( reader, selector, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( TextReader reader, StreamingContext context, Encoding encoding ) : this ( reader, null, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextReader"/> class.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextReader ( TextReader reader, ISurrogateSelector selector, StreamingContext context, Encoding encoding ) : base ( selector, context, encoding )
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
			string json = m_Reader.ReadToEnd ();
			StringBuilder builder = new StringBuilder ();
			for ( int i = 0; i < json.Length; i++ )
			{
				if ( json [ i ] == '\"' )
				{
					int index = json.IndexOf ( '\"', i + 1 );
					builder.Append ( json.Substring ( i, index - i + 1 ) );
					i = index;
					continue;
				}
				if ( char.IsWhiteSpace ( json [ i ] ) )
				{
					continue;
				}
				builder.Append ( json [ i ] );
			}
			return Read ( type, builder.ToString () );
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
			m_Reader.Dispose ();
		}

		#endregion

	}

}