using System.Collections.Generic;
using UnityEngine;

public class ItemDB
{
    public static Dictionary<string, (int value1, int value2)> itemDictionary = new Dictionary<string, (int value1, int value2)>
    {
        { "Wood", (50, 100) }, // Example: Wood has value1 = 50, value2 = 100
        { "Stone", (5, 50) }  // Example: Stone has value1 = 30, value2 = 60
    };
    public static (int value1, int value2) getItemValues(string itemName)
    {
        if (itemDictionary.TryGetValue(itemName, out var values))
        {
            return values;
        }
        else
        {
            UnityEngine.Debug.LogError("Elemento no encontrado: " + itemName);
            return (-1, -1); 
        }
    }
}