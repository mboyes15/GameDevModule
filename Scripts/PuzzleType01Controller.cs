using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PuzzleType01Controller : MonoBehaviour, Interactable
{
  public SpriteRenderer[] colours;

  private int colourSelect;

  public float stayLit;
  private float stayLitCounter;

  public float waitBetweenLights;
  private float waitBetweenCounter;

  private bool shouldBeLit;
  private bool shouldBeDark;

  public List<int> activeSequence;
  private int positionInSequence;

  private bool gameActive;
  private int inputInSequence;

  public int SequenceLengthMax;

  public GameManagerScript gameManager;

  private TextMeshProUGUI Title;
  public GameObject PuzzleUI;
  public bool complete;





    public void Interact()
    {
        Debug.Log("you will start the first puzzle");
        gameManager.showPuzzle(0);
        Title.text = "Simon Says";
        activeSequence.Clear();
        StartCoroutine(WaitStart());
      
    }
    private IEnumerator Complete()
    {
      Title.text = "PuzzleComplete";
      foreach (SpriteRenderer colour in colours){
        if (gameManager.isColourActivated){

              GameObject parentObject = colour.gameObject;
              GameObject icon = parentObject.transform.GetChild(0).gameObject;
              icon.SetActive(true);
            }
            colour.color = new Color(colour.color.r,colour.color.g,colour.color.b, 1f);
          }

      yield return new WaitForSeconds(1f);
      complete = true;
      gameManager.puzzleComplete();
      Destroy(gameObject);
    }

    private IEnumerator WaitStart(){
      yield return new WaitForSeconds(1f);
      StartGame();
    }
    void Awake()
    { 
      Title = PuzzleUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
    }
    private IEnumerator waitBefore(){
      yield return new WaitForSeconds (0.5f);
      stayLitCounter = stayLit;
      shouldBeLit = true;

    }
    void Update(){
      if(activeSequence.Count == SequenceLengthMax && !complete){
        StartCoroutine(Complete());
      }
      if (shouldBeLit){
        stayLitCounter -= Time.deltaTime;
        if(stayLitCounter<0)
        {
          colours[activeSequence[positionInSequence]].color = new Color(colours[activeSequence[positionInSequence]].color.r,colours[activeSequence[positionInSequence]].color.g,colours[activeSequence[positionInSequence]].color.b, 0.75f);
          
          if(gameManager.isColourActivated){
          GameObject icon = colours[activeSequence[positionInSequence]].gameObject.transform.GetChild(0).gameObject;
          icon.SetActive(false);}

          shouldBeLit = false;

          shouldBeDark=true;
          waitBetweenCounter = waitBetweenLights;

          positionInSequence++;
        }
      }
      if (shouldBeDark)
      {
        waitBetweenCounter -= Time.deltaTime;

        if(positionInSequence >= activeSequence.Count)
        {
          shouldBeDark = false;
          gameActive = true;

        } else{
          if (waitBetweenCounter <0){
            // colourSelect = Random.Range(0, colours.Length);

            // activeSequence.Add(colourSelect);

            // leave the RGB the same but just brighten the image
            colours[activeSequence[positionInSequence]].color = new Color(colours[activeSequence[positionInSequence]].color.r,colours[activeSequence[positionInSequence]].color.g,colours[activeSequence[positionInSequence]].color.b, 1f);


             if(gameManager.isColourActivated){
              GameObject icon = colours[activeSequence[positionInSequence]].gameObject.transform.GetChild(0).gameObject;
              icon.SetActive(true);
              }


            stayLitCounter = stayLit;
            shouldBeLit = true;
            shouldBeDark = false;

          }
        }
      }
    }

    public void StartGame()
    {
      positionInSequence = 0;
      inputInSequence = 0;

      colourSelect = Random.Range(0, colours.Length);

      activeSequence.Add(colourSelect);

      // leave the RGB the same but just brighten the image
      colours[activeSequence[positionInSequence]].color = new Color(colours[activeSequence[positionInSequence]].color.r,colours[activeSequence[positionInSequence]].color.g,colours[activeSequence[positionInSequence]].color.b, 1f);
       if(gameManager.isColourActivated){
          GameObject icon = colours[activeSequence[positionInSequence]].gameObject.transform.GetChild(0).gameObject;
          icon.SetActive(true);}


      stayLitCounter = stayLit;
      shouldBeLit = true;
    }

    public void ColourPressed(int whichButton){
      if (gameActive)
      {

        if(activeSequence[inputInSequence] == whichButton)
        {
          Debug.Log("Correct");
          inputInSequence++;
          

          StartCoroutine(waitBefore());


          if(inputInSequence >= activeSequence.Count)
          {
            positionInSequence = 0;
            inputInSequence = 0;

            colourSelect = Random.Range(0, colours.Length);

            activeSequence.Add(colourSelect);

            // leave the RGB the same but just brighten the image
            colours[activeSequence[positionInSequence]].color = new Color(colours[activeSequence[positionInSequence]].color.r,colours[activeSequence[positionInSequence]].color.g,colours[activeSequence[positionInSequence]].color.b, 1f);

             if(gameManager.isColourActivated){
            GameObject icon = colours[activeSequence[positionInSequence]].gameObject.transform.GetChild(0).gameObject;
            icon.SetActive(true);}

          
  
            gameActive= false;

          }

        } else {
          Debug.Log("Wrong");
          gameActive = false;

          StartCoroutine(GuessedWrong());
          positionInSequence = 0;
          inputInSequence = 0;
      
          // stayLitCounter = stayLit;
          // shouldBeLit = true;
        }
      }
    }



    private IEnumerator GuessedWrong()
    {
      Title.text="Incorrect: Try Again";
      foreach (SpriteRenderer colour in colours){
        // if colour blind settings are active then show icons when colours are shown
            if (gameManager.isColourActivated){
              Debug.Log("Preferences are on");
              GameObject parentObject = colour.gameObject;
              GameObject icon = parentObject.transform.GetChild(0).gameObject;
              Debug.Log($"this is the icon's name: {icon.name}");
              icon.SetActive(true);
            }
            colour.color = new Color(colour.color.r,colour.color.g,colour.color.b, 1f);
          }

      yield return new WaitForSeconds(1f);

      foreach (SpriteRenderer colour in colours){
          if (gameManager.isColourActivated){
              Debug.Log("Preferences are on");
              GameObject parentObject = colour.gameObject;
              GameObject icon = parentObject.transform.GetChild(0).gameObject;
              Debug.Log($"this is the icon's name: {icon.name}");
              icon.SetActive(false);
            }
            colour.color = new Color(colour.color.r,colour.color.g,colour.color.b, 0.75f);
          }
      Title.text ="";
      shouldBeDark=true;
      waitBetweenCounter = waitBetweenLights;

    }
}
