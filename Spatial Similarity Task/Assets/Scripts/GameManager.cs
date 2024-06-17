using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UXFGameManager UXFManager;


    [Header("GAME VERSION----------------------------------")]
    public bool OriginalScaleVersion; //arms and button goall
    public bool NewScaleVersion; //arms and button goall

    public bool Partitions; //visible glass arm
    public bool NoPartitions; //no glass arm

    public bool Arms_27; // if current task has 27 arms or not
    public bool SPA_14;

    public GameObject UXFCanvas;
    public GameObject SetupPort;
    public GameObject SetupTimings;
    public Image[] imageColorList2;
    public RawImage[] imageColorList; // images in the UI to change color

    public Color VersionColor;

    [Header("Task Setup  ----------------------------------")]
    public bool BaseLineTask;
    public bool NonVerbalTraining;

    public bool VerbalTraining;
    public bool Practice;

    public bool FullTask;

    public bool SeperationsPilotTask;
    public bool IsAlternateSeperationsPilot;

    public bool Task_SPS;
    public bool Task_SPA;



    [Header("Arm and Maze Setup  ----------------------------------")]

    public GameObject[] ArmGoalPositions; //in order from 1 to 14
    public GameObject[] ArmGoalPositions_27; //in order from 1 to 27

    public GameObject GoalObject;
    public GameObject GoalObjectSPA;
    public GameObject FoilObject;

    public GameObject GoalPointPosition;


    public GameObject Player;
    public RigidbodyFirstPersonController fpController;


    [Header("MAZE SETUP ------------------------------------------------------")]
    public MeshRenderer OriginalArm;
    public GameObject WaterArm;
    public GameObject GlassArm_14;
    public GameObject GlassArm_27;

    public MeshRenderer Walls;



    [Header("Landmarks ----------------------------------")]

    public bool DisableLandmarks;

    public GameObject LandmarkAll;
    public GameObject[] LandmarkPrefabs;
    public GameObject[] LandmarkPositions;

    public int Landmark_Amount;

    private List<GameObject> LandmarkObjects = new List<GameObject>();
    private List<int> used_index = new List<int>();


    [Header("Score  ----------------------------------")]
    public Text scoreText;
    public GameObject ScoreGameobject;

    [Header("Level animator   ----------------------------------")]
    public Text LevelText;
    public GameObject LevelObject;

    [Header("BReak Screen   ----------------------------------")]
    public bool IsBreak;
    bool BreakDone;
    public GameObject BreakScreen;


    [Header("Goal Interact Text ----------------------------------")]
    public GameObject InteractWithGoalText;
    public Text InteractText;

    public List<GameObject> CurrentGoalsNearby = new List<GameObject>();

    [Header("QA Screen   ----------------------------------")]
    public GameObject QA_Screen;
    public GameObject QA_Arrow;
    public GameObject[] QA_Positions;

    public bool QAOpen;
    float selectTimer;
    int curArrowIndex;
    [Header("Guide Text  ----------------------------------")]
    public GameObject FixationScreen;

    public Text GuideText;
    public GameObject GuidePanel;

    public Text GuideTextMiddle;

    public Text TransitionText;
    public GameObject TransitionPanel;

    public GameObject PressButtonText;

    private float countdownTemp;

    [Header("Guide Floor Arrow ----------------------------------")]

    public GameObject[] GuideArrowFloor;

    public GameObject GuideArrowPivot;
    public GameObject MainArrow;

    public GameObject GuideArrowFloor_TestPhase;
    public GameObject GuideArrowFloor2_TestPhase;


    public bool TestPhaseGuide;
    public float PlayerStopMoveTimer;



    [Header("Free Move Task----------------------------------")]
    public GameObject FreeMoveTaskSpawn;



    [Header("Spawns ----------------------------------")]
    //Task Setup
    public GameObject Task3Spawn;

    [Header("Test Setup ----------------------------------")]

    //Number of tests and goal arm placement
    public GoalManager goalmanager;

    public int NumberOfTests;

    public int currentTestNumber;

    public DoorTrigger EntranceTrigger; //trigger of the entrance, the end of the starting hallway



    [Header("Mini Virtual Fan Arena Task ----------------------------------")]
    public float DistanceFromGoal;

    [Header("States ----------------------------------")]
    public bool HasStarted;
    public bool InWaitingScreen;
    //General
    public bool IsPaused;

    public bool IsLearningPhase;
    public bool IsTestPhase;

    public bool GoalCollected_Returning;

    private int testsCorrect;
    private Vector3 playerInitialPos;

    private GoalObject currentGoalObject;
    private int correctGoalArmIndex;
    private int foilIndex;

    public List<GameObject> testobjects = new List<GameObject>();


    [Header("Timer/Log ----------------------------------")]

    public float CountdownTimer;
    bool hasReachedCountdown;

    private LogOutput log;

    public float TimeTakenLearningPhase; //from when door opens to going back to start
    public float TimeTakenTestPhase; //same but in trial phase
    public float TimeTakenLearningPhaseGoal; //from when door opens to when goal is chosen
    public float TimeTakenTestPhaseGoal;

    public bool startTimer;
    public bool gotCorrect;

    public int arm_chosen;

    //per trial data
    public float timeElapsed;
    public string currentEvent;

    [Header("Time Settings ----------------------------------")]
    public float trialLength = 45f;
    public float learningPhaseBoxLength = 2f;
    public float testingPhaseBoxLength = 4f;
    public float delayLength = 0.25f;
    public float fixationLength = 0.5f;

    [Header("Trigger Codes ----------------------------------")]
    public byte[] startTrigger;
    public byte[] endTrigger;
    public byte[] selectTrigger;
    public byte[] syncTrigger;

    private float syncTime = 2f;
    private float syncTimer = 0f;

    [Header("hidden states ----------------------------------")]
    bool SkipTestPhaseReturn;
    public bool timeoutSkip = false;
    public bool markerUsed = false;
    //public bool 

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        log = FindObjectOfType<LogOutput>();
        fpController = Player.GetComponent<RigidbodyFirstPersonController>();
        Time.timeScale = 0f;
        fpController.mouseLook.SetCursorLock(false);

        hasReachedCountdown = true;
        StartUI();
        SetupColor();
    }


    void SetupColor()
    {
        imageColorList2 = UXFCanvas.GetComponentsInChildren<Image>();
        imageColorList = UXFCanvas.GetComponentsInChildren<RawImage>();

        foreach (Image i in imageColorList2)
        {
            if (i.sprite == null)
            {

                i.color = VersionColor;
            }
            
        }

        foreach (RawImage i in imageColorList)
        {
            if (i.texture == null)
            {
                i.color = VersionColor;
            }
        }

        imageColorList2 = SetupPort.GetComponentsInChildren<Image>();
        imageColorList = SetupPort.GetComponentsInChildren<RawImage>();

        foreach (Image i in imageColorList2)
        {
            if (i.sprite == null)
            {

                i.color = Color.white;
            }
            
        }

        foreach (RawImage i in imageColorList)
        {
            if (i.texture == null)
            {
                i.color = Color.white;
            }
        }

        imageColorList2 = SetupTimings.GetComponentsInChildren<Image>();
        imageColorList = SetupTimings.GetComponentsInChildren<RawImage>();

        foreach (Image i in imageColorList2)
        {
            if (i.sprite == null)
            {

                i.color = Color.white;
            }
            
        }

        foreach (RawImage i in imageColorList)
        {
            if (i.texture == null)
            {
                i.color = Color.white;
            }
        }

        SetupTimings.GetComponent<Image>().color = Color.grey;
        SetupPort.GetComponent<Image>().color = Color.grey;
    }

    public void SetUpAndStart() //Called by UXFManager
    {
        Time.timeScale = 1f;

        fpController.mouseLook.SetCursorLock(true);
        fpController.CanRotate = false;

        CloseInteractWithGoalPrompt();
        ActivateTestPhaseFloorArrows(false);
        PlayerStopMoveTimer = 10f;


        //Disable start screen
        UXFCanvas.SetActive(false);


        //Initial Setup
        LandmarkAll.SetActive(true);

        //Task Habituation Setup
        if (BaseLineTask) // Set up by UXF manager
        {
            LandmarkAll.SetActive(false); // No landmarks in free task
            Player.transform.position = FreeMoveTaskSpawn.transform.position;
            ActivateFloorArrows(false,false,false);
            //Opens the maze door and stop the trigger callbacks
            EntranceTrigger.ManuallyCloseDoor();
            EntranceTrigger.Deactivate();

        }



        //Full Task and Learning Task Setup
        if (FullTask || NonVerbalTraining || VerbalTraining || SeperationsPilotTask)
        {
            EntranceTrigger.Deactivate();

            ActivateFloorArrows(false,false,false); // disable arrows first

            Player.transform.position = Task3Spawn.transform.position;

            playerInitialPos = Player.transform.position;

            testsCorrect = 0;
        }



        //MINI TASKS and arm SETUP
        OriginalArm.enabled = false;
        OriginalArm.gameObject.SetActive(false);

        WaterArm.SetActive(false);

        GlassArm_14.SetActive(false);
        GlassArm_27.SetActive(false);

        Walls.enabled = true;
        Walls.gameObject.SetActive(true);

        if (BaseLineTask)
        {
            Walls.enabled = true;
        }

        if (NonVerbalTraining || VerbalTraining || FullTask)
        {
            if (Partitions)
            {
                if(!Arms_27)
                {
                    GlassArm_14.SetActive(true);
                }
                if (Arms_27)
                {
                    GlassArm_27.SetActive(true);
                }

                Walls.enabled = true;
            }

            if (NoPartitions)
            {
                GlassArm_14.SetActive(false);
                GlassArm_27.SetActive(false);

                Walls.enabled = false;
            }
        }




        //New Setup
        SkipTestPhaseReturn = true;




        DialogueStartScreen();
    }

    void DialogueStartScreen()
    {
        //Starts with a blank scrreen and then shows please wait we are about to begin
        OpenTransition("");
        Invoke("DialogueStartScreen2", .25f);
    }
    void DialogueStartScreen2()
    {

        InWaitingScreen = true;
        OpenTransition("Please wait we are about to begin");
        currentEvent = "Waiting Screen";

        if (BaseLineTask)
        {
            LevelAnimationStart("0");
        }
        if (NonVerbalTraining)
        {
            LevelAnimationStart("1a");
        }
        if (VerbalTraining)
        {
            LevelAnimationStart("1b");
        }

        if (Practice)
        {
            LevelAnimationStart("2");
        }

        if (FullTask)
        {
            LevelAnimationStart("3");
        }

        if (SeperationsPilotTask)
        {
            LevelAnimationStart("3a");

            if(IsAlternateSeperationsPilot)
            {
                LevelAnimationStart("3b");
            }
        }

        if(Task_SPS)
        {
            LevelAnimationStart("5");
        }
        if (Task_SPS && NonVerbalTraining)
        {
            LevelAnimationStart("5a");
        }
        if (Task_SPS && VerbalTraining)
        {
            LevelAnimationStart("5b");
        }
        if (Task_SPS && Practice)
        {
            LevelAnimationStart("5c");
        }

        if (Task_SPA)
        {
            LevelAnimationStart("6");
        }
    }

    // Update is called once per frame
    void Update()
    {
        SmoothRotateUpdate();

        timeElapsed += Time.deltaTime;

        //Start the level 
        if (fpController.PlayerInput.actions["Start"].WasPressedThisFrame() && InWaitingScreen && !HasStarted)
        {

            currentEvent = "Performing Task";

            InWaitingScreen = false;
            HasStarted = true;
            //Play level index text then fixation screen then dialogue
            CloseTransition();
            LevelAnimationFinish();

            OpenFixationScreen(); //2 seconds

            if (BaseLineTask)
            {
               Invoke("FreeTaskDialogue",3f);
            }
            else
            {
                StartLearningPhase(1f);
            }
        }

        /*
        if (syncTimer > 0) {
            syncTimer -= Time.deltaTime;
            if (syncTimer < 0) {
                TriggersManager.instance.SendTrigger(syncTrigger);
                Debug.Log("Sync Trigger Sent");
            }
        }
        */


        //Collect Goal
        if(fpController.PlayerInput.actions["Yes"].WasPressedThisFrame() && CurrentGoalsNearby.Count > 0)
        {

            Debug.Log(CurrentGoalsNearby.Count);
            GameObject g = GetClosestFoal(CurrentGoalsNearby.ToArray());

            g.GetComponent<GoalObject>().Collect();
        }

        if(fpController.PlayerInput.actions["Speed"].WasPressedThisFrame())
        {
            Time.timeScale = 3;
            Time.fixedDeltaTime = 3 * 0.02f;
        }


            if (HasStarted) //Skip all the following code if the game has not started yet (to prevent these getting pressed when typing participant names)
        {

            if (fpController.PlayerInput.actions["Skip"].WasPressedThisFrame())
            {
                GoalCollected_Returning = true;
                IsLearningPhase = false;
                IsTestPhase = true;

                CancelInvoke();
                ReturnedToStart();

            }

            if (fpController.PlayerInput.actions["SkipOne"].WasPressedThisFrame())
            {
                GoalCollected_Returning = true;
                CancelInvoke();
                ReturnedToStart();
            }

            //Reset 
            if (fpController.PlayerInput.actions["Escape"].WasPressedThisFrame())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }


            if (IsPaused && fpController.PlayerInput.actions["Start"].WasPressedThisFrame() && !fpController.PlayerInput.actions["SkipOne"].WasPressedThisFrame())
            {
                CloseGuide();
                UnpauseGame();
            }


         
        }



        //scorecounter
        scoreText.text = "Score : " + testsCorrect.ToString();
        ScoreGameobject.SetActive(true);

        if (NonVerbalTraining || VerbalTraining || BaseLineTask)
        {
            scoreText.text = ""; //Dont show score in a learning block and practice block
            ScoreGameobject.SetActive(false);
        }


        //floor arrows

        if (NonVerbalTraining)
        {
            if (1 <= currentTestNumber && currentTestNumber <= 4) 
            {
                if (IsLearningPhase)
                {
                    ActivateFloorArrows(true, true, true);
                }
                if (IsTestPhase)
                {
                    ActivateFloorArrows(true, true, true);
                }
            }
            if (5 <= currentTestNumber)
            {
                if (IsLearningPhase)
                {
                    ActivateFloorArrows(true, true, true);
                }
                if (IsTestPhase)
                {
                    ActivateFloorArrows(true, false, true);
                }
            }
        }


        if (VerbalTraining)
        {
            if (IsLearningPhase)
            {
                ActivateFloorArrows(true, true, true);
            }
            if (IsTestPhase)
            {
                ActivateFloorArrows(true, false, true);
            }
        }
        if (FullTask || SeperationsPilotTask)
        {
            if (IsLearningPhase)
            {
                ActivateFloorArrows(false, false, false);
            }
            if (IsTestPhase)
            {
                ActivateFloorArrows(false, false, false);
            }
        }

        //Floor arrows in test phase for non verbal

        /*
        if (NonVerbalTraining && EntranceTrigger.Opened && IsTestPhase)
        {
            if (fpController.relativeMovementUpdate.magnitude < 0.1f)
            {
                PlayerStopMoveTimer -= Time.deltaTime;
            }
            else
            {
                PlayerStopMoveTimer = 10f;
            }

            if(PlayerStopMoveTimer < 0)
            {
                ActivateTestPhaseFloorArrows(true);
            }
        }*/





        //Break screen for certain tasks when reaching half points
        if (FullTask && !Practice)
        {
            if (currentTestNumber == goalmanager.Trials.Length/2)
            {
                Debug.Log("BREAK");
                if (!IsBreak && !BreakDone)
                {
                    IsBreak = true;
                    OpenBreakScreen();
                    PauseGame();
                }
            }
        }

        //Continue the game when the player presses any button
        if (IsBreak && fpController.PlayerInput.actions["Start"].WasPerformedThisFrame())
        {
            IsBreak = false;
            BreakDone = true;
            CloseBreakScreen();
            UnpauseGame();
        }


        //QA SCREEN

        selectTimer -= Time.deltaTime; //Cooldown for moving the arrows
        if(QAOpen)
        {
            if (fpController.movementInput.y < -0.5 && selectTimer < -0.2f)
            {
                selectTimer = 0f;
                curArrowIndex++;
                curArrowIndex = Mathf.Clamp(curArrowIndex, 0, QA_Positions.Length-1);
                QA_Arrow.transform.position = QA_Positions[curArrowIndex].transform.position;
            }
            if (fpController.movementInput.y > 0.5 && selectTimer < -0.2f)
            {
                selectTimer = 0f;
                curArrowIndex--;
                curArrowIndex = Mathf.Clamp(curArrowIndex, 0, QA_Positions.Length - 1);
                QA_Arrow.transform.position = QA_Positions[curArrowIndex].transform.position;

            }
        }

        //Pause
        if (fpController.PlayerInput.actions["Pause"].WasPressedThisFrame())
        {
            IsBreak = true;
            OpenBreakScreen();
            PauseGame();
        }

        //Timer

        if (startTimer && IsLearningPhase)
        {
            TimeTakenLearningPhase += Time.deltaTime;
            if(!GoalCollected_Returning)
            {
                TimeTakenLearningPhaseGoal += Time.deltaTime;
            }
        }
        
        if (startTimer && IsTestPhase)
        {
            TimeTakenTestPhase += Time.deltaTime;
            if (!GoalCollected_Returning)
            {
                TimeTakenTestPhaseGoal += Time.deltaTime;
            }
        }


        if(FullTask)
        {
            if (!GoalCollected_Returning)
            {
                CountdownTimer -= Time.deltaTime;
            }

            if(CountdownTimer < 0 && !hasReachedCountdown  && !GoalCollected_Returning)
            {
                hasReachedCountdown = true;
                timeoutSkip = true;

                if (CurrentGoalsNearby.Count != 0) {
                    CloseInteractWithGoalPrompt();
                    CurrentGoalsNearby.Clear();
                }

                if (IsLearningPhase)
                {
                    GoalCollected(true);
                    OpenGuidePause("Sorry, you ran out of time");

                    GoalCollected_Returning = true;
                    IsLearningPhase = false;
                    IsTestPhase = true;

                    CancelInvoke();                   

                    Invoke("ReturnedToStart", 0.5f);
                }

                if (IsTestPhase)
                {
                    GoalCollected(false);

                    OpenGuidePause("Sorry, you ran out of time");

                    Invoke("ReturnedToStart", 0.5f);
                }
            }
        }
    }





    //TASK 3 Specific Functions---------------------------------------------------------------------------------------------------------
    #region
    void StartTestPhase(float dialogueTimer)
    {
        markerUsed = false;
        TriggersManager.instance.SendTrigger(startTrigger);
        Debug.Log("Start trigger sent");
        //spawn a second wrong goal nearby the first, we also dont set the goalobject.correctgoal to true for this as it is a foil
        if (!Task_SPA)
        {
            GameObject g = Instantiate(GoalObject);
            //check if islearningtask
            if (NonVerbalTraining || VerbalTraining)
            {
                g.GetComponent<GoalObject>().AllowGoalCorrection = true;
            }

            foilIndex = goalmanager.Trials[currentTestNumber - 1].WrongArm;

            if (Arms_27)
            {
                g.transform.position = ArmGoalPositions_27[foilIndex-1].transform.position;
            }
            else
            {
                g.transform.position = ArmGoalPositions[foilIndex-1].transform.position;
            }
            //add foil object to list of all objects
            FoilObject = g;
            testobjects.Add(g);
        }
        if(Task_SPA) //spawn all foil arms except the correct arm position
        {
            if (!SPA_14) {
                for(int i = 0; i < ArmGoalPositions_27.Length; i++)
                {
                    if(i != correctGoalArmIndex-1)
                    {
                        GameObject g = Instantiate(GoalObject);
                        g.transform.position = ArmGoalPositions_27[i].transform.position;
                        //add foil object to list of all objects
                        testobjects.Add(g);
                    }
                }
            } 
            else {
                for(int i = 0; i < ArmGoalPositions.Length; i++)
                {
                    if(i != correctGoalArmIndex-1)
                    {
                        GameObject g = Instantiate(GoalObjectSPA);
                        g.transform.position = ArmGoalPositions[i].transform.position;
                        //add foil object to list of all objects
                        testobjects.Add(g);
                    }
                }
            }

            GoalObject[] g2 = FindObjectsOfType<GoalObject>();
            foreach(GoalObject g3 in g2) //turn all goals invisible
            {
                g3.DisableMesh();
            }
        }



        //Unreverse the floor arrows
        ReverseFloorArrows(false);


        //Reset the current goal so it can be collected again
        currentGoalObject.ResetCollect();

        //Countdown
        CountdownTimer = trialLength;
        hasReachedCountdown = false;

        IsTestPhase = true;
        IsLearningPhase = false;

        EntranceTrigger.ManuallyCloseDoor();



       
        if(VerbalTraining)
        {
            if (currentTestNumber == 1)
            {
                Invoke("VerbalDialogue_TestPhase", dialogueTimer); //Info for first time in test phase
                Invoke("CountdownTestPhase", dialogueTimer + testingPhaseBoxLength);
            }
            else
            {
                Invoke("CountdownTestPhase", dialogueTimer + testingPhaseBoxLength);
            }
        }
        if(NonVerbalTraining)
        {
            Invoke("CountdownTestPhase", dialogueTimer + testingPhaseBoxLength);
        }
        if(FullTask)
        {
            Invoke("CountdownTestPhase", dialogueTimer + testingPhaseBoxLength);
        }
    }

    void CountdownTestPhase()
    {

        //OpenGuideMiddle("Get Ready!");
        countdownTemp = 3f;
        Invoke("CountDownGuide", 0f); //Starts a countdown starting at 3 in 3 seconds

        Invoke("OpenDoor", 3f);
    }


    void StartLearningPhase()
    {
        StartLearningPhase(1f);
    }
    void StartLearningPhase(float dialogueStartTimer)
    {
        currentEvent = "Performing Task";
        markerUsed = false;

        //LOG STUFF
        if (currentTestNumber > 0f) //ends trial only after the first ends
        {
            if (currentTestNumber == NumberOfTests) //finish the task
            {
                TrialRecorder.instance.StopRecording();
                FinishTask();
                return;
            }
        }

        currentTestNumber++;
        //TrialRecorder.instance.StartRecording(currentTestNumber.ToString());
        TriggersManager.instance.SendTrigger(startTrigger);
        Debug.Log("Start trigger sent");
        syncTimer = syncTime;
        //Per trial logging
        log.SetDirectoryPerTrial();
        timeElapsed = 0f;
        ///////


        //Destroy old ones and Spawn in new landmarks
        if (!DisableLandmarks)
        {
            foreach (GameObject obj in LandmarkObjects)
            {
                Destroy(obj);
            }
            LandmarkObjects.Clear();
            used_index.Clear();

            for (int i = 0; i < Landmark_Amount; i++) //spawn 4 landmarks
            {
                GameObject landmark = Instantiate(LandmarkPrefabs[Random.Range(0, LandmarkPrefabs.Length)]);

                int pos_index = Random.Range(0, LandmarkPositions.Length);

                int limit = 0;
                while (used_index.Contains(pos_index) || used_index.Contains(pos_index + 1) || used_index.Contains(pos_index - 1)) //randomise again if the landmark position has been used
                {
                    pos_index = Random.Range(0, LandmarkPositions.Length);
                    limit++;
                }

                used_index.Add(pos_index);
                landmark.transform.position = LandmarkPositions[pos_index].transform.position;

                LandmarkObjects.Add(landmark);
            }
        }

        //delete all foil objects and goals
        foreach (GameObject obj in testobjects)
        {
            Destroy(obj);
        }
        testobjects.Clear();

        //spawn in the goal at a random arm
        GameObject g;
        if (Task_SPA && SPA_14) {
            g = Instantiate(GoalObjectSPA);
        }
        else {
            g = Instantiate(GoalObject);
        }

        currentGoalObject = g.GetComponent<GoalObject>();
        currentGoalObject.CorrectGoal = true;

        correctGoalArmIndex = goalmanager.Trials[currentTestNumber - 1].CorrectArm;

        if (Arms_27 && !SPA_14)
        {
            g.transform.position = ArmGoalPositions_27[correctGoalArmIndex-1].transform.position;
            //g.transform.rotation = ArmGoalPositions_27[correctGoalArmIndex - 1].transform.rotation;
        }
        else
        {
            g.transform.position = ArmGoalPositions[correctGoalArmIndex-1].transform.position;
            //g.transform.rotation = ArmGoalPositions[correctGoalArmIndex - 1].transform.rotation;
        }

        testobjects.Add(g);






        //Countdown
        CountdownTimer = trialLength;
        hasReachedCountdown = false;


        IsLearningPhase = true;
        IsTestPhase = false;


        EntranceTrigger.ManuallyCloseDoor();

        ReverseFloorArrows(false);
        RotateFloorArrows(currentGoalObject.transform.position);

        if(Task_SPA && currentTestNumber == 1)
        {
            Invoke("SPATaskDialogue", 0.6f);
            Invoke("FullTask_Learning_Dialogue", 1f);
            return;
        }

        if (FullTask)
        {
            FullTask_Learning_Dialogue(); //Play the guide text full task
        }
        else
        {
            Invoke("DialogueLearningPhase", 0.5f);
        }
    }

    void DialogueLearningPhase()
    {
        if (NonVerbalTraining)
        {
            NonVerbal_Learning_Dialogue(); //Play the guide text for non verbal
        }
        if (VerbalTraining)
        {
            Verbal_Learning_Dialogue(); //Play the guide text for verbal
        }
    }


    void GetFoilGoalIndex()
    {
        int i = correctGoalArmIndex + Random.Range(-2, 3); //ensure foil goal is no longer than 2 away
        i = Mathf.Clamp(i, 0, 13);
        foilIndex = i;
    }

    public void GoalCollected(bool CorrectGoal)
    {
        Debug.Log("Goal");
        TriggersManager.instance.SendTrigger(selectTrigger);
        markerUsed = false;
        Debug.Log("Select trigger sent");

        //TASK 3
        if (FullTask || NonVerbalTraining || VerbalTraining || SeperationsPilotTask)
        {
            if (IsLearningPhase)
            {
                ReverseFloorArrows(true);
                if (!FullTask && !SeperationsPilotTask && !NonVerbalTraining)
                {
                    OpenGuidePause("Great work! Now you'll want to remember this location because you'll be tested on it right after, so go ahead and look at your surroundings to help you remember. Please look around at least once, then navigate back to the starting area.", 1f);
                    Invoke("CloseGuide", 7f);
                }

                if(NonVerbalTraining)
                {
                    SmoothRotate(2f,GuideArrowPivot.transform.position);
                }

                GoalCollected_Returning = true;
            }

            if (IsTestPhase)
            {
                //Get the distance of the player from the real goal on click. Ignoring difference of the y axis (height)
                if(Task_SPA)
                {
                    Vector3 pos = Player.transform.position;
                    Vector3 newPlayerpos = new Vector3(pos.x, 0, pos.z);
                    Vector3 pos2 = currentGoalObject.transform.position;
                    Vector3 newGoalPos = new Vector3(pos2.x, 0, pos2.z);

                    DistanceFromGoal = Vector3.Distance(newPlayerpos, newGoalPos); //used by logOutput
                }

                if (CorrectGoal)
                {
                    //TriggersManager.instance.SendTrigger(correctTrigger);
                    if (!NonVerbalTraining && !timeoutSkip)
                    {
                        OpenGuidePause("Well done, you chose correctly!", 0.1f);
                    }
                    Invoke("CloseGuide", 3f);

                    GoalCollected_Returning = true;

                    testsCorrect++;
                    gotCorrect = true;

                    if (SkipTestPhaseReturn)
                    {
                        Invoke("ReturnedToStart", 1f);
                    }
                }
                else
                {
                    //TriggersManager.instance.SendTrigger(wrongTrigger);
                    if (!FullTask && !SeperationsPilotTask)
                    {
                        OpenGuidePause("Oops, you chose incorrectly. Try again, choose another arm of the maze.", 0.1f);
                    }
                    else
                    {
                        if (!timeoutSkip) {
                            OpenGuidePause("Oops... That is incorrect.", 0.1f);
                        }

                        Invoke("CloseGuide", 3f);

                        gotCorrect = false;

                        if (SkipTestPhaseReturn)
                        {
                            Invoke("ReturnedToStart", 1f);
                        }
                    }


                    GoalCollected_Returning = true;
                    
                }
            }
        }
    }

    public void ReturnedToStart() //when arrived at start position after the task play the transition
    {
        if (GoalCollected_Returning)
        {
            GoalCollected_Returning = false;
            startTimer = false;
            if (IsLearningPhase)
            {
                Debug.Log("StartTest");
                TeleportSpawn();

                OpenTransition("", delayLength); //15
                StartTestPhase(0.25f);
                return;
            }
            if (IsTestPhase)
            {
                //Log at the end of the test phase
                FindObjectOfType<LogOutput>().EndTrial();
                gotCorrect = false;
                TimeTakenTestPhase = 0;
                TimeTakenLearningPhase = 0;
                TimeTakenLearningPhaseGoal = 0;
                TimeTakenTestPhaseGoal = 0;

  
                if (Practice && currentTestNumber == NumberOfTests)
                {
                    Debug.Log("AAA");
                    if(testsCorrect >= NumberOfTests/2)
                    {
                        OpenQAScreen();
                    }
                    else
                    {
                        OpenGuidePause("Let's do a little more practice before we move on.\r\n");
                    }

                    //At this point experimenter must press the skip button to go to the main menu again
                    return;
                }

                //TriggersManager.instance.SendTrigger(endTrigger);
                //Debug.Log("End trigger sent");

                //TrialRecorder.instance.StopRecording();
                Debug.Log("StartLearning");
                TeleportSpawn();
                OpenTransition("", delayLength); //1 is fine
                Invoke("OpenFixationScreen", delayLength);
                StartLearningPhase(1f); // Play dialogue in 1 second
                BreakDone = false;

                return;

            }
        }
    }


    //DIALOGUE------------------------------------------------------------------------------------



    //SPA task
    void SPATaskDialogue()
    {
        OpenGuidePause("This task is similar to the last task, however, there is one important difference. In the first part of a trial, there will be an object at a particular location on the maze that you need to navigate to. In the second part of the trial, however, there will not be an object on the maze. You need to navigate back to that initial location in the absence of the object. Remember that each trial has a time limit, which is similar to the time limit in the previous task.");
    }




    //Dialogue starting
    #region
    public void FreeTaskDialogue()
    {
        OpenGuidePause("Welcome to the virtual maze! In this practice environment, we want you to become familiar with the environment and surroundings and become comfortable navigating using the controller. So go ahead and explore, walk around the maze, observe the scenery around you.  You can explore this environment as much and for as long as you like.");
        Invoke("FreeTaskDialogue2", 0.1f);
    }
    public void FreeTaskDialogue2()
    {
        OpenGuidePause("Let your experimenter know when you are satisfied with your knowledge of the environment and the use of the controller.");
    }
    public void Verbal_Learning_Dialogue()
    {
        if (currentTestNumber == 1)
        {
            VerbalTaskDialogue2();
        }
        else
        {
            VerbalTaskDialogue3();
        }
    }
    public void VerbalTaskDialogue2()
    {
        OpenGuidePause("This is the starting area, and you will begin each trial here. After a few seconds, the door will open, and you will see an object at one location on the maze. Follow the green arrows and navigate to the location of the object.");

        Invoke("VerbalTaskDialogue3", 1f);
    }
    public void VerbalTaskDialogue3()
    {
        OpenGuideMiddle("Get Ready!");
        Invoke("CloseGuideMiddle", 2f);
        Invoke("OpenDoor", learningPhaseBoxLength);

       // Invoke("VerbalTaskDialogue4", 3f);
    }
    public void VerbalTaskDialogue4()
    {
        OpenGuide("Follow the arrows to the goal");
        Invoke("CloseGuide", 10f);
    }

    void VerbalDialogue_TestPhase()
    {
        OpenGuidePause("When the door opens,  you'll see two identical objects in the maze. Return to the object location you chose previously in the first part of the trial.");
    }

    public void NonVerbal_Learning_Dialogue()
    {
       // OpenGuideMiddle("Get Ready!");
        Invoke("CloseGuideMiddle", 2f);
        Invoke("OpenDoor", learningPhaseBoxLength);
    }

    void FullTask_Learning_Dialogue()
    {
        OpenGuideMiddle("Get Ready!");
        Invoke("CloseGuideMiddle", 2f);
        Invoke("OpenDoor", learningPhaseBoxLength);
    }

    #endregion
    #endregion






    //General Use Functions-------------------------------------------------------------------------------------


    void OpenDoor() //usually means test starts
    {
        EntranceTrigger.ManuallyOpenDoor();
        startTimer = true;


        //Countdown
        CountdownTimer = trialLength;
        hasReachedCountdown = false;
        TriggersManager.instance.SendTrigger(syncTrigger);
        Debug.Log("Sync Trigger Sent");
    }

    void CountDownGuide()
    {
        if (countdownTemp >= 1f)
        {
            OpenGuideMiddle(((int)countdownTemp).ToString());
            countdownTemp--;
            Invoke("CountDownGuide", 1f);
        }
        else
        {
            CloseGuideMiddle();
        }
    }

    void OpenFixationScreen()
    {
        currentEvent = "Fixation Screen";

        FixationScreen.SetActive(true);  //Lasts 0.5 seconds
        Invoke("CloseFixationScreen", fixationLength);
    }
    void CloseFixationScreen()
    {
        currentEvent = "Performing Task";

        FixationScreen.SetActive(false);  //Lasts 0.5 seconds
        fpController.CanRotate = true;
    }


    void OpenBreakScreen()
    {
        currentEvent = "Break";

        BreakScreen.SetActive(true);  //Lasts 1 seconds
    }
    void CloseBreakScreen()
    {
        currentEvent = "Performing Task";

        BreakScreen.SetActive(false);  //Lasts 1 seconds
        fpController.CanRotate = true;
    }




    //OPEN QA SCREEN
    void OpenQAScreen()
    {
        QA_Screen.SetActive(true);
        QAOpen = true;
    }


    //teleports the player to the spawn with a smooth transition
    void TeleportSpawn()
    {
        Teleport();
    }
    void Teleport()
    {
        Player.transform.position = playerInitialPos;
        fpController.ChangeRotation(Quaternion.identity); //resets player rotation
        if (CurrentGoalsNearby.Count != 0) {
            CloseInteractWithGoalPrompt();
            CurrentGoalsNearby.Clear();
        }

        timeoutSkip = false;
    }


    //smoothly rotates the players camera 
    bool rotating;
    Quaternion curRot;
    Vector3 lookposTemp;
    float tLerp;
    float speedTemp;
    void SmoothRotate(float speed, Vector3 lookpos) //speed 1 is 1 second, 2 is 0.5 ...
    {
        curRot = Player.transform.rotation;
        rotating = true;
        lookposTemp = lookpos;
        speedTemp = speed;
    }
    void SmoothRotateUpdate()
    {
        Quaternion newRot = Quaternion.LookRotation(lookposTemp - Player.transform.position);
        Quaternion newrot2 = Quaternion.Euler(0, newRot.eulerAngles.y, 0);
        if (rotating)
        {
            tLerp += Time.deltaTime * speedTemp;
            Quaternion newrot3 = Quaternion.Lerp(curRot, newrot2, tLerp);
            fpController.ChangeRotation(newrot3); //resets player rotation
        }

        if (tLerp > 1)
        {
            rotating = false;
        }
    }

    //Controls the Guide Arrows on the Floor
    public void ActivateFloorArrows(bool ArrowActivate, bool PivotArrowActivate, bool MainArrowActivate)
    {
        if (ArrowActivate)
        {
            foreach (GameObject g in GuideArrowFloor)
            {
                g.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject g in GuideArrowFloor)
            {
                g.SetActive(false);
            }
        }

        if(PivotArrowActivate)
        {
            GuideArrowPivot.SetActive(true);
        }
        else
        {
            GuideArrowPivot.SetActive(false);
        }

        if(MainArrowActivate)
        {
            MainArrow.SetActive(true);
        }
        else
        {
            MainArrow.SetActive(false);
        }
    }

    public void RotateFloorArrows(Vector3 objectToPointTo)
    {
        Quaternion rot =
            Quaternion.LookRotation(objectToPointTo - GuideArrowPivot.transform.position);
        Quaternion rot2 = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        GuideArrowPivot.transform.rotation = rot2;
    }



    public void ReverseFloorArrows(bool Reversed)
    {
        if(Reversed)
        {
            foreach(GameObject g in GuideArrowFloor)
            {
                g.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
        }
        else
        {
            foreach (GameObject g in GuideArrowFloor)
            {
                g.transform.localRotation = Quaternion.Euler(-90, 0, 180);
            }
        }
     
    }


    //activates 2 floor arrows for test phase if player stands still for 10 seconds
    void ActivateTestPhaseFloorArrows(bool Activate)
    {
        if(Activate)
        {
            GuideArrowFloor_TestPhase.SetActive(true);
            GuideArrowFloor2_TestPhase.SetActive(true);
            TestPhaseGuide = true;

            Quaternion rot =
        Quaternion.LookRotation(currentGoalObject.transform.position - GuideArrowPivot.transform.position);
            Quaternion rot2 = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            GuideArrowFloor_TestPhase.transform.rotation = rot2;

             rot =
Quaternion.LookRotation(FoilObject.transform.position - GuideArrowPivot.transform.position);
             rot2 = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            GuideArrowFloor2_TestPhase.transform.rotation = rot2;
        }
        else
        {
            GuideArrowFloor_TestPhase.SetActive(false);
            GuideArrowFloor2_TestPhase.SetActive(false);
            TestPhaseGuide = false;
        }
    }







    //Pauses the game and can unpause with any key/Button
    void PauseGame()
    {
        Time.timeScale = 0.0001f;
        Time.fixedDeltaTime = 0f;
        fpController.CanRotate = false;
        IsPaused = true;
    }
    void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f * 0.02f;
        fpController.CanRotate = true;
    }
    //opens the guide prompt at the bottom of the screen

    void OpenGuide(string GuideText)
    {
        currentEvent = "Guide Prompt";

        CancelInvoke("CloseGuide");
        this.GuideText.text = GuideText;
    }
    void OpenGuidePause(string GuideText)
    {
        OpenGuide(GuideText);
        PressButtonText.SetActive(true);
        GuidePanel.SetActive(true);
        PauseGame();
    }


    string GuideTextTemp;
    void OpenGuidePause(string GuideText, float timer)
    {
        GuideTextTemp = GuideText;
        Invoke("OpenGuidePauseTimer", timer);
    }
    void OpenGuidePauseTimer()
    {
        OpenGuide(GuideTextTemp);
        PressButtonText.SetActive(true);
        GuidePanel.SetActive(true);
        PauseGame();
    }


    void CloseGuide()
    {
        currentEvent = "Performing Task";

        GuideText.text = "";
        GuidePanel.SetActive(false);
        PressButtonText.SetActive(false);
    }

    void OpenGuideMiddle(string text)
    {
        currentEvent = "Guide Prompt Middle";

        GuideTextMiddle.text = text;
    }
    void CloseGuideMiddle()
    {
        currentEvent = "Performing Task";

        GuideTextMiddle.text = "";
    }








    //opens the transition prompt for a set duration

    void OpenTransition(string text)
    {
        currentEvent = "Transition Screen";

        TransitionText.text = text;
        TransitionPanel.SetActive(true);
    }
    void OpenTransition(string text, float time)
    {
        OpenTransition(text);
        Invoke("CloseTransition", time);
    }
    void CloseTransition()
    {
        currentEvent = "Performing Task";

        TransitionText.text = "";
        TransitionPanel.SetActive(false);
    }

    public void OpenInteractWithGoalPrompt(string Prompt)
    {
        currentEvent = "Collect Goal Prompt";

        InteractWithGoalText.SetActive(true);
        InteractText.text = Prompt;
        //Debug.Log("Opening Goal Prompt");
    }
    public void CloseInteractWithGoalPrompt()
    {
        currentEvent = "Performing Task";
        InteractWithGoalText.SetActive(false);
        //Debug.Log("Closing Goal Prompt");
    }
    void LevelAnimation(string levelIndex, float time, float delay)
    {
        LevelText.text = "Level " + levelIndex;
        Invoke("LevelAnimationStart", delay);
        Invoke("LevelAnimationFinish", time + delay);
    }
    void LevelAnimationStart(string levelIndex)
    {
        LevelText.text = "Level " + levelIndex;
        LevelObject.SetActive(true);
    }
    void LevelAnimationFinish()
    {
        LevelObject.SetActive(false);
    }

    void StartUI()
    {
        LevelAnimationFinish();
        CloseTransition();
        CloseGuide();
        CloseGuideMiddle();
        PressButtonText.SetActive(false);
    }






    public void FinishTask()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }





    GameObject GetClosestFoal(GameObject[] goals)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in goals)
        {
            float dist = Vector3.Distance(t.transform.position, Player.transform.position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
