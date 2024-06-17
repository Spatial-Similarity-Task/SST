using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    public XRNode inputSource;
    public GameObject Camera;

    private Vector2 inputAxis;
    private CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        //get the left joystick
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        //rotation of the camera only in the y axis
        Quaternion headYaw = Quaternion.Euler(0, Camera.transform.eulerAngles.y, 0);
        Vector3 dir = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        //move the player
        cc.Move(dir * speed * Time.deltaTime); 
    }
}
