using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class Train : MonoBehaviour
{
    public TrainState currentState;
    public Stack<TrainState> stateStack = new Stack<TrainState>();
    public SnakeInputActions trainInput;
    public TrainState IdleState { get; private set; }
    public TrainState MoveState { get; private set; }
    public TrainState GrowState { get; private set; }
    public TrainState DeadState { get; private set; }
    public TrainState FlyingState { get; private set; }
    public TrainState RewindState { get; private set; }
    public TrainState CleanStartState { get; private set; }
    public TrainState JumpState { get; private set; }
    public TrainState WagonMergeState { get; private set; }
    public TrainState NumberControlState { get; private set; }
    public TrainState FinishState { get; private set; }
    public TrainState ReleaseState { get; private set; }

    public float playerSpeed = 2.0f;
    public float moveSpeed = 5f;
    public float bodyFollowSpeed = 10f; // Kuyruklarýn ne kadar hýzlý takip edeceðini belirleyen hýz
    public float turnSpeed = 100f;
    public float bodyDistance = 1.9f; // Kuyruk parçalarý arasýndaki mesafe
    public float positionRecordInterval = 4f; // Pozisyon kayýt sýklýðý
    public List<GameObject> bodyParts = new List<GameObject>();
    public bool isLeftTrain;
    public List<Vector3> positions = new List<Vector3>();
    public Animator animator;
    public Animator wingsAnimator;
    public List<GameObject> trainTails;
    public GameObject wings;
    private int selectedTrainIndex;
    public GameObject bodyPrefab;
    public Animator cinemachineState;
    
    // Rewind State
    public float maxRewindTime = 10f;  // Kaç saniyelik pozisyon kaydedilecek
    public float recordInterval = 1f;  // Kaç saniyede bir pozisyon kaydedilecek
    public Queue<Vector3> positionQueue = new Queue<Vector3>();
    public Queue<Quaternion> rotationQueue = new Queue<Quaternion>();
    private Coroutine recordCoroutine;
    // Clean Start
    public Transform[] cleanStartPathPoints;  // Trenimizin izleyeceði yolun noktalarýný belirtiyoruz.
    public Transform[] finishPathPoints;  // Trenimizin izleyeceði yolun noktalarýný belirtiyoruz.

    private bool isRewinding = false;

    public ScoreBoardManager scoreBoardManager;
    // Charcther Controller - Inpýt System

    public float gravityValue = -9.81f;

    public CharacterController characterController;
    public PlayerInput playerInput;
    public Vector3 playerVelocity;
    public bool groundedPlayer;

    public InputAction leftTurnAction;
    public InputAction rightTurnAction;


    public Button grow;
    private Queue<GameObject> wagonPool = new Queue<GameObject>();
    private int initialWagonCount = 40;  // Baþlangýçta üretilecek vagon sayýsý
    public int gameGoldCount=0;
    private int goldCount = 0;
    public TextMeshProUGUI goldCountText;
    public MergeWagonPool mergeWagonPool;
    public int number;
    public Color color;
    public List<int> wagonNumbers = new List<int>();
    public List<TextMeshProUGUI> numberTexts;
    public List<Color> numberUiColor;
    public List<Image> numberImage;
    public int mergeGoldCounter = 2;
    public GameObject gameOverCanvas;
    public GameObject finishLookTarget;
    public GameObject cleanStartLookAtTarget;
    //public Coroutine runMoveCoroutine;
    public bool isShieldActive = false;
    public GameObject scorePanel;
    public UIAnimationManager animationManager;
    public ScoreManager scoreManager;
    public ItemManager itemManager;
    public GameManager gameManager;
    public TrailRenderer RLWTireSkid; // Sað arka tekerlek izi
    public TrailRenderer RRWTireSkid; // Sol arka tekerlek izi

    //public ParticleSystem tireSmokeParticle; // Tekerlek dumaný partikül sistemi
    public ParticleSystem RTireSmokeParticle;
    public ParticleSystem LTireSmokeParticle;

    public ParticleSystem speedLinesEffectOut;
    public ParticleSystem speedLinesEffectIn;

    public ParticleSystem coinEffect;
    public int mod = 0;
    public GameObject craneHook;
    public Animator rightDoorAnimator;
    public Animator leftDoorAnimator;
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;

    private void Awake()
    {

        bodyDistance = 2.2f;
        selectedTrainIndex = PlayerPrefs.GetInt("SelectedTrainIndex", 0);
        bodyPrefab = trainTails[selectedTrainIndex].gameObject;
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        grow.onClick.AddListener(Grow);
        // Buton aksiyonlarý
        leftTurnAction = playerInput.actions["TurnLeft"];
        rightTurnAction = playerInput.actions["TurnRight"];
        trainInput = new SnakeInputActions();
        trainInput.Enable();
        characterController = GetComponent<CharacterController>();
        Application.targetFrameRate = 60;
        StartCoroutine(RecordPositions());
        mod = PlayerPrefs.GetInt("gameModNumber", 0);
    }
    public void Grow()
    {
        TransitionToState(GrowState);
    }

    private void Start()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        GrowState = new GrowState(this);
        DeadState = new DeadState(this);
        FlyingState = new FlyingState(this);
        RewindState = new RewindState(this);
        CleanStartState = new CleanStartState(this);
        JumpState = new JumpState(this);
        WagonMergeState = new WagonMergeState(this);
        NumberControlState = new NumberControlState(this);
        FinishState = new FinishState(this);
        ReleaseState = new ReleaseState(this);

        // Baþlangýç durumu

        TransitionToState(IdleState);
        animationManager.StartCountdown();
        // Baþlangýç pozisyonunu kaydet
        positions.Add(transform.position);
        goldCountText.text = gameGoldCount.ToString();
        // Pozisyonlarý kaydetmeye baþla
        StartRecording();
        for (int i = 0; i < initialWagonCount; i++)
        {
            GameObject newWagon = Instantiate(bodyPrefab, transform.position, Quaternion.identity);
            newWagon.SetActive(false);
            wagonPool.Enqueue(newWagon);
        }
    }
    public GameObject GetWagonFromPool()
    {
        if (wagonPool.Count > 0)
        {
            GameObject wagon = wagonPool.Dequeue();
            wagon.SetActive(true);
            return wagon;
        }
        else
        {
            // Eðer havuzda vagon kalmamýþsa, yeni bir vagon üret
            return Instantiate(bodyPrefab, transform.position, Quaternion.identity);
        }
    }
    public void ReturnAllWagonsToPool()
    {
        foreach (var wagon in bodyParts)
        {
            ReturnWagonToPool(wagon); // Vagonu havuza geri döndür
        }
        bodyParts.Clear(); // Aktif vagon listesini temizle
    }
    public void ReturnWagonToPool(GameObject wagon)
    {

        wagon.SetActive(false);
        wagon.transform.SetParent(null);
        wagonPool.Enqueue(wagon);
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
    private void Update()
    {
        currentState.UpdateState();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 5; i++)
            {
                TransitionToState(GrowState);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TransitionToState(RewindState);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TransitionToState(IdleState);
        }
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            TransitionToState(CleanStartState);
        }      
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TransitionToState(FlyingState);
        }     
        if (Input.GetKeyDown(KeyCode.E))
        {
            TransitionToState(MoveState);
        }       
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            TransitionToState(JumpState);
        }     
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SpeedDownn();
        }       
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SpeedUpp();
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            TransitionToState(FinishState);
        }

    }
    public void StartRecording()
    {
        if (recordCoroutine == null)
        {
            recordCoroutine = StartCoroutine(RecordPositions());
        }
    }

    public void StopRecording()
    {
        if (recordCoroutine != null)
        {
            StopCoroutine(RecordPositions());
            recordCoroutine = null;
        }
    }

    private IEnumerator RecordPositions()
    {
        while (true)
        {
            if (positionQueue.Count >= maxRewindTime)
            {
                positionQueue.Dequeue();
                rotationQueue.Dequeue();
            }

            positionQueue.Enqueue(transform.position);
            rotationQueue.Enqueue(transform.rotation);

            yield return new WaitForSeconds(1);
        }
    }



    public void TransitionToState(TrainState newState)
    {
        if (currentState != null)
        {
            stateStack.Push(currentState); // Mevcut durumu yýðýna ekle
            currentState.ExitState();
        }

        
        currentState = newState;
        currentState.EnterState();
        if (newState == IdleState)
        {
            StopCoroutine(RecordPositions());
        }

    }
    public void ReturnToPreviousState()
    {
        if (stateStack.Count > 0)
        {
            TrainState previousState = stateStack.Pop(); // Yýðýnýn tepesindeki duruma geri dön
            TransitionToState(previousState);
        }
    }

    public void Die()
    {
        // Yýlanýn ölme iþlemleri
        Debug.Log("Snake is dead!");
    }

    public void CloseWingsState()
    {
        StartCoroutine(RunMoveState());
    }

    IEnumerator RunMoveState()
    {
        yield return new WaitForSeconds(15);
        TransitionToState(MoveState);
    }

    public void CameraZoomOutState()
    {
        StartCoroutine(CameraZoomOut());
    }

    IEnumerator CameraZoomOut()
    {
        speedLinesEffectIn.Stop();
        speedLinesEffectOut.Play();
        Debug.Log("kamera geçiyor");
        cinemachineState.SetBool("isZoomOut", true);
        yield return new WaitForSeconds(15);
        cinemachineState.SetBool("isZoomOut", false);
        Debug.Log("Kamera geçti");
        speedLinesEffectIn.Play();
        speedLinesEffectOut.Stop();
    }
    public void Groww()
    {
        TransitionToState(GrowState);
    } 
    public void Rewindd()
    {
        TransitionToState(RewindState);
    }    
    public void Idlee()
    {
        TransitionToState(IdleState);
    }     
    public void CleanStartt()
    {
        TransitionToState(CleanStartState);
    }      
    public void Flyingg()
    {
        TransitionToState(FlyingState);
    }     
    public void Movee()
    {
        TransitionToState(MoveState);
    }
    List<float> bodyDistances = new List<float> { 2.25f, 2.2f, 1.9f, 1.85f, 1.8f, 1.8f };
    int distanceIndex = 0;
    public void SpeedUpp()
    {
        Debug.Log(playerSpeed);
        if (playerSpeed > 17.3)
            return;
        // Speed Up: Particle sisteminin startSpeed ve radius deðerlerini artýr
        UpdateParticleSystem(speedLinesEffectIn, 4f, -.3f);  // In efekti için
        UpdateParticleSystem(speedLinesEffectOut, 4f, -.3f); // Out efekti için



    playerSpeed = playerSpeed + .5f;
    moveSpeed = moveSpeed + .5f;
    bodyFollowSpeed = bodyFollowSpeed + .5f; // Kuyruklarýn ne kadar hýzlý takip edeceðini belirleyen hýz
    turnSpeed = turnSpeed + 3;
    distanceIndex++;
    bodyDistance = bodyDistances[distanceIndex];
    }   
    public void SpeedDownn()
    {
        Debug.Log(playerSpeed);
        if (playerSpeed < 15.2)
            return;
        UpdateParticleSystem(speedLinesEffectIn, -4f, .3f);  // In efekti için
        UpdateParticleSystem(speedLinesEffectOut, -4f, .3f); // Out efekti için
        playerSpeed = playerSpeed - .5f;
        moveSpeed = moveSpeed - .5f;
        bodyFollowSpeed = bodyFollowSpeed - .5f; // Kuyruklarýn ne kadar hýzlý takip edeceðini belirleyen hýz
        turnSpeed = turnSpeed - 3;
        distanceIndex--;
        bodyDistance = bodyDistances[distanceIndex];
    }

    public void GetColorAndNumber(int wagonnumber, Color wagonColor)
    {
        number = wagonnumber;
        color = wagonColor;
    }
    public void UpdateUI()
    {
        for (int i = 0; i < numberTexts.Count; i++)
        {
            
            
            if(i< wagonNumbers.Count)
            {
                if (wagonNumbers[i] > 3000)
                {
                    
                    numberTexts[i].text = (wagonNumbers[i] /1000).ToString() +"K";
                }
                else
                {
                    numberTexts[i].text = wagonNumbers[i].ToString();
                }
                
                int number = (int)Mathf.Log(wagonNumbers[i], 2) - 1;
                numberImage[i].color = numberUiColor[number];        
            }
            else
            {
                numberTexts[i].text = "";
                numberImage[i].color = Color.red;
            }
        }
    }
    // Particle System bileþenine referans

    private void UpdateParticleSystem(ParticleSystem particleSystem, float speedDelta, float radiusDelta)
    {
        // Main Module'e eriþip startSpeed'i artýr/azalt
        var mainModule = particleSystem.main;
        mainModule.startSpeed = Mathf.Max(0, mainModule.startSpeed.constant + speedDelta); // Hýzýn negatif olmamasýný saðla

        // Shape Module'e eriþip radius'u artýr/azalt
        var shapeModule = particleSystem.shape;
        shapeModule.radius = Mathf.Max(0, shapeModule.radius + radiusDelta); // Radiusun negatif olmamasýný saðla
    }

    public void AddMoney(int money)
    {
        goldCount = PlayerPrefs.GetInt("GoldAmount") + money;
        gameGoldCount += money;
        goldCountText.text = gameGoldCount.ToString();
        PlayerPrefs.SetInt("GoldAmount", goldCount);
    }


}
