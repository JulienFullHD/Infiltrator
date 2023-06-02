using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManger : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private float scoreComboMultiplier;
    [SerializeField] private float comboAddFadeTimer;
    [ReadOnly, SerializeField] private int currentMainScore;
    [ReadOnly, SerializeField] private int currentComboScore;
    [ReadOnly, SerializeField] private float currentMultiplier;
    private float scoreCalcDummy;

    [Header("Combo")]
    [SerializeField] private float comboMaxInactivityDuration;
    [ReadOnly, SerializeField] private bool comboIsCounting;
    [ReadOnly, SerializeField] private float comboCurrentInactivityDuration;

    [Header("Latest Scores")]
    private Dictionary<ScoreType, int> scoreTypeValues;
    //Shows latest score events + the amount of times it triggered in the current combo
    // Last 10 score events to track bonus points
    // New events added to the end

    [Header("UI References")]
    [SerializeField] private Slider comboTimerSlider;
    [SerializeField] private TextMeshProUGUI textMainScore;
    [SerializeField] private TextMeshProUGUI textComboScore;
    [SerializeField] private TextMeshProUGUI textComboScorePlus;
    [SerializeField] private TextMeshProUGUI textComboList;

    [Header("Trick: Full Arsenal")]
    [ReadOnly, SerializeField] private List<ScoreType> lastThreeHits;
    [SerializeField]private WinChecker winChecker;

    private void Start()
    {
        comboTimerSlider.value = 0;
        textMainScore.text = "0";
        currentMainScore = 0;
        textComboScore.text = "";
        textComboScorePlus.text = "";
        textComboList.text = "";

        scoreTypeValues = new Dictionary<ScoreType, int>();

        ResetLastThreeHits();

        ResetComboValues();
    }

    private void ResetComboValues()
    {
        scoreTypeValues.Clear();
        currentMultiplier = 1;
        currentComboScore = 0;
    }

    private void StartCombo()
    {
        currentMultiplier = 1;

        textComboScorePlus.text = "+";

        comboCurrentInactivityDuration = comboMaxInactivityDuration;
        //ResetComboValues();

        comboIsCounting = true;
    }

    private IEnumerator ComboFade(float duration, int comboScore)
    {
        Color textColorOriginal = textComboScore.color;
        Color textColor = textComboScore.color;
        float timer = duration;
        Vector3 mainScorePos = textMainScore.rectTransform.position;
        Vector3 comboScorePos = textComboScore.rectTransform.position;
        Vector3 lerpPos = textComboScore.rectTransform.position;

        while (timer > 0)
        {
            textColor.a = timer / duration;
            textComboScore.color = textColor;
            textComboScorePlus.color = textColor;

            lerpPos.y = Mathf.Lerp(mainScorePos.y, comboScorePos.y,  timer / duration);
            textComboScore.rectTransform.position = lerpPos;

            timer -= Time.deltaTime;
            yield return null;
        }

        textComboScore.text = "";
        textComboScorePlus.text = "";
        textComboScore.color = textColorOriginal;
        textComboScorePlus.color = textColorOriginal;
        textComboScore.rectTransform.position = comboScorePos;
        currentMainScore += comboScore;
        textMainScore.text = currentMainScore.ToString();
    }

    private void StopCombo()
    {
        StartCoroutine(ComboFade(comboAddFadeTimer, currentComboScore));

        ResetComboValues();

        ResetLastThreeHits();

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
        winChecker.EnemyKilled();
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

        CalculateScore(scoreType);

        if (!scoreTypeValues.ContainsKey(scoreType))
            scoreTypeValues.Add(scoreType, 0);

        scoreTypeValues[scoreType]++;

        textComboList.text = "";
        foreach (KeyValuePair<ScoreType, int> item in scoreTypeValues)
        {
            if(item.Value > 0)
            textComboList.text += item.Key + ":  x" + item.Value + "\n";
        }
    }

    private void CalculateScore(ScoreType scoreType)
    {
        switch (scoreType)
        {
            case ScoreType.UNDEFINED:
                break;
            case ScoreType.Kunai:
                scoreCalcDummy = 100;
                break;
            case ScoreType.Sword:
                scoreCalcDummy = 100;
                break;
            case ScoreType.Dash:
                scoreCalcDummy = 100;
                break;
            case ScoreType.Hattrick:
                scoreCalcDummy = 150;
                break;
            case ScoreType.Full_Arsenal:
                scoreCalcDummy = 200;
                break;
            default:
                break;
        }
        scoreCalcDummy *= currentMultiplier;

        currentComboScore += (int)scoreCalcDummy;

        currentMultiplier += scoreComboMultiplier;

        textComboScore.text = currentComboScore.ToString();
    }

    private void Update()
    {
        if (comboIsCounting)
        {
            comboCurrentInactivityDuration -= Time.deltaTime;

            comboTimerSlider.value = comboCurrentInactivityDuration / comboMaxInactivityDuration;

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
