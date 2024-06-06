using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LogOutput : MonoBehaviour
{
    public InputField OutputField;
    public InputField SubjectIDField;
    public InputField SessionField;
    public InputField DateField;


    public GameManager gameManager;
    public GoalManager goalManager;

    //log
    string ScaleVersion;
    string ArmsNumberVersion;
    string PartitionVersion;

    private string directory;
    private string fileName;
    string taskName;
    string LogPath;

    //trial log
    string LogTrialPath;

    public static string markerString;
    private string textToWrite;
    private string textToWrite_trial;

    //Popup if path exists
    public GameObject Popup;

    private void Start()
    {
        OutputField.text = Application.persistentDataPath;
        goalManager = FindObjectOfType<GoalManager>();
    }

    public void OpenPathDirectory()
    {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        string current = (string)OutputField.text;
        string[] selected = SFB.StandaloneFileBrowser.OpenFolderPanel("Select data directory", current, false);
#else
            Utilities.UXFDebugLogError("Cannot select directory unless on PC platform!");
#endif
    }


    public void SetDirectory()
    {


        directory = OutputField.text + "/Trial Logging Build " + goalManager.buildNumber;


        ArmsNumberVersion = "";
        if (gameManager.Arms_27)
        {
            ArmsNumberVersion = "27 Arms";
        }
        if (!gameManager.Arms_27 || gameManager.SPA_14)
        {
            ArmsNumberVersion = "14 Arms";
        }

        PartitionVersion = "";
        if (gameManager.Partitions)
        {
            PartitionVersion = "Normal Arms Version";
        }
        if (gameManager.NoPartitions)
        {
            PartitionVersion = "No Arms Version";
        }

        ScaleVersion = "";
        Debug.Log(gameManager.NewScaleVersion);
        if (gameManager.OriginalScaleVersion)
        {
            ScaleVersion = "Original Scale";
        }
        if (gameManager.NewScaleVersion)
        {
            ScaleVersion = "New Scale";
        }


        taskName = "";

        if (gameManager.BaseLineTask)
        {
            taskName = "Baseline";
        }
        if (gameManager.FullTask)
        {
            taskName = "Full Task";
        }

        if (gameManager.VerbalTraining)
        {
            taskName = "Verbal Task Training";
        }
        if (gameManager.NonVerbalTraining)
        {
            taskName = "Non Verbal Task Training";
        }
        if (gameManager.Practice)
        {
            taskName = "Practice";
        }

        if (gameManager.Task_SPA)
        {
            taskName = "SPA Task";
        }
        if (gameManager.Task_SPS)
        {
            taskName = "SPS Task";
        }
        if (gameManager.Task_SPS && gameManager.VerbalTraining)
        {
            taskName = "SPS Verbal Training";
        }
        if (gameManager.Task_SPS && gameManager.NonVerbalTraining)
        {
            taskName = "SPS Non Verbal Training";
        }

        if (gameManager.Task_SPS && gameManager.Practice)
        {
            taskName = "SPS Practice";
        }

        string directoryPath = "";

            directoryPath = Path.Combine(directory, ScaleVersion, PartitionVersion, ArmsNumberVersion, taskName);
            Debug.Log(directoryPath);


        directory = directoryPath;
        string subfolderName = SubjectIDField.text + "_" + SessionField.text + "_" + DateField.text + "_Recording";
        TrialRecorder.instance.filePath = Path.Combine(directoryPath, subfolderName);
        TrialRecorder.instance.InitializeRecorder();

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        fileName = SubjectIDField.text + "_" + SessionField.text + "_" + DateField.text + "_Log" + ".csv";
        string path = Path.Combine(directory, fileName);
        LogPath = path;

        if (File.Exists(LogPath))
        {
            Popup.SetActive(true);
        }
        else
        {
            SetDirectory2();
            gameManager.SetUpAndStart();
        }
    }

    public void SetDirectory2()
    {
        textToWrite = "trial_num,target_arm,lure_arm,subject_response,sample_phase_durationm,sample_phase_choice_duration,test_phase_duration,test_phase_choice_duration";


        if (gameManager.Task_SPA)
        {
            textToWrite = "trial_num,target_arm,lure_arm,subject_response,target_error,sample_phase_durationm,sample_phase_choice_duration,test_phase_duration,test_phase_choice_duration";
        }
        File.AppendAllText(LogPath, textToWrite + "\n");
    }

    public void EndTrial()
    {


        string testNumber = gameManager.currentTestNumber.ToString();
        TrialGoal t = gameManager.goalmanager.Trials[gameManager.currentTestNumber - 1];
        string armCorrect = t.CorrectArm.ToString();
        string armWrong = t.WrongArm.ToString();

        string correct = "0";
        if (gameManager.gotCorrect)
        {
            correct = "1";
        }
        string timelearning = gameManager.TimeTakenLearningPhase.ToString();
        string timetest = gameManager.TimeTakenTestPhase.ToString();

        string timelearninggoal = gameManager.TimeTakenLearningPhaseGoal.ToString();
        string timetestgoal = gameManager.TimeTakenTestPhaseGoal.ToString();

        textToWrite = testNumber + "," + armCorrect + "," + armWrong + "," + correct + "," + timelearning + "," + timelearninggoal + "," + timetest + "," + timetestgoal;



        if (gameManager.Task_SPA)
        {
            string distanceFromArm = gameManager.DistanceFromGoal.ToString();

            textToWrite = testNumber + "," + armCorrect + "," + armWrong + "," + correct + "," + distanceFromArm + ","
                + timelearning + "," + timelearninggoal + "," + timetest + "," + timetestgoal;
        }

        File.AppendAllText(LogPath, textToWrite + "\n");
    }



    bool startWritingTrialData;
    public void SetDirectoryPerTrial()
    {
        //add a subfolder for each session for all trials
        string subfolderName = SubjectIDField.text + "_" + SessionField.text + "_" + DateField.text + "_Trials";

        string directoryPath = Path.Combine(directory, subfolderName);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string fileName2 = "Trial_" + gameManager.currentTestNumber.ToString() + ".csv";
        string path = Path.Combine(directoryPath, fileName2);
        LogTrialPath = path;

        textToWrite_trial = "time,posX,posZ,event_type,phase,input,trigger,marker";

        File.AppendAllText(LogTrialPath, textToWrite_trial + "\n");

        startWritingTrialData = true;
    }

    private void Update()
    {
        if (startWritingTrialData)
        {
            float time = gameManager.timeElapsed;
            float posX = gameManager.Player.transform.position.x;
            float posZ = gameManager.Player.transform.position.z;
            string event_data = gameManager.currentEvent;
            string trigger = "";
            string marker = "";

            string phase = "Sample";
            if (gameManager.IsTestPhase)
            {
                phase = "Test";
            }

            string input = "";
            PlayerInput p = gameManager.fpController.PlayerInput;
            if (p.actions["Start"].IsPressed())
            {
                input += "_PressedStart";
            }
            if (p.actions["Yes"].IsPressed())
            {
                input += "_ChooseGoal";
            }
            if (gameManager.fpController.movementInput.y > 0.5f)
            {
                input += "_Moving Forward";
            }
            if (gameManager.fpController.movementInput.y < -0.5f)
            {
                input += "_Moving Backward";
            }
            if (gameManager.fpController.movementInput.x > 0.5f)
            {
                input += "_Moving Right";
            }
            if (gameManager.fpController.movementInput.x < -0.5f)
            {
                input += "_Moving Left";
            }
            if (gameManager.fpController.rotationInput.y > 0.5f)
            {
                input += "_Rotating Up";
            }
            if (gameManager.fpController.rotationInput.y < -0.5f)
            {
                input += "_Rotating Down";
            }
            if (gameManager.fpController.rotationInput.x > 0.5f)
            {
                input += "_Rotating Right";
            }
            if (gameManager.fpController.rotationInput.x < -0.5f)
            {
                input += "_Rotating Left";
            }
            if (TriggersManager.instance.hasSentTrigger == true) {
                int id = BitConverter.ToInt16(TriggersManager.instance.sentTrigger, 0);
                trigger = id.ToString();
                Debug.Log("Writing " + id + " to output");
                TriggersManager.instance.hasSentTrigger = false;
            }
            if (markerString != null) {
                marker = markerString;
                markerString = null;
            }

            //textToWrite_trial = "time,posX,posZ,event,phase,input";

            textToWrite_trial = time + "," + posX + "," + posZ + "," + event_data + ","
                + phase + "," + input + "," + trigger + "," + marker;

            File.AppendAllText(LogTrialPath, textToWrite_trial + "\n");
            //Debug.Log(textToWrite_trial);
        }
    }
}
