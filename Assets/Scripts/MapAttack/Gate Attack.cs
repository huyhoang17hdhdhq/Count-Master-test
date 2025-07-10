using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class GateAttack : MonoBehaviour
{
    [Header("List nút bấm cho từng hàng")]
    public List<Button> list1Buttons;
    public List<Button> list2Buttons;
    public List<Button> list3Buttons;

    [Header("Các điểm tương ứng (Transform)")]
    public List<Transform> list1Points;
    public List<Transform> list2Points;
    public List<Transform> list3Points;

    private int currentListIndex = 0; // Đang chọn list nào (0 -> 1 -> 2)
    private List<int> selectedIndices = new List<int>(); // Lưu index đã chọn
    private int lastSelectedIndex = -1; // Index của list trước đó

    private int numberOfStickmans;
    public Transform player;
    [SerializeField] private GameObject stickMan;
    [SerializeField] private GameObject stickManRun;
    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    [Header("Điểm cố định cuối cùng sau khi chạy qua 3 điểm")]
    public Transform finalPoint;

    public AudioSource audioGate;
    public AudioClip gate;

    public bool attack;
    public Transform target;
    public GameObject LoseCastle;

    [SerializeField] private Transform enemy;

    void Start()
    {
        player = transform;
        MakeStickMan(10);

        SetupButtons(list1Buttons, 0);
        SetupButtons(list2Buttons, 1);
        SetupButtons(list3Buttons, 2);

        // Reset màu ban đầu nếu cần
        ResetButtonColors(list1Buttons);
        ResetButtonColors(list2Buttons);
        ResetButtonColors(list3Buttons);
    }

    private void Update()
    {
        if (attack)
        {

            Transform targetChild = null;

            for (int i = 0; i < enemy.childCount; i++)
            {
                Transform child = enemy;
                if (child != null && child.gameObject.activeSelf)
                {
                    targetChild = child;
                    break;
                }
            }

            // Quay từng stickman của bạn về targetChild
            if (targetChild != null)
            {
                for (int i = 1; i < transform.childCount; i++)
                {
                    Transform stick = transform.GetChild(i);

                    Vector3 dirToTarget = new Vector3(
                        targetChild.position.x,
                        stick.position.y,
                        targetChild.position.z
                    ) - stick.position;

                    stick.rotation = Quaternion.Slerp(
                        stick.rotation,
                        Quaternion.LookRotation(dirToTarget.normalized, Vector3.up),
                        Time.deltaTime * 10f
                    );
                }
            }

            if (enemy.GetChild(1).childCount > 0)
            {
                Transform target = enemy.GetChild(1);

                List<Transform> stickmen = new List<Transform>();
                for (int i = 1; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.activeSelf)
                        stickmen.Add(transform.GetChild(i));
                }

                for (int i = 0; i < stickmen.Count; i++)
                {
                    Transform stick = stickmen[i];
                    Vector3 targetDir = (target.position - stick.position).normalized;

                    // index càng cao thì moveFactor càng lớn
                    float maxSpeed = 3f;
                    float minSpeed = 2f;
                    float t = (float)i / (stickmen.Count - 1 + 0.0001f);
                    float moveFactor = Mathf.Lerp(minSpeed, maxSpeed, t);

                    Vector3 newPos = stick.position + targetDir * moveFactor * Time.deltaTime;
                    stick.position = Vector3.Lerp(stick.position, newPos, Time.deltaTime * 10f);
                }
            }
            else
            {
                attack = false;
               

                FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                    transform.GetChild(i).rotation = Quaternion.identity;



                enemy.gameObject.SetActive(false);

            }
            if (transform.childCount == 1 && enemy != null && enemy.childCount > 1)
            {
                var enemyChild = enemy.transform.GetChild(1);

                var enemyMgr = enemyChild.GetComponent<EnemyMini>();
                if (enemyMgr != null)
                {
                    enemyMgr.StopAttacking();
                }


            }
        }
        if (transform.childCount == 1)
        {
            Destroy(transform.GetChild(0).gameObject);
            transform.GetComponent<Collider>().isTrigger = false;
            //StartCoroutine(DelayUICoroutine());
        }
       
        
       
        if(enemy.GetChild(1).childCount == 0 && player.childCount > 0)
        {

            for (int i = 0; i < player.childCount; i++)
            {
                Transform stickman = player.GetChild(i);
                StickManMove moveScript = stickman.GetComponent<StickManMove>();

                if (moveScript != null)
                {
                    moveScript.Move(target.position);
                }
            }
        }
       

    }
    public void MakeStickMan(int number)
    {
        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(stickMan, transform.position, quaternion.identity, transform);
        }

        numberOfStickmans = transform.childCount - 1;

        FormatStickMan();
    }
    public void MakeStickManRun(int number)
    {
        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(stickManRun, transform.position, quaternion.identity, transform);
        }

        numberOfStickmans = transform.childCount - 1;

        FormatStickMan();
    }

    public void FormatStickMan()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

            var NewPos = new Vector3(x, -0.55f, z);
            player.transform.GetChild(i).DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gate"))
        {
           

            var gateManager = other.GetComponent<GateManager>();
            numberOfStickmans = transform.childCount ;

            if (gateManager.multiply)
            {
                MakeStickManRun(numberOfStickmans * gateManager.randomNumber);
            }
            else
            {
                MakeStickManRun(numberOfStickmans + gateManager.randomNumber);
            }
            audioGate.PlayOneShot(gate);
        }
        if (other.CompareTag("enemy"))
        {
            enemy = other.transform;
            attack = true;

           
            other.transform.GetChild(1).GetComponent<EnemyMini>().AttackThem(transform);

            
            Debug.Log("Event True");

        }
    }
    

    void SetupButtons(List<Button> buttonList, int listIndex)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int capturedIndex = i; // Để tránh lỗi closure
            buttonList[i].onClick.AddListener(() => OnButtonClicked(listIndex, capturedIndex));
        }
    }

    void OnButtonClicked(int listIndex, int indexInList)
    {
        if (listIndex != currentListIndex)
            return; // Chỉ cho chọn đúng list

        List<Button> currentButtons = GetButtonListByIndex(listIndex);

        for (int i = 0; i < currentButtons.Count; i++)
        {
            Button btn = currentButtons[i];

            if (i == indexInList)
            {
                btn.GetComponent<Image>().color = Color.green; // ✅ Đổi màu nút đã chọn
            }

            btn.interactable = false; // ❌ Tắt tất cả button trong list (chỉ chọn 1 lần)
        }

        selectedIndices.Add(indexInList);
        lastSelectedIndex = indexInList;
        currentListIndex++;

        Debug.Log($"Chọn List {listIndex + 1}, Index: {indexInList}");

        if (currentListIndex < 3)
        {
            ApplySelectionRules(GetButtonListByIndex(currentListIndex), lastSelectedIndex);
        }
        else
        {
            Debug.Log("Đã chọn đủ 3 list:");
            for (int i = 0; i < selectedIndices.Count; i++)
            {
                Debug.Log($"- List {i + 1}: Index {selectedIndices[i]} → Điểm: {GetPointBySelection(i, selectedIndices[i]).name}");
            }

            MovePlayerAlongSelectedPoints();
            for (int i = 0; i < transform.childCount; i++)
            {
                Animator anim = transform.GetChild(i).GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetBool("run",true); 
                }
            }


        }
    }

    void ApplySelectionRules(List<Button> nextButtonList, int prevIndex)
    {
        for (int i = 0; i < nextButtonList.Count; i++)
        {
            bool interactable = true;

            if (prevIndex == 0 && i == 2) // Chọn 1 → không được chọn 3
                interactable = false;
            else if (prevIndex == 2 && i == 0) // Chọn 3 → không được chọn 1
                interactable = false;

            nextButtonList[i].interactable = interactable;
        }
    }

    void ResetButtonColors(List<Button> buttonList)
    {
        foreach (var btn in buttonList)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
    }

    List<Button> GetButtonListByIndex(int listIndex)
    {
        return listIndex switch
        {
            0 => list1Buttons,
            1 => list2Buttons,
            2 => list3Buttons,
            _ => null
        };
    }

    Transform GetPointBySelection(int listIndex, int pointIndex)
    {
        return listIndex switch
        {
            0 => list1Points[pointIndex],
            1 => list2Points[pointIndex],
            2 => list3Points[pointIndex],
            _ => null
        };
    }
    void MovePlayerAlongSelectedPoints()
    {
        List<Transform> path = new List<Transform>();

        for (int i = 0; i < selectedIndices.Count; i++)
        {
            Transform point = GetPointBySelection(i, selectedIndices[i]);
            path.Add(point);
        }

        // 👉 Thêm điểm cuối cố định
        if (finalPoint != null)
        {
            path.Add(finalPoint);
        }

        MoveAlongPoints(path, 1f);

    }


    void MoveAlongPoints(List<Transform> points, float durationPerPoint)
    {
        Sequence moveSequence = DOTween.Sequence();

        foreach (Transform point in points)
        {
            
            moveSequence.Append(player.DOMove(point.position, durationPerPoint).SetEase(Ease.Linear));
            
        }

        moveSequence.Play();
    }
    private IEnumerator DelayUICoroutine()
    {
        yield return new WaitForSeconds(2f);
        if (LoseCastle != null)
        {
            LoseCastle.SetActive(true);
        }
    }


}
