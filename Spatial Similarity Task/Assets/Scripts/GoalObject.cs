using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    public bool CorrectGoal;
    public bool Collected;

    public bool AllowGoalCorrection;

    public bool TestGoal;

    public GameObject GoalType_1;
    public GameObject GoalType_2;


    public MeshRenderer mr;
    public MeshRenderer mr2;
    public BoxCollider box_collider;

    public Animator anim;

    GameManager manager;
    public bool PromptOpen;
    RigidbodyFirstPersonController rbfps;


    public bool TranslucentGoalType;
    Color initialColor;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
        manager = FindObjectOfType<GameManager>();
        rbfps = FindObjectOfType<RigidbodyFirstPersonController>();

        GoalType_1.SetActive(false);
        GoalType_2.SetActive(false);
        if(TranslucentGoalType)
        {
            GoalType_2.SetActive(true);
        }
        else
        {
            GoalType_1.SetActive(true);
        }


        initialColor = mr2.material.color;


        //Set goals rotation to point the right way
        Quaternion rot = Quaternion.LookRotation(manager.GoalPointPosition.transform.position - transform.position);
        Quaternion newrot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        transform.rotation =  newrot;

        if(manager.NewScaleVersion)
        {
            float x = transform.localScale.x;
            float y = transform.localScale.y;
            float z = transform.localScale.z;
            transform.localScale = new Vector3(0.7f * x, 0.7f * y, 0.7f * z);
        }
    }


    //Task 3 player got close to goal


    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(manager.timeoutSkip);
        if (other.CompareTag("Player") && !Collected && !manager.timeoutSkip)
        {
            manager.OpenInteractWithGoalPrompt("Press A to press button");
            if (TranslucentGoalType)
            {
                manager.OpenInteractWithGoalPrompt("Press A to choose this location");
            }

            PromptOpen = true;        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Collected)
        {
            manager.CurrentGoalsNearby.Add(gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.CloseInteractWithGoalPrompt();
            PromptOpen = false;
            manager.CurrentGoalsNearby.Remove(gameObject);
        }
    }

    public void Collect()
    {
        if (!Collected)
        {
            Collected = true;
            Debug.Log("AA");

            CollectGoalTask3();
            PromptOpen = false;
            manager.CloseInteractWithGoalPrompt();
        }
    }

    //Task 3 player touched goal
    private void CollectGoalTask3()
    {
        anim.Play("Press", 0, 0.01f);

        manager.GoalCollected(CorrectGoal);
        manager.CurrentGoalsNearby.Clear();

        if (CorrectGoal)
        {
            mr.materials[1].color = Color.green;
            mr2.material.color = Color.green;

            Debug.Log("Green");
        }
        else
        {
            mr.materials[1].color = Color.red;
            mr2.material.color = Color.red;
        }

        //If the goal allows correction, dont make the other goals collected so the player can try again
        if (!AllowGoalCorrection)
        {
            CollectForAllGoals();
        }


        if (TestGoal)
        {
            if (CorrectGoal)
            {
                mr.material.color = Color.green;
                mr2.material.color = Color.green;
            }
            else
            {
                mr.material.color = Color.red;
                mr2.material.color = Color.red;
            }
        }
    }


    //Makes all goals colected so player cant collect another goal
    public void CollectForAllGoals()
    {
        GoalObject[] goals = FindObjectsOfType<GoalObject>();
        foreach (GoalObject g in goals)
        {
            g.Collected = true;
        }
    }


    //Resets all goals back to collectable state
    public void ResetCollect()
    {
        anim.Play("Idle", 0, 0.01f);
        Collected = false;
        mr.materials[1].color = Color.yellow;
        mr2.material.color = initialColor;
    }







    //Task 1 

    //Returns true if right goal else false
    public bool CollectGoal()
    {
        if (!Collected)
        {
            CollectForAllGoals();

            if (CorrectGoal)
            {
                mr.material.color = Color.green;
                mr2.material.color = Color.green;
                return true;
            }
            else
            {
                mr.material.color = Color.red;
                mr2.material.color = Color.red;
                return false;
            }

        }
        return false;

    }

    public void DisableMesh() //called by spatial task, disables collision and mesh, still leaves trigger
    {
        box_collider.enabled = false;
        MeshRenderer[] r = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer a in r)
        {
            a.enabled = false;
        }
    }
}
