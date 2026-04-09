using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    [Header("Movimiento")]
    private Vector2 movimientoActualEntrada;
    public float velocidadMovimiento =5f;

    [Header("Cámara")]
    private Vector2 ratonDelta;
    private float rotacionX;
    private float rotacionY;
    public Transform camera;
    public float minVista, maxVista;
    public float sensibilidad;



    private Rigidbody rb;
    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        VistaCamara();
    }

    private void FixedUpdate()
    {
        Movimiento();
    }

    //Función Evento
    public void OnMovimientoInput(InputAction.CallbackContext ctx)
    {
        // Ver el input dado, como un vector2 en caso de que se deje de encontrar se establece en cero para dejar de moverse. Examen.1
        if (ctx.phase == InputActionPhase.Performed)
        {
            movimientoActualEntrada = ctx.ReadValue<Vector2>();
        }
        else
        {
            movimientoActualEntrada = Vector3.zero;
        }
    }

    private void Movimiento()
    {
        // asignamos del input dado de movimiento los ejes para adelante y para los lados
        Vector3 direccion = transform.forward * movimientoActualEntrada.y + transform.right * movimientoActualEntrada.x;
        direccion.Normalize();
        // a esta direccion la multiplicamos por nuestra velocidad 
        direccion *= velocidadMovimiento;
        // la velocidad vertical (eje y) se mantiene a la que esté siendo aplicada en el rb
        direccion.y = rb.linearVelocity.y;

        rb.linearVelocity = direccion;
    }

    public void OnMirarInput(InputAction.CallbackContext ctx)
    {
        ratonDelta = ctx.ReadValue<Vector2>();
        //if (ctx.phase == InputActionPhase.Performed)
        //{
        //    ratonDelta = ctx.ReadValue<Vector2>();
        //}
        //else
        //{
        //    ratonDelta = Vector3.zero;
        //}

    }

    private void VistaCamara()
    {
        // Rotamos la camara de lo que tenemos en el input vistaCamara
        rotacionX -= ratonDelta.y * sensibilidad;
        rotacionX = Mathf.Clamp(rotacionX, minVista, maxVista);

        rotacionY += ratonDelta.x * sensibilidad;

        camera.localEulerAngles = new Vector3(rotacionX, 0, 0);
        transform.eulerAngles = new Vector3(0, rotacionY, 0);
    }
}
