using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public Button OnButton;
    public Button OffButton;
    GameObject soundManager;
    private void Start()
    {
        

        OnButton.onClick.AddListener(OnSound);
        OffButton.onClick.AddListener(OffSound);
    }

    public void OnSound()
    {
        SoundManager.StopSound();
    }
    public void OffSound()
    {
        SoundManager.ResumeSound();
    }
}
