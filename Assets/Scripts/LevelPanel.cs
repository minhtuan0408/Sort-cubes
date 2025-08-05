using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    public LevelUI levelUI;
    public Transform contentPanel;
    private void Awake()
    {
        LoadAllLevel();
    }

    private void LoadAllLevel()
    {
        int levelUnlock = ResourceManager.Instance.GetLevelUnlock();
        foreach (var level in ResourceManager.Instance.group.levels)
        {
            var newLevelUI = Instantiate(levelUI, contentPanel);
            newLevelUI.gameObject.SetActive(true);
            newLevelUI.levelNameText.text = level.no.ToString();
            if (levelUnlock >= level.no) 
            {
                newLevelUI.lockIcon.SetActive(false);
            }
        }
    }
}
