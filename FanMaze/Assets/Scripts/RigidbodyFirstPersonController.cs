using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidbodyFirstPersonController : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {

        public float ForwardSpeed = 8.0f;   // Speed when walking forward
        public float BackwardSpeed = 4.0f;  // Speed when walking backwards
        public float StrafeSpeed = 4.0f;    // Speed when walking sideways
    }


    public Vector3 relativeMovementUpdate; //relative movement of the player, used to check if player is moving in a certain direction
    public Vector3 relativeTransform;

    public Transform cam;
    public MovementSettings movementSettings = new MovementSettings();
    public MouseLook mouseLook = new MouseLook();


    [Header("More properties")]

    public DetectObstruction DetectGround; //checks for ground
    public bool IsGrounded; //tells us if the player is on the ground

    public bool CanRotate = true;
    public bool CanMove = true;
    public bool OnLadder = false;

    [Header("PlayerInput")]
    public PlayerInput PlayerInput; //Manages all the input from various controllers
    public Vector2 movementInput;
    public Vector2 rotationInput;


    private Rigidbody rb; //Rigidbody is the physics component of the player
    private CapsuleCollider cc;
    private Vector3 lastpos; //used for getting relative movement
    private void Start()
    {
        InvokeRepeating("LastPos", 0f, 2f);

        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        mouseLook.Init(transform, cam.transform); // initialize the player look rotation

    }

    //functions for getting relative movement
    void LastPos()
    {
        lastpos = transform.position;
        Invoke("UpdatePos", 1f);
    }
    void UpdatePos()
    {
        relativeTransform = transform.InverseTransformDirection(transform.position - lastpos);

    }



    private void RotateView()
    {


        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);


        cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.x, 0, 0);



        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        if (IsGrounded)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            rb.velocity = velRotation * rb.velocity;
        }
    }

    private void RotateOveride()
    {
            mouseLook.LookRotationOverride(transform, cam.transform);
    }

    private void RotateOverideCamOnly()
    {
        mouseLook.LookRotationOverideCam(transform, cam.transform);
    }

    public void CamGoBack(float speed)
    {
        mouseLook.CamGoBack(transform, cam.transform, speed);
    }

    public void CamReset()
    {
        mouseLook.CamReset(cam.transform);
    }



    private void Update()
    {
        if (CanRotate)
        {
            RotateView();
        }
        if (!CanRotate)
        {
            RotateOveride();
        }

        GroundCheck();
        relativeMovementUpdate = transform.InverseTransformDirection(rb.velocity);

        float h = movementInput.x;
        float v = movementInput.y;
        Vector3 inputVector = new Vector3(h, 0, v);
        inputVector = Vector3.ClampMagnitude(inputVector, 1);

        //Move the player if the player is on the ground and joystick is pushed in a direction
        rb.drag = 15f;

        if (!IsGrounded)
        {
            rb.drag = 2f;
        }
        if(OnLadder)
        {
            rb.drag = 10f;
        }

        rb.useGravity = true;
        if(OnLadder)
        {
            rb.useGravity = false;
        }

        if (CanMove && IsGrounded && !PlayerInput.actions["Run"].IsPressed())
        { 
            
            if (v > 0.1f)
            {
                rb.AddRelativeForce(0, 0, Time.deltaTime * movementSettings.ForwardSpeed * 100f * Mathf.Abs(inputVector.z));
            }
            if (v < -0.1f)
            {
                rb.AddRelativeForce(0, 0, Time.deltaTime * -movementSettings.BackwardSpeed * 100f * Mathf.Abs(inputVector.z));
            }
            if (h > 0.1f)
            {
                rb.AddRelativeForce(Time.deltaTime * movementSettings.StrafeSpeed * 100f * Mathf.Abs(inputVector.x), 0, 0);
            }
            if (h < -0.1f)
            {
                rb.AddRelativeForce(Time.deltaTime * -movementSettings.StrafeSpeed * 100f * Mathf.Abs(inputVector.x), 0, 0);
            }

        }

        if (CanMove && IsGrounded && PlayerInput.actions["Run"].IsPressed())
        {

            if (v > 0.1f)
            {
                rb.AddRelativeForce(0, 0, Time.deltaTime * movementSettings.ForwardSpeed * 2f * 100f * Mathf.Abs(inputVector.z));
            }
            if (v < -0.1f)
            {
                rb.AddRelativeForce(0, 0, Time.deltaTime * -movementSettings.BackwardSpeed * 2f * 100f * Mathf.Abs(inputVector.z));
            }
            if (h > 0.1f)
            {
                rb.AddRelativeForce(Time.deltaTime * movementSettings.StrafeSpeed * 2f * 100f * Mathf.Abs(inputVector.x), 0, 0);
            }
            if (h < -0.1f)
            {
                rb.AddRelativeForce(Time.deltaTime * -movementSettings.StrafeSpeed * 2f * 100f * Mathf.Abs(inputVector.x), 0, 0);
            }

        }



    }



    //Checking input
    public void OnMove(InputValue value) //called by playerInput
    {
        movementInput = value.Get<Vector2>();
    }
    public void OnLook(InputValue value) //Input handling for mouseLook
    {
        rotationInput = value.Get<Vector2>();

        mouseLook.yRot = value.Get<Vector2>().x;
       
        mouseLook.xRot = value.Get<Vector2>().y;

    }

    






    //Check for the ground
    private void GroundCheck()
    {
        IsGrounded = false;
        if (DetectGround.Obstruction)
        {
            IsGrounded = true;
        }
    }


    public void ChangeRotation(Quaternion rotation)
    {
        CanRotate = false; //so mouselook doesnt ovveride the new rotation
        transform.rotation = rotation;
        CamReset();

        CancelInvoke("CanRotateFunc");
        Invoke("CanRotateFunc", 0.1f);
    }
    void CanRotateFunc()
    {
        CanRotate = true;
    }
}
	
