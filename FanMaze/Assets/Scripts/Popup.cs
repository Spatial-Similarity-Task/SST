using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Popup : MonoBehaviour
{

    public Button yesButton;
    public Button cancelButton;

    public LogOutput log;
    public GameManager GameManager;


    private void Start()
    {
        this.gameObject.SetActive(false);
        yesButton.onClick.AddListener(overrideDirectory);
        cancelButton.onClick.AddListener(cancelDirectory);
        
    }

    void overrideDirectory()
    {
        log.SetDirectory2();
        GameManager.SetUpAndStart();
    }

    void cancelDirectory()
    {
        this.gameObject.SetActive(false);
    }

}