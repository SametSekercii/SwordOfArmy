using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{


    public void FailButton()
    {
        int failedLevelSceneIndex = GameManager.Instance.GetLastScene();
        SceneManager.LoadScene(failedLevelSceneIndex);
    }
    public void NextLevelButton()
    {

        int currentLevelSceneIndex = GameManager.Instance.GetLastScene();
        if (GameManager.Instance.GetPlayerLevel() >= 10)
        {
            int rand = Random.Range(1, 9);
            if (rand == currentLevelSceneIndex)
            {
                while (rand == currentLevelSceneIndex)
                {
                    rand = Random.Range(1, 9);
                }
            }
            SceneManager.LoadScene(rand);
        }
        else SceneManager.LoadScene(currentLevelSceneIndex + 1);
        GameManager.Instance.IncreaseLevel();




    }

}

