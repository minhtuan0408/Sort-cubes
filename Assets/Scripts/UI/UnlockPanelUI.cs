using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPanelUI : MonoBehaviour
{
    public GameObject LockFromHolder;

    public void UnlockIt()
    {
        int money = ResourceManager.Instance.GetMoney();
        if (money >= 25)
        {
            LockFromHolder.SetActive(false);
            SoundManager.PlaySFX("Freeze");
            ResourceManager.Instance.UpdateMoney(-25);
            LevelManager.instance.MoneyPanelUI.UpdateMoneyUI();
        }
        else
        {
            SoundManager.PlaySFX("Wrong");
            Debug.Log("Not enough money to unlock");
        }
    }
}
