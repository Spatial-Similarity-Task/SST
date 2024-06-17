using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to detect objects inside a trigger. Can be used to check if the player is on the ground or in front of a certain object for example
public class DetectObstruction : MonoBehaviour {

    public string ObjectTagName = "";
    public bool Obstruction; //Turns true if list of objects in the trigger is more than 0

    private bool detectObjectsUpdate; //Change to true if the object is one that gets destroyed/disabled

    public List<GameObject> Object = new List<GameObject>();
    public float count;
    
    private void Start()
    {
        if(ObjectTagName == "Goal")
        {
            detectObjectsUpdate = true;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (ObjectTagName != "")
        {
            if (col != null && !col.isTrigger && col.CompareTag(ObjectTagName))
            {
                AddObject(col.gameObject);
            }
        }

        if (ObjectTagName == "")
        {
            if (col != null && !col.isTrigger)
            {
                AddObject(col.gameObject);
            }
        }
    }






    void AddObject(GameObject g)
    {
        if (!detectObjectsUpdate)
        {
            if (!Object.Contains(g))
            {
                Object.Add(g);
                count++;
                if (count >= 1f)
                {
                    Obstruction = true;
                }
            }
        }
        if (detectObjectsUpdate)
        {
            if (!Object.Contains(g))
            {
                Object.Add(g);
                count++;
                if (count >= 1f)
                {
                    Obstruction = true;
                }
            }
        }
    }


    void OnTriggerExit(Collider col)
    {
        RemoveObject(col.gameObject);
    }

    void RemoveObject(GameObject g)
    {
        if (detectObjectsUpdate)
        {
            if (Object.Contains(g))
            {
                count--;
                if (count < 1f)
                {
                    Obstruction = false;
                }
                Object.Remove(g);
            }
        }
        else
        {
            if (Object.Contains(g))
            {
                count--;
                if (count < 1f)
                {
                    Obstruction = false;
                }
                Object.Remove(g);
            }
        }
    }
    private void Update()
    {
        if (count >= 1f)
        {
            Obstruction = true;
            if (detectObjectsUpdate)
            {
                for (int i = 0; i < count; i++)
                {
                    if (Object[i] == null)
                    {
                        count--;
                        if (count < 1f)
                        {
                            Obstruction = false;
                        }
                        Object.RemoveAt(i);
                    }
                }
            }
        }
        else
        {
            Obstruction = false;
        }
    }
}
