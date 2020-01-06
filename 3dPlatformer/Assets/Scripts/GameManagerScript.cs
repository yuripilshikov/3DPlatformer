using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //static instance, can be accessed from anywhere
    public static GameManagerScript instance = null;

    public int score = 0;
    public int highScore = 0;

    public int currentLevel = 1;
    public int highestLevel = 2;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        else if(instance != this)
        {
            Destroy(gameObject);

        }

        //do not destroy when loading scenes
        DontDestroyOnLoad(gameObject);
    }

    

    public void IncreaseScore(int amount)
    {
        score += amount;
        if(score > highScore)
        {
            highScore = score;
        }
    }

    public void ResetGame()
    {
        score = 0;
        currentLevel = 1;

        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void IncreaseLevel()
    {
        if(currentLevel < highestLevel)
        {
            currentLevel++;
        } else
        {
            currentLevel = 1;
        }
        SceneManager.LoadScene("Level" + currentLevel);
    }
}
