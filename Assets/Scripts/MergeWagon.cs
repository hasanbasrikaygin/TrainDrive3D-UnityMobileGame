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
    public List<GameObject> digitPrefabs; // Her rakam için prefablar (0-9)
    private List<GameObject> currentNumberObjects = new List<GameObject>(); // Þu anda aktif olan sayý objeleri
    public void ApplyWagonNumber(int number)
    {
        ClearCurrentNumberObjects(); // Eski sayý objelerini temizle
        if (number <= 2048)
        {
            Debug.Log("//\\---------------//\\ " + number + " //\\---------------//\\");
            ClearCurrentNumberObjects(); // Eski sayý objelerini temizle

            currentWagonNumber = number;
            string numberText = number.ToString(); // Sayý olarak elde et (2, 4, 8, ...)

            // Her basamak için prefab oluþtur
            for (int i = numberText.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(numberText[i].ToString()); // Basamaðý elde et
                GameObject digitObject = Instantiate(digitPrefabs[digit], numberPosition); // Prefabý instantiate et

                // Pozisyon ve ölçeði ayarla
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
            //Debug.Log("4kdan büyük");
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
                     digit = int.Parse(numberText[i].ToString()); // Basamaðý elde et
                }
                
                GameObject digitObject = Instantiate(digitPrefabs[digit], numberPosition); // Prefabý instantiate et

                // Pozisyon ve ölçeði ayarla
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
        // Mevcut sayý objelerini yok et ve listeyi temizle
        foreach (GameObject obj in currentNumberObjects)
        {
            Destroy(obj);
        }
        currentNumberObjects.Clear();
    }

    private Vector3 GetPositionForDigit(int digitIndex, int totalDigits)
    {
        // Basamak indexine göre pozisyon hesaplama (basit bir örnek)
        float spacing = .8f; // Basamaklar arasý mesafe
        float startPosition = -(totalDigits - 1) * spacing / 2;
        return new Vector3(0, 0, startPosition + digitIndex * spacing);
    }

    private Vector3 GetScaleForNumber(int digitCount)
    {
        
        // Sayýnýn uzunluðuna göre ölçek hesaplama
        if (digitCount == 1) return new Vector3(1f, 1.8f, 1.8f); // Tek haneli sayýlar için normal boyut
        if (digitCount == 2) return new Vector3(1f, 1.3f, 1.3f); // Ýki haneli sayýlar için daha küçük
        if (digitCount == 3) return new Vector3(1f, .9f, .9f); // Ýki haneli sayýlar için daha küçük
        if (digitCount == 4) return new Vector3(1f, .5f, .5f); // Ýki haneli sayýlar için daha küçük
        // Diðer ölçek ayarlamalarý...
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