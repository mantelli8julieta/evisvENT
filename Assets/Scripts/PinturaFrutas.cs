using UnityEngine;

public class PinturaFrutas : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer rend;

    // <<< ¡NUEVO! >>>
    // Variable para guardar el color que esta fruta debe aplicar
    private Color miColorDePintura; 

    [Header("Ajustes de comportamiento")]
    public float vidaDespuesImpacto = 1.5f; 
    public float tiempoMaximo = 5f; 

    private bool impactoRegistrado = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    void OnEnable()
    {
        impactoRegistrado = false;
        CancelInvoke();
        Invoke(nameof(Destruir), tiempoMaximo);
    }

    public void Lanzar(Vector3 direccion, float fuerza)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direccion * fuerza, ForceMode.Impulse);
    }

    // Tu función SetColor ahora es VITAL
    public void SetColor(Color color)
    {
        miColorDePintura = color; // Guarda el color
        if (rend != null)
            rend.material.color = color; // Pinta la fruta misma
    }

    void OnCollisionEnter(Collision col)
    {
        if (impactoRegistrado) return;
        impactoRegistrado = true;
        
        rb.isKinematic = true; 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = col.contacts[0].point + col.contacts[0].normal * 0.02f;

        // <<< ¡MODIFICADO! >>>
        // 1. Revisa si el objeto con el que chocamos TIENE el script ReceptorDePintura
        ReceptorDePintura lienzo = col.gameObject.GetComponent<ReceptorDePintura>();

        // 2. Si lo tiene...
        if (lienzo != null)
        {
            // 3. ¡Llama a su función Pintar() y pásale nuestro color!
            lienzo.Pintar(miColorDePintura);
        }

        // Destruir después de un pequeño delay
        Invoke(nameof(Destruir), vidaDespuesImpacto);
    }

    void Destruir()
    {
        gameObject.SetActive(false);
    }
}