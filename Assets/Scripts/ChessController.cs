using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessController : MonoBehaviour
{
    public Vector2[,] validPos = new Vector2[8, 8];
    private readonly string[,] pieceClass = ChessConfig.GetDesk();

    private readonly Vector2 startPos = new Vector2(-4.08f*2, -4.06f*2);
    private readonly Vector2 offsetX = new Vector2(1.2571430f*2, 0);
    private readonly Vector2 offsetY = new Vector2(0, 1.262857f*2);

    private List<Pieces> redTeam = new List<Pieces>();
    private List<Pieces> blackTeam = new List<Pieces>();
    private List<GameObject> potentialPiece = new List<GameObject>();
    private List<string> coord = new List<string>();
    public List<string> firstLinePawn = new List<string>();


    private GameObject desk;
    public GameObject black;
    public GameObject red;
    public GameObject white;
    public Pieces forMove;
    public string turn;
    
    // Start is called before the first frame update
    void Start(){
        turn = "black";
        desk = gameObject;
        FillLocation();
        FirstLinePawn();
        PlaceBlack();
        PlaceRed();
        SelectTeam();
        SwapTurn();
        //TestLocation();
    }

    private void FillLocation() {
        validPos[0, 0] = startPos;
        for (int i = 0; i < 8; i++) {
            for (int j = 1; j < 8; j++) {
                validPos[i, j] = validPos[i, j - 1] + offsetY;
            }
            if (i < 7)
                validPos[i + 1, 0] = validPos[i, 0] + offsetX;
        }
    }

    public void Move(int x, int z) {
        foreach (GameObject go in potentialPiece) {
            Destroy(go);
        }
        if (turn == "red") {
            foreach (Pieces p in blackTeam) {
                if (p.x == x && p.y == z) {
                    blackTeam.Remove(p);
                    Destroy(p.me);
                    if (p.isOurKing)
                        Debug.Log("BlackLose");
                    break;
                }
            }
        }
        else if (turn == "black") {
            foreach (Pieces p in redTeam) {
                if (p.x == x && p.y == z) {
                    redTeam.Remove(p);
                    Destroy(p.me);
                    if (p.isOurKing)
                        Debug.Log("RedLose");
                    break;
                }
            }
        }
        forMove.targetPosition = validPos[x, z];
        forMove.isMoving = true;
    }

    public void TakePiece(Pieces curr) {
        foreach(GameObject p in potentialPiece) {
            Destroy(p);
        }
        forMove = curr;
        coord.Clear();
        foreach (Pieces p in redTeam) {
            coord.Add(p.x.ToString() + p.y);
        }
        foreach (Pieces p in blackTeam) {
            coord.Add(p.x.ToString() + p.y);
        }

        CreateValidWay(curr.x, curr.y);
    }

    public void CreateValidWay(int x, int z) {
        switch (pieceClass[x, z]) {
            case "knight":
                VariantKnightStep(x, z);
                break;
            case "rook":
                VariantRookStep(x, z);
                break;
            case "pawn":
                VariantPawnStep(x, z);
                break;
            case "king":
                VariantKingStep(x, z);
                break;
            case "queen":
                VariantQueenStep(x, z);
                break;
            case "bishop":
                VariantBishopStep(x, z);
                break;
            default:
                Debug.Log("WARNING!!! Something wrong in CreateValidWay");
                break;
        }
    }

    //не смотри на это гавно!
    private void VariantKnightStep(int x, int z) {
        if (x + 1 < 8 && z + 2 < 8) {
            if (coord.IndexOf((x + 1).ToString() + (z + 2)) >= 0)
                LastWayCheck(x + 1, z + 2);
            else
                PlaceWhite(x + 1, z + 2);
        }
        if (x + 2 < 8 && z + 1 < 8) {
            if (coord.IndexOf((x + 2).ToString() + (z + 1)) >= 0)
                LastWayCheck(x + 2, z + 1);
            else 
                PlaceWhite(x + 2, z + 1);
        }
        if (x + 1 < 8 && z - 2 >= 0) {
            if (coord.IndexOf((x + 1).ToString() + (z - 2)) >= 0)
                LastWayCheck(x + 1, z - 2);
            else
                PlaceWhite(x + 1, z - 2);
        }
        if (x + 2 < 8 && z - 1 >= 0) {
            if (coord.IndexOf((x + 2).ToString() + (z - 1)) >= 0)
                LastWayCheck(x + 2, z - 1);
            else
                PlaceWhite(x + 2, z - 1);
        }
        if (x - 1 >= 0 && z - 2 >= 0) {
            if (coord.IndexOf((x - 1).ToString() + (z - 2)) >= 0)
                LastWayCheck(x - 1, z - 2);
            else
                PlaceWhite(x - 1, z - 2);
        }
        if (x - 2 >= 0 && z - 1 >= 0) {
            if (coord.IndexOf((x - 2).ToString() + (z - 1)) >= 0)
                LastWayCheck(x - 2, z - 1);
            else
                PlaceWhite(x - 2, z - 1);
        }
        if (x - 1 >= 0 && z + 2 < 8) {
            if (coord.IndexOf((x - 1).ToString() + (z + 2)) >= 0)
                LastWayCheck(x - 1, z + 2);
            else
                PlaceWhite(x - 1, z + 2);
        }
        if (x - 2 >= 0 && z + 1 < 8) {
            if (coord.IndexOf((x - 2).ToString() + (z + 1)) >= 0)
                LastWayCheck(x - 2, z + 1);
            else
                PlaceWhite(x - 2, z + 1);
        }
    }
    private void VariantRookStep(int x, int z) {
        int i = 0;

        for (i = z - 1; i >= 0; i--) {
            int temp = coord.IndexOf(x.ToString() + i);
            if (temp < 0)
                PlaceWhite(x, i);
            else {
                LastWayCheck(x, i);
                break;
            }
        }
        for (i = z + 1; i < 8; i++) {
            int temp = coord.IndexOf(x.ToString() + i);
            if (temp < 0)
                PlaceWhite(x, i);
            else {
                LastWayCheck(x, i);
                break;
            }
        }
        for (i = x - 1; i >= 0; i--) {
            int temp = coord.IndexOf(i.ToString() + z);
            if (temp < 0)
                PlaceWhite(i, z);
            else {
                LastWayCheck(i, z);
                break;
            }
        }
        for (i = x + 1; i < 8; i++) {
            int temp = coord.IndexOf(i.ToString() + z);
            if (temp < 0)
                PlaceWhite(i, z);
            else {
                LastWayCheck(i, z);
                break;
            }
        }
    }
    private void VariantPawnStep(int x, int z) {
        if (turn == "red") {
            if (coord.IndexOf((x).ToString() + (z + 1)) < 0) {
                PlaceWhite(x, z + 1);
                if (coord.IndexOf((x).ToString() + (z + 2)) < 0 && firstLinePawn.IndexOf(x.ToString() + z) >= 0) {
                    PlaceWhite(x, z + 2);
                }
            }
            if (x - 1 >= 0 && z + 1 < 8) {
                if (coord.IndexOf((x - 1).ToString() + (z + 1)) >= 0)
                    LastWayCheck(x - 1, z + 1);
            }
            if (x + 1 < 8 && z + 1 < 8) {
                if (coord.IndexOf((x + 1).ToString() + (z + 1)) >= 0)
                    LastWayCheck(x + 1, z + 1);
            }
        }

        if (turn == "black") {
            if (coord.IndexOf((x).ToString() + (z - 1)) < 0) {
                PlaceWhite(x, z - 1);
                if (coord.IndexOf((x).ToString() + (z - 2)) < 0 && firstLinePawn.IndexOf(x.ToString() + z) > 0) {
                    PlaceWhite(x, z - 2);
                }
            }
            if (x - 1 >= 0 && z - 1 >= 0) {
                if (coord.IndexOf((x - 1).ToString() + (z - 1)) >= 0)
                    LastWayCheck(x - 1, z - 1);
            }
            if (x + 1 < 8 && z - 1 >= 0) {
                if (coord.IndexOf((x + 1).ToString() + (z - 1)) >= 0)
                    LastWayCheck(x + 1, z - 1);
            }
        }
    }
    private void VariantKingStep(int x, int z) {
        for (int j = -1; j <= 1; j++) {
            for (int i = -1; i <= 1; i++) {
                if (x + j >= 0 && x + j < 8 && z + i >= 0 && z + i < 8) {
                    if (coord.IndexOf((x + j).ToString() + (z + i)) >= 0)
                        LastWayCheck(x + j, z + i);
                    else
                        PlaceWhite(x + j, z + i);
                }
            }
        }
    }
    private void VariantQueenStep(int x, int z) {
        VariantRookStep(x, z);
        VariantBishopStep(x, z);
    }
    private void VariantBishopStep(int x, int z) {
        int temp;
        for (int i = 1; i < 8; i++ ) {
            if (x + i < 8 && z + i < 8) {
                temp = coord.IndexOf((x + i).ToString() + (z + i));
                if (temp < 0)
                    PlaceWhite(x + i, z + i);
                else {
                    LastWayCheck(x + i, z + i);
                    break;
                }
            }
            else
                break;
        }

        for (int i = 1; i < 8; i++ ) {
            if (x + i < 8 && z - i >= 0) {
                temp = coord.IndexOf((x + i).ToString() + (z - i));
                if ( temp < 0)
                    PlaceWhite(x + i, z - i);
                else {
                    LastWayCheck(x + i, z - i);
                    break;
                }
            }
            else
                break;
        }

        for (int i = 1; i < 8; i++ ) {
            if (x - i >= 0 && z - i >= 0) {
                temp = coord.IndexOf((x - i).ToString() + (z - i));
                    if (temp < 0)
                    PlaceWhite(x - i, z - i);
                else {
                    LastWayCheck(x - i, z - i);
                    break;
                }
            }
            else
                break;
        }

        for (int i = 1; i < 8; i++ ) {
            if (x - i >= 0 && z + i < 8) {
                temp = coord.IndexOf((x - i).ToString() + (z + i));
                if (temp < 0)
                    PlaceWhite(x - i, z + i);
                else {
                    LastWayCheck(x - i, z + i);
                    break;
                }
            }
            else
                break;
        }
    }

    private void LastWayCheck(int x, int z) {
        string side = "white";
        foreach (Pieces p in redTeam) {
            if (p.x == x) {
                if (p.y == z)
                    side = "red";
            }
        }
        foreach (Pieces p in blackTeam) {
            if (p.x == x) {
                if (p.y == z)
                    side = "black";
            }
        }

        if (side == "white") {
            Debug.Log("Somthing wrong in LastWayCheck!");
        }

        if (forMove.team == side)
            return;
        else {
            PlaceWhite(x, z);
        }
    }


    public void SwapTurn() {
        if (turn == "black") {
            foreach (Pieces p in redTeam) {
                p.isActive = true;
            }
            foreach (Pieces p in blackTeam) {
                p.isActive = false;
            }
            turn = "red";
        }
        else if (turn == "red") {
            foreach (Pieces p in redTeam) {
                p.isActive = false;
            }
            foreach (Pieces p in blackTeam) {
                p.isActive = true;
            }
            turn = "black";
            ChessAI ai = new ChessAI(redTeam, blackTeam);
            ai.CalcStep(blackTeam);
        }
    }

    private void SelectTeam() {
        Pieces p;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("RedTeam")) {
            p = go.GetComponent<Pieces>();
            redTeam.Add(p);
            p.team = "red";
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("BlackTeam")) {
            p = go.GetComponent<Pieces>();
            blackTeam.Add(p);
            p.team = "black";
        }
    }

    private void PlaceBlack() {
        GameObject king = Instantiate(black, validPos[0, 7], Quaternion.identity, desk.transform);
        king.GetComponent<Pieces>().isOurKing = true;
        king.transform.localScale += new Vector3(0.4f, -0.1f, 0);
        for (int i = 1; i < 8; i++) {
            Instantiate(black, validPos[i, 7], Quaternion.identity, desk.transform);
            Instantiate(black, validPos[i, 6], Quaternion.identity, desk.transform);
        }
        Instantiate(black, validPos[0, 6], Quaternion.identity, desk.transform);
    }

    private void PlaceRed() { 
        GameObject king = Instantiate(red, validPos[7, 0], Quaternion.identity, desk.transform);
        king.GetComponent<Pieces>().isOurKing = true;
        king.transform.localScale += new Vector3(0.4f, -0.1f, 0);
        for (int i = 0; i < 7; i++) {
            Instantiate(red, validPos[i, 0], Quaternion.identity, desk.transform);
            Instantiate(red, validPos[i, 1], Quaternion.identity, desk.transform);
        }
        Instantiate(red, validPos[7, 1], Quaternion.identity, desk.transform);
    }

    private void PlaceWhite(int x, int z) {
        potentialPiece.Add(Instantiate(white, (Vector3)validPos[x, z] - new Vector3(0, 0, 0.05f), Quaternion.identity, desk.transform));
    }

    private void TestLocation() {
        foreach (Vector3 v in validPos) {
            Instantiate(red, v, Quaternion.identity, desk.transform);
        }
    }

    private void FirstLinePawn() {
        firstLinePawn.Clear();
        for (int i = 0; i < 8; i++) {
            if (pieceClass[i, 1] == "pawn") 
                firstLinePawn.Add(i.ToString() + 1);
            if (pieceClass[i, 6] == "pawn")
                firstLinePawn.Add( i.ToString() + 6);
        }
    }

    private void TestLocation(string piece) {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (pieceClass[i, j] == piece)
                    Instantiate(red, validPos[i, j], Quaternion.identity, desk.transform);
            }
        }
    }
}
