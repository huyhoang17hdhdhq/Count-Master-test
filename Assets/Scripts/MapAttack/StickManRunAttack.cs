using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickManRunAttack : MonoBehaviour
{
    public AudioSource audioAttack;
    public AudioClip attack;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red"))

        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            audioAttack.PlayOneShot(attack);
        }
        if (other.CompareTag("kill"))
        {
            Destroy(gameObject);
            audioAttack.PlayOneShot(attack);
        }
    }
}
