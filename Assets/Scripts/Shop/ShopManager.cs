using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    [Header("Lista de objetos de la tienda")] [SerializeField]
    private List<ShopItemData> allItems;
    private List<ShopItemData> objects = new List<ShopItemData>();
    private List<ShopItemData> candidates = new List<ShopItemData>();
    private List<ShopItemData> purchased = new List<ShopItemData>();
   
    [Header("Huecos en la tienda")]
    [SerializeField] private List<ShopSlot> slots = new List<ShopSlot>();
    
    [Header("Economia")]
    [SerializeField] private int money;

    [Header("Rondas")]
    [SerializeField] private int rounds = 1;
    
    [Header ("Cosas varias")]
    [SerializeField] private ItemPickUp itemPickUp;
    [SerializeField] private Transform buy;
    [SerializeField] private int buttonPrice;
    private bool bellReady = true ;
    [SerializeField] RarityTable rarityTable;

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
    
    [ContextMenu("Next Round")]
    public void NextRound()
    {
        rounds++;
    }
    public void RearmBell()
    {
        bellReady = true;
    }
    
    public void Reload()
    {
        ShopReload();
    }

    private void FillPot()
    {
        objects.Clear();
        
        objects.AddRange(allItems);

        foreach ( ShopItemData bought  in purchased)
        {
            objects.Remove(bought);
        }
    }

    

    private bool HasRarity( Rarity rarity )
    {
        foreach ( ShopItemData item in objects )
        {
            if (item.Rarity == rarity)
            {
                return true;
            }
        }
        return false;
    }

    private float GetSegment(Rarity rarity)
    {
        if (!HasRarity(rarity))
        {
            return 0;
        }

        return rarityTable.GetWeightForRound(rarity,rounds);
    }
    
    private ShopItemData PickItem()
    {
        Rarity chosen;
      float rarityCommon = GetSegment(Rarity.Common);
      float rarityRare = GetSegment(Rarity.Rare);
      float rarityEpic = GetSegment(Rarity.Epic);
      float rarityLegendary = GetSegment(Rarity.Legendary);
      
      float allRarity = rarityCommon + rarityRare + rarityEpic + rarityLegendary;

      if (allRarity == 0) return null;

      if (rarityLegendary > 0)
      {
          chosen = Rarity.Legendary;
      }
      else if (rarityEpic > 0)
      {
          chosen = Rarity.Epic;
      }
      else if (rarityRare > 0)
      {
          chosen = Rarity.Rare;
      }
      else
      {
          chosen = Rarity.Common;
      }
      
      float total = Random.Range(0f, allRarity);

      if (total < rarityCommon)
      {
          chosen = Rarity.Common;
      }
      else if (total < (rarityCommon +rarityRare))
      {
          chosen = Rarity.Rare;
      }
      else if (total < (rarityCommon +rarityRare + rarityEpic))
      {
          chosen = Rarity.Epic;
      }
      else if (total < allRarity)
      {
          chosen = Rarity.Legendary;
      }
      
      candidates.Clear();

      foreach (ShopItemData item in objects)
      {
          if (item.Rarity == chosen)
          {
              candidates.Add(item);
          }
      }
      
      return candidates[Random.Range(0, candidates.Count)];
    }
    
    void ShopReload()
    {
        
        FillPot();
        
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
                ShopItemData  itemRing= PickItem();

                if (itemRing == null)
                {
                    slots[i].ClearSlot();
                }
                else
                {
                    slots[i].ShowItem(itemRing);
                    objects.Remove(itemRing);
                }
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
