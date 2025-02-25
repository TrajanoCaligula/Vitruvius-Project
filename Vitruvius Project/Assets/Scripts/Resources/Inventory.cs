using System;
using UnityEngine;

public class Inventory
{
    public string itemName;         // Item name
    public int itemQty;             // Quantity we are currently holding
    public int itemMaxQty = 5;      // Max Quantity that a citizen or a building can hold

    public bool isBuilding = true;

    public Inventory()
    {
        itemName = "";
        itemQty = 0;
        itemMaxQty = 0;   
    }

    public bool AddItem(string itemName, int amount)
    {
        int result = itemQty + amount;
        if (this.itemName == itemName && result <= itemMaxQty)
        {
            itemQty = result;
            return true;
        }
        return false;
    }

    public void AddOneItem(string itemName)
    {
        if (!isFull()) itemQty++;
    }

    internal void emptyInventory()
    {
        itemQty = 0;
    }

    public void ClearInventory()
    {
        itemName = "";
        itemQty = 0;
        itemMaxQty = 0;
    }

    // Getters

    public bool isFull()
    {
        return (itemQty >= itemMaxQty);
    }

    public bool isEmpty()
    {
        return (itemQty <= 0);
    }

    // Setters

    public void setBool(bool newValue)
    {
        isBuilding = newValue;
    }

    public void setItemFromDB(string itemName)
    {
        (int maxQtyBuilding, int maxQtyCitizen) = ItemDB.getItemValues(itemName);

        this.itemName = itemName;
        itemQty = 0;

        if (isBuilding) itemMaxQty = maxQtyBuilding;
        else itemMaxQty = maxQtyCitizen;
    }
}
