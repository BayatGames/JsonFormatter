using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json formatter.
	/// </summary>
	public class JsonFormatter
	{

		#region Fields

		protected ISurrogateSelector m_SurrogateSelector;
		protected SerializationBinder m_Binder;
		protected StreamingContext m_Context;
		protected Encoding m_Encoding;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the surrogate selector.
		/// </summary>
		/// <value>The surrogate selector.</value>
		public ISurrogateSelector SurrogateSelector
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
		/// Gets or sets the binder.
		/// </summary>
		/// <value>The binder.</value>
		public SerializationBinder Binder
		{
			get
			{
				return m_Binder;
			}
			set
			{
				m_Binder = value;
			}
		}

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		/// <value>The context.</value>
		public StreamingContext Context
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
		public Encoding Encoding
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
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		public JsonFormatter () : this ( null, null, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		public JsonFormatter ( ISurrogateSelector selector ) : this ( selector, null, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="binder">Binder.</param>
		public JsonFormatter ( SerializationBinder binder ) : this ( null, binder, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public JsonFormatter ( StreamingContext context ) : this ( null, null, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( Encoding encoding ) : this ( null, null, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="binder">Binder.</param>
		public JsonFormatter ( ISurrogateSelector selector, SerializationBinder binder ) : this ( selector, binder, new StreamingContext ( StreamingContextStates.All ), Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonFormatter ( ISurrogateSelector selector, StreamingContext context ) : this ( selector, null, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( ISurrogateSelector selector, Encoding encoding ) : this ( selector, null, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="binder">Binder.</param>
		/// <param name="context">Context.</param>
		public JsonFormatter ( SerializationBinder binder, StreamingContext context ) : this ( null, binder, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="binder">Binder.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( SerializationBinder binder, Encoding encoding ) : this ( null, binder, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( StreamingContext context, Encoding encoding ) : this ( null, null, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="binder">Binder.</param>
		/// <param name="context">Context.</param>
		public JsonFormatter ( ISurrogateSelector selector, SerializationBinder binder, StreamingContext context ) : this ( selector, binder, context, Encoding.UTF8 )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="binder">Binder.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( ISurrogateSelector selector, SerializationBinder binder, Encoding encoding ) : this ( selector, binder, new StreamingContext ( StreamingContextStates.All ), encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="binder">Binder.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( SerializationBinder binder, StreamingContext context, Encoding encoding ) : this ( null, binder, context, encoding )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="binder">Binder.</param>
		/// <param name="context">Context.</param>
		/// <param name="encoding">Encoding.</param>
		public JsonFormatter ( ISurrogateSelector selector, SerializationBinder binder, StreamingContext context, Encoding encoding )
		{
			m_SurrogateSelector = selector;
			m_Binder = binder;
			m_Context = context;
			m_Encoding = encoding;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Deserialize the specified json.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( string json )
		{
			return ( T )Deserialize ( json, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified json and type.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <param name="type">Type.</param>
		public object Deserialize ( string json, Type type )
		{
			StringReader stringReader = new StringReader ( json );
			return Deserialize ( stringReader, type );
		}

		/// <summary>
		/// Deserialize the specified reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( TextReader reader )
		{
			return ( T )Deserialize ( reader, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified reader and type.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="type">Type.</param>
		public object Deserialize ( TextReader reader, Type type )
		{
			JsonReader jsonReader = new JsonTextReader ( reader );
			return jsonReader.Read ( type );
		}

		/// <summary>
		/// Deserialize the specified serializationStream.
		/// </summary>
		/// <param name="serializationStream">Serialization stream.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( Stream serializationStream )
		{
			return ( T )Deserialize ( serializationStream, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified serializationStream and type.
		/// </summary>
		/// <param name="serializationStream">Serialization stream.</param>
		/// <param name="type">Type.</param>
		public object Deserialize ( Stream serializationStream, Type type )
		{
			JsonReader reader = new JsonTextReader ( serializationStream );
			return reader.Read ( type );
		}

		/// <summary>
		/// Serialize the specified graph.
		/// </summary>
		/// <param name="graph">Graph.</param>
		public string Serialize ( object graph )
		{
			StringWriter writer = new StringWriter ();
			JsonWriter jsonWriter = new JsonTextWriter ( writer, SurrogateSelector, Context, Encoding );
			jsonWriter.Write ( graph );
			return writer.ToString ();
		}

		/// <summary>
		/// Serialize the specified writer and graph.
		/// </summary>
		/// <param name="writer">Writer.</param>
		/// <param name="graph">Graph.</param>
		public void Serialize ( TextWriter writer, object graph )
		{
			JsonWriter jsonWriter = new JsonTextWriter ( writer, SurrogateSelector, Context, Encoding );
			jsonWriter.Write ( graph );
		}

		/// <summary>
		/// Serialize the specified serializationStream and graph.
		/// </summary>
		/// <param name="serializationStream">Serialization stream.</param>
		/// <param name="graph">Graph.</param>
		public void Serialize ( Stream serializationStream, object graph )
		{
			JsonWriter writer = new JsonTextWriter ( serializationStream, SurrogateSelector, Context, Encoding );
			writer.Write ( graph );
		}

		/// <summary>
		/// Serialize the specified graph.
		/// </summary>
		/// <param name="graph">Graph.</param>
		public static string SerializeObject ( object graph )
		{
			StringWriter writer = new StringWriter ();
			JsonWriter jsonWriter = new JsonTextWriter ( writer );
			jsonWriter.Write ( graph );
			return writer.ToString ();
		}

		/// <summary>
		/// Deserialize the specified json.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeserializeObject<T> ( string json )
		{
			return ( T )JsonFormatter.DeserializeObject ( json, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified json and type.
		/// </summary>
		/// <param name="json">Json.</param>
		/// <param name="type">Type.</param>
		public static object DeserializeObject ( string json, Type type )
		{
			StringReader reader = new StringReader ( json );
			JsonReader jsonReader = new JsonTextReader ( reader );
			return jsonReader.Read ( type );
		}

		#endregion

	}

}