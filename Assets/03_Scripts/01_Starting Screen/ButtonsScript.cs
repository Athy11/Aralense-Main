using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class begin : MonoBehaviour
{
    public GameObject confirmPanel;
    // This will direct to the login/registration scene when the user clicks the "Begin" button.
    public void Begin() {
        SceneManager.LoadScene("02_ACCOUNT");
    }

    // This will exit the application when the user clicks the "Exit" button.
    public void ShowConfirmExit()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);
        }
    }

    // Called when user confirms to quit the app
    public void QuitApp()
    {
        Application.Quit();
       
    }

    // Called when user cancels exit
    public void CancelExit()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }
}


