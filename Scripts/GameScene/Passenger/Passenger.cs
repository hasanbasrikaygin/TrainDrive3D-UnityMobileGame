using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isRunning = false;
    public Transform target; // Hareket edilecek hedef (Tren)
    public float speed = 5f; // Hareket hýzý
    public float flipDistance = 2f; // Flip animasyonu için mesafe eþiði
    public GameObject targetObj;
    private Vector3 movePassengerStationCenter;
    private Vector3 passengerWaitPoint;
    public float moveDuration = 5f; // Hareket süresi
    public GameObject coinEffect;
    public List<Material> materials;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Train train;
    //private int passengerGold=10;
    //private int currentGoldCount = 10;

    //public TextMeshProUGUI goldCountText;
    //public int gameGoldCount = 0;
    int randomColor;
    void Start()
    {
        passengerWaitPoint = transform.position;
        movePassengerStationCenter = passengerWaitPoint + new Vector3(-4, 0, 0);
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        randomColor = Random.Range(0, materials.Count);
        skinnedMeshRenderer.material.color = materials[randomColor].color;
        //goldCountText.text = gameGoldCount.ToString();
    }

    void Update()
    {
        //currentGoldCount = PlayerPrefs.GetInt("GoldAmount") + 10;
        //gameGoldCount += 10;
        //goldCountText.text = gameGoldCount.ToString();
        //PlayerPrefs.SetInt("GoldAmount", currentGoldCount);

        if (isRunning)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;
        speed = train.playerSpeed+2;
        Vector3 direction = (target.position - transform.position).normalized;
        transform.LookAt(target);
        transform.position += direction * speed * Time.deltaTime;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < flipDistance)
        {

            animator.SetBool("isFlip", true);
            animator.SetBool("isRunning", false);
        }

        if (distanceToTarget < 0.1f) // Hedefe ulaþtýysa
        {


            animator.SetBool("isFlip", false);
            animator.SetBool("isWalking", true);
            isRunning = false;
            gameObject.transform.position = movePassengerStationCenter;
            gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            coinEffect.transform.position = target.position;
            coinEffect.GetComponentInChildren<ParticleSystem>().Play();
            AudioManager.Instance.PlayPassengerPickup();
            randomColor = Random.Range(0, materials.Count);
            skinnedMeshRenderer.material.color = materials[randomColor].color;
            StartCoroutine(MoveToWaitPoint());
        }
    }

    public void PassengerMove(Vector3 position)
    {
        target = targetObj.transform;
        animator.SetBool("isRunning", true);
        isRunning = true;
    }

    IEnumerator MoveToWaitPoint()
    {
        yield return new WaitForSeconds(3f); // 3 saniye bekle

        Vector3 startPosition = transform.position;
        Vector3 endPosition = passengerWaitPoint;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if(elapsedTime > moveDuration)
        {
            transform.position = endPosition; // Kesin konuma yerleþtir
            animator.SetBool("isWalking", false);
            PassengerPool.Instance.ReturnPassenger(gameObject);
        } // Yolcuyu havuza döndür
    }
    public void FinishCheering()
    {
        int randomNumber = Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                animator.SetBool("Charge",true);

                break;
            case 1:
                animator.SetBool("Cheering",true);
                break;
            case 2:
                animator.SetBool("Clapping",true);
                break;
            default:
                animator.SetBool("Clapping", true);
                break;
        }
            
        
    }
}
