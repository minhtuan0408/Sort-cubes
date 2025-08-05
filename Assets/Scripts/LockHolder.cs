using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHolder : MonoBehaviour
{
    public GameObject LockUI;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                LockHolder lockHolder = hit.collider.GetComponent<LockHolder>();
                if (lockHolder != null)
                {
                    LockUI.SetActive(true);
                }
            }
        }
    }
}
