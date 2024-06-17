using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Vector3 offset;
    public float maxYPos;

    private bool IsOnLadder;
    private RigidbodyFirstPersonController rbfps;

    private void Start()
    {
        rbfps = FindObjectOfType<RigidbodyFirstPersonController>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            IsOnLadder = true;
            rbfps.OnLadder = true;
        }

    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            IsOnLadder = false;
            rbfps.OnLadder = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(IsOnLadder)
        {
            if(rbfps.movementInput.y > 0 && rbfps.transform.position.y < maxYPos)
            {
                rbfps.transform.position += offset * Time.deltaTime;
            }
            if (rbfps.movementInput.y < 0 && !rbfps.IsGrounded)
            {
                rbfps.transform.position -= offset * Time.deltaTime;
            }
        }
    }
}
