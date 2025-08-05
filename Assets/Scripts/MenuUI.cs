using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public BGData BG;

    public RawImage RawImageBG;
    private void Awake()
    {
        int random = Random.Range(0, BG.sprites.Count);

        RawImageBG.texture = BG.sprites[random].texture;

    }
}
