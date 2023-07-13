using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Keybind : MonoBehaviour
{
    [ReadOnly, SerializeField] private bool isCheckingForKeyPress = false;
    private delegate void KeyDelegate(KeyCode key);
    private KeyDelegate keyRebindMethod;
    private Coroutine rebindRoutine;

    [SerializeField] private TextMeshProUGUI kunaiKeyText;
    [SerializeField] private TextMeshProUGUI smokebombKeyText;
    [SerializeField] private TextMeshProUGUI dashKeyText;

    private void Start()
    {
        kunaiKeyText.text = UserSettings.Instance.KeybindKunai.ToString();
        smokebombKeyText.text = UserSettings.Instance.KeybindSmokebomb.ToString();
        dashKeyText.text = UserSettings.Instance.KeybindDash.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StopRebindRoutine();
    }

    private void KeyRebindKunai(KeyCode key)
    {
        UserSettings.Instance.KeybindKunai = key;
        kunaiKeyText.text = key.ToString();
    }

    private void KeyRebindSmokebomb(KeyCode key)
    {
        UserSettings.Instance.KeybindSmokebomb = key;
        smokebombKeyText.text = key.ToString();
    }

    private void KeyRebindDash(KeyCode key)
    {
        UserSettings.Instance.KeybindDash = key;
        dashKeyText.text = key.ToString();
    }

    public void StartRebindCheck(string identifier)
    {
        if (rebindRoutine is not null)
        {
            StopCoroutine(rebindRoutine);
            rebindRoutine = null;
        }

        switch(identifier) 
        {
            case "Kunai":
                keyRebindMethod = KeyRebindKunai;
                break;
            case "Smokebomb":
                keyRebindMethod = KeyRebindSmokebomb;
                break;
            case "Dash":
                keyRebindMethod = KeyRebindDash;
                break;
            default:
                keyRebindMethod = null;
                break;
        }
        StartCoroutine(CheckForKey());
    }

    public void StopRebindRoutine()
    {
        StopCoroutine(rebindRoutine);
        rebindRoutine = null;
    }

    private IEnumerator CheckForKey()
    {
        if (keyRebindMethod is null)
            yield break;

        isCheckingForKeyPress = true;
        while (isCheckingForKeyPress)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey) && vKey != KeyCode.Escape)
                {
                    isCheckingForKeyPress = false;

                    keyRebindMethod(vKey);

                    break;
                }
            }
            yield return null;
        }
        
    }
}
