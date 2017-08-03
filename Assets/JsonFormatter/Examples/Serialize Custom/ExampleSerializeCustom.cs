using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.Serialization.Formatters.Json;

public class ExampleSerializeCustom : MonoBehaviour
{

	public class Custom
	{

		public string myName = "Example";
		public int myID = 12;

	}

	void Start ()
	{
		Custom custom = new Custom ();
		Debug.Log ( JsonFormatter.SerializeObject ( custom ) );
	}

}
