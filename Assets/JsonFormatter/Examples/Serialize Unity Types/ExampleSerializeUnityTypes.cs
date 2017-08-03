using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.Serialization.Formatters.Json;

public class ExampleSerializeUnityTypes : MonoBehaviour
{

	public Vector3 vector;

	void Start ()
	{
		Debug.Log ( JsonFormatter.SerializeObject ( vector ) );
	}

}
