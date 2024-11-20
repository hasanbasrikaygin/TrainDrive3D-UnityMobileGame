using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.SceneManagement;

public class SlowMotion : MonoBehaviour, IItem
{
public ItemBarManager barManager;
public GameObject slowMotionButton;
public Train train;

    public void Hide()
{
    gameObject.SetActive(false);
}

public void Initialize()
{
}
public void MakeSound()
{

}

public void MoveTo(Vector3 newPosition)
{
    transform.position = newPosition;
}

public void Show()
{
    gameObject.SetActive(true);
}
private void OnTriggerEnter(Collider other)
{
    if (other.tag == "Train")
    {
            slowMotionButton.SetActive(true);
            AudioManager.Instance.PlaySlowMotion();
            Hide();
    }

}
    public void ActiveSlowMotion()
    {

        barManager.ActivateBar("slowMotion", 1f);
        slowMotionButton.SetActive(false);
    }
}
