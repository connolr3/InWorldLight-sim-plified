using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bmlTUX;
using bmlTUX.Recorder;

using System.Reflection;

public class SetDirectories : MonoBehaviour
{
    [SerializeField]
    private string recordingFolder;

    private void Awake()
    {
        // Access the RecordingManager script and set the initial value using reflection
        RecordingManager recordingManager = FindObjectOfType<RecordingManager>();
        if (recordingManager != null)
        {
            SetRecordingFolder(recordingManager, recordingFolder);
        }
    }

    private void SetRecordingFolder(RecordingManager manager, string folderPath)
    {
        // Use reflection to set the recording folder
        FieldInfo field = manager.GetType().GetField("recordingFolder", BindingFlags.Instance | BindingFlags.NonPublic);

        if (field != null)
        {
            field.SetValue(manager, folderPath);
        }
        else
        {
            Debug.LogError("recordingFolder field not found in RecordingManager.");
        }
    }
}
