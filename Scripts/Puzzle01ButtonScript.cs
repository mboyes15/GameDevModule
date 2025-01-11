using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle01ButtonScript : MonoBehaviour
{

    private SpriteRenderer theSprite;
    private Color originalColor;

    public int thisButtonNumber;

    private PuzzleType01Controller puzzleController;

    // Start is called before the first frame update
    void Start()
    {
        theSprite = GetComponent<SpriteRenderer>();
        puzzleController = FindObjectOfType<PuzzleType01Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        originalColor = theSprite.color;
        theSprite.color = new Color(0,0,0, 1f);
    
    }

     private IEnumerator waitBefore(){
      yield return new WaitForSeconds (0.25f);
        puzzleController.ColourPressed(thisButtonNumber);
    }

    void OnMouseUp(){
        theSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.75f);
  
        StartCoroutine(waitBefore());
      
    }
}
