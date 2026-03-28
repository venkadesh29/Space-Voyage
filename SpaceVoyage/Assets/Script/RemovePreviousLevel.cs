using Unity.VisualScripting;
using UnityEngine;

public class RemovePreviousLevel : MonoBehaviour
{
    [SerializeField] private GameObject previousLvl;
    [SerializeField] private BoxCollider2D boxCollider2D;
    private void Start()
    {
        previousLvl = transform.parent.gameObject;
    }

    private void OnTriggerExit2D(Collider2D trigger2D)
    {
        if (trigger2D.gameObject.TryGetComponent(out Lander lander))
        {
            Destroy(previousLvl);
            boxCollider2D.isTrigger = false;
        }
    }
}
