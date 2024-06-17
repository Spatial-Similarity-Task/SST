using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UXFGameManager : MonoBehaviour
{
    public GameManager GameManager;
    public Dropdown levelDropDown;

    // Start is called before the first frame update
    public void Begin()
    {
        string TaskIndex = levelDropDown.options[levelDropDown.value].text;

        Debug.Log(TaskIndex);
        if (TaskIndex == "Baseline")
        {
            GameManager.BaseLineTask = true;
        }

        if (TaskIndex == "Training 2")
        {
            GameManager.Partitions = true;

            GameManager.NonVerbalTraining = true;
        }

        if (TaskIndex == "Verbal Training")
        {
            GameManager.Partitions = true;

            GameManager.VerbalTraining = true;
        }
        if (TaskIndex == "Verbal Practice") //Spatial task //Follow fulltask layout
        {
            GameManager.Partitions = true;

            GameManager.FullTask = true;
            GameManager.Practice = true;
        }
        if (TaskIndex == "Non Verbal Practice") //Mini Task fan arena //Follow fulltask layout
        {
            GameManager.Partitions = true;

            GameManager.FullTask = true;
            GameManager.Practice = true;
        }


        if (TaskIndex == "Full Task")
        {
            GameManager.Partitions = true;
            GameManager.FullTask = true;
        }

        //SET ARMS NUMBER AND ARMS VERSION MANUALLY FROM EDITOR

        if (TaskIndex == "Training") // SPS training
        {
            GameManager.Task_SPS = true;
            GameManager.VerbalTraining = true;
            GameManager.Partitions = false;
            GameManager.NoPartitions = true;
        }

        if (TaskIndex == "Training 2") // SPS training
        {
            GameManager.Task_SPS = true;
            GameManager.NonVerbalTraining = true;
            GameManager.Partitions = false;
            GameManager.NoPartitions = true;
        }

        if (TaskIndex == "Practice") // SPS practice
        {
            GameManager.Task_SPS = true;
            GameManager.Practice = true;
            GameManager.FullTask = true;
            GameManager.Partitions = false;
            GameManager.NoPartitions = true;
        }
        if (TaskIndex == "SST") // SPS open arena
        {
            Debug.Log("SST");
            GameManager.Task_SPS = true;
            GameManager.FullTask = true;
            GameManager.Partitions = false;
            GameManager.NoPartitions = true;
        }

        if (TaskIndex == "SST 2") // SPS open arena
        {
            GameManager.Task_SPS = true;
            GameManager.FullTask = true;
            GameManager.Partitions = true;
            GameManager.NoPartitions = false;
        }

        if (TaskIndex == "Full Task - SPA") // SPA open arena
        {
            GameManager.Task_SPA = true;
            GameManager.FullTask = true;
            GameManager.SPA_14 = false;
        }

        if (TaskIndex == "SPA Task") // SPA open arena
        {
            GameManager.Task_SPA = true;
            GameManager.FullTask = true;
            GameManager.SPA_14 = true;
        }
    
        GameManager.goalmanager.LoadSchedules();
        GameManager.NumberOfTests = GameManager.goalmanager.Trials.Length;
        FindObjectOfType<LogOutput>().SetDirectory();
        TrialRecorder.instance.StartRecording(GameManager.currentTestNumber.ToString());
    }


    public void SwitchScale()
    {
        TriggersManager.instance.ClosePort();
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(0);
            return;
        }
    }
}
