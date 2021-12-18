using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TimeManager;

public class ClockManagerScript : MonoBehaviour
{
    public TextMeshProUGUI Date, Time, Month;


    private void OnEnable()
    {
        TimeManagerScript.OnDateTimeChanged += UpdateDateTime;

    }

    private void OnDisable()
    {
        TimeManagerScript.OnDateTimeChanged -= UpdateDateTime;
    }

    private void UpdateDateTime(DateTime dateTime)
    {
        Date.text = dateTime.DateToString();
        Time.text = dateTime.TimeToString();
        Month.text = dateTime.MonthToString();

        
    }


}
