using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BayatGames.Serialization.Formatters.Json.Examples
{

	public class Example01 : MonoBehaviour
	{

		public struct Person
		{
			public string firstName;
			public string lastName;
		}

		[SerializeField]
		protected InputField m_FirstNameInput;
		[SerializeField]
		protected InputField m_LastNameInput;
		[SerializeField]
		protected InputField m_JsonInput;

		public void Serialize ()
		{
			Person person = new Person ();
			person.firstName = m_FirstNameInput.text;
			person.lastName = m_LastNameInput.text;
			m_JsonInput.text = JsonFormatter.SerializeObject ( person );
		}

		public void Deserialize ()
		{
			Person person = ( Person )JsonFormatter.DeserializeObject ( m_JsonInput.text, typeof ( Person ) );
			m_FirstNameInput.text = person.firstName;
			m_LastNameInput.text = person.lastName;
		}
		
		
	}

}