using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PuzzleType02Controller : MonoBehaviour, Interactable
{
    public GameManagerScript gameManager;

    public int scale;

    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    public Sprite[] puzzleSprites;

     private List<Transform> pieces;
    private int emptyLocation;
    private int size;

    private Vector2 mousePos;


    private TextMeshProUGUI Title;
    public GameObject PuzzleUI;
    public GameObject SlidePanel;

   public void Interact()
   {
    Debug.Log("You will start puzzle type 2");
    gameManager.showPuzzle(1);
    Title.text = "Sliding Puzzle";
    ClearGamePieces(SlidePanel);
    CreateGamePieces(0.01f);
    StartCoroutine(WaitShuffle(1f));
    }

    private void ClearGamePieces(GameObject panel){
    // Delete the children of the panel so that the puzzles don't get mixed if you open them both up
        foreach (Transform child in panel.transform)
            {
                Destroy(child.gameObject);
            }
    
        pieces.Clear();

        // Reset the empty location
        emptyLocation = -1;
    }


    // We name the pieces in order so we can use this to check completion

    private IEnumerator WaitShuffle(float duration){
        yield return new WaitForSeconds(duration);
        Shuffle();
    }

    void awake(){
        Title = PuzzleUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
    }
    void Start(){
        // for the tile puzzle (puzzle01)
        Title = PuzzleUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        pieces = new List<Transform>();
        size = 3;
    }

    void Update(){
         // when left mouse button clicks send out ray to see if we clicked on a piece
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("left mouse button has been clicked");
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for(int i =0; i<pieces.Count; i++)
                {
                    // Debug.Log($"mousePos: {mousePos}");
                    Vector2 piecePos = (Vector2)pieces[i].position;
                    // Debug.Log($"piece position: {piecePos}");
                    float distance = Vector2.Distance(mousePos, piecePos);
                    if (distance < 0.5f) // Adjust threshold based on piece size
                    {
                        Debug.Log($"Clicked on {pieces[i].name}");
                        if(swapIfValid(i, -size, size)){
                            CheckCompletion();
                             break;}
                        // down
                        if(swapIfValid(i, +size, size)){
                            CheckCompletion();
                            break;}
                        // left
                        if(swapIfValid(i, -1, 0)){
                            CheckCompletion();
                            break;}
                        // right
                        if(swapIfValid(i, +1, size-1)){
                            CheckCompletion();
                            break;}
                    }
                }
        }


    }

      private bool swapIfValid(int i, int offset, int colCheck){
        // Debug.Log("Checking if swap is valid");
        // Debug.Log($"Empty Position is: {emptyLocation}");
        // Debug.Log($"i + offset is: {i+offset}");
        // Debug.Log($"i%size is: {i%size}");
        // Debug.Log($"colcheck is: {colCheck}");
        // check if border tile
        // Debug.Log($"col of ")
        if (((i%size)!= colCheck)&& ((i + offset)==emptyLocation)){
            // Debug.Log("makes it past the first step");
            // swap them in game state
            (pieces[i],pieces[i+offset]) = (pieces[i+offset],pieces[i]);
            // swap their transformations: so they actually move
            (pieces[i].localPosition, pieces[i+offset].localPosition) = ((pieces[i+offset].localPosition, pieces[i].localPosition));
            // update the empty location
            emptyLocation = i;
            return true;

        }
        return false;
    }
    private IEnumerator Complete(){
        Title.text = "Complete";
        yield return new WaitForSeconds(0.5f);
        gameManager.puzzleComplete();
        Destroy(gameObject);
    }

    private bool CheckCompletion(){
        for (int i=0;i<pieces.Count;i++){
            if(pieces[i].name != $"{i}"){
                return false;
            }
        }
        StartCoroutine(Complete());
        
        // need a function to say puzzle complete
        return true;
    }

    // Brute force shuffling to make sure the solution works
      private void Shuffle(){
        int count = 0;
        int last = 0;
        while(count<(size *size*size)){
            // pick a random location
            int rnd = Random.Range(0,size * size);
            // Only this we forbid is undoing the last move
            if (rnd==last){continue;}
            last = emptyLocation;
            if (swapIfValid(rnd, -size,size)){
                count++;
            } else if (swapIfValid(rnd, +size, size)){
                count++;
            } else if (swapIfValid(rnd, -1, 0)){
                count++;
            } else if (swapIfValid(rnd, +1, size -1)){
                count++;
            }
        }
    }

 // create tile puzzle setup with size x size pieces
    // reference: https://github.com/Firnox/SlidingPuzzle/blob/main/Assets/Textures/train_square.png

        private void CreateGamePieces(float gapThickness)
    {
        // width of each tile
        float width=1/(float)size;
        for (int row=0; row<size; row++)
        {
            for (int col=0; col<size;col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                // Pieces will be in a game board going centred and the spacing is *150
                piece.localPosition = new Vector3(-150 + ((2 * width * col)+width)* 150f,
                                                    // y-col 
                                                    +50 - ((2*width * row)-width)*150f,
                                                    0);
                // scaled by 4
                piece.localScale = ((2*width)-gapThickness) * Vector3.one * scale;
                piece.name = $"{(row * size)+ col}";
                // We want an empty space in the bottom right
                if ((row == size -1)&&(col==size-1)) {
                    emptyLocation = (size * size)-1;
                    piece.gameObject.SetActive(false);
                } else{
                    // we want to map the UV coordinates appropiately, they are 0->1
                    float gap = gapThickness /2;
                    var spriteRenderer = piece.GetComponent<SpriteRenderer>();
                    //grab all of the separate pieces of the image
                    spriteRenderer.sprite = puzzleSprites[row*size + col];
                }
            }
        }


    }
}
