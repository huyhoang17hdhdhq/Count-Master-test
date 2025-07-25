﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform player;
    public int numberOfStickmans, numberOfEnemyStickmans;
    [SerializeField] private TextMeshPro CounterTxt;
    public List<GameObject> stickManMain;

    public List<GameObject> stickManPrefabs;

    //****************************************************

    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    //*********** move the player ********************

    public bool moveByTouch, gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed;
    private Camera camera;

    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    public bool attack;
    public static PlayerManager PlayerManagerInstance;
   
    public GameObject SecondCam;
    public bool FinishLine, moveTheCamera;
    public bool moveThePlayer;
    
    [Header("audio")]
    public AudioSource gate;
    public AudioSource jump;

    [Header("Nguồn phát âm thanh")]
    public AudioSource audioFinish;
    public AudioClip finishClip;

    public AudioSource runAudio;

    private void Awake()
    {
        PlayerManagerInstance = this;
    }
    void Start()
    {
        player = transform;

        numberOfStickmans = transform.childCount - 1;

        CounterTxt.text = numberOfStickmans.ToString();

        camera = Camera.main;

        PlayerManagerInstance = this;

        gameState = false;

        moveThePlayer = true;
    }

    void Update()
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

            if (enemy.GetChild(1).childCount > 1)
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
                    float maxSpeed = 1.5f;
                    float minSpeed = 0.6f;
                    float t = (float)i / (stickmen.Count - 1 + 0.0001f);
                    float moveFactor = Mathf.Lerp(minSpeed, maxSpeed, t);

                    Vector3 newPos = stick.position + targetDir * moveFactor * Time.deltaTime;
                    stick.position = Vector3.Lerp(stick.position, newPos, Time.deltaTime * 10f);
                }
            }






            else
            {
                attack = false;
                roadSpeed = 4f;

                FormatStickMan();

                for (int i = 1; i < transform.childCount; i++)
                    transform.GetChild(i).rotation = Quaternion.identity;


                
                    enemy.gameObject.SetActive(false);

            }
            if (transform.childCount == 1 && enemy != null && enemy.childCount > 1)
            {
                var enemyChild = enemy.transform.GetChild(1);

                var enemyMgr = enemyChild.GetComponent<enemyManager>();
                if (enemyMgr != null)
                {
                    enemyMgr.StopAttacking();
                }


            }

        }
        else
        {
            MoveThePlayer();

        }


        if (transform.childCount == 1 && FinishLine)
        {
            gameState = false;
        }
        if (!gameState)
        {
            runAudio.Pause();
        }


        if (gameState)
        {
            road.Translate(road.forward * Time.deltaTime * roadSpeed);

           
        }
        

        if (moveTheCamera && transform.childCount > 1)
        {
            var cinemachineTransposer = SecondCam.GetComponent<CinemachineVirtualCamera>()
              .GetCinemachineComponent<CinemachineTransposer>();

            var cinemachineComposer = SecondCam.GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineComposer>();

            cinemachineTransposer.m_FollowOffset = new Vector3(4.5f, Mathf.Lerp(cinemachineTransposer.m_FollowOffset.y,
                transform.GetChild(1).position.y + 2f, Time.deltaTime * 1f), -5f);

            cinemachineComposer.m_TrackedObjectOffset = new Vector3(0f, Mathf.Lerp(cinemachineComposer.m_TrackedObjectOffset.y,
                4f, Time.deltaTime * 1f), 0f);

        }
       
        numberOfStickmans = transform.childCount-1;
        UpdateUI();
       

    }

    void MoveThePlayer()
    {
        if (!moveThePlayer) return;



        if (Input.GetMouseButtonDown(0) && gameState)
        {
            moveByTouch = true;

            var plane = new Plane(Vector3.up, 0f);

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                mouseStartPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            moveByTouch = false;

        }

        if (moveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);

                var move = mousePos - mouseStartPos;

                var control = playerStartPos + move;


                if (numberOfStickmans > 50)
                    control.x = Mathf.Clamp(control.x, -0.7f, 0.7f);
                else
                    control.x = Mathf.Clamp(control.x, -1.1f, 1.1f);

                transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * playerSpeed)
                    , transform.position.y, transform.position.z);

            }
        }
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

    public void MakeStickMan(int number)
    {
        // Lấy index đã chọn từ PickSkin (được lưu trong PlayerPrefs)
        int selectedIndexMain = PlayerPrefs.GetInt("SelectedPlayerIndex", 0);

        // Bảo vệ nếu index vượt ngoài danh sách
        if (selectedIndexMain < 0 || selectedIndexMain >= stickManMain.Count)
        {
            Debug.LogWarning("Selected index không hợp lệ trong stickManMain, dùng index 0.");
            selectedIndexMain = 0;
        }

        // Lấy prefab tương ứng
        GameObject prefabToSpawnMain = stickManMain[selectedIndexMain];

        // Sinh số lượng stickman theo yêu cầu
        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(prefabToSpawnMain, transform.position, Quaternion.identity, transform);
        }

        // Cập nhật số lượng và UI
        numberOfStickmans = transform.childCount - 1;
        CounterTxt.text = numberOfStickmans.ToString();
        FormatStickMan();
    }

    public void MakeStickManRun(int number)
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedPlayerIndex", 0);

        // Bảo vệ nếu index không hợp lệ
        if (selectedIndex < 0 || selectedIndex >= stickManPrefabs.Count)
        {
            Debug.LogWarning("Selected index không hợp lệ, dùng index 0 mặc định.");
            selectedIndex = 0;
        }

        GameObject prefabToSpawn = stickManPrefabs[selectedIndex];

        for (int i = numberOfStickmans; i < number; i++)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity, transform);
        }

        numberOfStickmans = transform.childCount - 1;
        CounterTxt.text = numberOfStickmans.ToString();
        FormatStickMan();
    }

    public void WinBoss()
    {
        attack = false;
        FormatStickMan();
        for (int i = 1; i < transform.childCount; i++) // Nếu bỏ qua index 0
        {
            Transform child = transform.GetChild(i);
            Animator anim = child.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("run",false); 
            }
        }

    }
    public void ClearCloneStickmans()
    {
        // Bắt đầu từ index 2 → xóa các stickman clone
        for (int i = transform.childCount - 1; i >= 2; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        numberOfStickmans = 0;
    }




    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("gate"))
        {
            // Lấy 2 cổng
            BoxCollider gate1 = other.transform.parent.GetChild(0).GetComponent<BoxCollider>();
            BoxCollider gate2 = other.transform.parent.GetChild(1).GetComponent<BoxCollider>();

            // Tắt collider tạm thời
            StartCoroutine(DisableCollidersTemporarily(gate1, gate2, 2f));

            var gateManager = other.GetComponent<GateManager>();
            numberOfStickmans = transform.childCount - 1;

            if (gateManager.multiply)
            {
                MakeStickManRun(numberOfStickmans * gateManager.randomNumber);
            }
            else
            {
                MakeStickManRun(numberOfStickmans + gateManager.randomNumber);
            }

            gate.Play();
        }


        if (other.CompareTag("gatebost"))
            {
            other.transform.GetComponent<BoxCollider>().enabled = false;
            var gateBost = other.GetComponent<CloneGateBost>();
            numberOfStickmans = transform.childCount - 1;

            if (gateBost != null)
                {
                MakeStickManRun(numberOfStickmans + gateBost.randomGateBost);
            }
        }


        if (other.CompareTag("enemy"))
        {
            enemy = other.transform;
            attack = true;

            roadSpeed = 0.5f;

            other.transform.GetChild(1).GetComponent<enemyManager>().AttackThem(transform);

            StartCoroutine(UpdateTheEnemyAndPlayerStickMansNumbers());
            Debug.Log("Event True");

        }
        if (other.CompareTag("Boss"))
        {
            enemy = other.transform;
            roadSpeed = 0.5f;
            attack = true;
            SecondCam.SetActive(true);
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);

            other.transform.GetChild(1).GetComponent<BossManager>().Move();
            
  

        }
        if(other.CompareTag("stop"))
        {
            gameState = false;
           
        }


        if (other.CompareTag("Finish"))
        {
            SecondCam.SetActive(true);
            FinishLine = true;

            moveThePlayer = false;


            transform.position = new Vector3(0f, transform.position.y, transform.position.z);

            // Gọi tạo tháp
            Tower.TowerInstance.CreateTower(transform.childCount - 1);

            // Tắt Stickman chính (giả sử nằm ở index 0)
            transform.GetChild(0).gameObject.SetActive(false);

            if (audioFinish != null && finishClip != null)
            {
                audioFinish.PlayOneShot(finishClip);
                Debug.Log("Đã phát âm thanh finish từ: " + other.name);
            }
        }
        


        if (other.CompareTag("kill"))
        {

            numberOfStickmans = transform.childCount - 1;
            CounterTxt.text = numberOfStickmans.ToString();
            FormatStickMan();
        }
        if (other.CompareTag("jump"))
        {
            jump.Play();
            Debug.Log("da phat jump");
        }
        

    }
    public void UpdateUI()
    {
        CounterTxt.text = numberOfStickmans.ToString();
    }

    IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {

        numberOfEnemyStickmans = enemy.transform.GetChild(1).childCount - 1;
        numberOfStickmans = transform.childCount - 1;

        while (numberOfEnemyStickmans > 0 && numberOfStickmans > 0)
        {
            numberOfEnemyStickmans--;
            numberOfStickmans--;

            enemy.transform.GetChild(1).GetComponent<enemyManager>().CounterTxt.text = numberOfEnemyStickmans.ToString();

            CounterTxt.text = numberOfStickmans.ToString();

            yield return null;
        }

        if (numberOfEnemyStickmans == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }
        }
    }
    private IEnumerator DisableCollidersTemporarily(BoxCollider col1, BoxCollider col2, float delay)
    {
        col1.enabled = false;
        col2.enabled = false;

        yield return new WaitForSeconds(delay);

        col1.enabled = true;
        col2.enabled = true;
    }


}
