using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeScript : MonoBehaviour
{
    public Texture2D mazeTexture;
    public float pixelScale = 1.0f; // Scale of the maze in Unity units

    private Vector2 mazeOffset;

    public LayerMask solidObjectsLayer;

    public float moveSpeed;
    public float stepSize;

    public bool isMoving;

    public Vector2 input;



    void Start()
    {
        // Calculate the offset to align the texture with the world position
        mazeOffset = new Vector2(-mazeTexture.width / 2, -mazeTexture.height / 2) * pixelScale;
    }

    private void Update()
    {
        // Debug.Log($"Normal isMoving check before: {isMoving}");
        if (!isMoving)
        {
            // get the button being pushed
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Debug.Log("This is input.x" + input.x);
            // Debug.Log("This is input.y" + input.y);

            if (input.x!=0) input.y = 0;
            if (input != Vector2.zero)
            {

                var targetPos = transform.position;
                targetPos.x += input.x * stepSize;
                targetPos.y += input.y * stepSize;

            //    // check if there are objects
            //    Debug.Log("Checking");
            //    if (CanMoveToPosition(targetPos)){
            //     // actually move the character
            //     Debug.Log("THIS SHOULD DISPLAY");
            //     StartCoroutine(Move(targetPos));
            //    }
            //    else{
            //     isMoving = false;
            //    }

                if (IsWalkable(targetPos)){
                // actually move the character
                StartCoroutine(Move(targetPos));
               }
            }
        }
    }

     IEnumerator Move(Vector3 targetPos)
   {
        // isMoving = true;

        // // Debug.Log($"this should be true: {(targetPos - transform.position).sqrMagnitude > Mathf.Epsilon}");
        // while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        // {
        //     // Store the current position before attempting to move
        //     Vector3 currentPos = transform.position;
        //     // Debug.Log("This will continually happen until position has moved");
        //     transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        //      if (transform.position == currentPos)
        //         {
        //             // If the position didn't change, we're stuck (hit a wall), so break the loop
        //             Debug.Log("Hit a wall or obstacle, stopping movement.");
        //             break;
        //         }
        //     yield return null;
        // }
        // transform.position = targetPos;
        // // Debug.Log("If here then isMoving is returned as false");
        // isMoving = false;
        isMoving = true;
        Debug.Log("just before loop");
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            Debug.Log($"IN the loop: {(targetPos - transform.position).sqrMagnitude}");
            Debug.Log($"mathfEplison: {Mathf.Epsilon}");
        // {   Vector3 currentPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            // if (transform.position == currentPos)
            //     {
                    // // If the position didn't change, we're stuck (hit a wall), so break the loop
                    // Debug.Log("Hit a wall or obstacle, stopping movement.");
                    // isMoving=false;
                //     break;
                // }
            yield return null;
        }
        Debug.Log("If here then isMoving is returned as false");
        transform.position = targetPos;

        isMoving = false;
        Debug.Log($"isMoving: {isMoving}");
   }
    public bool CanMoveToPosition(Vector2 worldPosition)
    {
        Debug.Log($"worldPosition: {worldPosition}");
        // Convert world position to texture coordinates
        Vector2Int pixelPos = WorldToPixel(worldPosition);
        Debug.Log($"pixel Pos: {pixelPos}");
        // Check bounds
        if (pixelPos.x < 0 || pixelPos.x >= mazeTexture.width || pixelPos.y < 0 || pixelPos.y >= mazeTexture.height)
            return false;

        // Get the color of the pixel
        Color pixelColor = mazeTexture.GetPixel(pixelPos.x, pixelPos.y);
        Debug.Log($"Pixel Colour: {pixelColor.grayscale}");
        // Allow movement only if the pixel is not dark (e.g., black)
        return pixelColor.grayscale < 0.1f;
    }

    private Vector2Int WorldToPixel(Vector2 worldPosition)
    {
        Vector2 texturePosition = (worldPosition / pixelScale) - mazeOffset;
        return new Vector2Int(Mathf.RoundToInt(texturePosition.x), Mathf.RoundToInt(texturePosition.y));
    }
    private bool IsWalkable(Vector3 targetPos)
   {
     // if we are overlapping then return false
     if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer)!=null)
     {
          return false;
     }
     return true;

   }
}
