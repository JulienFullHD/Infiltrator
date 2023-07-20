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
    private Coroutine fadeAnimationRoutine;
    private float animationRoutineTimer;

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

    [Header("Wwise Events")]        //Wwise
    public AK.Wwise.Event ComboOne;
    public AK.Wwise.Event ComboTwo;
    public AK.Wwise.Event ComboThree;
    public AK.Wwise.Event ComboOver;

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
        textComboList.text = "";
        currentMultiplier = 1;
        currentComboScore = 0;
    }

    private void StartCombo()
    {
        if (fadeAnimationRoutine is not null)
        {
            animationRoutineTimer = 0;            
        }

        currentMultiplier = 1;

        ComboOne.Post(gameObject);  //Wwise

        textComboScorePlus.text = "+";

        comboCurrentInactivityDuration = comboMaxInactivityDuration;
        //ResetComboValues();

        comboIsCounting = true;
    }

    private IEnumerator ComboFade(float duration, int comboScore)
    {
        Color textColorOriginal = textComboScore.color;
        Color textColor = textComboScore.color;
        animationRoutineTimer = duration;
        Vector3 mainScorePos = textMainScore.rectTransform.position;
        Vector3 comboScorePos = textComboScore.rectTransform.position;
        Vector3 lerpPos = textComboScore.rectTransform.position;

        while (animationRoutineTimer > 0)
        {
            textColor.a = animationRoutineTimer / duration;
            textComboScore.color = textColor;
            textComboScorePlus.color = textColor;

            lerpPos.y = Mathf.Lerp(mainScorePos.y, comboScorePos.y, animationRoutineTimer / duration);
            textComboScore.rectTransform.position = lerpPos;

            animationRoutineTimer -= Time.deltaTime;
            yield return null;
        }

        

        if(comboCurrentInactivityDuration < 0)
        {
            textComboScore.text = "";
            textComboScorePlus.text = "";
        }
        
        textComboScore.color = textColorOriginal;
        textComboScorePlus.color = textColorOriginal;
        textComboScore.rectTransform.position = comboScorePos;
        currentMainScore += comboScore;
        textMainScore.text = currentMainScore.ToString();
    }

    private void StopCombo()
    {
        fadeAnimationRoutine = StartCoroutine(ComboFade(comboAddFadeTimer, currentComboScore));

        ResetComboValues();

        ComboOver.Post(gameObject); //Wwise

        ResetLastThreeHits();

        comboIsCounting = false;
    }

    public void AddScoreType(ScoreType scoreType)
    {
        AddToLastThreeHits(scoreType: scoreType);
        AddScoreToList(scoreType: scoreType);
    }

    public void HitToScore(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.UNDEFINED:
                break;
            case HitType.Sword:
                AddScoreType(scoreType: ScoreType.Sword);
                break;
            case HitType.Kunai:
                AddScoreType(scoreType: ScoreType.Kunai);
                break;
            case HitType.Dash:
                AddScoreType(scoreType: ScoreType.Dash);
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

        TestForBonus();
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
            case ScoreType.FullArsenal:
                scoreCalcDummy = 100;
                break;
            case ScoreType.DoubleDash:
                scoreCalcDummy = 120;
                break;
            case ScoreType.TripleDash:
                scoreCalcDummy = 150;
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
            ComboTwo.Post(gameObject);  //Wwise
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
            AddScoreToList(scoreType: ScoreType.FullArsenal);
            ComboThree.Post(gameObject);    //Wwise
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

    public int GetScore()
    {
        return currentMainScore;
    }

    public enum ScoreType
    {
        UNDEFINED,
        Kunai,
        Sword,
        Dash,
        Hattrick,
        FullArsenal,
        DoubleDash,
        TripleDash,
    }
}
