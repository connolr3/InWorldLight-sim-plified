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
public class ProximityTrial : Trial {

     ProximityRunner myRunner;

    // Required Constructor.
    public ProximityTrial(ExperimentRunner runner, DataRow data) : base(runner, data) {
         myRunner = (ProximityRunner)runner;  //cast the generic runner to your custom type.  
    }

    GameObject inSceneAI;
    GameObject LastSceneAI;


        public void matchTransform( GameObject toChange,Transform toMatch){
        toChange.transform.position = toMatch.position;
            toChange.transform.rotation =toMatch.rotation;
        }

public Texture thisScenesTexture;
    // Optional Pre-Trial code. Useful for setting unity scene for trials. Executes in one frame at the start of each trial
    protected override void PreMethod() {
     //   Display character
        string thisTemperment = (string)Data["Temperment"];
        string thisTGender = (string)Data["AIGender"];
        int thisIndex = (int)Data["Index"];
        displayAI(thisTemperment, thisTGender,thisIndex);
        if(thisScenesTexture!=null){
        myRunner.ImageHolder.texture = thisScenesTexture;
        }
         Data["AIName"] = inSceneAI.name;

     //display painting
    }

  

public void displayAI(string Temperment, string Gender, int index){
    LastSceneAI=inSceneAI;
  if(Gender=="Female"&&Temperment=="Nice"){
        inSceneAI=myRunner.NiceFemales[index];
        thisScenesTexture= myRunner.NiceFemalesPaintings[index];
        }
        else if(Gender=="Male"&&Temperment=="Nice"){
        inSceneAI=myRunner.NiceMales[index];
          thisScenesTexture= myRunner.NiceMalesPaintings[index];
        }
        else if(Gender=="Female"&&Temperment=="Neutral"){
        inSceneAI=myRunner.NeutralFemales[index];
          thisScenesTexture= myRunner.NeutralFemalesPaintings[index];
        }
        else if(Gender=="Male"&&Temperment=="Neutral"){
        inSceneAI=myRunner.NeutralMales[index];
          thisScenesTexture= myRunner.NeutralMalesPaintings[index];
        }
        else{
            Debug.Log("Error in variable names... no matches found");
        }
        inSceneAI.SetActive(true);
if(index%2==0)
matchTransform(inSceneAI,myRunner.SpawnA);
    else
 matchTransform(inSceneAI,myRunner.SpawnB);
    DisableCanvas();

    }

        public void DisableCanvas(){
            GameObject canvasObject = inSceneAI.transform.Find("Canvas").gameObject;
        if (canvasObject != null)
        {
            canvasObject.SetActive(false);
        }
        }
    // Optional Pre-Trial code. Useful for waiting for the participant to
    // do something before each trial (multiple frames). Also might be useful for fixation points etc.
    protected override IEnumerator PreCoroutine() {
        yield return null; //required for coroutine
    }

    public void beginNextTrial(){
  ExperimentEvents.SkipToNextTrial();
    }
    // Main Trial Execution Code.
  /*  protected override IEnumerator RunMainCoroutine() {
    
        // You might want to do a while-loop to wait for participant response: 
        bool waitingForParticipantResponse = true;
        Debug.Log("Press the spacebar to end this trial.");
        while (waitingForParticipantResponse) {   // keep check each frame until waitingForParticipantResponse set to false.
            if (Input.GetKeyDown(KeyCode.Space)) { // check return key pressed
                waitingForParticipantResponse = false;  // escape from while loop
            }
        
            yield return null; // wait for next frame while allowing rest of program to run (without this the program will hang in an infinite loop)
        }
    
    }*/



bool waitingForParticipantResponse=true;

protected override IEnumerator RunMainCoroutine()
{
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
    protected override IEnumerator PostCoroutine() {
        yield return null;
    }

    // Optional Post-Trial code. useful for writing data to dependent variables and for resetting everything.
    // Executes in a single frame at the end of each trial
    protected override void PostMethod() {
        // How to write results to dependent variables: 
         Data["ProximityBeginInt"] =2f;
          Data["TimeToEngage"] = 5f;
          
    inSceneAI.SetActive(false);
    InworldController.CurrentCharacter = null;
    }
}

