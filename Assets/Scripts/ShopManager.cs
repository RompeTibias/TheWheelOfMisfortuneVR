using UnityEngine;
using System.Collections.Generic;


public class ShopManager : MonoBehaviour
{
    [Header ("Lista de objetos de la tienda")]
    [SerializeField] private List<ShopItemData> reload1;   // Primera lista de objetos de la tienda
    [SerializeField] private List<ShopItemData> reload2;   // Segunda lista de objetos de la tienda
    [SerializeField] private List<ShopItemData> reload3;   // tercera lista de objetos de la tienda
    private List<ShopItemData> objects = new List<ShopItemData>();//Lista de todos los objetos segun el dinero que tienes
   
   
    [Header("Huecos en la tienda")]
    [SerializeField] private List<ShopSlot> slots = new List<ShopSlot>();
    [Header("Economia")]
    [SerializeField] private int money;

    private ShopSlot slotShop;

    
    
    [ContextMenu("Recargar")]
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
}
