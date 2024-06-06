using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Evereal.VideoCapture;
using System.ComponentModel;
using System.IO;

public class TrialRecorder : MonoBehaviour
{
    public static TrialRecorder instance;
    public GoalManager goalManager;
    [SerializeField] private VideoCapture videoCapture;
    [SerializeField] public string filePath;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (instance != null) {
            return;
        }
        else {
            instance = this;
        }
    }

    public void StartRecording(string trial_num) {
        Debug.Log("Record Start: Trial " + trial_num);
        videoCapture.StartCapture();
        mainCamera.depth = mainCamera.depth - 1;
        mainCamera.depth = mainCamera.depth + 1;
    }

    public void StopRecording() {
        Debug.Log("Record Stop");
        videoCapture.StopCapture();
    }

    public void InitializeRecorder() {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        videoCapture.saveFolder = filePath;
    }
}
