using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanelUI : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;

    private void Start()
    {
        MoneyText.text = ResourceManager.Instance.GetMoney().ToString();
    }

    public void UpdateMoneyUI()
    {
        MoneyText.text = ResourceManager.Instance.GetMoney().ToString();
    }
}
