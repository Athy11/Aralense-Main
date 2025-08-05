using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quarters : MonoBehaviour
{
    public GameObject quarter1, quarter2, quarter3, quarter4;

    public void ShowQ1()
    {
        quarter1.SetActive(true);
        quarter2.SetActive(false);
        quarter3.SetActive(false);
        quarter4.SetActive(false);
    }
    public void ShowQ2()
    {
        quarter1.SetActive(false);
        quarter2.SetActive(true);
        quarter3.SetActive(false);
        quarter4.SetActive(false);
    }
    public void ShowQ3()
    {
        quarter1.SetActive(false);
        quarter2.SetActive(false);
        quarter3.SetActive(true);
        quarter4.SetActive(false);
    }
    public void ShowQ4()
    {
        quarter1.SetActive(false);
        quarter2.SetActive(false);
        quarter3.SetActive(false);
        quarter4.SetActive(true);
    }
    public void LoadQ1()
    {
        SceneManager.LoadScene("04_LESSON AR");
    }

  
}
