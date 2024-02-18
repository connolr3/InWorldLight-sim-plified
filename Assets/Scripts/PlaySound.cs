using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource ping;

    public void PlayNoise(){
        ping.Play();
    }
}
