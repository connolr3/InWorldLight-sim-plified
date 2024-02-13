using System.Collections;
using System.Data;
using bmlTUX;
using UnityEngine;
using UnityEngine.UI;
using Inworld;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// Classes that inherit from Trial define custom behaviour for your experiment's trials.
/// Most experiments will need to edit this file to describe what happens in a trial.
///
/// This template shows how to set up a custom trial script using the toolkit's built-in functions.
///
/// You can delete any unused methods and unwanted comments. The only required parts are the constructor and the MainCoroutine.
/// </summary>
public class ProximityTrial : Trial
{

    ProximityRunner myRunner;
    string thisGender;
    string thisTemperment;

    // Required Constructor.
    public ProximityTrial(ExperimentRunner runner, DataRow data) : base(runner, data)
    {
        myRunner = (ProximityRunner)runner;  //cast the generic runner to your custom type.  
    }

    GameObject inSceneAI;
    int AccessIndex;
    bool[] checkOrder = new bool[4];

    public void matchTransform(GameObject toChange, Transform toMatch)
    {
        toChange.transform.position = toMatch.position;
        toChange.transform.rotation = toMatch.rotation;
    }

    public Texture thisScenesTexture;
    // Optional Pre-Trial code. Useful for setting unity scene for trials. Executes in one frame at the start of each trial
    protected override void PreMethod()
    {
        thisTemperment = (string)Data["Temperment"];
        thisGender = (string)Data["AIGender"];
       // int thisIndex = (int)Data["Index"];
        getRandomIndex(thisTemperment, thisGender);
        displayAI();
        if (thisScenesTexture != null)
        {
            myRunner.ImageHolder.texture = thisScenesTexture;
        }

        Data["AIName"] = inSceneAI.name;
        setCurrentCharacter();
        //display painting
    }


    public int getRandomIndex(string Temperment, string Gender)
    {
        if (Gender == "Female" && Temperment == "Nice")
        {
            checkOrder = myRunner.NiceFemalesAccessed;
        }
        else if (Gender == "Male" && Temperment == "Nice")
        {
            checkOrder = myRunner.NiceMalesAccessed;
        }
        else if (Gender == "Female" && Temperment == "Neutral")
        {
            checkOrder = myRunner.NeutralFemalesAccessed;
        }
        else if (Gender == "Male" && Temperment == "Neutral")
        {
            checkOrder = myRunner.NeutralMalesAccessed;
        }
        else
        {
            Debug.Log("Error in variable names... no matches found");
        }
        while (true) // Corrected loop condition
        {
            int index = Random.Range(0, 4);
            if (!checkOrder[index])
            {
                AccessIndex = index;
                return AccessIndex;
            }
            // No need for a return statement here; continue the loop if checkOrder[index] is true
        }

    }

    public void setAccessed()
    {
        Data["AccessIndex"] = AccessIndex;
        if (thisGender == "Female" && thisTemperment == "Nice")
        {
            myRunner.NiceFemalesAccessed[AccessIndex] = true;
        }
        else if (thisGender == "Male" && thisTemperment == "Nice")
        {
            myRunner.NiceMalesAccessed[AccessIndex] = true;
        }
        else if (thisGender == "Female" && thisTemperment == "Neutral")
        {
            myRunner.NeutralFemalesAccessed[AccessIndex] = true;
        }
        else if (thisGender == "Male" && thisTemperment == "Neutral")
        {
            myRunner.NeutralMalesAccessed[AccessIndex] = true;
        }
        else
        {
            Debug.Log("Error in variable names... no matches found");
        }
    }

    public void displayAI()
    {
        // LastSceneAI=inSceneAI;
        if (thisGender == "Female" && thisTemperment == "Nice")
        {
            inSceneAI = myRunner.NiceFemales[AccessIndex];
            thisScenesTexture = myRunner.NiceFemalesPaintings[AccessIndex];
        }
        else if (thisGender == "Male" && thisTemperment == "Nice")
        {
            inSceneAI = myRunner.NiceMales[AccessIndex];
            thisScenesTexture = myRunner.NiceMalesPaintings[AccessIndex];
        }
        else if (thisGender == "Female" && thisTemperment == "Neutral")
        {
            inSceneAI = myRunner.NeutralFemales[AccessIndex];
            thisScenesTexture = myRunner.NeutralFemalesPaintings[AccessIndex];
        }
        else if (thisGender == "Male" && thisTemperment == "Neutral")
        {
            inSceneAI = myRunner.NeutralMales[AccessIndex];
            thisScenesTexture = myRunner.NeutralMalesPaintings[AccessIndex];
        }
        else
        {
            Debug.Log("Error in variable names... no matches found");
        }
        inSceneAI.SetActive(true);
        Transform armature = inSceneAI.transform.Find("Armature");
        if (armature != null)
        {
            armature.gameObject.SetActive(true);
        }


        if (IndexInBlock % 2 == 0)
            matchTransform(inSceneAI, myRunner.SpawnA);
        else
            matchTransform(inSceneAI, myRunner.SpawnB);
         DisableCanvas();

    }

    public void setCurrentCharacter()
    {

        Inworld.InworldCharacter inworldCharacterComponent = inSceneAI.GetComponent<Inworld.InworldCharacter>();

        // Check if the component is not null before assigning
        if (inworldCharacterComponent != null)
        {
            InworldController.CurrentCharacter = inworldCharacterComponent;
        }
        else
        {
            // Handle the case where the component is not found
            Debug.LogError("InworldCharacter component not found on the GameObject.");
        }
        Debug.Log(inSceneAI.name);
        Debug.Log("Inworld character set to:" + inSceneAI.name);
        Debug.Log("Inworld current character set to:" + InworldController.CurrentCharacter);
    }

    public void DisableCanvas()
    {
        GameObject canvasObject = inSceneAI.transform.Find("Canvas").gameObject;
        if (canvasObject != null)
        {
            Canvas canvasComponent = canvasObject.GetComponent<Canvas>();
            canvasComponent.enabled = false;
        }
    }
    // Optional Pre-Trial code. Useful for waiting for the participant to
    // do something before each trial (multiple frames). Also might be useful for fixation points etc.
    protected override IEnumerator PreCoroutine()
    {
        yield return null; //required for coroutine
    }

    public void beginNextTrial()
    {
        ExperimentEvents.SkipToNextTrial();
    }


    bool waitingForParticipantResponse = true;

    protected override IEnumerator RunMainCoroutine()
    {
        // Debug.Log(inSceneAI.name);
        // setCurrentCharacter();
        waitingForParticipantResponse = true;
        Debug.Log("Interact with the object to end this trial.");

        // Wait for the next frame while allowing the rest of the program to run
        yield return null;

        // Subscribe to the XR Interaction events specific to XRSimpleInteractable
        myRunner.xrSimpleInteractable.onSelectEntered.AddListener(OnObjectSelected);

        while (waitingForParticipantResponse)
        {
            yield return null;
        }

        // Unsubscribe from the XR Interaction events
        myRunner.xrSimpleInteractable.onSelectEntered.RemoveListener(OnObjectSelected);
    }

    private void OnObjectSelected(XRBaseInteractor interactor)
    {
        Debug.Log("Object selected. Ending the trial.");
        waitingForParticipantResponse = false;
    }

    /*
    THIS WORKS!


    protected override IEnumerator RunMainCoroutine()
       {
            waitingForParticipantResponse = true;
           Debug.Log("Press the spacebar or VR input to end this trial.");

           // Wait for the next frame while allowing the rest of the program to run
           yield return null;

           // Subscribe to the XR Input event
           myRunner.NextTrial.action.performed += OnNextTrialAction;

           while (waitingForParticipantResponse)
           {
               yield return null;
           }

           // Unsubscribe from the XR Input event
           myRunner.NextTrial.action.performed -= OnNextTrialAction;
       }

       private void OnNextTrialAction(InputAction.CallbackContext context)
       {
           Debug.Log("Next trial action performed.");
           waitingForParticipantResponse = false;
       }
   */

    // Optional Post-Trial code. Useful for waiting for the participant to do something after each trial (multiple frames)
    protected override IEnumerator PostCoroutine()
    {
        yield return null;
    }

    // Optional Post-Trial code. useful for writing data to dependent variables and for resetting everything.
    // Executes in a single frame at the end of each trial
    protected override void PostMethod()
    {
        setAccessed();
        // How to write results to dependent variables: 
        Data["ProximityBegin"] = 2f;
        Data["TimeToEngage"] = 5f;

        inSceneAI.SetActive(false);
        InworldController.CurrentCharacter = null;
    }
}

