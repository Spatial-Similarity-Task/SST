using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public bool TestingTrials;

    public TrialGoal[] Trials;

    public string buildNumber;

    GameManager gameManager;

    string directory;
    string scheduleDirectory;

    private void Start()
    {
        directory = Application.persistentDataPath + "/Trial Logging Build " + buildNumber;
        gameManager = FindObjectOfType<GameManager>();

        scheduleDirectory = directory + "/Schedule";

        if (!Directory.Exists(scheduleDirectory))
        {
            Directory.CreateDirectory(scheduleDirectory);
          
            string path = Path.Combine(scheduleDirectory, "SPS_Verbal_Training_Schedule.csv");
            File.AppendAllText(path, "");

            path = Path.Combine(scheduleDirectory, "SPS_NonVerbal_Training_Schedule.csv");
            File.AppendAllText(path, "");

            path = Path.Combine(scheduleDirectory, "SPS_Practice_Schedule.csv");
            File.AppendAllText(path, "");

            path = Path.Combine(scheduleDirectory, "SPS_FullTask_Schedule.csv");
            File.AppendAllText(path, "");

            path = Path.Combine(scheduleDirectory, "SPA_FullTask_Schedule.csv");
            File.AppendAllText(path, "");

        }



    }

    public void LoadSchedules()
    {
        string task = "";
        if (gameManager.NonVerbalTraining)
        {
            task = "SPS_NonVerbal_Training_Schedule.csv";
        }
        if (gameManager.VerbalTraining)
        {
            task = "SPS_Verbal_Training_Schedule.csv";
        }
        if (gameManager.Practice)
        {
            task = "Practice_Schedule.csv";
        }
        if (gameManager.FullTask)
        {
            task = "FullTask_Schedule.csv";
        }

        if (gameManager.FullTask && gameManager.Task_SPS)
        {
            task = "SPS_FullTask_Schedule.csv";
        }
        if (gameManager.Task_SPS && gameManager.VerbalTraining)
        {
            task = "SPS_Verbal_Training_Schedule.csv";
        }
        if (gameManager.Task_SPS && gameManager.NonVerbalTraining)
        {
            task = "SPS_NonVerbal_Training_Schedule.csv";
        }
        if (gameManager.Task_SPS &&  gameManager.Practice)
        {
            task = "SPS_Practice_Schedule.csv";
        }
        if (gameManager.FullTask && gameManager.Task_SPA)
        {
            task = "SPA_FullTask_Schedule.csv";
        }


        Debug.Log(task);


        if (TestingTrials)
        {
            return;
        }

        if (task != "")
        {
            string path = Path.Combine(scheduleDirectory, task);
            readTextFile(path);

            string[] AllLines = File.ReadAllLines(path);

            List<TrialGoal> readTrials = new List<TrialGoal>();

            for (int i = 1; i < AllLines.Length; i++)
            {
                int targetArm = int.Parse(AllLines[i].Split(',')[1]);
                int lureArm = int.Parse(AllLines[i].Split(',')[2]);

                //Sanity Checks
                if (gameManager.Arms_27) {
                    if (targetArm == 0 ||  targetArm > 27) {
                        Debug.Log("Arm out of range");
                        continue;
                    }
                    if (lureArm == 0 ||  lureArm > 27) {
                        Debug.Log("Arm out of range");
                        continue;
                    }
                }

                if (!gameManager.Arms_27) {
                    if (targetArm == 0 ||  targetArm > 14) {
                        Debug.Log("Arm out of range");
                        continue;
                    }
                    if (targetArm == 0 ||  targetArm > 14) {
                        Debug.Log("Arm out of range");
                        continue;
                    }
                }

                if (gameManager.SPA_14) {
                    if (targetArm == 0 ||  targetArm > 14) {
                        Debug.Log("Arm out of range");
                        continue;
                    }
                    if (targetArm == 0 ||  targetArm > 14) {
                        Debug.Log("Arm out of range");
                        continue;
                    }
                }

                TrialGoal t = new TrialGoal
                {
                    WrongArm = lureArm,
                    CorrectArm = targetArm
                };
                readTrials.Add(t);
            }

            Trials = readTrials.ToArray();
        }


    }


    string curString;
    void readTextFile(string file_path)
    {
        if (File.Exists(file_path))
        {
            StreamReader inp_stm = new StreamReader(file_path);

            while (!inp_stm.EndOfStream)
            {
                string text = inp_stm.ReadLine();
                curString = text;
            }

            inp_stm.Close();
        }
        else
        {
            Debug.Log("Not exist");
        }

    }
}
