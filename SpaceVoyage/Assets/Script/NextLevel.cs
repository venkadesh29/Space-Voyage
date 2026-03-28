using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private GameObject nextLevel;
    [SerializeField] private GameObject spawnLocation;
    
    private void OnTriggerEnter2D(Collider2D triiger)
    {
        if(triiger.gameObject.TryGetComponent(out Lander lander))
        {
            Instantiate(nextLevel, spawnLocation.transform.position, Quaternion.identity);
        }
    }
}
