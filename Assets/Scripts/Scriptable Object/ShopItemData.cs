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
    public static ShopItemData Instance;
    [Header("Information")]
    [SerializeField] private string displayName;
    [TextArea]
    [SerializeField] private string description;
    [Min(0)]
    [SerializeField] private int price;

    [SerializeField] private Mesh meshRing;
    [SerializeField] private Material[] materials;

    [SerializeField] private Rarity rarity;


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
}
