using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObject lockObject;   // Prefab del candado
    [SerializeField] private Renderer exitRenderer;   // Renderer de la salida

    private bool isUnlocked = false;

    public void UnlockExit()
    {
        isUnlocked = true;

        if (lockObject != null)
            Destroy(lockObject);

        if (exitRenderer != null)
            exitRenderer.material.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUnlocked) return;

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.HasKey())
            {
                Debug.Log("¡Has ganado!");
                //Cargar la escena de nuevo para un nuevo mapa
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
        else
            Debug.Log("Necesitas la llave para salir.");
    }
}
