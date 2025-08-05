using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitResourceData();
    }


}

public partial class ResourceManager
{
    [SerializeField] private TextAsset lvAssets;
    public int TotalLevel => group.levels.Count;
    public LevelGroup group;



    private void InitResourceData()
    {
        group = JsonUtility.FromJson<LevelGroup>(lvAssets.text);
    }

    public Level GetLevel(int no)
    {
        return Instance.group.levels[no-1];
    }
    public int GetLevelUnlock()
    {
        return PlayerPrefs.GetInt("LevelUnlock", 1);
    }
    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel", 1);
    }
    public void SetLevelUnlock(int levelComplete)
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        int levelUnlock = PlayerPrefs.GetInt("LevelUnlock", 1);
        if (currentLevel == levelUnlock)
        {
            PlayerPrefs.SetInt("LevelUnlock", levelComplete + 1);
            PlayerPrefs.Save();

            Debug.Log("Đã lưu");
            Debug.Log("Level Unlock" + PlayerPrefs.GetInt("LevelUnlock"));
        }
        
    }
    
}

public partial class ResourceManager
{
    int money;

    public int GetMoney()
    {
        return PlayerPrefs.GetInt("Money", 0); ;
    }
    public void UpdateMoney(int value)
    {
        int crntMoney = GetMoney();
        PlayerPrefs.SetInt("Money", crntMoney + value);
        PlayerPrefs.Save();
    }
}