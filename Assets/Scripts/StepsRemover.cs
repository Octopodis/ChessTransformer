using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StepRemover {
    public static List<StepData> GetRemainningSteps(char color, char[,] currDesk, Field[,] fields) {
        List<StepData> allSteps = new List<StepData>();
        string pieceType;
        int size = ChessConfig.size;
        int crutch;
        int x, y;

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                if (currDesk[i, j] == color || currDesk[i, j] == char.ToUpper(color)) {
                    crutch = i * size + j;

                    foreach (StepData sd in fields[i, j].posibleSteps) {
                        pieceType = fields[i, j].type;
                        x = sd.dest / size;
                        y = sd.dest % size;

                        sd.stepType = StepCheck(i, j, x, y, currDesk, pieceType, sd.isPawnAtack);
                        sd.eatScore = 0;

                        if (sd.stepType == " ") {
                            sd.stepType = null;
                            continue;
                        }

                        if (sd.stepType == "eat") {
                            if (currDesk[x, y] == 'R' || currDesk[x, y] == 'B')
                                sd.eatScore = ChessConfig.GetPiecePower("Main");

                            else     //points for            eating  ↓          and the         power of the enemy’s piece ↓
                                sd.eatScore = ChessConfig.GetPiecePower("ordinary") + ChessConfig.GetPiecePower(fields[x, y].type); 
                        }
                        allSteps.Add(sd);
                    }
                }
            }
        }
        return allSteps;
    }

    private static string StepCheck(int xStart, int yStart, int xFinish, int yFinish, char[,] currDesk, string type, bool isPawnAtack) {
        char startColor = currDesk[xStart, yStart];
        char finishColor = currDesk[xFinish, yFinish];

        if (startColor == finishColor || startColor == char.ToUpper(finishColor) || char.ToUpper(startColor) == finishColor)
            return " ";

        if (type == "king" || type == "knight") {
            if (finishColor == ' ')
                return "step";
            else
                return "eat";
        }

        if (type == "pawn") {
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
}