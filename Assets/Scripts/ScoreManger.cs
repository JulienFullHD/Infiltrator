using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManger : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int currentMainScore;
    [SerializeField] private float scoreComboMultiplier;

    [Header("Combo")]
    [SerializeField] private bool comboIsCounting;
    [SerializeField] private float comboCurrentTotalTimer;
    [SerializeField] private float comboMaxInactivityDuration;
    [SerializeField] private float comboCurrentInactivityDuration;

    [Header("Latest Scores")]
    [SerializeField] private float timeToDequeue; 
    [SerializeField] private Dictionary<ScoreType, int> scoringTextbox; 
    //Shows latest score events + the amount of times it triggered in the current combo
    // Last 10 score events to track bonus points
    // New events added to the end

    [Header("Trick: Full Arsenal")]
    private List<ScoreType> lastThreeHits;

    private void Start()
    {
        lastThreeHits.Add(ScoreType.UNDEFINED);
        lastThreeHits.Add(ScoreType.UNDEFINED);
        lastThreeHits.Add(ScoreType.UNDEFINED);
    }

    private void ResetComboValues()
    {
        scoringTextbox.Add(ScoreType.Kunai, 0);
        scoringTextbox.Add(ScoreType.Sword, 0);
        scoringTextbox.Add(ScoreType.Dash, 0);
        scoringTextbox.Add(ScoreType.Hattrick, 0);
        scoringTextbox.Add(ScoreType.Full_Arsenal, 0);
    }

    private void StartCombo()
    {
        comboCurrentInactivityDuration = comboMaxInactivityDuration;
        ResetComboValues();

        comboIsCounting = true;
    }

    private void StopCombo()
    {
        comboIsCounting = false;
    }


    private void AddToLastThreeHits(ScoreType scoreType)
    {
        if(lastThreeHits.Count >= 3)
            lastThreeHits.RemoveAt(0);

        lastThreeHits.Add(scoreType);
    }

    private void AddScoreToList(ScoreType bonusScoreType)
    {
        scoringTextbox[bonusScoreType]++;
    }

    private void Update()
    {
        TestForBonus();
    }

    private void TestForBonus()
    {
        // Hattrick
        if (lastThreeHits[0] == ScoreType.Kunai && 
            lastThreeHits[1] == ScoreType.Kunai && 
            lastThreeHits[2] == ScoreType.Kunai)
        {
            AddScoreToList(ScoreType.Hattrick);
        }

        // Full_Arsenal
        if (ScoreTypeIsWeapon(lastThreeHits[0]) &&
            ScoreTypeIsWeapon(lastThreeHits[1]) &&
            ScoreTypeIsWeapon(lastThreeHits[2]) &&
            lastThreeHits[0] != lastThreeHits[1] &&
            lastThreeHits[0] != lastThreeHits[2] &&
            lastThreeHits[1] != lastThreeHits[2])
        {
            AddScoreToList(ScoreType.Full_Arsenal);
        }
    }

    private bool ScoreTypeIsWeapon(ScoreType scoreType)
    {
        return scoreType == ScoreType.Kunai || scoreType == ScoreType.Sword || scoreType == ScoreType.Dash;
    }

    public enum ScoreType
    {
        UNDEFINED,
        Kunai,
        Sword,
        Dash,
        Hattrick,
        Full_Arsenal,
    }
}
