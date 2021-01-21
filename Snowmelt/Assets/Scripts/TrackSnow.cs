using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackSnow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToUpdate = null;

    private void Update()
    {
        textToUpdate.text = $"Snow: {SnowManager.SnowLeft}";
    }
}
