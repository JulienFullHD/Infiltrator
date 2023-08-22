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

    /// <summary>
    /// Reset values to default
    /// </summary>
    private void ResetComboValues()
    {
        scoreTypeValues.Clear();
        textComboList.text = "";
        currentMultiplier = 1;
        currentComboScore = 0;
    }

    /// <summary>
    /// Start a combo and the timer
    /// </summary>
    /// <param name="playSound">Should a sound be played</param>
    private void StartCombo(bool playSound = true)
    {
        if (fadeAnimationRoutine is not null)
        {
            animationRoutineTimer = 0;            
        }

        currentMultiplier = 1;

        if(playSound)
            ComboOne.Post(gameObject);  //Wwise

        textComboScorePlus.text = "+";

        comboCurrentInactivityDuration = comboMaxInactivityDuration;
        //ResetComboValues();

        comboIsCounting = true;
    }

    /// <summary>
    /// Play UI Animation of Combo-Counter merging with the score display
    /// </summary>
    /// <param name="duration">Duration of merge animation</param>
    /// <param name="comboScore">Current combo score</param>
    /// <returns></returns>
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

    /// <summary>
    /// Stops a combo, resets values and prepares for the next combo
    /// </summary>
    /// <param name="playSound"></param>
    private void StopCombo(bool playSound = true)
    {
        fadeAnimationRoutine = StartCoroutine(ComboFade(comboAddFadeTimer, currentComboScore));

        ResetComboValues();

        if(playSound)
            ComboOver.Post(gameObject); //Wwise

        ResetLastThreeHits();

        comboIsCounting = false;
    }

    /// <summary>
    /// Forcefully stops a combo and ends the animation instantly
    /// </summary>
    public void ForceStopCombo()
    {
        if (fadeAnimationRoutine is not null)
        {
            StopCoroutine(fadeAnimationRoutine);
        }

        currentMainScore += currentComboScore;
    }

    /// <summary>
    /// Add a score type to the combo list
    /// </summary>
    /// <param name="scoreType">Type of score</param>
    public void AddScoreType(ScoreType scoreType)
    {
        AddToLastThreeHits(scoreType: scoreType);
        AddScoreToList(scoreType: scoreType);
    }

    /// <summary>
    /// Convert a hit type to a score type
    /// </summary>
    /// <param name="hitType"></param>
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

    /// <summary>
    /// Add type of score to the last three hits list
    /// </summary>
    /// <param name="scoreType"></param>
    private void AddToLastThreeHits(ScoreType scoreType)
    {
        if(lastThreeHits.Count >= 3)
            lastThreeHits.RemoveAt(0);

        lastThreeHits.Add(item: scoreType);

        TestForBonus();
    }

    /// <summary>
    /// Add a score to the combo list
    /// </summary>
    /// <param name="scoreType">Type of score</param>
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

    /// <summary>
    /// Adds points to the combo score based on the score type
    /// </summary>
    /// <param name="scoreType">Type of score</param>
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

    /// <summary>
    /// Counts down combo timer
    /// </summary>
    private void Update()
    {
        if (comboIsCounting && !LevelManager.Instance.isWon)
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

    /// <summary>
    /// Test for bonus scores based on the last three score types
    /// </summary>
    /// <param name="playSound"></param>
    private void TestForBonus(bool playSound = true)
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

    /// <summary>
    /// Clear last three hits list to prepare for a new bonus
    /// </summary>
    private void ResetLastThreeHits()
    {
        lastThreeHits.Clear();
        lastThreeHits.Add(ScoreType.UNDEFINED);
        lastThreeHits.Add(ScoreType.UNDEFINED);
        lastThreeHits.Add(ScoreType.UNDEFINED);
    }

    /// <summary>
    /// Checks if achieved score type is a damaging weapon
    /// </summary>
    /// <param name="scoreType">Type of score</param>
    /// <returns></returns>
    private bool ScoreTypeIsWeapon(ScoreType scoreType)
    {
        return scoreType == ScoreType.Kunai || scoreType == ScoreType.Sword || scoreType == ScoreType.Dash;
    }

    /// <summary>
    /// Returns current score, not counting the current combo
    /// </summary>
    /// <returns></returns>
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
