using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;

    public GameObject player;

    public GameObject puzzleMainUI;
    public GameObject[] puzzlePanels;

    public bool isColourActivated;

    private int CompletedPuzzles; 
    private TextMeshProUGUI Title;

    public GameObject Resumebtn;
    public GameObject NextLvlBtn;

    public TextMeshProUGUI CompletionTrackerText;

    public GameObject DialogUI;

    public List<GameObject> PuzzleActivators = new List<GameObject>();
   

    // Start is called before the first frame update
    void Start()
    {
        // Default to true if doesn't exist
        isColourActivated = PlayerPrefs.GetInt("ColourActivate", 1) == 1;

        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;
        Title = gameOverUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        CompletionTrackerText.text = CompletedPuzzles +"/" + PuzzleActivators.Count+" Completed";
    }

    // Update is called once per frame
    void Update()
    {

        if (gameOverUI.activeInHierarchy | puzzleMainUI.activeInHierarchy | DialogUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState= CursorLockMode.Locked;
        }

        // if(Input.GetMouseButtonDown(0));

    }

  

    public void GameOver()
    {
        CloseAllPuzzles();
        gameOverUI.SetActive(true);
        Resumebtn.SetActive(false);
        NextLvlBtn.SetActive(false);

        Title.text="GAME OVER";
    }

    public void showPuzzle(int puzzleIndex)
    {
        Debug.Log("Reached this point");
        // disable player movement
        player.GetComponent<PlayerController>().enabled = false;
        puzzleMainUI.SetActive(true);
        foreach (var panel in puzzlePanels)
            panel.SetActive(false); // Hide all panels
        Debug.Log("Now about to set the specific puzzle to true");
        puzzlePanels[puzzleIndex].SetActive(true); // Show the selected puzzle
        Debug.Log("Sucess");   
    }

    public void CloseAllPuzzles()
    {
        // enable player movement
        player.GetComponent<PlayerController>().enabled = true;
        puzzleMainUI.SetActive(false);
        foreach (var panel in puzzlePanels)
            panel.SetActive(false); // Hide all panels
    }



  
  public void puzzleComplete(){
    CompletedPuzzles++;
    Debug.Log("Complete a puzzle");
    Debug.Log($"This is the game object: {this.gameObject}");

    CompletionTrackerText.text = CompletedPuzzles +"/" + PuzzleActivators.Count+" Completed";

    if (CompletedPuzzles == PuzzleActivators.Count){
        CloseAllPuzzles();
        player.GetComponent<PlayerController>().enabled = false;
        gameOverUI.SetActive(true);
        NextLvlBtn.SetActive(true);
        Resumebtn.SetActive(false);
        Title.text= "LEVEL COMPLETE";
    }

    CloseAllPuzzles();
  }

  public void nextPuzzle(){
     // Get the current scene index
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    // Load the next scene
    SceneManager.LoadScene(currentSceneIndex + 1);
  }
    

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   
    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void pause(){
    Debug.Log("Game is paused");
    CloseAllPuzzles();
    player.GetComponent<PlayerController>().enabled = false;
    gameOverUI.SetActive(true);
    Resumebtn.SetActive(true);
    NextLvlBtn.SetActive(false);
    Title.text= "PAUSED";


    }
public void resume(){
    Debug.Log("Game is resumed");
    player.GetComponent<PlayerController>().enabled = true;
    gameOverUI.SetActive(false);
}
    public void quit()
    {
        Application.Quit();
    }

   
   
}
