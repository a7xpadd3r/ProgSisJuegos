using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 100)] public float speed = 2;
    [Range(0, 5000)] public float sensibility = 2;
    public Camera camera;
    
    private CharacterController _controller;
    private float _xRot;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            Vector3 move = transform.right * x + transform.forward * z;
            _controller.Move(speed * move * delta);
        }
        
        float mouseX = Input.GetAxis("Mouse X") * sensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibility * Time.deltaTime;
        
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, -90f, 90f);
        
        camera.transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
