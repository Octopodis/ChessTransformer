using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessController : MonoBehaviour {
    private readonly Vector2 startPos = new Vector2(-7.5f, -7.5f); 
    private readonly Vector2 offsetX = new Vector2(2f, 0);
    private readonly Vector2 offsetY = new Vector2(0, 2f);

    public Vector2[,] validPos = new Vector2[8, 8];
    public Field[,] fields = new Field[8, 8];
    public char[,] desk = new char[8, 8];
         
    [SerializeField] private GameObject[] red = new GameObject[2];
    [SerializeField] private GameObject[] black = new GameObject[2];
    [SerializeField] private GameObject pieceMark = null;
    [SerializeField] private GameObject stepMark = null;
    [SerializeField] private GameObject eatMark = null;
    [SerializeField] private TextManadger text = null;

    private List<GameObject> allMarks = new List<GameObject>();
    Dictionary<int, List<StepData>> allSteps = new Dictionary<int, List<StepData>>();
    private Dictionary<string, List<Piece>> teams = new Dictionary<string, List<Piece>>() { ["red"] = new List<Piece>(),
                                                                                            ["black"] = new List<Piece>()};
    private int turn = 0;

    void Start() {
        FillLocation();
        StartNewGame();
    }
    
    private void StartNewGame() {
        Clear();

        text.Introduction();
        PlacePiece(red, ChessConfig.GetKingCoord("red"));
        PlacePiece(black, ChessConfig.GetKingCoord("black"));
        SwapTurn();
        //Debug.Log(Enumerable.SequenceEqual(new int[] { 1, 2, 3 }, new int[] { 1, 3, 3 }));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void FillLocation() {
        validPos[0, 0] = startPos;
        fields[0, 0] = new Field(0, 0, ChessConfig.GetFieldType(0, 0));

        for (int i = 0; i < 8; i++) {
            for (int j = 1; j < 8; j++) {
                validPos[i, j] = validPos[i, j - 1] + offsetY;
                fields[i, j] = new Field(i, j, ChessConfig.GetFieldType(i, j));
                desk[i, j] = ' ';
            }
            if (i < 7) {
                validPos[i + 1, 0] = validPos[i, 0] + offsetX;
                fields[i + 1, 0] = new Field(i + 1, 0, ChessConfig.GetFieldType(i + 1, 0));
            }
        }
    }

    private void PlacePiece(GameObject[] piece, int[] coord) {
        int col = coord[0];
        int row1 = coord[1];
        int row2 = coord[2];

        CreatePiece(piece[1], col, row1, true);                           // put OurKing 

        for (int i = (col + 1) % 8; i != col; i = (i + 1) % 8) {       //put others piece
            CreatePiece(piece[0], i, row1);
            CreatePiece(piece[0], i, row2);
        }

        CreatePiece(piece[0], col, row2);                                 //put last piece
    }

    private void CreatePiece(GameObject piece, int x, int y, bool isKing = false) {
        GameObject go = Instantiate(piece, validPos[x, y], Quaternion.identity, gameObject.transform);
        Piece p = go.GetComponent<Piece>();
        p.x = x;
        p.y = y;
        desk[x, y] = p.team[0];
        if (isKing)  
            p.isMain = isKing;
        teams[p.team].Add(p);
    }

    public void SwapTurn() {
        turn++;
        if (turn % 2 != 0) {
            allSteps = GetAllSteps('r', desk);
            SetPieceActive("red", true);
            SetPieceActive("black", false);
        }
        else {
            SetPieceActive("red", false);
            SetPieceActive("black", true);
            //ChessAI ai = new ChessAI(desk, fields, teams);
        }
    }

    private void SetPieceActive(string color, bool isActive) {
        foreach (Piece p in teams[color]) {
            p.isActive = isActive;
        }
    }

    public Dictionary<int, List<StepData>> GetAllSteps(char color, char[,] currDesk) {
        Dictionary<int, List<StepData>> allSteps = new Dictionary<int, List<StepData>>();
        string checkResult = "";

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (currDesk[i, j] == color || currDesk[i, j] == Char.ToUpper(color)) {
                    int crutch =  i * 8 +  j ;
                    allSteps[crutch] = new List<StepData>();

                    foreach (StepData sd in fields[i, j].posibleSteps) {
                        checkResult = StepCheck(i, j, sd.x, sd.y, currDesk, sd.isPawnAtack);

                        sd.eatScore = 0;
                        if (checkResult == " ") {
                            sd.stepType = null;
                            continue;
                        }

                        sd.stepType = checkResult; 
                        if (checkResult == "eat") {
                            if (currDesk[sd.x, sd.y] == 'R' || currDesk[sd.x, sd.y] == 'B') 
                                sd.eatScore = ChessConfig.GetPiecePower("Main") + ChessConfig.GetPiecePower(fields[i, j].type);
                            else 
                                sd.eatScore = ChessConfig.GetPiecePower("ordinary") + ChessConfig.GetPiecePower(fields[i, j].type);
                        }
                        allSteps[crutch].Add(sd);
                    }
                }
            }
        }
        return allSteps;
    }

    public int[] FindCoord(Vector2 piecePosition) {
        for (int i = 0; i < 8; i++) {
            if ((int)validPos[i, 0].x == (int)piecePosition.x) {
                for (int j = 0; j < 8; j++) {
                if ((int)validPos[i, j].y == (int)piecePosition.y)
                    return new int[] { i, j };
                }
            }
        }
        throw new Exception($"Something goes Wrong: ChessController.FindCoord can't find coord for Vector {piecePosition}");
    }

    public void MarkSelectedPiece(Piece piece) {
        AllMarksClear();
        allMarks.Add(Instantiate(pieceMark, validPos[piece.x, piece.y], Quaternion.identity, piece.gameObject.transform));

        int key = piece.x * 8 + piece.y ;

        Debug.Log(key);
        TestDictionary();
        Debug.Log(allSteps[key].Count);

        foreach (StepData step in allSteps[key]) {
            if (step.stepType == "step")
                allMarks.Add(Instantiate(stepMark, validPos[step.x, step.y], Quaternion.identity, piece.gameObject.transform));
            else if (step.stepType == "eat")
                allMarks.Add(Instantiate(eatMark, validPos[step.x, step.y], Quaternion.identity, piece.gameObject.transform));
        }
    }

    private string StepCheck(int xStart, int yStart, int xFinish, int yFinish, char[,] currDesk, bool isPawnAtack) {
        string startType = fields[xStart, yStart].type;
        char startColor = currDesk[xStart, yStart];
        char finishColor = currDesk[xFinish, yFinish];

        if (startColor == finishColor || startColor == char.ToUpper(finishColor) || char.ToUpper(startColor) == finishColor)
            return " ";

        if (startType == "king" || startType == "knight") {
            if (finishColor == ' ')
                return "step";
            else
                return "eat";
        }

        if (startType == "pawn") {
            if ((startColor == 'r' || startColor == 'R') && yFinish < yStart)
                return " ";
            if ((startColor == 'b' || startColor == 'B') && yFinish > yStart) 
                return " ";
            if (isPawnAtack && finishColor == ' ')
                return " ";
            if (!isPawnAtack && finishColor != ' ')
                return " ";
        }
        
        Vector2 start = new Vector2(xStart, yStart);
        Vector2 finish = new Vector2(xFinish, yFinish);
        Vector2 direction = start - finish;               //step direction vector
        Vector2 normal = direction / direction.magnitude; //normalized vector

        int x = (int)Math.Round(normal.x);
        int y = (int)Math.Round(normal.y);

        xFinish += x;                         //check the way from start to finish
        yFinish += y;
        while (xFinish != xStart || yFinish != yStart) {
            if (currDesk[xFinish, yFinish] != ' ')
                return " ";
            xFinish += x;
            yFinish += y;
        }

        if (finishColor == ' ')
            return "step";
        return "eat";
        
    }

    public void DeskChange(int x, int y, int[] newCoord) {
        desk[newCoord[0], newCoord[1]] = desk[x, y];
        desk[x, y] = ' ';
    }

    public void AllMarksClear() {
        foreach (GameObject go in allMarks) {
            Destroy(go);
        }
        allMarks.Clear();
    }

    public void Destroy(string color, Vector3 position) {
        Piece p = teams[color].Find(elem=> elem.transform.position == position);
        if (!p.isMain) {
            teams[color].Remove(p);
            Destroy(p.gameObject);
        }
        else
            GameOver(color);
    }

    public void GameOver(string color) {
        if (color == "black")
            text.WinText("Red");
        else if (color == "red")
            text.WinText("Black");
        Invoke("StartNewGame", 5f);
    }

    private void Clear() {
        AllMarksClear();
        foreach (List<Piece> list in teams.Values) {
            foreach (Piece p in list) {
                Destroy(p.gameObject);
            }
            list.Clear();
        }

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                desk[i, j] = ' ';
            }
        }
    }

    private void PrintMass<T>(T[,] a) {
        string text = "\n";
        for (int i = 7; i >= 0; i--) {
            for (int j = 0; j < 8; j++) {
                text += a[j, i] + " | ";
            }
            text += "\n";
        }
        Debug.Log(text);
    }

    private void TestDictionary() {
        string text = "";
        foreach (int i in allSteps.Keys) {
            text += i + " | ";
        }
        Debug.Log(text);
    }
}

  
