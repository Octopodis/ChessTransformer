using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChessConfig {
    public static readonly int size = 8;
    public static int baseDepthAI = 1;

    private static Dictionary<string, int> piecesPower = new Dictionary<string, int>() {
        ["pawn"] = 1,
        ["king"] = 2,
        ["knight"] = 3,
        ["bishop"] = 3,
        ["rook"] = 4,
        ["queen"] = 5,
        ["ordinary"] = 2,          //nominal power of piece (sum whith power of type)
        ["Main"] = 100
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

    private static int[] redMainCoord = new int[] { 7, 0, 1};       //column, row1 , row2
    private static int[] blackMainCoord = new int[] { 0, 7, 6};     //column, row1 , row2

    public static string GetFieldType(int x, int y) {
        return desk[x, y];
    }

    public static int GetPiecePower(string piecesClass) {
        return piecesPower[piecesClass];
    }

    public static int[] GetMainCoord(string team) {
        if (team == "red")
            return redMainCoord;
        else if (team == "black")
            return blackMainCoord;
        else
            throw new System.Exception($"Something goes Wrong: team in ChessConfig.GetKingCoord = {team}");
    }
}