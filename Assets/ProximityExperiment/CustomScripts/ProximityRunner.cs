using bmlTUX;
// ReSharper disable once RedundantUsingDirective
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using UnityEngine.InputSystem;//required to read input
using UnityEngine.XR.Interaction.Toolkit;
using Inworld;
/// <summary>
/// This class is the main communication between the toolkit and the Unity scene. Drag this script onto an empty gameObject in your Unity scene.
/// In the gameObject's inspector you need to drag in your design file and any custom scripts.
/// </summary>
public class ProximityRunner : ExperimentRunner {

    //Here is where you make a list of objects in your unity scene that need to be referenced by your scripts.
      public GameObject TeleportingInstructions;
    public GameObject [] NiceFemales;
      public GameObject [] NeutralFemales;
         public GameObject [] NiceMales;
      public GameObject [] NeutralMales;

      public GameObject MyRig;
      public string[] Components;

public Transform SpawnA;
public Transform SpawnB;
 public InputActionProperty NextTrial; 
public XRSimpleInteractable NextInteractable;
public XRSimpleInteractable ReadyInteractable;


public GameObject PreTrial;
public GameObject PostTrial;

public InworldController inworldCon;

public static string[] names;
public RawImage ImageHolder;


    public Texture [] NiceFemalesPaintings;
      public Texture [] NeutralFemalesPaintings;
         public Texture [] NiceMalesPaintings;
      public Texture [] NeutralMalesPaintings;
    [HideInInspector]





public bool[] NiceFemalesAccessed = new bool[4];
public bool[] NeutralFemalesAccessed = new bool[4];
public  bool[] NiceMalesAccessed = new bool[4];
public bool[] NeutralMalesAccessed = new bool[4];



public CSVReader csvReader;





 public void allocate(){
    names = new string[16];
 }

 public string[] getNames(){
  return names;
 }

 public void beginNextTrial(){
   //if(NextTrial.action.ReadValue<float>()>0.5f){
      Debug.Log("Next Trial");
  ExperimentEvents.SkipToNextTrial();
     //   }

    }

   
}


 
    

