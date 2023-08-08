using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    // [SerializeField] private GameObject loaderCanvas;
    // [SerializeField] private Image loadProgressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadLastScene();
    }
    public async void LoadScene(int sceneIndex)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;
        // loaderCanvas.SetActive(true);

        // do
        // {

        //     loadProgressBar.fillAmount = scene.progress;
        //     await Task.Yield();

        // } while (scene.progress < 0.9f);
        // await Task.Yield();
        scene.allowSceneActivation = true;
        // loaderCanvas.SetActive(false);

    }
    void LoadLastScene()
    {
        int lastSceneIndex = GameManager.Instance.GetLastScene();
        if (lastSceneIndex != 0)
            LoadScene(GameManager.Instance.GetLastScene());

    }

}
