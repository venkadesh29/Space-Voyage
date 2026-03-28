using UnityEngine;

public class PushLander : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 forceDirection = Vector2.right;
    [SerializeField] private float forceMagnitude = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lander lander))
        {
           rb = lander.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if(trigger.gameObject.TryGetComponent(out Lander lander))
        {
            rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lander lander))
        {
            rb = null;
        }
    }
}
