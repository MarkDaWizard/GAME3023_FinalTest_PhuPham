using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManager;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CalendarPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public KeyDates KeyDate;
    public TextMeshProUGUI Date;
    public Image panelImage;
    public Image highlight;


    private void Awake()
    {
        HideHighlight();
    }


    public void HideHighlight()
    {
        highlight.gameObject.SetActive(false);
        highlight.color = Color.clear;
    }

    public void ShowHighlight()
    {
        highlight.color = Color.white;
        highlight.gameObject.SetActive(true);
    }

    public void AssignKeyDate(KeyDates keyDate)
    {
        KeyDate = keyDate;
        panelImage.sprite = KeyDate.thumbnail;
        panelImage.color = Color.white;
    }

    public void SetUpDate(string date)
    {
        Date.text = date;
        KeyDate = null;
        panelImage.sprite = null;
        panelImage.color = Color.clear;
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (KeyDate != null)
        {
            CalendarManager.DescriptionText.text = KeyDate.Desc;
        }
        else
        {
            CalendarManager.DescriptionText.text = "";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CalendarManager.DescriptionText.text = "";
    }
}
