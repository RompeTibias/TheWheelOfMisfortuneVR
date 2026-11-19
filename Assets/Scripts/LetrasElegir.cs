using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LetrasElegir : MonoBehaviour
{
    public static event Action<char> OnLetraElegida;

    [SerializeField] private TextMeshPro letra;
    public char letraElegida;

    private XRSimpleInteractable simpleInteractable;

    private void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        if (simpleInteractable != null)
        {
            // Cambiamos 'selectEntered' por 'activated' (Gatillo de atrás / Trigger)
            simpleInteractable.activated.AddListener(OnLetraSeleccionada);
        }
    }

    private void OnDisable()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.activated.RemoveListener(OnLetraSeleccionada);
        }
    }

    void Start()
    {
        if (letra != null)
        {
            letra.text = letraElegida.ToString();
        }
    }

    // Se ejecuta al pulsar el gatillo de atrás mirando al objeto
    private void OnLetraSeleccionada(ActivateEventArgs args)
    {
        Interracionado();
    }

    private void OnMouseDown()
    {
        Interracionado();
        Debug.Log("El click de prueba funciona");
    }

    void Interracionado()
    {
        Debug.Log("Letra elegida: " + letraElegida);
        OnLetraElegida?.Invoke(letraElegida);
    }
}