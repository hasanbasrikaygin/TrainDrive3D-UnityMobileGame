using System.Collections;
using UnityEngine;

public class WagonFood : MonoBehaviour, IItem
{
    private AudioSource audioSource;
    public float shrinkDuration = 2f; // Nesnenin küçülme süresi
    public float shrinkPercentage = 0.7f; // Küçülme yüzdesi
    private Vector3 originalScale; // Baþlangýç ölçeði
    public Animator animator; // Nesneye baðlý animatör
    public Train train;
    void Awake()
    {
        // Nesnenin baþlangýç ölçeðini kaydediyoruz
        //originalScale = transform.localScale;
        // Animator bileþenini alýyoruz
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
            Debug.Log("Büyüyoruz");
            Debug.Log("girdi 1111");
            //PlayAnimationAndShrink();
            AudioManager.Instance.PlayItemCollect();
            train.TransitionToState(train.GrowState);
            Debug.Log("çýktý 1111");
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

    //    // Nesnenin küçülmesi ve yok olmasý için Coroutine baþlatýyoruz
    //    //StartCoroutine(ShrinkAndDestroy());
    //}

    // Nesneyi belirli bir sürede küçült ve yok et
    //IEnumerator ShrinkAndDestroy()
    //{
    //    Debug.Log("ShrinkAndDestroy baþlýyor.");

    //    yield return new WaitForSeconds(0.3f); // Küçülmeden önce bir süre bekleme
    //    //float elapsedTime = 0f;
    //    //float deltaTime = Time.deltaTime * shrinkDuration; // Küçülme süresi boyunca geçen zaman
    //    //Vector3 targetScale = originalScale * shrinkPercentage; // Küçülme hedefi
    //    //Vector3 newScale;

    //    //// Küçülme iþlemi belirlenen süre boyunca devam eder
    //    //while (elapsedTime < shrinkDuration)
    //    //{
    //    //    float currentShrink = Mathf.Clamp(transform.localScale.x * (1 - deltaTime), targetScale.x, originalScale.x);
    //    //    newScale = new Vector3(currentShrink, currentShrink, currentShrink);
    //    //    transform.localScale = newScale;

    //    //    elapsedTime += Time.deltaTime;
    //    //    yield return null;
    //    //}

    //    // Küçülme tamamlandýktan sonra nesneyi gizle
        
    //    transform.localScale = originalScale; // Nesne tekrar eski boyutuna döner
    //    Debug.Log("ShrinkAndDestroy tamamlandý.");
    //}
}

