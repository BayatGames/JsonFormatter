using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json text writer.
	/// </summary>
	public class JsonTextWriter : JsonWriter
	{

		#region Fields

		protected TextWriter m_Writer;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the writer.
		/// </summary>
		/// <value>The writer.</value>
		public virtual TextWriter Writer
		{
			get
			{
				return m_Writer;
			}
			set
			{
				m_Writer = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public JsonTextWriter ( Stream stream ) : this ( new StreamWriter ( stream, Encoding.UTF8 ) )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		public JsonTextWriter ( Stream stream, ISurrogateSelector selector ) : this ( new StreamWriter (
				stream,
				Encoding.UTF8 ), selector )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="context">Context.</param>
		public JsonTextWriter ( Stream stream, StreamingContext context ) : this ( new StreamWriter ( stream, Encoding.UTF8 ), context )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( Stream stream, Encoding encoding ) : this ( new StreamWriter ( stream, encoding ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonTextWriter ( Stream stream, ISurrogateSelector selector, StreamingContext context ) : this ( new StreamWriter (
				stream,
				Encoding.UTF8 ), selector, context )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( Stream stream, ISurrogateSelector selector, Encoding encoding ) : this ( new StreamWriter (
				stream,
				encoding ), selector, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( Stream stream, StreamingContext context, Encoding encoding ) : this ( new StreamWriter (
				stream,
				encoding ), context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( Stream stream, ISurrogateSelector selector, StreamingContext context, Encoding encoding ) : this ( new StreamWriter (
				stream,
				encoding ), selector, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public JsonTextWriter ( TextWriter writer ) : this ( writer, null, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="selector">Selector.</param>
		public JsonTextWriter ( TextWriter writer, ISurrogateSelector selector ) : this ( writer, selector, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="context">Context.</param>
		public JsonTextWriter ( TextWriter writer, StreamingContext context ) : this ( writer, null, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( TextWriter writer, Encoding encoding ) : this ( writer, null, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonTextWriter ( TextWriter writer, ISurrogateSelector selector, StreamingContext context ) : this ( writer, selector, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( TextWriter writer, ISurrogateSelector selector, Encoding encoding ) : this ( writer, selector, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( TextWriter writer, StreamingContext context, Encoding encoding ) : this ( writer, null, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonTextWriter"/> class.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonTextWriter ( TextWriter writer, ISurrogateSelector selector, StreamingContext context, Encoding encoding ) : base ( selector, context, encoding )
		{
			m_Writer = writer;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Writes the raw.
		/// </summary>
		/// <param name="value">Value.</param>
		public override void WriteRaw ( string value )
		{
			m_Writer.Write ( value );
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
			m_Writer.Dispose ();
		}

		#endregion

	}

}