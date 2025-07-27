using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackHome : MonoBehaviour
{
    public void BackToHome()
    {
        // Load the scene named "Home"
        SceneManager.LoadScene("02_ACCOUNT");
    }
}
