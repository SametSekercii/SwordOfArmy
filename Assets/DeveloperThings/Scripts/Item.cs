using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public int id;
    public int itemLevel;
    public string itemName;
    public float damage;
    public float health;
    public enum ItemType
    {
        Weapon,
        Armor,
        Bow
    }

}
