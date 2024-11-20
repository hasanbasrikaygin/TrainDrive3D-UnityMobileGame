using DG.Tweening;
using System.Buffers.Text;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuUiAnimationController;

public class UIAnimationManager : MonoBehaviour
{

    public Train train;
    public ScoreManager scoreManager;
    public GameObject baseUi;
    [Header("Score")]
    public GameObject scoreCanvas;
    public TextMeshProUGUI continueText;
    public TextMeshProUGUI scoreText;
   

    [Header("Countdown")]
    [SerializeField] private GameObject countdownPanel; // Sayaç için panel 
    [SerializeField] private int countdownTime = 3; // Baþlangýç sayýsý, 3-2-1
    public TextMeshProUGUI countdownText;


    [Header("Transition")]
    [SerializeField] public GameObject gameStartBackground;
    [SerializeField] public GameObject gameStartGG;

    [Header("CleanStartCanvas")]
    [SerializeField] public GameObject cleanStartPanel; 

    [Header("Settings")]
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject settingsBackgroundPanel;

    [Header("2048")]
    [SerializeField] public GameObject mergeCanvas;
    [SerializeField] public GameObject mergePanel;


    private void Start()
    { 
        //ChangeScaleText();
        GGImagesOut();
    }
    //////////////      START                            //////////////
    public void GGImagesOut()
    {
        AnimatePanelSlideBounce(gameStartGG);
        AnimatePanelSlide(gameStartGG, SlideDirection.Right, SlideType.Out, .7f, .6f);
        AnimatePanelFade(gameStartBackground, SlideType.Out, .4f, 1.4f);
        
        
    }

    //////////////      START CLEAN START STATE         //////////////
    public void PanelSlideInOut()
    {
        StartCoroutine(PanelSlide());
    }
    IEnumerator PanelSlide()
    {
        float delay = .3f;
        if (train.mod != 0)
        {
            delay = 1.2f;
        }
        AnimatePanelFade(cleanStartPanel, SlideType.In, .6f, .1f);
        yield return new WaitForSeconds(1f);
        AnimatePanelFade(cleanStartPanel, SlideType.Out, .6f, delay);
    }


    //////////////      GAME FINISH END     //////////////
    public void PanelChangeScene()
    {

        scoreCanvas.SetActive(false);
        StartCoroutine(ChangeSceneAnim());
    }
    IEnumerator ChangeSceneAnim()
    {
        AudioManager.Instance.StopTrainMusic();
        AnimatePanelFade(cleanStartPanel, SlideType.In, .7f, 0f);
        yield return new WaitForSeconds(1f);
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }
    public void SettingsOpen()
    {
        if (train.speedLinesEffectIn.isPlaying)
        {
            train.speedLinesEffectIn.Stop();
        }
        if (train.speedLinesEffectOut.isPlaying)
        {
            train.speedLinesEffectOut.Stop();
        }
        if (train.mod != 0)
        {
            AnimatePanelSlide(mergePanel, SlideDirection.Top, SlideType.Out, .1f, .5f);
        }
        train.TransitionToState(train.IdleState);
        scoreManager.isGameOver = true;
        AnimatePanelFade(settingsBackgroundPanel, SlideType.In, .5f, .3f);
        AnimatePanelSlide(settingsPanel, SlideDirection.Bottom, SlideType.In, 1f, .8f);
    }
    public void SettingsClose()
    {
        StartCountdown();
        AnimatePanelFade(settingsBackgroundPanel, SlideType.Out, .5f, .6f);
        AnimatePanelSlide(settingsPanel, SlideDirection.Bottom, SlideType.Out, .5f, .4f);
        if (train.mod != 0)
        {
            AnimatePanelSlide(mergePanel, SlideDirection.Top, SlideType.In, .7f, .5f);
        }
    }
    public void ChangeScaleText()
    {
       scoreText.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    //////////////      3  -  2  -  1  -  GO!       //////////////
    public void StartCountdown()
    {
        scoreManager.isGameOver = true;
        countdownPanel.SetActive(true); // Paneli aktif hale getir
        countdownText.transform.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        baseUi.SetActive(false);
        yield return new WaitForSeconds(1); // 1 saniye bekle
        // Geri sayýmý baþlat
        AudioManager.Instance.PlayCountdown();
        for (int i = countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString(); // Sayýyý ekranda göster
            yield return new WaitForSeconds(1); // 1 saniye bekle
        }
        // "GO!" yazýsý
        countdownText.text = "GO!";
        scoreManager.isGameOver = false;
        train.TransitionToState(train.MoveState);
        if (train.speedLinesEffectIn !=null)
        {
            train.speedLinesEffectIn.Play();
        }
        yield return new WaitForSeconds(1); // 1 saniye "GO!" yazýsýný göster
        baseUi.SetActive(true);
        countdownPanel.SetActive(false); // Paneli gizle
        countdownText.text = "";
        if (train.mod != 0)
        {
            mergeCanvas.SetActive(true);
        }
    }
    public void AnimatePanelSlide(GameObject panel, SlideDirection direction, SlideType type, float duration = 0.5f, float delay = 0f, Vector2? customTargetPosition = null, LeanTweenType? tweenType = null)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();

        // Hedef pozisyonu customTargetPosition parametresine göre belirle
        Vector2 targetPosition = customTargetPosition ?? rectTransform.anchoredPosition;
        Vector2 startPosition = targetPosition;

        // Baþlangýç ve hedef pozisyonlarý ayarla
        switch (direction)
        {
            case SlideDirection.Left:
                startPosition.x = type == SlideType.In ? -Screen.width : targetPosition.x;
                targetPosition.x = type == SlideType.Out ? -Screen.width : targetPosition.x;
                break;
            case SlideDirection.Right:
                startPosition.x = type == SlideType.In ? Screen.width : targetPosition.x;
                targetPosition.x = type == SlideType.Out ? Screen.width : targetPosition.x;
                break;
            case SlideDirection.Top:
                startPosition.y = type == SlideType.In ? Screen.height : targetPosition.y;
                targetPosition.y = type == SlideType.Out ? Screen.height : targetPosition.y;
                break;
            case SlideDirection.Bottom:
                startPosition.y = type == SlideType.In ? -Screen.height : targetPosition.y;
                targetPosition.y = type == SlideType.Out ? -Screen.height : targetPosition.y;
                break;
        }

        // Giriþ için paneli etkinleþtir ve baþlangýç pozisyonuna taþý
        if (type == SlideType.In)
        {
            panel.SetActive(true);
            rectTransform.anchoredPosition = startPosition;
        }

        // Ýsteðe baðlý tweenType varsa onu, yoksa varsayýlan tweenType'ý kullan
        LeanTweenType selectedTweenType = tweenType ?? (type == SlideType.In ? LeanTweenType.easeOutExpo : LeanTweenType.easeInExpo);

        // Animasyonu baþlat
        LeanTween.move(rectTransform, targetPosition, duration)
            .setDelay(delay)
            .setEase(selectedTweenType)
            .setOnComplete(() =>
            {
                if (type == SlideType.Out)
                {
                    // Çýkýþ tamamlandýðýnda paneli gizle ve pozisyonunu sýfýrla
                    panel.SetActive(false);
                    rectTransform.anchoredPosition = startPosition;
                }
            });
    }
    // Panel karartarak açma/kapama animasyonu
    public void AnimatePanelFade(GameObject panel, SlideType type, float duration = 0.5f, float delay = 0f)
    {
        panel.SetActive(true);
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }

        float targetAlpha = type == SlideType.In ? 1f : 0f;

        if (type == SlideType.In)
        {
            panel.SetActive(true);
            canvasGroup.alpha = 0; // Baþlangýç alfa deðeri
        }

        LeanTween.alphaCanvas(canvasGroup, targetAlpha, duration)
            .setDelay(delay)
            .setEase(type == SlideType.In ? LeanTweenType.easeOutQuad : LeanTweenType.easeInQuad)
            .setOnComplete(() =>
            {
                if (type == SlideType.Out)
                {
                    panel.SetActive(false); // Animasyon bitince paneli kapat
                }
            });
    }
    // Bounce etkisi ve opsiyonel döndürme ile panel kaydýrma fonksiyonu
    public void AnimatePanelSlideBounce(GameObject panel)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();


        LeanTween.rotateZ(rectTransform.gameObject, -180f, 0.4f).setEase(LeanTweenType.easeInOutQuad);
    }
    // Buton týklama animasyonu
    public void AnimateButtonPress(Button button)
    {
        // Týklanan butonun GameObject'ini alýyoruz ve animasyonu baþlatýyoruz
        LeanTween.scale(button.gameObject, Vector3.one * 0.9f, 0.1f)
            .setEase(LeanTweenType.easeOutCubic)
            .setLoopPingPong(1);
    }
}
