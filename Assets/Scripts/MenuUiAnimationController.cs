using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuUiAnimationController : MonoBehaviour
{
    [Header("Base Ui")]


    [SerializeField] public GameObject leftPanel;
    [SerializeField] public GameObject rightPanel;
    [SerializeField] public GameObject bottomPanel;

    [SerializeField] private Button outfit;
    [SerializeField] private Button chest;
    [SerializeField] private Button scoreboard;
    [SerializeField] private Button settings;
    [SerializeField] private Button mod1;
    [SerializeField] private Button mod2;

    [Header("Settings")]
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject settingsPanelBackgorund;
    [SerializeField] private Button settingsExit;

    [Header("Chest")]
    [SerializeField] public GameObject chestPanel;
    [SerializeField] public GameObject chestBackgroundPanel;
    [SerializeField] private Button chestExit;

    [Header("Score")]
    [SerializeField] public GameObject scoreboardPanel;
    [SerializeField] public GameObject backgroundScoreboardPanel;
    [SerializeField] private Button scoreExit;
    [Header("Outfit")]
    [SerializeField] public GameObject outfitTop;
    [SerializeField] public GameObject outfitBottomBuy;
    [SerializeField] public GameObject outfitBottomSelect;
    [SerializeField] public GameObject outfitLeft;
    [SerializeField] public GameObject outfitRight;
    [SerializeField] public GameObject outfitCanvas;
    [SerializeField] public GameObject currentPrice;
    [SerializeField] public TextMeshProUGUI price;
    [SerializeField] private Button outfitExit;

    [Header("Transition")]
    [SerializeField] public GameObject ggBackground;
    [SerializeField] public GameObject leftG;
    [SerializeField] public GameObject rightG;
    [Space]
    [Space]
    [Space]
    public Image transitionShape; // Ortasý boþ olan geçiþ þekli
    public float transitionDuration = 1f;
    public Vector3 largeScale = new Vector3(5f, 5f, 5f); // Baþlangýç boyutu
    public Vector3 smallScale = new Vector3(0.1f, 0.1f, 0.1f); // Küçük boyut





    public enum SlideDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public enum SlideType
    {
        In,
        Out
    }

    public void Start()
    {

        BaseUiIn(false);

        StartTransition();
    }


    public void BaseUiIn(bool isOutfit)
    {
        if(!isOutfit)
        {
            AnimatePanelSlide(leftPanel, SlideDirection.Left, SlideType.In, .5f, 1f);
        }
        
        AnimatePanelSlide(rightPanel, SlideDirection.Right, SlideType.In, .5f, 1f);
        AnimatePanelSlide(bottomPanel, SlideDirection.Bottom, SlideType.In, .5f, 1f);
        outfit.enabled = true;
        scoreboard.enabled = true;
        chest.enabled = true;
        settings.enabled = true;
        mod1.enabled = true;
        mod2.enabled = true;
    }
    public void BaseUiOut(bool isOutfit)
    {
        outfit.enabled = false;
        scoreboard.enabled = false;
        chest.enabled = false;
        settings.enabled = false;
        mod1.enabled = false;
        mod2.enabled = false;
        if (!isOutfit)
        {
            AnimatePanelSlide(leftPanel, SlideDirection.Left, SlideType.Out, .5f, 0f);
        }
        AnimatePanelSlide(rightPanel, SlideDirection.Right, SlideType.Out, .5f, 0f);
        AnimatePanelSlide(bottomPanel, SlideDirection.Bottom, SlideType.Out, .5f, 0f);
        
    }
    public void SettingsUiIn()
    {
        
        BaseUiOut(false);
        AnimatePanelFade(settingsPanelBackgorund, SlideType.In,.5f,.3f);
        AnimatePanelSlide(settingsPanel, SlideDirection.Bottom, SlideType.In, 1f, .8f);
        settingsExit.enabled = true;

    }   
    public void SettingsUiOut()
    {
        settingsExit.enabled = false;
        BaseUiIn(false) ;
        AnimatePanelFade(settingsPanelBackgorund, SlideType.Out, .5f, .6f);
        AnimatePanelSlide(settingsPanel, SlideDirection.Bottom, SlideType.Out, .5f, .4f);
        
    }
    public void ScoreboardIn()
    {
        BaseUiOut(false);
        AnimatePanelFade(backgroundScoreboardPanel, SlideType.In, .5f, .3f);
        AnimatePanelSlide(scoreboardPanel, SlideDirection.Bottom, SlideType.In, 1f, .8f);
        scoreExit.enabled = true;
    }  
    public void ScoreboardOut()
    {
        scoreExit.enabled = false;
        BaseUiIn(false);
        AnimatePanelFade(backgroundScoreboardPanel, SlideType.Out, .5f, .6f);
        AnimatePanelSlide(scoreboardPanel, SlideDirection.Bottom, SlideType.Out, .5f, .4f);
    }
    public void OutfitIn()
    {
        outfitCanvas.SetActive(true);
        BaseUiOut(true);
        AnimatePanelSlide(outfitLeft, SlideDirection.Left, SlideType.In, .5f, .5f);
        AnimatePanelSlide(outfitRight, SlideDirection.Right, SlideType.In, .5f, .5f);
        AnimatePanelSlide(outfitBottomSelect, SlideDirection.Bottom, SlideType.In, .5f, .5f);
        AnimatePanelSlide(outfitBottomBuy, SlideDirection.Bottom, SlideType.In, .5f, .5f);
        AnimatePanelSlide(outfitTop, SlideDirection.Top, SlideType.In, .5f, .5f);
        outfitExit.enabled = true;

    }  
    public void OutfitOut()
    {
        outfitExit.enabled = false;
        BaseUiIn(true);
        AnimatePanelSlide(outfitLeft, SlideDirection.Left, SlideType.Out, .5f, .2f);
        AnimatePanelSlide(outfitRight, SlideDirection.Right, SlideType.Out, .5f, .2f);
        AnimatePanelSlide(outfitBottomBuy, SlideDirection.Bottom, SlideType.Out, .5f, .2f);
        AnimatePanelSlide(outfitBottomSelect, SlideDirection.Bottom, SlideType.Out, .5f, .2f);
        AnimatePanelSlide(outfitTop, SlideDirection.Top, SlideType.Out, .5f, .2f);
    }

    public void ChestPanelIn()
    {
        BaseUiOut(false);
        AnimatePanelFade(chestBackgroundPanel, SlideType.In, .5f, .3f);
        AnimatePanelSlide(chestPanel, SlideDirection.Left, SlideType.In, .5f, .5f);
        chestExit.enabled = true;
    }   
    public void ChestPanelOut()
    {
        chestExit.enabled = false;
        BaseUiIn(false);
        AnimatePanelFade(chestBackgroundPanel, SlideType.Out, .5f, .6f);
        AnimatePanelSlide(chestPanel, SlideDirection.Right, SlideType.Out, .5f, .2f);
    }
    public void StartTransition()
    {
        AnimatePanelFade(transitionShape.gameObject, SlideType.Out, .5f, .3f);
    }
    public void GGImagesIn()
    {
        AnimatePanelFade(ggBackground, SlideType.In, .4f, .3f);
        AnimatePanelSlide(leftG, SlideDirection.Left, SlideType.In, .7f, .7f, new Vector2(0, 0));
        AnimatePanelSlideBounce(leftG);
    }
    // Buton týklama animasyonu
    public void AnimateButtonPress(Button button)
    {
        // Týklanan butonun GameObject'ini alýyoruz ve animasyonu baþlatýyoruz
        LeanTween.scale(button.gameObject, Vector3.one * 0.9f, 0.1f)
            .setEase(LeanTweenType.easeOutCubic)
            .setLoopPingPong(1);
    }

    // Genel panel kaydýrma animasyonu
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
        panel.SetActive(true);

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


        LeanTween.rotateZ(rectTransform.gameObject, 180f, 0.5f).setDelay(1.4f).setEase(LeanTweenType.easeInOutQuad);
    }
    public void PriceTextAnimation()

    {
        Color originalColor = price.color;
        Vector3 originalScale = price.transform.localScale;

        // Renk deðiþikliði ve geri dönüþ
        LeanTween.value(price.gameObject, originalColor, Color.red, 0.3f).setOnUpdate((Color val) =>
        {
            price.color = val;
        }).setOnComplete(() =>
        {
            LeanTween.value(price.gameObject, Color.red, originalColor, 0.3f).setOnUpdate((Color val) =>
            {
                price.color = val;
            });
        });

        // Boyut deðiþikliði ve geri dönüþ
        LeanTween.scale(price.gameObject, originalScale * 1.2f, 0.3f).setEasePunch().setOnComplete(() =>
        {
            LeanTween.scale(price.gameObject, originalScale, 0.3f).setEasePunch();
        });
    }
}