using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI {
    private int score;
    private char[,] desk = new char[8, 8];
    private Field[,] fields = new Field[8, 8];

    public ChessAI(char[,] desk, Field[,] fields, int[] redMainCoord, int[] blackMainCoord) {
        this.desk = CopyMass(desk);
        this.desk[redMainCoord[0], redMainCoord[1]] = 'R';
        this.desk[blackMainCoord[0], blackMainCoord[1]] = 'B';
        PrintMass(this.desk);

        this.fields = fields;
    }

    //public void CalcNextStep() {
    //    CalcNextStep(desk);
    //}

    //private void CalcNextStep(char [,] desk) {
    //    Dictionary<int[], List<StepData>> allSteps = TakeAllSteps('b', desk);
    //    foreach (int[] coord in allSteps.Keys) { 
    //        char[,] tempDesk = CopyMass(desk);
    //        tempDesk[]
    //        TakeAllSteps('r', );
    //    }
    //}

    private T[,] CopyMass<T>(T[,] mass) {
        T[,] res = new T[8, 8];
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                res[i, j] = mass[i, j];
            }
        }
        return res;
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
}
