using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBost : MonoBehaviour
{
   
    public GameObject randomClone;
    public AudioSource audiorun;
   

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            audiorun.Pause();
           
            randomClone.SetActive(true);



        }
    }
}
