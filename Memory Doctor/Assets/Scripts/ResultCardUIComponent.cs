using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ResultCardUIComponent : MonoBehaviour {
    public TextMeshProUGUI [] MemoryResults;
    public Image [] CrossImages;
    public TextMeshProUGUI DisclaimerText;
    public TextMeshProUGUI Grade;

    public Signal SurgeryFinishedSignal;

    void Start () {
        Debug.Log("Here");
        SurgeryFinishedSignal.addListener(onSurgeryFinished);
    }

    public void onSurgeryFinished (SignalData data) {
        Dictionary<string, bool> results = data.get<Dictionary<string, bool>>("Results");
        int patientRemarkIndex = data.get<int>("PatientRemarkIndex");
        string grade = data.get<string>("Grade");
        //  for(int i = 0 ; i < MemoryResults.Length; i++) {
        //     MemoryResults[i].text = results.ElementAt(i).Key;
        //     CrossImages[i].gameObject.SetActive(results.ElementAt(i).Value);
        // }
        DisclaimerText.text = constants.PatientRemarks[patientRemarkIndex].ToStringNewLine();
        Grade.text = grade;
    }

}