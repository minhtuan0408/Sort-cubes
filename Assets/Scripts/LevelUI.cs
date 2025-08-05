using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Button playButton;
    public TextMeshProUGUI levelNameText;
    public GameObject lockIcon;
    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        int level;
        if (int.TryParse(levelNameText.text, out level) && level > 0)
        {
            PlayerPrefs.SetInt("CurrentLevel", level);
            PlayerPrefs.Save();
            SceneManager.LoadScene("GamePlay");
        }
    }
}
