using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    private static FloatingTextManager instance;
    public static FloatingTextManager Instance => instance;
    public GameObject floatingTextPrfab;

    private void Awake()
    {
        instance = this;
    }

    public void ShowFloatingText(string text, Vector3 position)
    {
        var go = Instantiate(floatingTextPrfab, position + Vector3.up * 2f, Quaternion.identity);
        go.GetComponent<TextMeshPro>().text = text;
        Destroy(go, 2f);
    }
}