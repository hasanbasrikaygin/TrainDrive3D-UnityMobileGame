using UnityEngine;

public class Obstacle : MonoBehaviour, IItem
{
    public GameManager gameManager;
    public Train train;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        // Engelin baþlatýlmasý için gerekli kod
    }

    public void MakeSound()
    {
        
    }

    public void MoveTo(Vector3 newPosition)
    {
        // Engeli yeni konuma taþýma kodu
        transform.position = newPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Train")
        {
            if (!train.isShieldActive)
            {
                AudioManager.Instance.PlayItemCollect();
                Debug.Log("Game Over");
                gameManager.GameOver();
                //train.TransitionToState(train.GrowState);
                
            }       
            
        }
    }
}
