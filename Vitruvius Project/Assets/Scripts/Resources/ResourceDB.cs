using System.Collections.Generic;
using UnityEngine;

public class ResourceDB : MonoBehaviour
{
    public static Dictionary<string, int> itemDictionary = new Dictionary<string, int>
    {
        { "Wood", 50 },
        { "Stone", 30 }
    };
}