using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public int itemLevel;
    public string itemName;
    public float value;
    public ItemType itemType;
    public enum ItemType
    {
        Weapon,
        Armor,
        Bow
    }

}
