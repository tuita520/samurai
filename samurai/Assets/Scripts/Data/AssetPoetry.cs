using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetPoetry", menuName = "Samurai/AssetPoetry")]
public class AssetPoetry : ScriptableObject
{
    public List<string> poetries = new List<string>();

    public string Get()
    {
        return poetries[Random.Range(0, poetries.Count)];
    }
}