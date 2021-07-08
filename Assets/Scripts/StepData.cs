using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepData : IComparable<StepData> {

    public int begin;  //starting position
    public int dest;   //destination coordinates
    public int score {get; private set;}    //the difference between the power of the piece before step and after
    public int eatScore;
    public string stepType;
    public bool isPawnAtack;

    public StepData() { }

    public StepData(int dest, int begin, int score, bool isPawnAtack = false) {
        this.dest = dest; 
        this.begin = begin;
        this.score = score;
        this.isPawnAtack = isPawnAtack;
        this.eatScore = 0;
    }

    public StepData(int dest, int begin, int score, string stepType) : this(dest, begin, score) {
        this.stepType = stepType;
    }

    int IComparable<StepData>.CompareTo(StepData other) {
        return this.score + this.eatScore - other.score - other.eatScore;
    }

    public static bool operator > (StepData thisStep, StepData oterStep) {
        return (thisStep.score + thisStep.eatScore) > (oterStep.score + oterStep.eatScore);
    }

    public static bool operator < (StepData thisStep, StepData oterStep) {
        return (thisStep.score + thisStep.eatScore) < (oterStep.score + oterStep.eatScore);
    }

    public static StepData operator * (StepData step, int n) {
        step.score *= n;
        step.eatScore *= n;
        return step;
    }

    public static StepData operator +(StepData thisStep, int oterStep) {
        int sumScore = thisStep.score + thisStep.eatScore + oterStep;
        return new StepData(thisStep.dest, thisStep.begin, sumScore, thisStep.stepType);
    }

    public override string  ToString() {
        return $"from {begin / 8}   {begin % 8}  |   to{dest/8}  {dest%8}  |   score {score}  eat{eatScore} | {stepType}";
    }
}