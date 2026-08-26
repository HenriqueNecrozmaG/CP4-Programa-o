using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        GameManager.Instance.UpdateLives(3);
    }
}
