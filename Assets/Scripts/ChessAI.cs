using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI{
    //private string[,] currentDesk = new string[8,8];
    List<Pieces> pieces = new List<Pieces>();
    private string[,] globalDesk = ChessConfig.GetDesk();
    private int score;

    public ChessAI(List<Pieces> redTeam, List<Pieces> blackTeam) {
        pieces.AddRange(redTeam);
        //Debug.Log(redTeam.Count);
        //Debug.Log(blackTeam.Count);
        pieces.AddRange(blackTeam);
        //FillCurrDesk(redTeam);
        //FillCurrDesk(blackTeam);
    }

    /*private void FillCurrDesk(List<Pieces> team) {
        foreach (Pieces p in team) {
            if (p.isOurKing)
                currentDesk[p.x, p.y] = p.team + "King";
            else
                currentDesk[p.x, p.y] = p.team;
        }
    }*/

    public void CalcStep(List<Pieces> blackTeam) {
        CalcScore();
        Debug.Log(score);
    }

    private void CalcScore() {
        score = 0;
        foreach (Pieces p in pieces) {
            if (p.team == "red") 
                score -= PiecePower(p.x, p.y);
            if (p.team == "black")
                score += PiecePower(p.x, p.y);
        }
    }

    private int PiecePower(int x, int y) {
        return ChessConfig.GetPiecePower(globalDesk[x, y]);
    }


    override
    public string ToString() {
        /*string ret = "\n";
        for (int i = 7; i >= 0; i--) {
            for (int j = 0; j < 8; j++) {
                ret += currentDesk[j, i] + "|\t";
            }
            ret += "\n";
        }*/
        return score.ToString();
    }

}
