using System.Collections;
using System.Data;
using bmlTUX;
// ReSharper disable once RedundantUsingDirective
using UnityEngine;


/// <summary>
/// Classes that inherit from Block define custom behaviour for your experiment's blocks.
/// This might be useful for instructions that differ between each block, setting up the scene for each block, etc.
///
/// This template shows how to set up a custom Block script using the toolkit's built-in functions.
///
/// You can delete any unused methods and unwanted comments. The only required part is the constructor.
///
/// You cannot edit the main execution part of Blocks since their main execution is to run their trials.
/// </summary>
public class ProximityBlock : Block {
    
    // // You usually want to store a reference to your Experiment runner
     ProximityRunner myRunner;


    // Required Constructor. Good place to set up references to objects in the unity scene
    public ProximityBlock(ExperimentRunner runner, DataTable trialTable, DataRow data, int index) : base(runner, trialTable, data, index) {
         myRunner = (ProximityRunner)runner;  //cast the generic runner to your custom type.
        // GameObject myGameObject = myRunner.MyGameObject;  // get reference to gameObject stored in your custom runner
    
    }

public GameObject rig;
public  string[] components ;
    // Optional Pre-Block code. Useful for calibration and setup common to all blocks. Executes in a single frame at the start of the block
    protected override void PreMethod() {
rig =  myRunner.MyRig;
        string thisBlocksLocomotion = (string)Data["Locomotion"]; // Read values of independent variables
        components = myRunner.Components;
        if(thisBlocksLocomotion=="Walking"){
           componentSet(false);
        }
        else if (thisBlocksLocomotion=="Teleporting"){
componentSet(true);
        }
        else{

            Debug.Log("Error in variable names... no matches found");
        }
        // myGameObject.transform.position = new Vector3(thisBlocksDistanceValue, 0, 0); // set up scene based on value
    }

public void componentSet(bool enabled){
     if (rig != null)
        {
            foreach (string componentName in components)
            {
                // Try to find the component by name
                MonoBehaviour component = rig.GetComponent(componentName) as MonoBehaviour;

                // If the component is found, disable it
                if (component != null)
                {
                    component.enabled = enabled;
                    Debug.Log($"Disabled component: {componentName}");
                }
                else
                {
                    Debug.LogWarning($"Component not found: {componentName}");
                }
            }
        }
}

    // Optional Pre-Block code spanning multiple frames. Useful for pre-Block instructions.
    // Can execute over multiple frames at the start of a block
    protected override IEnumerator PreCoroutine() {
        yield return null; // yield return required for coroutine. Waits until next frame
        
        // Other ideas:
        // yield return new WaitForSeconds(5);     Waits for 5 seconds worth of frames;
        // can also wait for user input in a while-loop with a yield return null inside.
    }


    // Optional Post-Block code spanning multiple frames. Useful for Block debrief instructions.
    protected override IEnumerator PostCoroutine() {
        yield return null; //required for coroutine
    }


    // Optional Post-Block code.
    protected override void PostMethod() {
        // cleanup code (happens all in one frame at end of block)
    }
}

