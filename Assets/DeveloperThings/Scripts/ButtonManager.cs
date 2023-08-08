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

        int currentLevelSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
            GameManager.Instance.SetLastScene(SceneManager.GetSceneByBuildIndex(rand).buildIndex);


        }
        else
        {


            SceneManager.LoadScene(currentLevelSceneIndex + 1);
            GameManager.Instance.SetLastScene(SceneManager.GetSceneByBuildIndex(currentLevelSceneIndex + 1).buildIndex);

        }
        GameManager.Instance.IncreaseLevel();




    }

}

