using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI {
    readonly int size = ChessConfig.size;

    private int depth;
    private Field[,] fields;
    private char[] teamQueue = new char[] { 'b', 'r' };

    public ChessAI(Field[,] fields) {
        this.fields = fields;
        this.depth = ChessConfig.baseDepthAI * 2;
    }

    public StepData CalcNextStep(char[,] desk) {
        char[,] virtualDesk;
        List<StepData> allStep = StepRemover.GetRemainningSteps(teamQueue[depth % 2], desk, fields);
        int score = -1000;
        int temp;
        StepData result = new StepData();
        int count = 0;

        foreach (StepData sd in allStep) {
            virtualDesk = MakeVirtualStep(desk, sd);
            temp = CalcNextStep(virtualDesk, depth - 1, -1);
            temp += sd.score + sd.eatScore;

            if (score < temp) {
                score = temp;
                result = sd + temp;
            }
        }
        count += allStep.Count;
        Debug.Log(count);
        count = 0;
        return result;
    }

    private int CalcNextStep(char[,] desk, int depth, int c) {
        char[,] virtualDesk;
        List<StepData> allStep = StepRemover.GetRemainningSteps(teamQueue[depth % 2], desk, fields);
        int score = -1000 * c;
        int temp = 0;

        foreach (StepData sd in allStep) {
            virtualDesk = MakeVirtualStep(desk, sd);
            temp = (sd.score + sd.eatScore) * c;
            if (depth - 1 > 0) {
                temp += CalcNextStep(virtualDesk, depth - 1, -c);
            }
            
            if (c == 1) {
                if (score <= temp) 
                    score = temp;
            }
            else if (c == -1) {
                if (score >= temp) 
                    score = temp;
            }
        }
        return score;
    }

    public char[,] MakeVirtualStep(char[,] desk, StepData step) {
        char[,] newDesk = CopyMass(desk);
        int x = step.begin / size;
        int y = step.begin % size;
        newDesk[step.dest / 8, step.dest % 8] = desk[x, y];
        newDesk[x, y] = ' ';
        return newDesk;
    }

    private T[,] CopyMass<T>(T[,] mass) {
        T[,] res = new T[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
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