using UnityEngine;

public abstract class HabilidadQuesito : ScriptableObject
{
    // letra es null en post giro y al resolver.
    public abstract void Ejecutar(GameManager partida, char? letra, int aciertos);
}
