using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bmlTUX;
using bmlTUX.Recorder;
using System.IO;

public class RecordActions : MonoBehaviour
{
    public void AddTeleport()
    {
        // Access the time and frame values here
        //Debug.Log($"Teleported at Time: {currentTime}, Frame: {currentFrame}");
        string[] dataRow1 = { currentFrame.ToString(), currentTime.ToString(), "Teleport" };
        WriteToCSV(dataRow1);
    }

    public string filePath = "C:/Users/Rosie/Downloads/test";

    private RecordingManager recordingManager;
    private float currentTime;
    private int currentFrame;

    private void Start()
    {
        // Assuming RecordingManager is attached to a GameObject in the scene
        recordingManager = GameObject.FindObjectOfType<RecordingManager>();

        if (recordingManager == null)
        {
            Debug.LogError("RecordingManager not found in the scene.");
            return;
        }

        // Subscribe to the UpdateRecording event to get frame and time updates
        recordingManager.UpdateRecording += OnUpdateRecording;
        string[] dataRow1 = { "Frame", "Time", "Action" };
        WriteToCSV(dataRow1);
    }

    // Write a single row of data to the CSV file
    private void WriteToCSV(string[] data)
    {
        // Check if the file exists, create it if not
        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                // Write the header row
                sw.WriteLine(string.Join(",", data));
            }
        }
        else
        {
            // Append data to the existing file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                // Write the data row
                sw.WriteLine(string.Join(",", data));
            }
        }
    }

    private void OnUpdateRecording(float time, int frame)
    {
        // Update class fields with the current time and frame values
        currentTime = time;
        currentFrame = frame;

        // Use the time and frame values here as needed
        Debug.Log("Time: " + time + ", Frame: " + frame);
    }

    // Remember to unsubscribe from events when your script is disabled or destroyed
    private void OnDisable()
    {
        if (recordingManager != null)
        {
            recordingManager.UpdateRecording -= OnUpdateRecording;
        }
    }
}
