using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;
    ShopItemData data;


    public void SetUp(ShopItemData data)
    {
        meshFilter.sharedMesh = data.MeshRing;
        meshRenderer.sharedMaterials = data.Materials;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }
}
