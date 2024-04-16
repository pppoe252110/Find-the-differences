using AppodealAds.Unity.Api;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

public class InitWithEnv : MonoBehaviour
{
    async void Awake()
    {
        Appodeal.setTesting(true);

        var options = new InitializationOptions();

        options.SetEnvironmentName("dev");
        await UnityServices.InitializeAsync(options);
    }
}
