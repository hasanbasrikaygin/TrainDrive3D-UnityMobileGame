using UnityEngine;

public class ObstacleFactory : Factory
{
    [SerializeField] private Obstacle[] preInstantiatedObstacles; // Sahnedeki önceden var olan engellerin referansý
    private int randomObject = 0;

    public override IItem CreateItem(Vector3 position)
    {
        // Rastgele bir engel seç
        

        // Seçilen engeli yeniden konumlandýr ve aktif hale getir
        if(randomObject> 29){
            Debug.Log(" ======================================");
        }

        Obstacle selectedObstacle = preInstantiatedObstacles[randomObject];
        selectedObstacle.transform.position = position;
        selectedObstacle.gameObject.SetActive(true);

        //selectedObstacle.Initialize();
        //selectedObstacle.MakeSound();
        randomObject++;
        return selectedObstacle;
    }
}
