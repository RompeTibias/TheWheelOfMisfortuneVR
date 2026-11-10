using TMPro;
using UnityEngine;

public class LetrasElegir : MonoBehaviour
  
{
    [SerializeField] private managerPanel manager;//preguntar a paco si esto o findobjects por el prefab y rendimiento
    [SerializeField] private TextMeshPro letra;
    public char letraElegida;
    private void OnMouseDown()//temporal para probar sin vr
    {
        Interracionado();
        Debug.Log("el click funciona " );
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()//si uqeremos jugar en algun momento con esto habria que poner por codigo la letra por si hacemos un panel que se muevan o algo `pero de momento no hace falta
    {
        letra.text = letraElegida.ToString();//el etxtmesh no coge chars
    }
    void Interracionado()//no se como llamr con lo de vr tengo que mirar es con lo de gran sera ongrab o algo
    {
        manager.Aciertos(letraElegida);
        Debug.Log("letra elegida: " + letraElegida);
    }
        
    
}