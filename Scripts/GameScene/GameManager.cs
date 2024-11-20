using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ChestManager;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    private int mod = 0;
    public GameObject stationChef;
    public GameObject passengerController;
    public GameObject stationPoints;
    public GameObject foodFactory;

    public GameObject mergeWagonPool;
    public GameObject mergeCanvas;
    public GameObject wagonFoodFactory;

    public Train train;

    public GameObject gameOverCanvas;
    public GameObject reverseTailPanel;
    public GameObject adsManagerPanel;
    public GameObject wagonCountUi;

    public Button freshStart;
    public Button reverseTail;
    public Button next;

    public ScoreManager scoreManager;
    public UIAnimationManager uiAnimationManager;
    public GoldValues goldValues;

    public TextMeshProUGUI totalgoldCount;
    public ChestAdsManager chestAdsManager;
    public GameObject noInternetPanel;
    public GameObject noInternetImage;
    private void Awake()
    {

        gameOverCanvas.SetActive(false);
        mod = PlayerPrefs.GetInt("gameModNumber", 0);
        if (mod == 0)
        {
            BasicMod();
        }
        else
        {
            MergeMod();
        }
        

        //if (_instance == null)
        //{
        //    _instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void BasicMod()
    {
        stationChef.SetActive(true);
        passengerController.SetActive(true);
        stationPoints.SetActive(true);
        foodFactory.SetActive(true);

        wagonFoodFactory.SetActive(false);
        mergeWagonPool.SetActive(false);
        mergeCanvas.SetActive(false);
        wagonCountUi.SetActive(false);
    }
    public void MergeMod()
    {
        stationChef.SetActive(false);
        passengerController.SetActive(false);
        stationPoints.SetActive(false);
        foodFactory.SetActive(false);


        wagonFoodFactory.SetActive(true);
        mergeWagonPool.SetActive(true);
        //mergeCanvas.SetActive(true);
        //wagonCountUi.SetActive(true);

    }
    public void GameOver()
    {
        scoreManager.StopScore();
        train.TransitionToState(train.IdleState);
        adsManagerPanel.SetActive(true);
        if (train.bodyParts.Count < 1)
        {
            reverseTailPanel.SetActive(false);
        }

        gameOverCanvas.SetActive(true);
        totalgoldCount.text = PlayerPrefs.GetInt("GoldAmount", 0).ToString();
        
        

        // game over ui 
        // diðer tüm stateler duracak
    }
    public void FreshStart()
    {
        int goldCount = goldValues.cleanStartCost;
        if (CheckGoldCount(goldCount))
        {
            //gameOverCanvas.SetActive(false);
            reverseTailPanel.SetActive(true);
            gameOverCanvas.SetActive(false);
            train.animationManager.PanelSlideInOut();
            StartCoroutine(FreshStartDelay());
        }
        else
        {
            PriceTextAnimation();
            UnityEngine.Debug.Log("You dont have enough money");
        }
    }
        IEnumerator FreshStartDelay()
    {
        
        yield return new WaitForSeconds(1f);
        train.TransitionToState(train.CleanStartState);
    }

    
    public void ReverseTail()
    {
        int goldCount = goldValues.directionChangeCost;
        if (CheckGoldCount(goldCount))
        {
            train.TransitionToState(train.RewindState);
            gameOverCanvas.SetActive(false);
            //gameOverCanvas.SetActive(false);
        }
        else
        {
            PriceTextAnimation();
            //Debug.Log("You dont have enough money");
        }

    }
    public bool CheckGoldCount(int goldCount)
    {
        
        int currentGold = PlayerPrefs.GetInt("GoldAmount", 0);
        //Debug.Log("current gold : " + currentGold);
        if (currentGold > goldCount) 
        {
            currentGold -= goldCount;
            //Debug.Log("current gold : " + currentGold);
            PlayerPrefs.SetInt("GoldAmount", currentGold);
            return true;
        }
        return false;
    }
    public void PauseGame(bool isPause)
    {
        if (isPause)
        {
            scoreManager.isGameOver=true;
            train.TransitionToState(train.IdleState);
            
        }
        else
        {
            scoreManager.isGameOver = false;
            uiAnimationManager.StartCountdown();
        }
    }
    public void CollectChest()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            

            if (chestAdsManager.LoadRewardedAddIsNull())
            {
                chestAdsManager.LoadRewardedAd();
            }
            chestAdsManager.ShowRewardedAd(goldValues.adsGold);
            adsManagerPanel.SetActive(false);
        }
        else
        {
            noInternetPanel.SetActive(true);
            NoInterNetAnim(noInternetImage);
            Debug.Log("Ýnternet baðlantýsý yok. Reklam gösterilemedi.");
        }

    }
    public void NoInterNetAnim(GameObject obj)
    {

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 1.3f;

        // Önce butonu büyüt
        LeanTween.scale(obj, targetScale, 0.3f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(obj, originalScale, 0.3f)
                              .setEase(LeanTweenType.easeInOutQuad)
                              .setOnComplete(() =>
                              {
                                  // 1 saniye sonra paneli kapat
                                  LeanTween.delayedCall(.5f, () =>
                                  {
                                      noInternetPanel.SetActive(false);
                                  });
                              });
                 });
    }


    public void PriceTextAnimation()

    {
        Color originalColor = totalgoldCount.color;
        Vector3 originalScale = totalgoldCount.transform.localScale;

        // Renk deðiþikliði ve geri dönüþ
        LeanTween.value(totalgoldCount.gameObject, originalColor, Color.red, 0.3f).setOnUpdate((Color val) =>
        {
            totalgoldCount.color = val;
        }).setOnComplete(() =>
        {
            LeanTween.value(totalgoldCount.gameObject, Color.red, originalColor, 0.3f).setOnUpdate((Color val) =>
            {
                totalgoldCount.color = val;
            });
        });

        // Boyut deðiþikliði ve geri dönüþ
        LeanTween.scale(totalgoldCount.gameObject, originalScale * 1.2f, 0.3f).setEasePunch().setOnComplete(() =>
        {
            LeanTween.scale(totalgoldCount.gameObject, originalScale, 0.3f).setEasePunch();
        });
    }
}
