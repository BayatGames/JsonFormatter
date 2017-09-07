using System;
using System.Runtime.Serialization;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json writer.
	/// </summary>
	public abstract class JsonWriter : IDisposable
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
		/// Initializes a new instance of the <see cref="BayatGames.Serialization.Formatters.Json.JsonWriter"/> class.
		/// </summary>
		/// <param name="selector">Selector.</param>
		/// <param name="context">Context.</param>
		public JsonWriter ( ISurrogateSelector selector, StreamingContext context )
		{
			m_SurrogateSelector = selector;
			m_Context = context;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Write the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public abstract void Write ( object value );

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