using UnityEngine;

public class CamaraTestPC : MonoBehaviour
{
    public float velocidad = 2.0f;
    private float rotacionX = 0;
    private float rotacionY = 0;

    void Update()
    {
        // Mover solo si mantienes presionado CTRL o CLIC DERECHO
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(1)) 
        {
            float mouseX = Input.GetAxis("Mouse X") * velocidad;
            float mouseY = Input.GetAxis("Mouse Y") * velocidad;

            rotacionY += mouseX;
            rotacionX -= mouseY;
            
            // Aplicar la rotación a la cámara
            transform.eulerAngles = new Vector3(rotacionX, rotacionY, 0);
        }
    }
}