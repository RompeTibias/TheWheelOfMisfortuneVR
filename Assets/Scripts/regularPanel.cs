using UnityEngine;

public class regularPanel : managerPanel
{
    // De momento dejamos esto aquí para que puedas probar el panel normal 
    // dándole al Play antes de conectar tu futuro CSV Reader.
    protected override void Start()
    {
        base.Start();
        CargarFrase(frasePrueba);
    }
}