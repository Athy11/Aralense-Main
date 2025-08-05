using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navbar : MonoBehaviour
{
    public GameObject settingsPanel;
 

    public void BackToHome()
    {
        // Store which panel to open when the scene loads
        PlayerPrefs.SetString("PanelToOpen", "Home");
        PlayerPrefs.Save();

        // Load the scene
        SceneManager.LoadScene("02_ACCOUNT");
    }

    public void Profile()
    {
        // Store which panel to open when the scene loads
        PlayerPrefs.SetString("PanelToOpen", "Profile");
        PlayerPrefs.Save();

        // Load the scene
        SceneManager.LoadScene("02_ACCOUNT");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void Leaderboards()
    {
        SceneManager.LoadScene("06_LEADERBOARD");
    }

    public void Chatbot()
    {
        SceneManager.LoadScene("03_SEARCH");
    }

    public void Logout()
    {
        LoginRegister loginRegister = FindObjectOfType<LoginRegister>();
        if (loginRegister != null)
        {
            loginRegister.LogOut();
        }
        else
        {
            Debug.LogError("LoginRegister script not found in scene!");
        }
    }
}
