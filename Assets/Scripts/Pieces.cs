using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public GameObject me;
    public bool isOurKing = false;
    public bool isActive = false;
    public bool isMoving = false;
    public string team;
    public ChessController master;
    public int x, y;
    public Vector3 targetPosition;
    private float stepValue;

    private void Start() {
        master = me.GetComponentInParent<ChessController>();
        stepValue = 0.5f;
        IndexOf();
    }

    private void FixedUpdate() {
        if (isMoving) {
            transform.position = Vector2.MoveTowards(me.transform.position, targetPosition, stepValue);
            //Debug.Log(me.transform.position + "  |  " + targetPosition + "  |  " );
            if (me.transform.position == targetPosition) {
                master.firstLinePawn.Remove(x.ToString() + y);
                isMoving = false;
                IndexOf();
                master.SwapTurn();
            }
        }
    }

    public Vector2 GetPosition() {
        return me.transform.position;
    }

    public void OnMouseDown() {
        if (isActive) {
            master.TakePiece(this);
        }
        if (me.tag == "WhitePiece") {
            master.Move(x, y);
        }
    }

    public void IndexOf() {
        int i, j;
        float x = me.transform.position.x;
        float y = me.transform.position.y;
        Vector2[,] validPos = master.GetComponent<ChessController>().validPos;
        for (i = 0; i < 8; i++) {
            if ((int)validPos[i, 0].x == (int)x) {
                for (j = 0; j < 8; j++) {
                    if ((int)validPos[i, j].y == (int)y) {
                        this.x = i;
                        this.y = j;
                        return ;
                    }
                }
            }
        }
    }

    public void Destroy() {
        Destroy(me);
    }
}
