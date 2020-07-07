using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field {
    public string type;
    private int x;
    private int y;

    public List<StepData> posibleSteps { get; private set; }

    public Field(int x, int y, string type) {
        this.type = type;
        this.x = x;
        this.y = y;
        posibleSteps = new List<StepData>();

        FindAllSteps();
    }

    private void FindAllSteps() {
        switch (type) {
            case "knight":
                KnightStep(x, y);
                break;
            case "rook":
                RookStep(x, y);
                break;
            case "pawn":
                PawnStep(x, y);
                break;
            case "king":
                KingStep(x, y);
                break;
            case "bishop":
                BishopStep(x, y);
                break;
            case "queen":
                QueenStep(x, y);
                break;
        }
    }

    private void AddStep(int x, int y, bool isPawnAtack = false) {   //isPawnAtack = true in case of a pawn move-attack
        if (x < ChessConfig.size && x >= 0 && y < ChessConfig.size && y >= 0) {
            string newType = ChessConfig.GetFieldType(x, y);
            
            //Calculation the difference between the power of the piece before step and after
            int score = ChessConfig.GetPiecePower(newType) - ChessConfig.GetPiecePower(type);
            posibleSteps.Add(new StepData(x * 8 + y, this.x * 8 + this.y, score, isPawnAtack));
        }
    }

    private void KnightStep(int x, int y) {
        AddStep(x + 1, y + 2);
        AddStep(x + 2, y + 1);

        AddStep(x + 1, y - 2);
        AddStep(x + 2, y - 1);

        AddStep(x - 1, y - 2);
        AddStep(x - 2, y - 1);

        AddStep(x - 1, y + 2);
        AddStep(x - 2, y + 1);

    }
    private void RookStep(int x, int y) {
        for (int i = 0; i < ChessConfig.size; i++) {
            if (i != x)
                AddStep(i, y);
            if (i != y)
                AddStep(x, i);
        }
    }
    private void PawnStep(int x, int y) {
        AddStep(x, y + 1);
        AddStep(x, y - 1);

        AddStep(x - 1, y + 1, true);
        AddStep(x - 1, y - 1, true);

        AddStep(x + 1, y + 1, true);
        AddStep(x + 1, y - 1, true);

        if (y == 1 || y == 6) {
            AddStep(x, y + 2);
            AddStep(x, y - 2);
        }
    }
    private void KingStep(int x, int y) {
        for (int j = -1; j <= 1; j++) {
            for (int i = -1; i <= 1; i++) {
                if (!(i == 0 && j == 0))
                    AddStep(x + j, y + i);
            }
        }
    }
    private void BishopStep(int x, int y) {
        int x1, x2, y1, y2;                 //coordinates of the beginning of two diagonals

        if (x < y) {                        //calc beginning of first diagonals
            x1 = x - y;
            y1 = 0;
        }
        else {
            x1 = 0;
            y1 = y - x;
        }

        if (x + y > 7) {                    //calc beginning of second diagonals
            x2 = 7;
            y2 = (x + y) % 7;
        }
        else {
            x2 = x + y;
            y2 = 0;
        }

        for (int i = 0; i < ChessConfig.size; i++) {
            if (!(x1 + i == x && x2 - i == x && y1 + i == y && y2 + i == y)) {
                AddStep(x1 + i, y1 + i);
                AddStep(x2 - i, y2 + i);
            }
        }
    }
    private void QueenStep(int x, int y) {
        RookStep(x, y);
        BishopStep(x, y);
    }
}