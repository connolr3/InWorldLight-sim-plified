using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    public GameObject toDisable;
public GameObject toEnable;
    // Update is called once per frame
    public void DisableObj()
    {
        toDisable.SetActive(false);
        toEnable.SetActive(true);
    }
}
