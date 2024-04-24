using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun;

    [SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private float sunRotationSpeed;

    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;

    private void Update()
    {
        timeOfDay += Time.deltaTime * sunRotationSpeed;
        if (timeOfDay > 24)
            timeOfDay = 0;
        UpdateSunRotation();
        UpdateLighting();

        // Check if it's night
        if (timeOfDay >= 19 || timeOfDay < 6)
        {
            Debug.Log("ChangeMaterial method called in DayNight script = night");
            SetNight();
        }

        // Check if it's day
        if (timeOfDay >= 6 && timeOfDay < 19)
        {
            Debug.Log("ChangeMaterial method called in DayNight script = day");
            SetDay();
        }

        // Check if it's midnight
        if (Mathf.Approximately(timeOfDay, 0))
        {
            Debug.Log("ChangeMaterial method called in DayNight script 0");
            SetNight();
        }
    }

    private void SetDay()
    {
        DayNight dayNightScript = FindObjectOfType<DayNight>();
        if (dayNightScript != null)
        {
            dayNightScript.isNight = false;
            dayNightScript.ChangeMaterial();
        }
    }

    private void SetNight()
    {
        DayNight dayNightScript = FindObjectOfType<DayNight>();
        if (dayNightScript != null)
        {
            dayNightScript.isNight = true;
            dayNightScript.ChangeMaterial();
        }
    }

    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();
    }

    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
    }

    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }
}
