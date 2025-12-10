using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

public class LanzadorFrutas : MonoBehaviour
{
    [Header("Referencias VR")]
    public Transform manoDerecha; // <--- Arrastra aquí el RightHandAnchor o el OVRHandPrefab derecho
    
    [Header("Configuración Disparo")]
    public float fuerzaDeLanzamiento = 25f; 

    [Header("Sonidos FMOD")]
    public EventReference sonidoLanzamiento;

    [Header("Canasta de Frutas")]
    public List<InfoFruta> inventarioFrutas; 

    // Escudo de seguridad (Cooldown)
    private float tiempoUltimoDisparo = 0f;
    private float esperaEntreDisparos = 0.4f; 

    // Esta función la llama tu celular/Processing
    public void Disparar(int zona, float intensidad)
    {
        // 1. Escudo de seguridad
        if (Time.time < tiempoUltimoDisparo + esperaEntreDisparos) return;

        // 2. SIEMPRE usamos la mano derecha
        if (manoDerecha != null)
        {
            LanzarProyectil(manoDerecha, intensidad);
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void LanzarProyectil(Transform mano, float multiplicador)
    {
        if (inventarioFrutas == null || inventarioFrutas.Count == 0) return;

        InfoFruta fruta = inventarioFrutas[Random.Range(0, inventarioFrutas.Count)];
        
        // Nacer 20cm delante de la palma
        Vector3 posSalida = mano.position + (mano.forward * 0.5f);
        
        GameObject proyectil = Instantiate(fruta.prefab, posSalida, mano.rotation);
        
        RuntimeManager.PlayOneShot(sonidoLanzamiento, mano.position);

        PinturaFrutas pintura = proyectil.GetComponent<PinturaFrutas>();
        if (pintura != null) pintura.SetColor(fruta.colorPintura);

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Disparamos hacia donde apunta la palma
            
            
            float fuerzaFinal = fuerzaDeLanzamiento; // Asegúrate que en el Inspector sea 20 o 25
            
            // Dispara hacia adelante de la mano
            rb.AddForce(mano.forward * fuerzaFinal, ForceMode.VelocityChange);
            
            
        }
    }
}

[System.Serializable]
public class InfoFruta
{
    public string nombre;
    public GameObject prefab;
    public Color colorPintura;
}