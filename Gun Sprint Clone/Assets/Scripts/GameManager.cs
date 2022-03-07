using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject finishPanel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Finish()
    {
        finishPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadScene(string sceneName)
    {
        if (sceneName == "Exit")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
