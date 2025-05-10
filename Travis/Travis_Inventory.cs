using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travis_Inventory : MonoBehaviour
{
    public static Travis_Inventory Instance;

    private List<InventoryItem> m_Melee { get; }
    private List<InventoryItem> m_Gun { get; }
    private List<InventoryItem> m_Supply { get; }
    private List<InventoryItem> m_Key { get; }
        
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public List<InventoryItem> GetMeleeList()
    {
        return m_Melee;
    }

    public List<InventoryItem> GetGunList()
    {
        return m_Gun;
    }

    public List<InventoryItem> GetSupplyList()
    {
        return m_Supply;
    }

    public List<InventoryItem> GetKeyList()
    {
        return m_Key;
    }
}
