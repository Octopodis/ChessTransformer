using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessController : MonoBehaviour {
    private readonly Vector2 startPos = new Vector2(-7.5f, -7.5f); 
    private readonly Vector2 offsetX = new Vector2(2f, 0);
    private readonly Vector2 offsetY = new Vector2(0, 2f);

    [HideInInspector] readonly public int size = ChessConfig.size;
    public Vector2[,] validPos;
    public Field[,] fields;
    public char[,] desk; 
         
    [SerializeField] private GameObject[] red = new GameObject[2];
    [SerializeField] private GameObject[] black = new GameObject[2];
    [SerializeField] private GameObject pieceMark = null;
    [SerializeField] private GameObject stepMark = null;
    [SerializeField] private GameObject eatMark = null;
    [SerializeField] private TextManadger text = null;

    private List<GameObject> allMarks = new List<GameObject>();
    List<StepData> allSteps = new List<StepData>();
    private Dictionary<string, List<Piece>> teams = new Dictionary<string, List<Piece>>() { ["red"] = new List<Piece>(),
                                                                                            ["black"] = new List<Piece>()};
    private int turn;
    private ChessAI ai;

    void Start() {
        validPos = new Vector2[size, size];
        fields = new Field[size, size];
        desk = new char[size, size];

        FillLocation();
        ai = new ChessAI(fields);
        StartNewGame();
    }
    
    private void StartNewGame() {
        Clear();

        turn = 0;
        text.Introduction();
        PlacePiece(red, ChessConfig.GetMainCoord("red"));
        PlacePiece(black, ChessConfig.GetMainCoord("black"));
        SwapTurn();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void FillLocation() {
        validPos[0, 0] = startPos;
        fields[0, 0] = new Field(0, 0, ChessConfig.GetFieldType(0, 0));

        for (int i = 0; i < size; i++) {
            for (int j = 1; j < size; j++) {
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

        for (int i = (col + 1) % size; i != col; i = (i + 1) % size) {       //put others piece
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
        if (isKing) {
            p.isMain = isKing;
            desk[x, y] = char.ToUpper(p.team[0]);
        }
        else
            desk[x, y] = p.team[0];
        teams[p.team].Add(p);
    }

    public void SwapTurn() {
        turn++;
        if (turn % 2 != 0) {
            allSteps = StepRemover.GetRemainningSteps('r', desk, fields);
            SetPieceActive("red", true);
            SetPieceActive("black", false);
        }
        else {
            allSteps = StepRemover.GetRemainningSteps('b', desk, fields);
            SetPieceActive("red", false);
            SetPieceActive("black", true);
            StepData nextStep = ai.CalcNextStep(desk);
            Debug.Log(nextStep);
            costil(nextStep);
        }
    }

    private void costil(StepData next) {
        int x = next.begin / 8;
        int y = next.begin % 8;
        StepData nextStep = next;
        Piece pi = teams["black"].Find(p => (p.x == x && p.y == y));
        if (nextStep.stepType == "eat")
            Destroy("red", validPos[nextStep.dest / 8, nextStep.dest % 8]);

        pi.targetPosition = validPos[nextStep.dest / 8, nextStep.dest % 8];
        pi.isMoving = true;
    }

    private void SetPieceActive(string color, bool isActive) {
        foreach (Piece p in teams[color]) {
            p.isActive = isActive;
        }
    }

    public void MarkSelectedPiece(Piece piece) {
        AllMarksClear();
        allMarks.Add(Instantiate(pieceMark, validPos[piece.x, piece.y], Quaternion.identity, piece.gameObject.transform));

        int key = piece.x * size + piece.y ;

        foreach (StepData step in allSteps) {
            if (step.begin != key)
                continue;
            //Debug.Log($"step {step.x}  {step.y}  score {step.score}  eat{step.eatScore}");
            if (step.stepType == "step")
                allMarks.Add(Instantiate(stepMark, validPos[step.dest / 8, step.dest % 8], Quaternion.identity, piece.gameObject.transform));
            else if (step.stepType == "eat")
                allMarks.Add(Instantiate(eatMark, validPos[step.dest / 8, step.dest % 8], Quaternion.identity, piece.gameObject.transform));
        }
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
        Piece p = teams[color].Find(elem => elem.transform.position == position);
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

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                desk[i, j] = ' ';
            }
        }
    }

    private void PrintMass<T>(T[,] a) {
        string text = "\n";
        for (int i = 7; i >= 0; i--) {
            for (int j = 0; j < size; j++) {
                text += a[j, i] + " | ";
            }
            text += "\n";
        }
        Debug.Log(text);
    }
}