using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartSceneManager : MonoBehaviour
{

    public TextMeshProUGUI HighScoreText;

    public void StartScene()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void Start()
    {
        GetHighScore();
    }

    public void GetHighScore()
    {
        HighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }



}
