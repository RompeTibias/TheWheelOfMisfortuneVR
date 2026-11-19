using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class ShopManager : MonoBehaviour
{
    [Header ("Lista de objetos de la tienda")]
    [SerializeField] private List<ShopItemData> reload1;   // Primera lista de objetos de la tienda
    [SerializeField] private List<ShopItemData> reload2;   // Segunda lista de objetos de la tienda
    [SerializeField] private List<ShopItemData> reload3;   // tercera lista de objetos de la tienda
    private List<ShopItemData> objects = new List<ShopItemData>();//Lista de todos los objetos segun el dinero que tienes
    private List<ShopItemData> purchased = new List<ShopItemData>();
   
    [Header("Huecos en la tienda")]
    [SerializeField] private List<ShopSlot> slots = new List<ShopSlot>();
    [Header("Economia")]
    [SerializeField] private int money;
    
    [SerializeField] private ItemPickUp itemPickUp;
    [SerializeField] private Transform buy;
    [SerializeField] private int buttonPrice;
    private bool bellReady = true ;

    private void Start()
    {
        
        foreach (ShopSlot slot in slots)
        {
            slot.SetManager(this) ;
        }
    }

    public void TryReload()
    {
        if (!bellReady) return;
        
        bellReady = false;
        
        if(money < buttonPrice) return;
        
        money -= buttonPrice;
        
        Reload();

        
    }

    public void RearmBell()
    {
        bellReady = true;
    }
    
    public void Reload()
    {
        ShopReload();
    }
    void ShopReload()
    {
        
        objects.Clear();
        if (money>=0 )
        {   
            objects.AddRange(reload1);
        }
        if(money>6 )
        {
            objects.AddRange(reload2);
        }
        if (money > 12 )
        {
            objects.AddRange(reload3);
        }

        foreach ( ShopItemData bought  in purchased)
        {
            objects.Remove(bought);
        }
        
        int amountToShow;
        
        if (objects.Count == 0)
        {
            amountToShow = 0;
        }
        else if(objects.Count >= slots.Count)
        {
            amountToShow = slots.Count;
        }
        else
        {
            amountToShow = Random.Range(1, objects.Count+1);
        }
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < amountToShow)
            {
                int j = Random.Range(0, objects.Count);
                slots[i].ShowItem(objects[j]);
                objects.RemoveAt(j);
            }
            else
            {
                slots[i].ClearSlot();
            }
            
        }    
            
    }

    public void TryBuy(ShopSlot slot)
    {
        
        if(slot.IsEmpty) return;
        
        var item = slot.CurrentItem;

        if (money < item.Price) return;
        
        money -= item.Price;
        
        purchased.Add(item);
        
        ItemPickUp newItem = Instantiate(itemPickUp, buy.position, buy.rotation);
        
        newItem.SetUp(item);

        slot.ClearSlot();
    }
    
}
