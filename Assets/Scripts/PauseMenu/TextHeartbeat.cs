using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHeartbeat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private float beatDuration;
    [ReadOnly, SerializeField] private float timer;

    [Header("Color")]
    [SerializeField] private AnimationCurve textAlpha;
    [ReadOnly, SerializeField] private Color32 clr;

    [Header("Size")]
    [SerializeField] private AnimationCurve textSize;
    [ReadOnly, SerializeField] private float sizeMultiplier;
    private float defaultSize;

    private void Awake()
    {
        timer = 0;
        clr = textField.color;
        defaultSize = textField.fontSize;
    }

    /// <summary>
    /// Pulsates the text using Animation Curves
    /// </summary>
    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if(timer > beatDuration)
        {
            timer = 0;
        }

        clr.a = (byte)(textAlpha.Evaluate(timer / beatDuration) * 255);
        textField.color = clr;

        sizeMultiplier = textSize.Evaluate(timer / beatDuration);
        textField.fontSize = defaultSize * sizeMultiplier;
    }
}
