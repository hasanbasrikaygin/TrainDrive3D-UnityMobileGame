using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class TrainChanger : MonoBehaviour
{
    private Vector3 centerPosition = Vector3.zero; // Merkez pozisyon

    public Button rightButton;
    public Button leftButton;
    public TextMeshProUGUI trainName;

    private int selectedTrainIndex = 0;
    public List<GameObject> trainObjects; // Bu listeyi edit�rden atayabilirsiniz.
    private bool isTransitioning = false;
    private bool isRightButtonDown = false;
    private bool isLeftButtonDown = false;

    public float spacing = 5f; // Trenler aras�ndaki bo�luk
    public float transitionSpeed = 5f; // Ge�i� h�z�
    public float swayAmount = .0001f; // Sallanma miktar�
    public float swaySpeed = 10f; // Sallanma h�z�

    [Header("Buy")]
    public Button buyButton;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI currenntPriceText;
    public GameObject selectIcon;
    public List<int> trainPrices; // Tren fiyatlar�n� bu listede tutun
    public MenuUiAnimationController menuUiAnimationController;
    private GameObject currentSwingTrain; // �u an sallanma efektine sahip tren

    void Start()
    {
        selectedTrainIndex = PlayerPrefs.GetInt("SelectedTrainIndex", 0);
        UpdateTrainPositions();
        rightButton.onClick.AddListener(OnRightButtonDown);
        leftButton.onClick.AddListener(OnLeftButtonDown);
        UpdateTargetTrain();
        ApplySwingEffect();
    }

    void UpdateTrainPositions()
    {
        for (int i = 0; i < trainObjects.Count; i++)
        {
            float offset = (i - selectedTrainIndex) * spacing;
            trainObjects[i].transform.localPosition = centerPosition + new Vector3(offset, 0, 0);
        }
    }

    void FixedUpdate()
    {
        if (isRightButtonDown)
        {
            int previousIndex = selectedTrainIndex;
            selectedTrainIndex = (selectedTrainIndex + 1) % trainObjects.Count;
            StartCoroutine(TransitionTrains(selectedTrainIndex));
            isRightButtonDown = false;
            //Debug.Log("selectedindex " + selectedTrainIndex);
            CancelSwingEffect(previousIndex);
            ApplySwingEffect();
        }

        if (isLeftButtonDown)
        {
            int previousIndex = selectedTrainIndex;
            selectedTrainIndex = (selectedTrainIndex - 1 + trainObjects.Count) % trainObjects.Count;
            StartCoroutine(TransitionTrains(selectedTrainIndex));
            
            isLeftButtonDown = false;
            //Debug.Log("selectedindex " + selectedTrainIndex);
            CancelSwingEffect(previousIndex);
            ApplySwingEffect();
        }
    }

    IEnumerator TransitionTrains(int targetIndex)
    {
       
        isTransitioning = true;
        float elapsedTime = 0f;
        Vector3[] startPositions = new Vector3[trainObjects.Count];

        for (int i = 0; i < trainObjects.Count; i++)
        {
            startPositions[i] = trainObjects[i].transform.localPosition;
        }

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed * 8f;
            for (int i = 0; i < trainObjects.Count; i++)
            {
                float offset = (i - selectedTrainIndex) * spacing;
                Vector3 targetPosition = centerPosition + new Vector3(offset, 0, 0);
                trainObjects[i].transform.localPosition = Vector3.Lerp(startPositions[i], targetPosition, elapsedTime);
            }
            yield return null;
        }
        isTransitioning = false;
        UpdateTargetTrain();
        ShowTrainPrice();
    }

    void UpdateTargetTrain()
    {
        transform.position = trainObjects[selectedTrainIndex].transform.position;
        if (PlayerPrefs.GetInt("TrainPurchased_" + selectedTrainIndex, 0) == 1)
        {
            PlayerPrefs.SetInt("SelectedTrainIndex", selectedTrainIndex);
        }
        //Debug.Log("selectedindex " + PlayerPrefs.GetInt("SelectedTrainIndex", 0));
        trainName.text = trainObjects[selectedTrainIndex].name.ToUpper(new CultureInfo("en-US"));

        //ShowTrainPrice();
    }
    public void ShowTrainPrice()
    {
        // BUY

        // Tren fiyat�n� g�ncelle
        int price = trainPrices[selectedTrainIndex];
        priceText.text = price.ToString("N0");

        // Tren sat�n al�nd� m� kontrol et
        bool isPurchased = PlayerPrefs.GetInt("TrainPurchased_" + selectedTrainIndex, 0) == 1;
        buyButton.gameObject.SetActive(!isPurchased);
        selectIcon.SetActive(isPurchased);
    }
    public void BuySelectedTrain()
    {
       
        int userMoney = PlayerPrefs.GetInt("GoldAmount",0);
        int price = trainPrices[selectedTrainIndex];

        if (userMoney >= price)
        {
            // Sat�n alma i�lemi
            PlayerPrefs.SetInt("GoldAmount", userMoney - price);
            PlayerPrefs.SetInt("TrainPurchased_" + selectedTrainIndex, 1);
            UpdateTargetTrain();
            Debug.Log("Train purchased!");
            currenntPriceText.text = PlayerPrefs.GetInt("GoldAmount", 0).ToString();
            MenuAudioManager.Instance.PlayBuySound();
        }
        else
        {
            menuUiAnimationController.PriceTextAnimation();
            Debug.Log("Not enough money!");
            MenuAudioManager.Instance.PlayInsufficientCoinSound();
        }
        ShowTrainPrice();
    }
    void ApplySwingEffect()
    {
        GameObject selectedTrain = trainObjects[selectedTrainIndex];
        LeanTween.rotateZ(selectedTrain, 7f, 0.5f).setEaseInOutSine().setLoopPingPong();
    }
    void CancelSwingEffect(int index)
    {
        LeanTween.cancel(trainObjects[index]);
    }
    void OnRightButtonDown()
    {
        if (!isTransitioning)
        {
            isRightButtonDown = true;
        }
    }

    void OnLeftButtonDown()
    {
        if (!isTransitioning)
        {
            isLeftButtonDown = true;
        }
    }
}
