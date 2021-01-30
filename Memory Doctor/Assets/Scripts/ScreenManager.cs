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

    public void start() {
        ScreenChangeSignal.addListener(onScreenChange);
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
    }

}