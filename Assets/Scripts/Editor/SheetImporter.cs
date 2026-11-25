using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
[CreateAssetMenu(
    fileName = "SheetImporter",
    menuName = "Editor/SheetImporter",
    order = 1
    
)]
public class SheetImporter : ScriptableObject
{

    [SerializeField] private string objectLink;
    [SerializeField] private string parameterLink;
    [SerializeField] private RarityTable rarityTable;

    [ContextMenu("Import")]
    public void Test()
    {
        Download(objectLink);
        Download(parameterLink);
    }
    private string Download(string url)
    {
        double unityTime = EditorApplication.timeSinceStartup;
        
        using UnityWebRequest request = UnityWebRequest.Get(url);// using significa que cuando este metodo termine, libera la memoria de esta variable.
        
        double timePassed = unityTime- EditorApplication.timeSinceStartup;
        
        request.SendWebRequest();
        
        while (request.isDone == false)
        {
            if (timePassed > 10)
            {
                Debug.LogError("La descarga ha tardado demasiado");
                return null;
            }
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        return request.downloadHandler.text;

    }
}
