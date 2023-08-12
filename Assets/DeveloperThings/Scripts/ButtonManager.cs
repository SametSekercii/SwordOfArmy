using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{


    public void FailButton()
    {
        int failedLevelSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(failedLevelSceneIndex);
    }
    public void NextLevelButton()
    {
        GameManager.Instance.IncreaseLevel();
        SceneManager.LoadScene(GameManager.Instance.GetPlayerLevel());




    }

    

}

