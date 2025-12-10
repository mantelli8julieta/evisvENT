using UnityEngine;
using extOSC;



public class RecibidorMocion : MonoBehaviour
{
  

    [Header("OSC Settings")]
    public int puertoEscucha = 9000;
    private OSCReceiver receiver;

    [Header("Matrix Mapping")]
    public int columnas = 20;      // debe coincidir con lo que usás en Processing
    public int filas = 15;         // idem
    public int targetZones = 3;    // número de zonas que querés mapear (ej. 3 -> -1,0,1 ; 5 -> -2..2)

    [Header("Referencia al Lanzador")]
    public LanzadorFrutas lanzador;  // arrastrá aquí tu GestorLanzamientos (GameObject con LanzadorDeProyectiles)

    [Header("Opcionales")]
    public Transform PuntoLanzamiento; // transform hijo del CenterEyeAnchor
    public float separacion = 0.25f;   // tamaño de celda virtual (si necesitas posicionar debug)

    void Start()
    {
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = puertoEscucha;
        receiver.Bind("/motion", OnMotionReceived);
        Debug.Log($" RecibidorMocion escuchando OSC en puerto {puertoEscucha}");
    }

    void OnMotionReceived(OSCMessage message)
    {
        if (message.Values.Count < 3) return;

        int i = message.Values[0].IntValue;
        int j = message.Values[1].IntValue;
        float intensidad = message.Values[2].FloatValue; // Este es el 'val' de Processing

        float dx = (message.Values.Count > 3) ? message.Values[3].FloatValue : 0f;
        float dy = (message.Values.Count > 4) ? message.Values[4].FloatValue : 0f;

        // Mapear columna i a una zona entera en rango [-half..+half]
        int zona = MapIToZone(i, columnas, targetZones);

        // Debug
        // Debug.Log($"MSG i:{i} j:{j} zona:{zona} int:{intensidad} dx:{dx} dy:{dy}");

        if (lanzador != null)
        {
            // Llamamos a la función que ahora acepta intensidad
            lanzador.Disparar(zona, intensidad); 
        }
        else
        {
            Debug.LogWarning("RecibidorMocion: lanzador no asignado en inspector.");
        }
    }

    // Mapea la columna 'i' (0..columnas-1) a un índice de zona entero centrado.
    // Ej: targetZones=3 -> devuelve -1,0,1
    //     targetZones=5 -> devuelve -2,-1,0,1,2
    int MapIToZone(int i, int columnasCount, int targetZonesCount)
    {
        if (columnasCount <= 1) return 0;
        int zones = Mathf.Max(1, targetZonesCount);
        float t = (float)i / (float)(columnasCount - 1); // 0..1
        int index = Mathf.FloorToInt(t * zones);         // 0 .. zones-1 (but if t==1 -> zones)
        if (index >= zones) index = zones - 1;

        // Convertir 0..zones-1 a centrado -half..+half
        int half = zones / 2;
        int centered = index - half;
        // Si zones es even, esto produce range like -1..+2; es preferible usar odd counts.
        return centered;
    }
    
    // --- TODO TU CÓDIGO TERMINA ANTES DE ESTA LLAVE ---
}

// NADA DEBE IR AQUÍ FUERA TAMPOCO