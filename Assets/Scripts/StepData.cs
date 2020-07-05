using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepData : IComparable<StepData>{

    public int x, y;    //destination coordinates
    public int score;   //the difference between the power of the piece before step and after
    public int eatScore = 0;
    public string stepType;
    public bool isPawnAtack;

    public StepData(int x, int y, int score, bool isPawnAtack = false) {
        this.x = x;
        this.y = y;
        this.score = score;
        this.isPawnAtack = isPawnAtack;
    }

    int IComparable<StepData>.CompareTo(StepData other) {
        return this.score + this.eatScore - other.score - other.eatScore;
    }
}
