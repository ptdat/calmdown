using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private GameObject content;

    public void ShowView()
    {
        content.SetActive(true);
    }

    public void HideView()
    {
        content.SetActive(false);
    }
}
