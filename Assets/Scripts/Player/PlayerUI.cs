using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    public TextMeshProUGUI HealthAmount;
    // Start is called before the first frame update

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
