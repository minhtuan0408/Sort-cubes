using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelUI : MonoBehaviour
{
    int currentLevel;
    int levelUnlock;

    private void OnEnable()
    {

        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        levelUnlock = PlayerPrefs.GetInt("LevelUnlock", 1);
        int totalLevel = ResourceManager.Instance.TotalLevel;
        if (currentLevel >= totalLevel)
        {
            gameObject.SetActive(false);
        }
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GamePlay");
    }
}
