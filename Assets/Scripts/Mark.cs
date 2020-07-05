using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public string type;
    private Piece piece;
    private ChessController master;

    void Start() {
        master = gameObject.transform.root.GetComponent<ChessController>();
        piece = gameObject.transform.parent.GetComponent<Piece>();
    }

    private void OnMouseDown() {
        switch (type) {
            case "eat":
                if (piece.team == "red")
                    Eat("black");
                else if (piece.team == "black")
                    Eat("red");
                else
                    throw new Exception($"Something goes wrong: in Mark.Eat found bad piece.team {piece.team}");

                Move();
                break;
            case "active":
                master.AllMarksClear();
                break;
            case "step":
                Move();
                break;
            default:
                throw new Exception($"Something goes wrong: in Mark.OnMoseDown found bad type {type}");
        }
    }

    private void Eat(string color) {
        master.Destroy(color, gameObject.transform.position);
    }

    private void Move() {
        piece.targetPosition = gameObject.transform.position;
        piece.isMoving = true;
        master.AllMarksClear();
    }

}
