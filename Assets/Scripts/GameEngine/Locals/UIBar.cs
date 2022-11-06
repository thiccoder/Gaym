using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform bar;
    private float MaxSize;
    private float CurSize = 0;
    public float Value { get { return CurSize / MaxSize; } set { CurSize = value * MaxSize;bar.sizeDelta = new Vector2(CurSize, 0); } }
    void Start()
    {
        MaxSize = bar.sizeDelta.x;
        print(MaxSize);
    }
}
