using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserSettings : MonoBehaviour
{
    [ReadOnly,SerializeField] public static UserSettings Instance;
    private void Awake()
    {
        if(Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        LoadAllPrefs();
    }

    public void LoadAllPrefs()
    {
        LoadPrefsAudio();
        LoadPrefsGraphics();
        LoadPrefsControls();
        ResetBrokenValues();
    }

    public void SaveAllPrefs()
    {
        SetPrefsAudio();
        SetPrefsGraphics();
        SetPrefsControls();
        PlayerPrefs.Save();
    }

    private void ResetBrokenValues()
    {
        if (keybindKunai == KeyCode.None)
            keybindKunai = KeyCode.Mouse1;
        if (keybindSmokebomb == KeyCode.None)
            keybindSmokebomb = KeyCode.F;
        if (keybindDash == KeyCode.None)
            keybindDash = KeyCode.LeftShift;
        if(sensitivityHorizontal == 0)
            sensitivityHorizontal = 1;
        if(sensitivityVertical == 0)
            sensitivityVertical = 1;
        if ((int)graphicsQuality > 3 || (int)graphicsQuality < 0)
            graphicsQuality = 0;
    }

    #region Audio
    [Header("Audio")]
    [ReadOnly, SerializeField] private float volumeMaster;
    [ReadOnly, SerializeField] private float volumeMenu;
    [ReadOnly, SerializeField] private float volumeMusic;
    [ReadOnly, SerializeField] private float volumeEffects;
    [ReadOnly, SerializeField] private float volumeAmbience;

    public void LoadPrefsAudio()
    {
        volumeMaster = PlayerPrefs.GetFloat("VolumeMaster", 0.6f);
        volumeMenu = PlayerPrefs.GetFloat("VolumeMenu", 1f);
        volumeMusic = PlayerPrefs.GetFloat("VolumeMusic", 1f);
        volumeEffects = PlayerPrefs.GetFloat("VolumeEffects", 1f);
        volumeAmbience = PlayerPrefs.GetFloat("VolumeAmbience", 1f);
    }
    public void SetPrefsAudio()
    {
        PlayerPrefs.SetFloat("VolumeMaster", volumeMaster);
        PlayerPrefs.SetFloat("VolumeMenu", volumeMenu);
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
        PlayerPrefs.SetFloat("VolumeAmbience", volumeAmbience);
    }

    public float VolumeMaster
    {
        get
        {
            return volumeMaster;
        }
        set
        {
            volumeMaster = value;
            AkSoundEngine.SetRTPCValue("MasterVolume", volumeMaster);
        }
    }
    public float VolumeMenu
    {
        get
        {
            return volumeMenu;
        }
        set
        {
            volumeMenu = value;
            AkSoundEngine.SetRTPCValue("MenuVolume", volumeMenu);
        }
    }
    public float VolumeMusic
    {
        get
        {
            return volumeMusic;
        }
        set
        {
            volumeMusic = value;
            AkSoundEngine.SetRTPCValue("MusicVolume", volumeMusic);
        }
    }
    public float VolumeEffects
    {
        get
        {
            return volumeEffects;
        }
        set
        {
            volumeEffects = value;
            AkSoundEngine.SetRTPCValue("SFXVolume", volumeEffects);
        }
    }
    public float VolumeAmbience
    {
        get
        {
            return volumeAmbience;
        }
        set
        {
            volumeAmbience = value;
            AkSoundEngine.SetRTPCValue("AmbienceVolume", volumeAmbience);
        }
    }
    #endregion


    #region Graphics
    [Header("Graphics")]
    [SerializeField] private QualityLevels graphicsQuality;
    public enum QualityLevels
    {
        High = 0,
        Medium = 1,
        Low = 2
    }

    public void LoadPrefsGraphics()
    {
        graphicsQuality = (QualityLevels)PlayerPrefs.GetInt("GraphicsQuality", 0);
    }
    public void SetPrefsGraphics()
    {
        PlayerPrefs.SetInt("GraphicsQuality", (int)graphicsQuality);
    }

    public int GraphicsQuality
    {
        get
        {
            return (int)graphicsQuality;
        }
        set
        {
            graphicsQuality = (QualityLevels)value;
        }
    }
    #endregion


    #region Controls
    [Header("Controls")]
    [SerializeField] private float sensitivityVertical;
    [SerializeField] private float sensitivityHorizontal;
    [SerializeField] private KeyCode keybindKunai;
    [SerializeField] private KeyCode keybindSmokebomb;
    [SerializeField] private KeyCode keybindDash;

    public void LoadPrefsControls()
    {
        sensitivityVertical = PlayerPrefs.GetFloat("ControlsSensitivityVertical", 1);
        sensitivityHorizontal = PlayerPrefs.GetFloat("ControlsSensitivityHorizontal", 1);
        keybindKunai = (KeyCode)PlayerPrefs.GetInt("KeybindKunai", (int)KeyCode.Mouse1);
        keybindSmokebomb = (KeyCode)PlayerPrefs.GetInt("KeybindSmokebomb", (int)KeyCode.F);
        keybindDash = (KeyCode)PlayerPrefs.GetInt("KeybindDash", (int)KeyCode.LeftShift);
    }
    public void SetPrefsControls()
    {
        PlayerPrefs.SetFloat("ControlsSensitivityVertical", sensitivityVertical);
        PlayerPrefs.SetFloat("ControlsSensitivityHorizontal", sensitivityHorizontal);
        PlayerPrefs.SetInt("KeybindKunai", (int)keybindKunai);
        PlayerPrefs.SetInt("KeybindSmokebomb", (int)keybindSmokebomb);
        PlayerPrefs.SetInt("KeybindDash", (int)keybindDash);
    }

    public float SensitivityVertical
    {
        get
        {
            return sensitivityVertical;
        }
        set
        {
            sensitivityVertical = value;
        }
    }
    public string SensitivityVerticalString
    {
        get
        {
            return SensitivityVertical.ToString();
        }
        set
        {
            SensitivityVertical = Convert.ToInt32(value);
        }
    }
    public float SensitivityHorizontal
    {
        get
        {
            return sensitivityHorizontal;
        }
        set
        {
            sensitivityHorizontal = value;
        }
    }
    public string SensitivityHorizontalString
    {
        get
        {
            return SensitivityHorizontal.ToString();
        }
        set
        {
            SensitivityHorizontal = Convert.ToInt32(value);
        }
    }
    public KeyCode KeybindKunai
    {
        get
        {
            return keybindKunai;
        }
        set
        {
            keybindKunai = value;
        }
    }
    public KeyCode KeybindSmokebomb
    {
        get
        {
            return keybindSmokebomb;
        }
        set
        {
            keybindSmokebomb = value;
        }
    }
    public KeyCode KeybindDash
    {
        get
        {
            return keybindDash;
        }
        set
        {
            keybindDash = value;
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        SaveAllPrefs();
    }
}
