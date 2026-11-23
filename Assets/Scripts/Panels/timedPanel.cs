using System.Collections;
using UnityEngine;

public class timedPanel : managerPanel
{
    private Coroutine timerCoroutine;
    public bool timerActive = true;
    public float timerDuration = 10f; // en secs
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        
        CargarFrase(frasePrueba);
       // Timer(10f);
    }
   
    IEnumerator Timer(float timerDuration)
    {
    while (timerDuration>0)
        {
            if (timerActive==true)
            {
                timerDuration -= Time.deltaTime;
                Debug.Log("Tiempo restante: " + Mathf.Ceil(timerDuration) + " segundos");
            }

        }
    //en el yield habra que llamar al manager del juegon pra la lose/win condition 
        yield return new WaitForSeconds(0.1f);
    }

    private void TimerBoolTrue()
    {
        //aqui la logica de que la letra "gritara" lo de mates y activara esto
    }
    private void TimerBoolFalse()
    {
        //aqui la logica de que la letra "gritara" lo de mates y activara esto
    }
}
