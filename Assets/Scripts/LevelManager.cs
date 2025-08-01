using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Holder holderPrefab;
    public List<Holder> holders = new List<Holder>();
    public Level CurrentLevel { get; private set; } = new Level
    {
        no = 0,
        map = new List<HoldersColumn>
        {
            new HoldersColumn { values = new List<int> { 1, 1, 1, 2 } },
            new HoldersColumn { values = new List<int> { 1, 0, 2, 0 } },
            new HoldersColumn { values = new List<int> { 2, 2 } },
            new HoldersColumn { values = new List<int> { 0,0 } }
        }
    };

    private bool isMove;
    public State CurrentState { get; private set; } = State.None;
    void Start()
    {
        CurrentState = State.Playing;

        LoadLevel();
    }
    private void LoadLevel()
    {
        var listPos = SettingPosForHolders(CurrentLevel.map.Count);

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
    }
    private IEnumerable<int> GetColorPointID(List<int> column)
    {
        foreach (var color in column)
        {
            yield return color;
        }
    }
    private List<Vector3> SettingPosForHolders(int count, float distance = 1.7f)
    {
        List<Vector3> positions = new List<Vector3>();
        int maxPerRow = 3;
        int rows = Mathf.CeilToInt(count / maxPerRow);
        Vector2 center = transform.position;

        for (int row = 0; row < rows; row++)
        {
            int itemsInRow = (row == rows - 1) ? count - row * maxPerRow : maxPerRow;

            float rowY = center.y - row * (distance + 1.5f); 
            float totalWidth = (itemsInRow - 1) * distance;
            float startX = center.x - totalWidth / 2f;

            for (int i = 0; i < itemsInRow; i++)
            {
                float x = startX + i * distance;
                positions.Add(new Vector2(x, rowY));
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
            print("Pick holder thứ 2");
            if (holder.TopColorPoint == null || holder.TopColorPoint.ID == nextHolder.TopColorPoint.ID && !holder.isFull)
            {
                isMove = true;
                StartCoroutine(ExecuteAndThen(nextHolder.PushOutColor(Taked: holder), () =>
                {
                    isMove = false;
                    print("đổi chỗ");
                }));

                nextHolder.UndoPickedThis();
            }
            else
            {
                nextHolder.UndoPickedThis();
            }
        }
        else if (holder.TopColorPoint != null)
        {
            if (!holder.IsPicked)
            {
                holder.PickThis();

            }
            else
            {
                holder.UndoPickedThis();
            }
        }
    }

    private IEnumerator ExecuteAndThen(IEnumerator coroutine, Action onComplete)
    {
        yield return StartCoroutine(coroutine);  // đợi coroutine chạy xong
        onComplete?.Invoke();                    // sau đó chạy hành động tiếp theo
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