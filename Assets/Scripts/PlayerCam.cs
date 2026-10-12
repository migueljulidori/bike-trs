using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float SensX, SensY;
    public Transform Orientation;
    float RotationX, RotationY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensX;
        float MouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensY;

        RotationY += MouseX;
        RotationX -= MouseY;
        //RotationX = Mathf.Clamp(RotationX, -90f, 90f);
        transform.rotation = Quaternion.Euler(RotationX, RotationY, 0);
        Orientation.rotation = Quaternion.Euler(0, RotationY, 0);
    }
}
