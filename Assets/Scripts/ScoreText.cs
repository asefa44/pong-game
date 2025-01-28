using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public Animator animator;

    public void Highlight()
    {
        animator.SetTrigger("Highlight");
    }
    public void SetScore(int value)
    {
        textMeshProUGUI.text = value.ToString();
    }
}
