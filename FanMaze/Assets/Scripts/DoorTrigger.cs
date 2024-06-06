using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("EntranceTrigger")]
    public bool IsEntranceTrigger;
    public bool IsInactive;

    public Animator Door;
    public bool Opened;

    [Header("StartTrigger")]
    public bool IsStartTrigger;

    GameManager manager;


    //General use trigger

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void ManuallyOpenDoor()
    {
        Door.Play("Open");
        Opened = true;
    }
    public void ManuallyCloseDoor()
    {
        Door.Play("Idle");
        Opened = false;
    }
    public void Deactivate()
    {
        IsInactive = true;
    }

    public void Activate()
    {
        IsInactive = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsInactive)
        {
            if (other.CompareTag("Player"))
            {
                if (IsEntranceTrigger)
                {
                    if (!manager.GoalCollected_Returning)
                    {
                        Door.Play("Open");
                        Opened = true;
                    }
                    else
                    {
                        Door.CrossFade("Idle", 1f, 0);
                        Opened = false;
                    }
                }
                if (IsStartTrigger)
                {
                    manager.ReturnedToStart();
                }
            }
        }

        if (IsEntranceTrigger) {
            Debug.Log("Player");
            if (manager.markerUsed) {
                return;
            }
            else {
                if (manager.GoalCollected_Returning) {
                    LogOutput.markerString = "Enter";
                }
                else {
                    LogOutput.markerString = "Exit";
                }

                manager.markerUsed = true;
                Debug.Log("Marker");
            }
        }
    }

}
