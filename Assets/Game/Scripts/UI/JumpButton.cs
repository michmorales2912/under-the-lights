using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Botón de salto táctil para Android.
/// Coloca este script en el botón de salto del Canvas.
/// Asigna el FatherController en el Inspector.
/// </summary>
public class JumpButton : MonoBehaviour, IPointerDownHandler
{
    [Header("Referencias")]
    public FatherController father;

    public void OnPointerDown(PointerEventData eventData)
    {
        father?.Jump();
    }
}