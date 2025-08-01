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
    }

    private void Start()
    {
        InitResourceData();
    }
}

public partial class ResourceManager
{
    [SerializeField] private TextAsset lvAssets;
    public LevelGroup group;

    private void InitResourceData()
    {
        group = JsonUtility.FromJson<LevelGroup>(lvAssets.text);
    }

    public Level GetLevel(int no)
    {
        return Instance.group.levels[no];
    }
}
