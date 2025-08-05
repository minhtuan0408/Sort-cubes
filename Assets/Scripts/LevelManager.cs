using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;



public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public MoneyPanelUI MoneyPanelUI;
    [SerializeField] private Holder holderPrefab;
    [SerializeField] private Holder holderLock;
    public List<Holder> holders = new List<Holder>();
    public Level CurrentLevel { get; private set; }
    public int LevelIndex { get; private set; }
    private bool isMove;
    public State CurrentState { get; private set; } = State.None;
    public GameObject GameOverPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        CurrentState = State.Playing;

        LevelIndex = ResourceManager.Instance.GetCurrentLevel();
        CurrentLevel = ResourceManager.Instance.GetLevel(LevelIndex);

        if (CurrentLevel != null)
        {
            LoadLevel();
        }
        
    }
    private void LoadLevel()
    {
        var listPos = SettingPosForHolders(CurrentLevel.map.Count + 1);
        
        for (int i = 0; i < CurrentLevel.map.Count; i++)
        {
            var holder = Instantiate(holderPrefab, listPos[i], Quaternion.identity);
            holder.name = "Hoder : " + i;
            holders.Add(holder);
            var column = GetColorPointID(CurrentLevel.map[i].values);
            foreach (var colorId in CurrentLevel.map[i].values)
            {
                holder.AddPointColor(colorId);
            }
        }

        holderLock.gameObject.transform.position = listPos[CurrentLevel.map.Count];
        holders.Add(holderLock);
    }
    private IEnumerable<int> GetColorPointID(List<int> column)
    {
        foreach (var color in column)
        {
            yield return color;
        }
    }

    private List<Vector3> SettingPosForHolders(int count, float distance = 1.5f)
    {
        List<Vector3> positions = new List<Vector3>();
        int maxPerInRow = 4;
        int rows = Mathf.CeilToInt((float)count / maxPerInRow); // Sửa chỗ này

        float rowDistance = 5f;
        float totalRowDistance = (rows - 1) * rowDistance;

        int placed = 0;
        float startY = transform.position.y + (totalRowDistance / 2f);

        for (int i = 0; i < rows; i++)
        {
            int itemsInRow = (i == rows - 1) ? count - placed : maxPerInRow;

            float totalWidth = (itemsInRow - 1) * distance;
            float startX = transform.position.x - totalWidth / 2f;
            float y = startY - i * rowDistance;

            for (int j = 0; j < itemsInRow; j++)
            {
                float x = startX + j * distance;
                positions.Add(new Vector3(x, y, transform.position.z));
                placed++;
            }
        }

        return positions;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Holder holder = hit.collider.GetComponent<Holder>();
                if (holder != null)
                {
                    PickThisHolder(holder);
                }
            }
        }
    }
    private void PickThisHolder(Holder holder)
    {
        if (isMove) return;
        var nextHolder = holders.FirstOrDefault(h => h.IsPicked);
        if (nextHolder != null && nextHolder != holder)
        {
            //print("Pick holder thứ 2");
            if (holder.TopColorPoint == null || holder.TopColorPoint.ID == nextHolder.TopColorPoint.ID && !holder.isFull)
            {
                SoundManager.PlaySFX("Pick");
                isMove = true;
                StartCoroutine(ExecuteAndThen(nextHolder.PushOutColor(Taked: holder), () =>
                {
                    
                    holder.CheckDoneColumn();

                    CheckGameOver();
                    nextHolder.UndoPickedThis();
                    isMove = false;
                }));
                
                
            }
            else
            {
                SoundManager.PlaySFX("Wrong");
                nextHolder.UndoPickedThis();
            }
        }
        else if (holder.TopColorPoint != null)
        {
            if (!holder.IsPicked)
            {
                holder.PickThis();
                SoundManager.PlaySFX("Pick");
            }
            else
            {
                SoundManager.PlaySFX("Wrong");
                holder.UndoPickedThis();
            }
        }
    }

    private IEnumerator ExecuteAndThen(IEnumerator coroutine, Action onComplete)
    {
        yield return StartCoroutine(coroutine);  
        onComplete?.Invoke();                    
    }


    private void CheckGameOver()
    {
        foreach (var holder in holders)
        {
            //print(holder.name + " : " + holder.TopColorPoint);
            if (holder.IsHidden)
            {
                continue;
            }
            if (holder.TopColorPoint != null)
            {
                return;
            }
        }
        ResourceManager.Instance.SetLevelUnlock(LevelIndex);
        ResourceManager.Instance.UpdateMoney(10);
        MoneyPanelUI.UpdateMoneyUI();
        GameOverPanel.SetActive(true);
    }

}

public enum State
{
    None,
    Playing,
    Over
}

[System.Serializable]
public class LevelGroup 
{ 
    public List<Level> levels; 
}
[System.Serializable]
public class Level
{
    public int no;
    public List<HoldersColumn> map;
}
[System.Serializable]
public class HoldersColumn 
{ 
    public List<int> values; 
}