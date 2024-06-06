using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class TriggersManager : MonoBehaviour
{
    public static TriggersManager instance;
    
    //Port Variables
    public string portName = "COM3";
    public int baudRate = 9600;
    public SerialPort serialPort;
    public bool hasSentTrigger = false;

    public byte[] sentTrigger;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        if (SerialPort.GetPortNames().ToList().Contains(portName)) {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            serialPort.WriteTimeout = 10000;
        }
        else {
            Debug.LogError("Serial Port does not exist");
        }
    }

    public void SetupPort() {
        if (serialPort != null && serialPort.IsOpen) {
            ClosePort();
        }

        if (SerialPort.GetPortNames().ToList().Contains(portName)) {
            serialPort = new SerialPort(portName, baudRate);

            serialPort.Open();
            serialPort.WriteTimeout = 10000;
        }
        else {
            Debug.LogError("Serial Port does not exist");
        }
    }

    public void SendTrigger(byte[] bytes) {
        /*
        if (!serialPort.IsOpen) {
            Debug.Log("No serial port open");
            return;
        }

        serialPort.Write(bytes, 0, bytes.Count());
        sentTrigger = bytes;
        hasSentTrigger = true;
        */

        StartCoroutine(BufferCoroutine(bytes));
    }

    public void ClosePort() {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Close();
            Debug.Log("Closing port");
        }
    }

    private IEnumerator BufferCoroutine(byte[] bytes) {
        if (serialPort == null || !serialPort.IsOpen) {
            Debug.Log("No serial port open");
            yield break;
        }

        while (serialPort.BytesToWrite != 0) {
            continue;
        }

        serialPort.Write(bytes, 0, bytes.Count());
        sentTrigger = bytes;
        hasSentTrigger = true;
    }
}
