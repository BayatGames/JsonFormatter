using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BayatGames.Serialization.Formatters.Json.Examples
{

	public class Example02 : MonoBehaviour
	{

		[SerializeField]
		protected InputField m_TypeInput;
		[SerializeField]
		protected InputField m_JsonInput;

		public void Deserialize ()
		{
			Debug.Log ( JsonFormatter.DeserializeObject ( m_JsonInput.text, Type.GetType ( m_TypeInput.text ) ) );
		}


	}

}