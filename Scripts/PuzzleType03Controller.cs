using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Puzzle03 : MonoBehaviour, Interactable
{
    [SerializeField] private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();
    public GameManagerScript gameManager;

    [SerializeField] private Transform puzzleField;

    [SerializeField] private GameObject btn;

    public int NoPuzzles;

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    private TextMeshProUGUI Title;
    public GameObject PuzzleUI;

    public void Interact()
    {
        // Ensure the cursor is visible and unlocked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log($"The game object: {this.gameObject}");
        Title.text = "Match the Candy";
        gameManager.showPuzzle(2);
        Debug.Log($"button count: {btns.Count}");
        clearPuzzles();
        Debug.Log($"button count: {btns.Count}");

        if (btns.Count == 0)
        {
            CreateButtons();
            AddListeners();
            AddGamePuzzles();
            Shuffle(gamePuzzles);
            gameGuesses = gamePuzzles.Count / 2;
        }
    }

    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Candy Icons");
        Title = PuzzleUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
    }

    void clearPuzzles()
    {
        // Clear all buttons from the UI
        Button[] buttons = puzzleField.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (Button button in btns)
        {
            button.gameObject.SetActive(true);
        }

        firstGuess = false;
        secondGuess = false;
    }

    void CreateButtons()
    {
        for (int i = 0; i < NoPuzzles; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(puzzleField, false);
            btns.Add(button.GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;

            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            StartCoroutine(CheckIfThePuzzlesMatch());
        }
    }

    private IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(0.5f);
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckIfTheGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }
        yield return new WaitForSeconds(0.25f);
        firstGuess = secondGuess = false;
    }

    void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            Title.text = "Completed!";
            gameManager.puzzleComplete();
            Destroy(gameObject);
            Debug.Log("Game Finished");
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int RandomIndex = Random.Range(i, list.Count);
            list[i] = list[RandomIndex];
            list[RandomIndex] = temp;
        }
    }
}
