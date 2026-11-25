using UnityEngine;

[CreateAssetMenu(fileName = "Sumar Puntos", menuName = "Ruleta/Habilidades/Sumar puntos")]
public class HabilidadSumarPuntos : HabilidadQuesito
{
    [SerializeField] private float puntosExtra = 100f;

    public override void Ejecutar(GameManager partida, char? letra, int aciertos)
    {
        partida.pointsActual += puntosExtra;
    }
}
