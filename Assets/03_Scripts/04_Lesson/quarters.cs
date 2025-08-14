using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quarters : MonoBehaviour
{
    
    public void LoadQ1()
    {
        SceneManager.LoadScene("04_LESSON AR");
    }

    public void LoadQ2()
    {
        SceneManager.LoadScene("04_LESSONQ2");
    }
    public void LoadQ3()
    {
        SceneManager.LoadScene("04_LESSONQ3");
    }
    public void LoadQ4()
    {
        SceneManager.LoadScene("04_LESSONQ4");
    }


}
