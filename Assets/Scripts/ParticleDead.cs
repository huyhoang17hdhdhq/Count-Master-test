using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDead : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue") )
        {
           
            Instantiate(blood,transform.position, Quaternion.identity);


        }
    }
}
