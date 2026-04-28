using UnityEngine;
using System.Collections;
using TMPro;

public class TransitionUI : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject transitionPanel;
    public TextMeshProUGUI transitionText;

    private string[] messages = {
        "Algo oscuro se aproxima...",
        "El suelo tiembla...",
        "¡Prepárate!"
    };

    void Start()
    {
        transitionPanel.SetActive(false);
    }

    public void ShowTransition()
    {
        transitionPanel.SetActive(true);
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        foreach (string message in messages)
        {
            transitionText.text = message;
            yield return new WaitForSeconds(1f);
        }
    }
}