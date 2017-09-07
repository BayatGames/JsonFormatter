using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BayatGames.Serialization.Formatters.Json
{

	/// <summary>
	/// Json extensions.
	/// </summary>
	public static class JsonExtensions
	{

		/// <summary>
		/// Serializes the objec to it's json representation.
		/// </summary>
		/// <returns>The json.</returns>
		/// <param name="value">Value.</param>
		public static string ToJson ( this object value )
		{
			return JsonFormatter.SerializeObject ( value );
		}

		/// <summary>
		/// Appends the string until end. (Reaches to quotation mark)
		/// </summary>
		/// <param name="builder">Builder.</param>
		/// <param name="json">Json.</param>
		/// <param name="index">Index.</param>
		public static void AppendUntilStringEnd ( this StringBuilder builder, string json, ref int index )
		{
			if ( json.Length <= index )
			{
				return;
			}
			builder.Append ( json [ index ] );
			index++;
			while ( json [ index ] != '\"' )
			{
				builder.Append ( json [ index ] );
				index++;
			}
			builder.Append ( json [ index ] );
		}

		/// <summary>
		/// Removes the whitespace.
		/// </summary>
		/// <returns>The whitespace.</returns>
		/// <param name="json">Json.</param>
		public static string RemoveWhitespaceJson ( this string json )
		{
			if ( string.IsNullOrEmpty ( json ) )
			{
				return json;
			}
			StringBuilder builder = new StringBuilder ();
			for ( int i = 0; i < json.Length; i++ )
			{
				if ( json [ i ] == '\"' )
				{
					builder.AppendUntilStringEnd ( json, ref i );
					continue;
				}
				else if ( char.IsWhiteSpace ( json [ i ] ) )
				{
					continue;
				}
				else
				{
					builder.Append ( json [ i ] );
				}
			}
			return builder.ToString ();
		}

		/// <summary>
		/// Split the specified json.
		/// </summary>
		/// <param name="json">Json.</param>
		public static string[] SplitJson ( this string json )
		{
			if ( string.IsNullOrEmpty ( json ) )
			{
				return new string[0];
			}
			List<string> result = new List<string> ();

			// Prevent going deeper
			int depth = 0;
			StringBuilder builder = new StringBuilder ();
			for ( int i = 1; i < json.Length - 1; i++ )
			{
				switch ( json [ i ] )
				{
					case '[':
					case '{':
						depth++;
						break;
					case ']':
					case '}':
						depth--;
						break;
					case '\"':
						builder.AppendUntilStringEnd ( json, ref i );
						continue;
					case ',':
					case ':':
						// Stop going deep
						if ( depth == 0 )
						{
							result.Add ( builder.ToString () );
							builder.Length = 0;
							continue;
						}
						break;
				}
				builder.Append ( json [ i ] );
			}

			// Add ending entry
			result.Add ( builder.ToString () );
			return result.ToArray ();
		}
	
	}

}