using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Quesito", menuName = "Ruleta/Quesito")]
public class QuesitoRuleta : ScriptableObject
{
    public enum Rareza
    {
        Comun,
        Rara,
        Epica,
        Legendaria
    }

    [Header("Datos del quesito")]
    public string nombre;
    [TextArea] public string descripcion;
    public Rareza rareza;
    public Material material;

    [Header("Habilidades disponibles")]
    public HabilidadQuesito postGiro;
    public HabilidadQuesito fallo;
    public HabilidadQuesito acierto;
    public HabilidadQuesito resolver;
}
