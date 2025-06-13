using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private string filePath;
    public ScoreList scoreList = new ScoreList();

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
        LoadScores();
    }

    public void AddScore(string playerName, int score)
    {
        scoreList.scores.Add(new ScoreIndividual { playerName = playerName, score = score });
        scoreList.scores.Sort((a, b) => b.score.CompareTo(a.score)); // Orden de puntuaciones descendente
        SaveScores();
    }

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(filePath, json);
    }

    public void ClearScores()
    {
        scoreList.scores.Clear();
        SaveScores();

        MuestraRanking mr = FindAnyObjectByType<MuestraRanking>();
        mr.MostrarRanking();

        Debug.Log("Ranking limpiado");
    }

    public void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
        }
    }
}
