using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MergeWagon : MonoBehaviour
{

   public  List<GameObject> wagonNumbers;
   public  List<Material> wagonMaterials;
   public  MeshRenderer meshRenderer;
   private int currentWagonNumber;

    public Transform numberPosition;
    public List<GameObject> digitPrefabs; // Her rakam i�in prefablar (0-9)
    private List<GameObject> currentNumberObjects = new List<GameObject>(); // �u anda aktif olan say� objeleri
    public void ApplyWagonNumber(int number)
    {
        ClearCurrentNumberObjects(); // Eski say� objelerini temizle
        if (number <= 2048)
        {
            Debug.Log("//\\---------------//\\ " + number + " //\\---------------//\\");
            ClearCurrentNumberObjects(); // Eski say� objelerini temizle

            currentWagonNumber = number;
            string numberText = number.ToString(); // Say� olarak elde et (2, 4, 8, ...)

            // Her basamak i�in prefab olu�tur
            for (int i = numberText.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(numberText[i].ToString()); // Basama�� elde et
                GameObject digitObject = Instantiate(digitPrefabs[digit], numberPosition); // Prefab� instantiate et

                // Pozisyon ve �l�e�i ayarla
                digitObject.transform.localPosition = GetPositionForDigit(numberText.Length - 1 - i, numberText.Length);
                digitObject.transform.localScale = GetScaleForNumber(numberText.Length);

                // Listeye ekle
                currentNumberObjects.Add(digitObject);
            }
            
                currentWagonNumber = (int)Mathf.Log(currentWagonNumber, 2) - 1;
            if (currentWagonNumber < wagonMaterials.Count)
            {
                // Wagon materyalini ayarla
                meshRenderer.materials[0].color = wagonMaterials[currentWagonNumber].color;
            }
            else
            {
                meshRenderer.materials[0].color = wagonMaterials[0].color;
            }



        }
        else
        {
            //Debug.Log("4kdan b�y�k");
            number /= 1000;
            Debug.Log(number);
            string numberText = number.ToString() +"k";
            for (int i = numberText.Length - 1; i >= 0; i--)
            {
                int digit;
                if (i== numberText.Length-1)
                {
                    digit = 10;
                }
                else
                {
                     digit = int.Parse(numberText[i].ToString()); // Basama�� elde et
                }
                
                GameObject digitObject = Instantiate(digitPrefabs[digit], numberPosition); // Prefab� instantiate et

                // Pozisyon ve �l�e�i ayarla
                digitObject.transform.localPosition = GetPositionForDigit(numberText.Length - 1 - i, numberText.Length);
                digitObject.transform.localScale = GetScaleForNumber(numberText.Length);

                // Listeye ekle
                currentNumberObjects.Add(digitObject);
            }
            currentWagonNumber = (int)Mathf.Log(currentWagonNumber, 2) - 1;
            if (currentWagonNumber < wagonMaterials.Count)
            {
                meshRenderer.materials[0].color = wagonMaterials[currentWagonNumber].color;
            }
            else
            {
                meshRenderer.materials[0].color = wagonMaterials[0].color;
            }
            // Wagon materyalini ayarla
            
        }
        
    }

    private void ClearCurrentNumberObjects()
    {
        // Mevcut say� objelerini yok et ve listeyi temizle
        foreach (GameObject obj in currentNumberObjects)
        {
            Destroy(obj);
        }
        currentNumberObjects.Clear();
    }

    private Vector3 GetPositionForDigit(int digitIndex, int totalDigits)
    {
        // Basamak indexine g�re pozisyon hesaplama (basit bir �rnek)
        float spacing = .8f; // Basamaklar aras� mesafe
        float startPosition = -(totalDigits - 1) * spacing / 2;
        return new Vector3(0, 0, startPosition + digitIndex * spacing);
    }

    private Vector3 GetScaleForNumber(int digitCount)
    {
        
        // Say�n�n uzunlu�una g�re �l�ek hesaplama
        if (digitCount == 1) return new Vector3(1f, 1.8f, 1.8f); // Tek haneli say�lar i�in normal boyut
        if (digitCount == 2) return new Vector3(1f, 1.3f, 1.3f); // �ki haneli say�lar i�in daha k���k
        if (digitCount == 3) return new Vector3(1f, .9f, .9f); // �ki haneli say�lar i�in daha k���k
        if (digitCount == 4) return new Vector3(1f, .5f, .5f); // �ki haneli say�lar i�in daha k���k
        // Di�er �l�ek ayarlamalar�...
        return Vector3.one;
    }
    public void UpgradeWagonNumber(int newWagonNumber)
    {
        ApplyWagonNumber(newWagonNumber * 2);
    }


}
//2
//4 
//8 
//16 
//32 
//64 
//128 
//256 
//512 
//1024 
//2048