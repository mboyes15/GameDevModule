using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ColourImage;
    private bool colourBlindActive;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        
    }

    public void OpenLevel(int LevelId)
    {
        PlayerPrefs.SetInt("ColourActivate", colourBlindActive ? 1:0);
        PlayerPrefs.Save();

        string levelName = "Level" + LevelId;
        SceneManager.LoadScene(levelName);
    }

     public void colourSetting()
    {
        if (ColourImage.activeInHierarchy)
        {
            ColourImage.SetActive(false);
            colourBlindActive = false;
        }
        else{
            ColourImage.SetActive(true);
            colourBlindActive = true;
        }

    }
   
   
}
