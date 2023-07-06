using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    #region Instance
    public static AbilityUI Instance;
    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
    #endregion


    #region Kunai
    [Header("Kunai Settings")]
    [SerializeField] private GameObject kunaiNoAmmo;
    [SerializeField] private GameObject kunaiOneAmmo;
    [SerializeField] private GameObject kunaiTwoAmmo;
    [SerializeField] private GameObject kunaiThreeAmmo;

    public void ChangeKunaiAmmoUI(int ammo)
    {
        kunaiNoAmmo.SetActive(false);
        kunaiOneAmmo.SetActive(false);
        kunaiTwoAmmo.SetActive(false);
        kunaiThreeAmmo.SetActive(false);

        if (ammo <= 0)
        {
            kunaiNoAmmo.SetActive(true);
        }
        else if (ammo == 1)
        {
            kunaiOneAmmo.SetActive(true);
        }
        else if (ammo == 2)
        {
            kunaiTwoAmmo.SetActive(true);
        }
        else if (ammo >= 3)
        {
            kunaiThreeAmmo.SetActive(true);
        }
    }
    #endregion

    #region Dash
    [Header("Dash Settings")]
    [ReadOnly, SerializeField] private bool dashIsOnCooldown = false;
    [SerializeField] private GameObject DashReady;
    [SerializeField] private GameObject DashOnCooldown;
    [SerializeField] private TextMeshProUGUI DashCooldownText;
    [ReadOnly, SerializeField] public float dashCooldownTimer;

    public void StartDashCooldown(float timer)
    {
        dashCooldownTimer = timer;
        dashIsOnCooldown = true;

        DashReady.SetActive(false);
        DashOnCooldown.SetActive(true);
    }

    private void StopDashingCooldown()
    {
        dashIsOnCooldown = false;
        DashCooldownText.text = "";
        DashReady.SetActive(true);
        DashOnCooldown.SetActive(false);
    }
    #endregion
    #region Smoke
    [Header("Smoke Settings")]
    [ReadOnly, SerializeField] private bool smokeIsOnCooldown = false;
    [SerializeField] private GameObject SmokeReady;
    [SerializeField] private GameObject SmokeOnCooldown;
    [SerializeField] private TextMeshProUGUI SmokeCooldownText;
    [ReadOnly, SerializeField] public float smokeCooldownTimer;

    public void StartSmokeCooldown(float timer)
    {
        smokeCooldownTimer = timer;
        smokeIsOnCooldown = true;

        SmokeReady.SetActive(false);
        SmokeOnCooldown.SetActive(true);
    }

    private void StopSmokeCooldown()
    {
        smokeIsOnCooldown = false;
        SmokeCooldownText.text = "";
        SmokeReady.SetActive(true);
        SmokeOnCooldown.SetActive(false);
    }
    #endregion

    private void Update()
    {
        if (dashIsOnCooldown)
        {
            dashCooldownTimer -= Time.deltaTime;
            DashCooldownText.text = ((int)(dashCooldownTimer + 1)).ToString();

            if (dashCooldownTimer <= 0)
            {
                StopDashingCooldown();
            }
        }

        if (smokeIsOnCooldown)
        {
            smokeCooldownTimer -= Time.deltaTime;
            SmokeCooldownText.text = ((int)(smokeCooldownTimer + 1)).ToString();

            if (smokeCooldownTimer <= 0)
            {
                StopSmokeCooldown();
            }
        }
    }
}
