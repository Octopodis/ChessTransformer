using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessConfig{
    private static readonly Dictionary<string, int> piecesPower = new Dictionary<string, int>() {
        ["pawn"]   = 10,
        ["king"]   = 20,
        ["knight"] = 30,
        ["bishop"] = 30,
        ["rook"]   = 40,
        ["queen"]  = 50
    };

    private static readonly string[,] desk = new string[8, 8] {
        { "knight", "pawn",   "king",   "queen",  "king",   "bishop", "knight", "rook"},
        { "king",   "knight", "pawn",   "bishop", "rook",   "queen",  "pawn",   "knight"},
        { "knight", "pawn",   "king",   "pawn",   "king",   "pawn",   "rook",   "queen"},
        { "bishop", "queen",  "rook",   "knight", "knight", "pawn",   "king",   "knight"},
        { "knight", "king",   "pawn",   "knight", "knight", "rook",   "queen",  "bishop"},
        { "queen",  "rook",   "pawn",   "king",   "pawn",   "king",   "pawn",   "knight"},
        { "knight", "pawn",   "queen",  "rook",   "bishop", "pawn",   "knight", "king"},
        { "rook",   "knight", "bishop", "king",   "queen",  "king",   "pawn",   "knight"}
        };

    public static string[,] GetDesk() {
        return desk;
    }

    public static int GetPiecePower(string piecesClass) {
        return piecesPower[piecesClass];
    }
}
