using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject customizationPanel;
    public string levelSceneName = "GameLevel";
    public float defaultRunSpeed = 10f;

    public void PlayGame()
    {
        TrackTile.moveSpeed = defaultRunSpeed;
        SceneManager.LoadScene(levelSceneName);
    }

    public void OpenCustomization()
    {
        mainPanel.SetActive(false);
        customizationPanel.SetActive(true);
    }

    public void CloseCustomization()
    {
        customizationPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}