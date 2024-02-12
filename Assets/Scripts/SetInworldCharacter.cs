using System.Collections;
using UnityEngine;
using Inworld;

public class SetInworldCharacter : MonoBehaviour
{
    void Start()
    {
        // Start the coroutine to check for InworldCharacter every second
       // StartCoroutine(CheckForInworldCharacter());
    }

    void Update(){
          if (Input.GetKeyDown(KeyCode.C)) { // check return key pressed
                Debug.Log("Inworld current character set to:"+InworldController.CurrentCharacter);
            }
        
    }

    IEnumerator CheckForInworldCharacter()
    {
        while (true)
        {
            // Find all active InworldCharacter instances in the scene
            InworldCharacter[] inworldCharacters = FindObjectsOfType<InworldCharacter>();

            foreach (InworldCharacter inworldCharacter in inworldCharacters)
            {
                // Check if the InworldCharacter has a child named "Armature" set to active
                Transform armature = inworldCharacter.transform.Find("Armature");
                if (armature != null && armature.gameObject.activeSelf)
                {
                    // You've found an InworldCharacter with an active "Armature" child
                    Debug.Log("Found InworldCharacter with active Armature: " + inworldCharacter.gameObject.name);

                    // Now you can access and manipulate the InworldCharacter instance
                    // For example, you can deactivate it
                    // inworldCharacter.gameObject.SetActive(false);
                    InworldController.CurrentCharacter = inworldCharacter;
                    Debug.Log("Current Character set to: " + inworldCharacter.gameObject.name);
                }
            }

            // Wait for one second before checking again
            yield return new WaitForSeconds(1f);
        }
    }
}
