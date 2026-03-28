using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Lander lander))
        {
            Lander.Instance.ResetLander(transform);
            this.gameObject.SetActive(false);
        }
    }
}
