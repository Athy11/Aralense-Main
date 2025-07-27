using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navbar : MonoBehaviour
{
    public GameObject settingsPanel;
    
    public void BackToHome()
    {
        // Load the scene named "02_ACCOUNT"
        SceneManager.LoadScene("02_ACCOUNT");
    }

    public void OpenSettings()
    {
        // Toggle the settings panel visibility
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void Leaderboards()
    {
        // Load the scene named "06_LEADERBOARD"
        SceneManager.LoadScene("06_LEADERBOARD");
    }


    public void Chatbot()
    {
        // Load the scene named "03_SEARCH"
        SceneManager.LoadScene("03_SEARCH");
    }
}
