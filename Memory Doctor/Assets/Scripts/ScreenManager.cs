using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Manages the different screens and transitions between them
public class ScreenManager : MonoBehaviour
{
    public GameObject [] screens;

    // This signal is to allow screens to be changed from the code
    public Signal ScreenChangeSignal;
    GameObject currentScreen;

    void Start() {
        InitializeScreenState();
        ScreenChangeSignal.addListener(onScreenChange);
    }

    void InitializeScreenState () {
        currentScreen = screens[0];
        for (int i = 1; i < screens.Length; i++) {
            screens[i].SetActive(false);
        }
    }

    public void switchScreen(int screenIndex) {
        if(currentScreen != null) {
            currentScreen.SetActive(false);
        }
        currentScreen = screens[screenIndex];
        currentScreen.SetActive(true);
    }

    void onScreenChange (SignalData data) {
        int screenIndex = data.get<int>("ScreenIndex");
        switchScreen(screenIndex);
        // Passing additional data for screens
    }

}