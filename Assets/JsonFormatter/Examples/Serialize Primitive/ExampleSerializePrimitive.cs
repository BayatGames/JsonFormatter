using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.Serialization.Formatters.Json;

public class ExampleSerializePrimitive : MonoBehaviour
{

	public string myName;
	public int sample;

	void Start ()
	{
		Debug.Log ( JsonFormatter.SerializeObject ( myName ) );
		Debug.Log ( JsonFormatter.SerializeObject ( sample ) );
	}

}
