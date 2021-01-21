using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighlightOnMode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToHighlight = null;
    [SerializeField] private PlayerMode highlightMode = PlayerMode.Build;
    [SerializeField] private Color highlightColor = Color.red;
    private Color initialColor;

    private void Start()
    {
        initialColor = textToHighlight.color;
    }

    private void Update()
    {
        if (ModeManager.CurrentMode == highlightMode)
        {
            textToHighlight.color = highlightColor;
        }
        else
        {
            textToHighlight.color = initialColor;
        }
    }
}
