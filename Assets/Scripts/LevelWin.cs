using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinManager : MonoBehaviour
{
    public PhysicsButton1[] buttons;
    public Transform player;
    public float fallWinHeight = -5f;

    private bool hasWon = false;

    void Update()
    {
        if (hasWon) return;

        bool allPressed = true;
        foreach (PhysicsButton1 button in buttons)
        {
            if (!button.isPressed)
            {
                allPressed = false;
                break;
            }
        }

        bool playerFell = player.position.y < fallWinHeight;
        if (allPressed && playerFell)
        {
            WinLevel();
        }
    }

    void WinLevel()
    {
        hasWon = true;
        Debug.Log("Level Complete!");

        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
}
