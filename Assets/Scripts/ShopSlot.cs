using TMPro;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMeshPro precio;
    [SerializeField] private GameObject visual;
    private ShopItemData currentItem;
    [SerializeField] private ShopItemData testItem;

    public bool IsEmpty => currentItem == null;

    public ShopItemData CurrentItem
    {
        get
        {
            return currentItem;   
        }   
    }
   
    
    public void ShowItem(ShopItemData item)
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        currentItem = item;

        meshFilter.sharedMesh = item.MeshRing;

        meshRenderer.sharedMaterials = item.Materials;

        precio.text = item.Price.ToString();

        precio.gameObject.SetActive(true);
        visual.SetActive(true);
        
    }

   public void ClearSlot()
    {
        currentItem = null;
        precio.gameObject.SetActive(false);
        visual.SetActive(false);
        
    }

}
