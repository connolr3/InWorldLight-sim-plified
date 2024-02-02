using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveExcessEventSystems : MonoBehaviour
{
    public string preservedEventSystemName = "PreservedEventSystem"; // Specify the name of the EventSystem to preserve

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyExcessES", 0.01f);
    }

    public void DestroyExcessES()
    {
        var esArray = FindObjectsOfType<EventSystem>();
        Debug.Log(esArray.Length);

        if (esArray.Length > 1)
        {
            for (int i = 1; i < esArray.Length; i++)
            {
                if (esArray[i].gameObject.name != preservedEventSystemName)
                {
                    Destroy(esArray[i].gameObject);
                }
            }
        }
    }
}
