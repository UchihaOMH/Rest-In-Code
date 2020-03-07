using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talker
{
    private Image illustBox;
    private Text textBox;

    private CanvasGroup illustBoxCG;

    public Talker(Image _illustBox, Text _textBox)
    {
        illustBox = _illustBox;
        textBox = _textBox;

        illustBoxCG = _illustBox.GetComponentInParent<CanvasGroup>();
    }
    
    public void SetIllust(Sprite _illust)
    {
        illustBox.sprite = _illust;
        illustBox.SetNativeSize();
    }
    public void Speak(string _text)
    {
        illustBoxCG.alpha = 1f;
        textBox.text = _text;
    }
    public void Wait()
    {
        illustBoxCG.alpha = 0.7f;
    }
    public void Leave()
    {
        illustBox.sprite = null;
        illustBoxCG.alpha = 0f;
    }
}
