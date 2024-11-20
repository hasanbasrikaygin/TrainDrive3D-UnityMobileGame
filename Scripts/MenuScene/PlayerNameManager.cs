using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // Ýsim giriþi için InputField

    private void Start()
    {
        // Oyun baþladýðýnda PlayerPrefs'teki adý InputField'a yükleyelim
        if (PlayerPrefs.HasKey("playerName"))
        {
            nameInputField.text = PlayerPrefs.GetString("playerName");
        }
        else
        {
            nameInputField.text = "Unknown"; // Varsayýlan isim
        }

        nameInputField.onValueChanged.AddListener(UpdatePlayerName); // Ýsim her deðiþtiðinde güncelle
    }

    public void UpdatePlayerName(string newName)
    {
        // Ýsmi PlayerPrefs'e kaydet
        PlayerPrefs.SetString("playerName", newName);
        PlayerPrefs.Save();
    }
}
