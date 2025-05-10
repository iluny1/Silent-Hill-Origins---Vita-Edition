using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Scrpitable Object/Inventory Item", order = 1)]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private bool isCountable;
    [SerializeField] private Enums.ItemTypes type;
    [SerializeField] private int count;
    [SerializeField] private Texture2D image;
    [SerializeField] private GameObject model;
}
