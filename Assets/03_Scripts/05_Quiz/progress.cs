using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class progress : MonoBehaviour
{
    public GameObject quarterAct1, quarterAct2, quarterAct3, quarterAct4;

    public void ShowQ1()
    {
        quarterAct1.SetActive(true);
        quarterAct2.SetActive(false);
        quarterAct3.SetActive(false);
        quarterAct4.SetActive(false);
    }
    public void ShowQ2()
    {
        quarterAct1.SetActive(false);
        quarterAct2.SetActive(true);
        quarterAct3.SetActive(false);
        quarterAct4.SetActive(false);
    }
    public void ShowQ3()
    {
        quarterAct1.SetActive(false);
        quarterAct2.SetActive(false);
        quarterAct3.SetActive(true);
        quarterAct4.SetActive(false);
    }
    public void ShowQ4()
    {
        quarterAct1.SetActive(false);
        quarterAct2.SetActive(false);
        quarterAct3.SetActive(false);
        quarterAct4.SetActive(true);
    }

    public void CloseQ1()
    {
        quarterAct1.SetActive(false);
    }
    public void CloseQ2()
    {
        quarterAct2.SetActive(false);
    }
    public void CloseQ3()
    {
        quarterAct3.SetActive(false);
    }
    public void CloseQ4()
    {
        quarterAct4.SetActive(false);
    }
}
