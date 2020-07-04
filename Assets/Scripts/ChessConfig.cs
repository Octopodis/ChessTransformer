using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessConfig {
    private static Dictionary<string, int> piecesPower = new Dictionary<string, int>() {
        ["pawn"] = 1,
        ["king"] = 2,
        ["knight"] = 3,
        ["bishop"] = 3,
        ["rook"] = 4,
        ["queen"] = 5,
        ["orginary"] = 2,          //nominal power of piece (sum whith power of type)
        ["isOurKing"] = 100
    };

    private static string[,] desk = new string[8, 8] {
        { "knight", "pawn",   "king",   "queen",  "king",   "bishop", "knight", "rook"},
        { "king",   "knight", "pawn",   "bishop", "rook",   "queen",  "pawn",   "knight"},
        { "knight", "pawn",   "king",   "pawn",   "king",   "pawn",   "rook",   "queen"},
        { "bishop", "queen",  "rook",   "knight", "knight", "pawn",   "king",   "knight"},
        { "knight", "king",   "pawn",   "knight", "knight", "rook",   "queen",  "bishop"},
        { "queen",  "rook",   "pawn",   "king",   "pawn",   "king",   "pawn",   "knight"},
        { "knight", "pawn",   "queen",  "rook",   "bishop", "pawn",   "knight", "king"},
        { "rook",   "knight", "bishop", "king",   "queen",  "king",   "pawn",   "knight"}
    };

    private static int[] redKingCoord = new int[] { 7, 0, 1};       //column, row1 , row2
    private static int[] blackKingCoord = new int[] { 0, 7, 6};     //column, row1 , row2
    private static Vector3 kingIncrease = new Vector3(0.2f, -0.05f, 0);

    public static string[,] GetDesk() {
        return desk;
    }

    public static string GetFieldType(int x, int y) {
        return desk[x, y];
    }

    public static int GetPiecePower(string piecesClass) {
        return piecesPower[piecesClass];
    }

    public static int[] GetKingCoord(string team) {
        if (team == "red")
            return redKingCoord;
        else if (team == "black")
            return blackKingCoord;
        else
            throw new System.Exception($"Something goes Wrong: team in ChessConfig.GetKingCoord = {team}");
    }
    
    public static Vector3 GetKingSize() {
        return kingIncrease;
    }
}
