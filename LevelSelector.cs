using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] public int levelIndex; // Add this variable to specify the level index
    [SerializeField] public bool unlocked;
    public int LevelNumber;

    public GameObject lockedEffect;
    public GameObject unlockedEffect;

    public string sceneName; // The name of the scene this trigger corresponds to
    public Button levelButton; // Reference to the button in the UI


    private void Update()
    {
        UpdateEffect();
    }

    private void Start()
    {
        // Initially hide the UI elements
        levelButton.gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && unlocked)
        {
            // Show the UI elements when the player enters the trigger
            levelButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the UI elements when the player exits the trigger
            levelButton.gameObject.SetActive(false);
        }
    }

    public void LoadLevel()
    {
        // Load the corresponding scene when the button is clicked
        if (unlocked)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void UpdateEffect()
    {
        if (!unlocked)
        {
            unlockedEffect.gameObject.SetActive(false);
            lockedEffect.gameObject.SetActive(true);
        }
        else
        {
            lockedEffect.gameObject.SetActive(false);
            unlockedEffect.gameObject.SetActive(true);
        }
    }
}

