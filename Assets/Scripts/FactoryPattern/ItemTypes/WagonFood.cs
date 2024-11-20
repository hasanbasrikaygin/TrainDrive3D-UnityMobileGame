using System.Collections;
using UnityEngine;

public class WagonFood : MonoBehaviour, IItem
{
    private AudioSource audioSource;
    public float shrinkDuration = 2f; // Nesnenin k���lme s�resi
    public float shrinkPercentage = 0.7f; // K���lme y�zdesi
    private Vector3 originalScale; // Ba�lang�� �l�e�i
    public Animator animator; // Nesneye ba�l� animat�r
    public Train train;
    void Awake()
    {
        // Nesnenin ba�lang�� �l�e�ini kaydediyoruz
        //originalScale = transform.localScale;
        // Animator bile�enini al�yoruz
        //animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        //particleSystem.SetActive(true);
        //particleSystem?.Stop();
        //particleSystem?.Play();
        //Debug.Log("Food Initialized");
    }

    public void MakeSound()
    {
        //audioSource.Play();
    }

    public void MoveTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag=="Train")
        {
            Train train = other.GetComponent<Train>();
            Debug.Log("B�y�yoruz");
            Debug.Log("girdi 1111");
            //PlayAnimationAndShrink();
            AudioManager.Instance.PlayItemCollect();
            train.TransitionToState(train.GrowState);
            Debug.Log("��kt� 1111");
            Hide();
        }
        
    }
    //public void PlayAnimationAndShrink()
    //{
    //    Debug.Log("girdi 222222222");
    //    if (TryGetComponent(out ResizeableObject component))
    //    {
    //        Debug.Log("girdi 33333333333");
    //        component.TakeDamageAnim();
    //    }
    //    else
    //    {
    //        Debug.Log("girdi 4444444444");
    //    }

    //    // Nesnenin k���lmesi ve yok olmas� i�in Coroutine ba�lat�yoruz
    //    //StartCoroutine(ShrinkAndDestroy());
    //}

    // Nesneyi belirli bir s�rede k���lt ve yok et
    //IEnumerator ShrinkAndDestroy()
    //{
    //    Debug.Log("ShrinkAndDestroy ba�l�yor.");

    //    yield return new WaitForSeconds(0.3f); // K���lmeden �nce bir s�re bekleme
    //    //float elapsedTime = 0f;
    //    //float deltaTime = Time.deltaTime * shrinkDuration; // K���lme s�resi boyunca ge�en zaman
    //    //Vector3 targetScale = originalScale * shrinkPercentage; // K���lme hedefi
    //    //Vector3 newScale;

    //    //// K���lme i�lemi belirlenen s�re boyunca devam eder
    //    //while (elapsedTime < shrinkDuration)
    //    //{
    //    //    float currentShrink = Mathf.Clamp(transform.localScale.x * (1 - deltaTime), targetScale.x, originalScale.x);
    //    //    newScale = new Vector3(currentShrink, currentShrink, currentShrink);
    //    //    transform.localScale = newScale;

    //    //    elapsedTime += Time.deltaTime;
    //    //    yield return null;
    //    //}

    //    // K���lme tamamland�ktan sonra nesneyi gizle
        
    //    transform.localScale = originalScale; // Nesne tekrar eski boyutuna d�ner
    //    Debug.Log("ShrinkAndDestroy tamamland�.");
    //}
}

