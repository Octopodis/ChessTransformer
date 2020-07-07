using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isMain = false;
    public bool isMoving = false;
    public bool isActive = false;

    public int x;
    public int y;
    private float stepValue = 0.5f;

    public Vector3 targetPosition;

    public string team;

    private ChessController master;
    
    void Start(){
        master = gameObject.transform.parent.GetComponent<ChessController>();
    }

    private void FixedUpdate() {
        if (isMoving) {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, stepValue);
            if (gameObject.transform.position == targetPosition) {
                isMoving = false;
                int[] newCoord = (FindCoord(gameObject.transform.position));
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

    private int[] FindCoord(Vector2 piecePosition) {
        for (int i = 0; i < ChessConfig.size; i++) {
            if ((int)master.validPos[i, 0].x == (int)piecePosition.x) {
                for (int j = 0; j < ChessConfig.size; j++) {
                    if ((int)master.validPos[i, j].y == (int)piecePosition.y)
                        return new int[] { i, j };
                }
            }
        }
        throw new Exception($"Something goes Wrong: ChessController.FindCoord can't find coord for Vector {piecePosition}");
    }

}