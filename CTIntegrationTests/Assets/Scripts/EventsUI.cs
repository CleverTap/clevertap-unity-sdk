using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsUI : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public VerticalLayoutGroup VerticalLayoutGroup;

    private GameObject _fetchButton;

    void Start()
    {

        InitPanel();
    }

    public void Restore()
    {
        InitPanel();
    }

    private void InitPanel()
    {
        foreach (Transform child in VerticalLayoutGroup.GetComponent<RectTransform>())
        {
            Destroy(child.gameObject);
        }

       

    }

}
