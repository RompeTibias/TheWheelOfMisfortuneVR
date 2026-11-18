using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    ShopItemData data;


    public void SetUp(ShopItemData data)
    {
        meshFilter.sharedMesh = data.MeshRing;
        meshRenderer.sharedMaterials = data.Materials;
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }
}
