using UnityEngine;

public class Key : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.CollectKey();
            Destroy(gameObject);
        }
    }
}
