using UnityEngine;
public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
[CreateAssetMenu(
    fileName = "ShopItemData",
    menuName = "ScriptableObjects/ShopItemData",
    order = 1
    
    )] 

public class ShopItemData : ScriptableObject
{
    [Header("Information")]
    [SerializeField] private string displayName;
    [TextArea]
    [SerializeField] private string description;
    [Min(0)]
    [SerializeField] private int price;

    [SerializeField] private Mesh meshRing;
    [SerializeField] private Material[] materials;

    [SerializeField] private Rarity rarity;
    
    [Header("Identificador")]
    [SerializeField] private string id;


    public int Price
    {
        get
        {
            return price;
        }
    }

    public Mesh MeshRing
    {
        get
        {
            return meshRing;
        }
    }

    public Material[] Materials
    {
        get
        {
            return materials;
        }
    }

    public Rarity Rarity
    {
        get
        {
            return rarity;
        }
    }
    
    #if UNITY_EDITOR
    public void ApplyImport(
        string newName, string newDescription, int newPrice, Rarity newRarity 
    ){}
    #endif
}
