using Google.XR.Cardboard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.Management;

public class InitVideo : MonoBehaviour
{
    public VideoPlayer Video;
    public Button StartButton;
    public TMPro.TextMeshProUGUI Label;

    private bool vrPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        if (vrPlaying)
        {
            StopXR();
        }
        else
        {
            StartCoroutine(InitializeXRAndStart());
        }
    }

    private void Log(string message, bool error = false)
    {
        Label.text = message;
        Label.color = (error) ? Color.red : Color.white;
        if (error)
        {
            Debug.LogError(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    private IEnumerator InitializeXRAndStart()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Log("Initializing XR Failed.");
        }
        else
        {
            Log("XR initialized.");
            if (!Api.HasDeviceParams())
            {
                Api.ScanDeviceParams();
            }
            Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Log("XR started.");
            Video.Play();
            vrPlaying = true;
        }
    }

    private void StopXR()
    {
        Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Log("XR stopped.");

        Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Log("XR deinitialized.");

        Video.Stop();
    }

    public void Update()
    {
        if (vrPlaying)
        {
            if (Api.IsCloseButtonPressed)
            {
                StopXR();
            }

            if (Api.IsGearButtonPressed)
            {
                Api.ScanDeviceParams();
            }

            Api.UpdateScreenParams();
        }
    }
}
