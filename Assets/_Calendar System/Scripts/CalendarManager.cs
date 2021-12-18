using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using TimeManager;
using static TimeManager.DateTime;


public class CalendarManager : MonoBehaviour
{
    public int daysInMonth = 1;
    public CalendarPanel panelPrefab;

    public List<CalendarPanel> calendarPanels;
    public List<KeyDates> keyDates;
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI setDescText;
    public static TextMeshProUGUI DescriptionText;



    private int currentMonthView = 0;
    private DateTime previousDateTime;

    public Transform particleSystemHolder;
    public ParticleSystem[] particleSystems;
    public AudioSource[] audioSources;
    private ParticleSystem[] currentParticleSystems;
    

    void Awake()
    {
        TimeManagerScript.OnDateTimeChanged += DateTimeChanged;
    }

    private void OnDisable()
    {
        TimeManagerScript.OnDateTimeChanged -= DateTimeChanged;
    }
    
    private void Start()
    {
        daysInMonth = TimeManagerScript.DateTime.TotalDaysPerMonth;
        
        for (int i = 0; i < daysInMonth; i++)
        {
            CalendarPanel currentPanel = Instantiate(panelPrefab, transform);

            calendarPanels.Add(currentPanel);
        }

        

        DescriptionText = setDescText;
        DescriptionText.text = "";
        previousDateTime = TimeManagerScript.DateTime;
        SortDate();
        FillPanels(0);

        BuildAllFXs();
    }

    private void Update()
    {
        
    }

    void DateTimeChanged(DateTime _date)
    {
        if(currentMonthView == _date.Month)
        {
            if(previousDateTime.Date != _date.Date)//Increment day
            {
                var index = (previousDateTime.Date - 1) < 0 ? 0 : (previousDateTime.Date - 1);
                calendarPanels[index].HideHighlight();
                calendarPanels[_date.Date - 1].ShowHighlight();
                CheckCurrentDate();
            }

            calendarPanels[_date.Date - 1].ShowHighlight();
            previousDateTime = _date;

            
        }
    }

    private void SortDate()
    {
        keyDates = keyDates
            .OrderBy(d => d.KeyDate.Month)
            .ThenBy(d => d.KeyDate.Date)
            .ToList();
    }

    private void FillPanels(int _month )
    {
        monthText.text = FindObjectOfType<TimeManagerScript>().MonthsInAYear[_month];

        for (int i = 0; i < calendarPanels.Count; i++)
        {
            calendarPanels[i].SetUpDate((i + 1).ToString());

            if(currentMonthView == (int)TimeManagerScript.DateTime.Month && (i+1) == TimeManagerScript.DateTime.Date)
            {
                calendarPanels[i].ShowHighlight();
            }
            else
            {
                calendarPanels[i].HideHighlight();
            }

            foreach(var date in keyDates)
            {
                

                if ((i +1) == date.KeyDate.Date && date.KeyDate.Month == _month)
                {
                    calendarPanels[i].AssignKeyDate(date);
                }

                
            }
        }

    }

    private void BuildAllFXs()
    {
            currentParticleSystems = particleSystems;
    }


    //Check current date for events
    private void CheckCurrentDate()
    {
        foreach (var date in keyDates)
        {
            if(TimeManagerScript.DateTime.Month == date.KeyDate.Month && TimeManagerScript.DateTime.Date == date.KeyDate.Date)
            {
                Debug.Log(date.Desc);
                PlayParticleFX(date);
                PlaySoundFX(date);
            }
            else
            {
                StopAllParticleFX(date);
                StopAllSoundFX(date);
            }
        }
    }

    private void PlayParticleFX(KeyDates date)
    {
        if(date.hasParticleFX)
        {
            if(Random.Range(0,100) <= date.particleFXChance)
            {
                currentParticleSystems[date.particleSystemID].Play();
                Debug.Log("Playing Particle");
            }
        }
    }

    private void StopAllParticleFX(KeyDates date)
    {
        if (currentParticleSystems[date.particleSystemID] != null)
        {
            if (date.hasParticleFX && currentParticleSystems[date.particleSystemID].isPlaying)
            {
                currentParticleSystems[date.particleSystemID].Stop();
            }
        }
    }

    private void PlaySoundFX(KeyDates date)
    {
        if(date.hasSoundFX)
        {
            if(Random.Range(0,100) <= date.soundFXChance)
            {
                
                audioSources[date.audioSourceID].Play();
                Debug.Log("Playing Sound");
            }
        }
    }

    private void StopAllSoundFX(KeyDates date)
    {
        if (date.hasSoundFX && audioSources[date.audioSourceID].isPlaying)
        {
            audioSources[date.audioSourceID].Stop();
        }
    }
    public void OnNextSeason()
    {
        currentMonthView += 1;
        if (currentMonthView >= TimeManagerScript.DateTime.TotalMonthsPerYear)
            currentMonthView = 0;
        FillPanels(currentMonthView);
    }

    public void OnPreviousSeason()
    {
        currentMonthView -= 1;
        if (currentMonthView < 0)
            currentMonthView = TimeManagerScript.DateTime.TotalMonthsPerYear-1;
        FillPanels(currentMonthView);
    }


}
