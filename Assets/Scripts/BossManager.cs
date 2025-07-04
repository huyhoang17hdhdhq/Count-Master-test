using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool attack;
    private Animator boss;
    [SerializeField] private GameObject stickMan;

    private void Start()
    {
        boss = transform.GetChild(0).GetComponent<Animator>();

        for (int i = 0; i < Random.Range(20, 120); i++)
        {
            Instantiate(stickMan, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }

        
       

    }

    void Update()
    {
        if (attack)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            boss.SetBool("Run", true);
        }
        else
        {
            boss.SetBool("Run", false);
        }
    }

    public void Move()
    {
        attack = true;
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("stop"))
        {
            attack = false;
            
        }
    }
    
}
