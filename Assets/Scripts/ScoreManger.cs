using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class ScoreManger : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private int currentMainScore;
    [SerializeField] private float scoreComboMultiplier;

    [Header("Combo")]
    [SerializeField] private bool comboIsCounting;
    //[SerializeField] private float comboCurrentTotalTimer;
    [SerializeField] private float comboMaxInactivityDuration;
    [ReadOnly, SerializeField] private float comboCurrentInactivityDuration;

    [Header("Latest Scores")]
    private Dictionary<ScoreType, int> scoreTypeValues;
    //Shows latest score events + the amount of times it triggered in the current combo
    // Last 10 score events to track bonus points
    // New events added to the end

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoringTextbox;

    [Header("Trick: Full Arsenal")]
    [ReadOnly, SerializeField] private List<ScoreType> lastThreeHits;

    private void Start()
    {
        scoreTypeValues = new Dictionary<ScoreType, int>();

        ResetLastThreeHits();

        ResetComboValues();
    }

    private void ResetComboValues()
    {
        scoreTypeValues.Clear();
        scoreTypeValues.Add(ScoreType.Kunai, 0);
        scoreTypeValues.Add(ScoreType.Sword, 0);
        scoreTypeValues.Add(ScoreType.Dash, 0);
        scoreTypeValues.Add(ScoreType.Hattrick, 0);
        scoreTypeValues.Add(ScoreType.Full_Arsenal, 0);
    }

    private void StartCombo()
    {
        comboCurrentInactivityDuration = comboMaxInactivityDuration;
        //ResetComboValues();

        comboIsCounting = true;
    }

    private void StopCombo()
    {
        ResetComboValues();

        comboIsCounting = false;
    }

    public void HitToScore(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.UNDEFINED:
                break;
            case HitType.Sword:
                AddScoreToList(scoreType: ScoreType.Sword);
                AddToLastThreeHits(scoreType: ScoreType.Sword);
                break;
            case HitType.Kunai:
                AddScoreToList(scoreType: ScoreType.Kunai);
                AddToLastThreeHits(scoreType: ScoreType.Kunai);
                break;
            case HitType.Dash:
                AddScoreToList(scoreType: ScoreType.Dash);
                AddToLastThreeHits(scoreType: ScoreType.Dash);                
                break;
            case HitType.Smokebomb:
                break;
            case HitType.Environment:
                break;
            default:
                break;
        }
    }

    private void AddToLastThreeHits(ScoreType scoreType)
    {
        if(lastThreeHits.Count >= 3)
            lastThreeHits.RemoveAt(0);

        lastThreeHits.Add(item: scoreType);
    }

    private void AddScoreToList(ScoreType scoreType)
    {
        if(!comboIsCounting)
        {
            StartCombo();
        }
        else
        {
            comboCurrentInactivityDuration = comboMaxInactivityDuration;
        }

        scoreTypeValues[scoreType]++;

        scoringTextbox.text = "";
        foreach (KeyValuePair<ScoreType, int> item in scoreTypeValues)
        {
            scoringTextbox.text += item.Key + ": " + item.Value + "\n";
        }
    }

    private void Update()
    {
        if (comboIsCounting)
        {
            comboCurrentInactivityDuration -= Time.deltaTime;

            if(comboCurrentInactivityDuration < 0)
            {
                StopCombo();
            }
        }


        TestForBonus();
    }

    private void TestForBonus()
    {
        // Hattrick
        if (lastThreeHits[0] == ScoreType.Kunai && 
            lastThreeHits[1] == ScoreType.Kunai && 
            lastThreeHits[2] == ScoreType.Kunai)
        {
            ResetLastThreeHits();
            AddScoreToList(scoreType: ScoreType.Hattrick);
        }

        // Full_Arsenal
        if (ScoreTypeIsWeapon(scoreType: lastThreeHits[0]) &&
            ScoreTypeIsWeapon(scoreType: lastThreeHits[1]) &&
            ScoreTypeIsWeapon(scoreType: lastThreeHits[2]) &&
            lastThreeHits[0] != lastThreeHits[1] &&
            lastThreeHits[0] != lastThreeHits[2] &&
            lastThreeHits[1] != lastThreeHits[2])
        {
            ResetLastThreeHits();
            AddScoreToList(scoreType: ScoreType.Full_Arsenal);
        }
    }

    private void ResetLastThreeHits()
    {
        lastThreeHits.Clear();
        lastThreeHits.Add(ScoreType.UNDEFINED);
        lastThreeHits.Add(ScoreType.UNDEFINED);
        lastThreeHits.Add(ScoreType.UNDEFINED);
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
