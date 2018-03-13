using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.Serialization.Formatters.Json;

public class Test : MonoBehaviour
{

    private void Start()
    {
        Save();
    }

    public void Save()
    {
        var saveData = new List<IA>();
        string json = JsonFormatter.SerializeObject(saveData);
        Debug.Log(json);
    }

    [Serializable]
    public class A : IA
    {

        public int Uno { get; set; }

    }

    public interface IA
    {
    }

}
