using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSave")]
public class GamePlayData : ScriptableObject {

    int Points;
    int Lifes;
    int ScorePerLife;

    public void SetPoints(int amount)
    {
        Points = amount;
    }
    public int GetPoints()
    {
        return Points;
    }

    public void SetLifes(int amount)
    {
        Lifes = amount;
    }
    public int GetLifes()
    {
        return Lifes;
    }

    public void SetScorePerLife(int amount)
    {
        ScorePerLife = amount;
    }
    public int GetScorePerLife()
    {
        return ScorePerLife;
    }
}
