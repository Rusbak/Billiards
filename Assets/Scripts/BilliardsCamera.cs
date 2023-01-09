using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilliardsCamera : MonoBehaviour
{
    public Camera myCamera;
    public Rigidbody tableCenter;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        tableCenter = GetComponent<Rigidbody>();
        menuCamera();
    }

    public void menuCamera()
    {
        rotationSpeed = 0.05f;
        myCamera.transform.position = new Vector3(-125, 175, 0);
        myCamera.transform.rotation = Quaternion.Euler(40, 90, 0);
    }

    public void gameCamera()
    {
        rotationSpeed = 0;
        myCamera.transform.position = new Vector3(0, 125, 0);
        myCamera.transform.rotation = Quaternion.Euler(90, 180, 0);
        //make camera projection orthographic
    }

    // Update is called once per frame
    void Update()
    {
        tableCenter.transform.Rotate(0, -rotationSpeed, 0, Space.Self);
    }
}
