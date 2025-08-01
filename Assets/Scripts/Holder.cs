using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public ColorPoint ColorPointPrefab;
    public Transform Stack;
    public bool IsPicked;

    public List<ColorPoint> stackColor = new List<ColorPoint>();
    public bool isFull => stackColor.Count >= 4;
    public ColorPoint TopColorPoint => stackColor.LastOrDefault();

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        IsPicked = false;
    }

    public void AddPointColor(int id)
    {
        Vector3 topPoint = Stack.position + Vector3.up * (stackColor.Count * 1.01f);
        var newColorPoint = Instantiate(ColorPointPrefab,transform);
        newColorPoint.transform.position = topPoint;
        newColorPoint.ID = id;
        newColorPoint.SetColor(id);
        newColorPoint.gameObject.SetActive(true);
        stackColor.Add(newColorPoint);

    }

    public void PickThis()
    {
        Debug.Log("PickThis - Holder.cs");
        IsPicked = true;
        if (stackColor.Count <= 0)
        {
            return;
        }
        List<ColorPoint> list = GetColorOnTop();

        foreach (var colorPoint in list)
        {
            print("Tên Color trong stack : " + colorPoint.name);
            colorPoint.MoveOn(colorPoint.TargetPoint);
        }
    }
    public void UndoPickedThis()
    {
        IsPicked = false;
        List<ColorPoint> list = GetColorOnTop();
        foreach (var colorPoint in list)
        {
            print("Tên Color trong stack : " + colorPoint.name);
            colorPoint.MoveOn(colorPoint.OriginalPoint);
        }
    }
    public List<ColorPoint> GetColorOnTop()
    {
        List<ColorPoint> transfer = new List<ColorPoint>();
        for (int i = stackColor.Count - 1; i >= 0; i--)
        {
            transfer.Add(stackColor[i]);
            if (i == 0 || stackColor[i].ID != stackColor[i-1].ID)
            {
                break;
            }
        }
        transfer.Reverse();
        return transfer;
    }

    public IEnumerator PushOutColor(Holder Taked)
    {
        print("Put Out" + gameObject.name);
        int maxSlotTaked = Taked.stackColor.Count;
        int availableSlot = 4 - maxSlotTaked;
        
        if (TopColorPoint == null) yield break;
        print(TopColorPoint);
        if (maxSlotTaked >= 0) 
        {
            print("availableSlot " + availableSlot);
            List<ColorPoint> list = GetColorOnTop();
            int transferCount = Mathf.Min(availableSlot, list.Count);
            for (int i = 0; i < transferCount; i++)
            {
                var colorPoint = list[list.Count - (i+1)];
                stackColor.Remove(colorPoint);
                Taked.stackColor.Add(colorPoint);
                colorPoint.transform.SetParent(Taked.transform);

                Vector3 newPos = Taked.Stack.position + new Vector3(0, (Taked.stackColor.Count-1) * 1.01f, 0);

                colorPoint.MoveOut(newPos, 10f);
                colorPoint.NewOriginalPoint(newPos);
            }
        }
        yield return null;
    }


}
