using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json formatter.
	/// Serialize and Deserialize json representations.
	/// </summary>
	public class JsonFormatter
	{
		
		#region Fields

		/// <summary>
		/// The surrogate selector.
		/// </summary>
		protected ISurrogateSelector m_SurrogateSelector;
		
		/// <summary>
		/// The context.
		/// </summary>
		protected StreamingContext m_Context;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the surrogate selector.
		/// </summary>
		/// <value>The surrogate selector.</value>
		public virtual ISurrogateSelector surrogateSelector
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
		public virtual StreamingContext context
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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		public JsonFormatter () : this ( null, new StreamingContext ( StreamingContextStates.All ) )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonFormatter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonFormatter ( ISurrogateSelector selector, StreamingContext context )
		{
			m_SurrogateSelector = selector;
			m_Context = context;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Serialize the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public string Serialize ( object value )
		{
			StringWriter output = new StringWriter ();
			Serialize ( output, value );
			return output.ToString ();
		}

		/// <summary>
		/// Serialize the specified output and value.
		/// </summary>
		/// <param name="output">Output.</param>
		/// <param name="value">Value.</param>
		public void Serialize ( Stream output, object value )
		{
			Serialize ( new StreamWriter ( output ), value );
		}

		/// <summary>
		/// Serialize the specified output and value.
		/// </summary>
		/// <param name="output">Output.</param>
		/// <param name="value">Value.</param>
		public void Serialize ( TextWriter output, object value )
		{
			using ( JsonWriter writer = new JsonTextWriter ( output, m_SurrogateSelector, m_Context ) )
			{
				writer.Write ( value );
			}
		}

		/// <summary>
		/// Deserialize the specified input.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( string input )
		{
			return ( T )Deserialize ( input, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified input.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( Stream input )
		{
			return ( T )Deserialize ( input, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified input.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T> ( TextReader input )
		{
			return ( T )Deserialize ( input, typeof ( T ) );
		}

		/// <summary>
		/// Deserialize the specified input and type.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="type">Type.</param>
		public object Deserialize ( string input, Type type )
		{
			return Deserialize ( new StringReader ( input ), type );
		}

		/// <summary>
		/// Deserialize the specified input and type.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="type">Type.</param>
		public object Deserialize ( Stream input, Type type )
		{
			return Deserialize ( new StreamReader ( input ), type );
		}

		/// <summary>
		/// Deserialize the specified input and type.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="type">Type.</param>
		public object Deserialize ( TextReader input, Type type )
		{
			using ( JsonReader reader = new JsonTextReader ( input, m_SurrogateSelector, m_Context ) )
			{
				return reader.Read ( type );
			}
		}

		/// <summary>
		/// Serializes the object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="value">Value.</param>
		public static string SerializeObject ( object value )
		{
			JsonFormatter formatter = new JsonFormatter ();
			return formatter.Serialize ( value );
		}

		/// <summary>
		/// Deserializes the object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="json">Json.</param>
		/// <param name="type">Type.</param>
		public static object DeserializeObject ( string json, Type type )
		{
			JsonFormatter formatter = new JsonFormatter ();
			return formatter.Deserialize ( json, type );
		}

		#endregion
		
	}

}