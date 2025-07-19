using System;
using System.Collections.Generic;
using UnityEngine;

public class StickmanMover : MonoBehaviour
{
    public List<GameObject> waypoints;
    public float speed = 2;
    public float rotateSpeed = 5f;
    int index = 0;
    private Animator StickManAnimator;
    private AudioSource StickManAudio;

    private void Start()
    {

        StickManAnimator = GetComponent<Animator>();
        StickManAnimator.SetBool("run", true);

        StickManAudio = GetComponent<AudioSource>();
        StickManAudio.Play();


    }
    private void Update()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Vector3 destination = waypoints[index].transform.position;
        Vector3 direction = destination - transform.position;
        direction.y = 0; // Chỉ quay theo trục Y

        // Di chuyển
        Vector3 newpos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newpos;

        // Thêm: Quay mặt theo hướng di chuyển
        if (direction.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.05f)
        {
            if (index < waypoints.Count - 1)
            {
                index++;
            }
           
        }
       
    }
}
