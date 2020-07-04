using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isOurKing = false;
    public bool isMoving = false;
    public bool isActive = false;

    public int x;
    public int y;
    private float stepValue = 0.5f;

    public Vector2 targetPosition;

    public string team;

    private ChessController master;
    
    void Start(){
        master = gameObject.transform.parent.GetComponent<ChessController>();
    }

    private void FixedUpdate() {
        if (isMoving) {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, stepValue);
            if ((Vector2)gameObject.transform.position == targetPosition) {
                isMoving = false;
                int[] newCoord = (master.FindCoord(gameObject.transform.position));
                master.DeskChange(x, y, newCoord);
                x = newCoord[0];
                y = newCoord[1];
                master.SwapTurn();
            }
        }
    }

    private void OnMouseDown() {
        if (isActive)
        master.MarkSelectedPiece(this);
    }

}
