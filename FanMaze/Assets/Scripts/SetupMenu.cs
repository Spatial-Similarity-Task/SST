using UnityEngine;
using UnityEngine.UI;

public class SetupMenu : MonoBehaviour
{
    public GameManager gameManager;
    public byte[] testTrigger;
    public GameObject setupMenu;
    public GameObject customTimingsMenu;
    public Toggle defaultTimingsToggle;
    public Toggle customTimingsToggle;

    private float defTrial = 45f;
    private float defLearning = 2f;
    private float defTest = 4f;
    private float defFixation = 0.5f;
    private float defDelay = 0.25f;

    public void ComName(string name) {
        TriggersManager.instance.portName = name;
        TriggersManager.instance.SetupPort();
    }

    public void ChangeTrialLength(string time) {
        float timef = defTrial; 
        float.TryParse(time, out timef);
        if (timef == 0) {
            gameManager.delayLength = defTrial;
            return;
        }

        gameManager.trialLength = timef;
    }

    public void ChangeLearningLength(string time) {
        float timef = defLearning; 
        float.TryParse(time, out timef);
        if (timef == 0) {
            gameManager.delayLength = defLearning;
            return;
        }

        gameManager.learningPhaseBoxLength = timef;
    }

    public void ChangeTestingLength(string time) {
        float timef = defTest; 
        float.TryParse(time, out timef);
        if (timef == 0) {
            gameManager.delayLength = defTest;
            return;
        }

        gameManager.testingPhaseBoxLength = timef;
    }

    public void ChangeDelayLength(string time) {
        float timef = defDelay; 
        float.TryParse(time, out timef);
        if (timef == 0) {
            gameManager.delayLength = defDelay;
            return;
        }

        gameManager.delayLength = timef;
    }

    public void ChangeFixationLength(string time) {
        float timef = defFixation; 
        float.TryParse(time, out timef);
        if (timef == 0) {
            gameManager.fixationLength = defFixation;
            return;
        }

        gameManager.fixationLength = timef;
    }

    public void SendTestTrigger() {
        TriggersManager.instance.SendTrigger(testTrigger);
    }

    public void ToggleCustomTimings(bool isActive) {
        customTimingsMenu.SetActive(isActive);
        defaultTimingsToggle.isOn = !isActive;
        if (defaultTimingsToggle.isOn) {
            Debug.Log("setting defaults");
            gameManager.trialLength = defTrial;
            gameManager.learningPhaseBoxLength = defLearning;
            gameManager.testingPhaseBoxLength = defTest;
            gameManager.delayLength = defDelay;
            gameManager.fixationLength = defFixation;
        }
    }

    public void ToggleDefaultTimings(bool isActive) {
        customTimingsToggle.isOn = !isActive;
        if (defaultTimingsToggle.isOn) {
            Debug.Log("setting defaults");
            gameManager.trialLength = defTrial;
            gameManager.learningPhaseBoxLength = defLearning;
            gameManager.testingPhaseBoxLength = defTest;
            gameManager.delayLength = defDelay;
            gameManager.fixationLength = defFixation;
        }
    }

    public void CloseMenu() {
        setupMenu.SetActive(false);
    }
}
