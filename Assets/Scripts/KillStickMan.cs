//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class KillStickMan : MonoBehaviour
//{
//    private HashSet<GameObject> alreadyKilled = new HashSet<GameObject>();

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("blue") && !alreadyKilled.Contains(other.gameObject))
//        {
//            alreadyKilled.Add(other.gameObject); // Đánh dấu đã xử lý

//            Animator anim = other.GetComponent<Animator>();
//            if (anim != null)
//            {
//                anim.SetTrigger("die");
//                StartCoroutine(DestroyAfterDelay(other.gameObject, 2f));
//            }

//            Debug.Log("Đã chết");
//        }
//    }

//    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        Destroy(obj);
//    }
//}
