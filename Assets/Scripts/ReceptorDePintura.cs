using UnityEngine;

public class ReceptorDePintura : MonoBehaviour
{
    private Renderer rend;

    void Awake()
    {
        // Guardamos nuestro renderer para no buscarlo cada vez
        rend = GetComponent<Renderer>();
        
        // Opcional: Si tus objetos tienen muchos sub-materiales
        // podrías necesitar GetComponentInChildren<Renderer>();
    }

    // Esta es la función CLAVE que llamará la fruta
    public void Pintar(Color nuevoColor)
    {
        if (rend != null)
        {
            // Cambia el color del material
            rend.material.color = nuevoColor;
        }
        else
        {
            Debug.LogWarning("¡Quisieron pintarme pero no tengo Renderer!", this.gameObject);
        }
    }
}