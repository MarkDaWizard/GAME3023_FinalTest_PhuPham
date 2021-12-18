using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace TimeManager
{
    public class TimeManagerScript : MonoBehaviour
    {
        [Header("Starting Date & Time Settings")]
        [Range(1, 28)]
        public int dateInMonth;
        [Range(1, 4)]
        public int month;
        [Range(1, 99)]
        public int year;
        [Range(0, 24)]
        public int hour;
        [Range(0, 6)]
        public int minutes;

        [Header("Global Date Settings")]
        public string[] DaysInAWeek;
        public string[] MonthsInAYear;

        public int DaysPerWeek = 7;
        public int DaysPerMonth = 28;
        public int MonthsPerYear = 4;
        public int DaysPerYear = 112;




        public static DateTime DateTime;

        [Header("Tick Settings")]
        public int TickMinutesIncrease = 10;
        public float TimeBetweenTicks = 1;
        private float currentTimeBetweenTicks = 0;

        public static UnityAction<DateTime> OnDateTimeChanged;

        private void Awake()
        {
            DaysPerWeek = DaysInAWeek.Length - 1;
            MonthsPerYear = MonthsInAYear.Length;
            DateTime = new DateTime(dateInMonth, month - 1, year, hour, minutes * 10);
            

            
        }

        private void Start()
        {
            OnDateTimeChanged?.Invoke(DateTime);
            
        }

        private void Update()
        {
            currentTimeBetweenTicks += Time.deltaTime;

            if (currentTimeBetweenTicks >= TimeBetweenTicks)
            {
                currentTimeBetweenTicks = 0;
                Tick();
            }
        }


        void Tick()
        {
            AdvanceTime();
        }

        void AdvanceTime()
        {
            DateTime.AdvanceMinutes(TickMinutesIncrease);

            OnDateTimeChanged?.Invoke(DateTime);
        }
        

    }

    [System.Serializable]
    public struct DateTime
    {


        //Fields
        private int day;
        [SerializeField] private int date;
        [SerializeField] private int year;

        [SerializeField] private int hour;
        [SerializeField] private int minutes;

        [SerializeField] private int month;

        private int totalNumDays;
        private int totalDaysPerWeek;
        private int totalDaysPerMonth;
        private int totalDaysPerYear;
        private int totalMonthsPerYear;


        //Properties
        public int Day => day;
        public int Date => date;
        public int Hour => hour;
        public int Minutes => minutes;
        public int Month => month;
        public int Year => year;
        public int TotalNumDays => totalNumDays;

        public int TotalDaysPerWeek => totalDaysPerWeek;
        public int TotalDaysPerMonth => totalDaysPerMonth;
        public int TotalDaysPerYear => totalDaysPerYear;

        public int TotalMonthsPerYear => totalMonthsPerYear;



        //Constructor

        public DateTime(int date, int month, int year, int hour, int minutes)
        {
            TimeManagerScript timeManager;
            timeManager = GameObject.FindObjectOfType<TimeManagerScript>();
            totalDaysPerWeek = timeManager.DaysPerWeek;
            totalMonthsPerYear = timeManager.MonthsPerYear;
            totalDaysPerMonth = timeManager.DaysPerMonth;
            totalDaysPerYear = timeManager.DaysPerYear;

            this.day = date % totalDaysPerWeek;
            if (day == 0)
                day = totalDaysPerWeek;
            this.date = date;
            this.month = month;
            this.year = year;

            this.hour = hour;
            this.minutes = minutes;


            totalNumDays = date + (totalDaysPerMonth * month) + ((totalDaysPerWeek * totalMonthsPerYear) * (year - 1));


        }


        //Time Advancement

        public void AdvanceMinutes(int MinutesToAdvance)
        {
            if (minutes + MinutesToAdvance >= 60)
            {
                minutes = (minutes + MinutesToAdvance) % 60;
                AdvanceHour();
            }
            else
            {
                minutes += MinutesToAdvance;
            }

           
        }

        private void AdvanceHour()
        {
            if ((hour + 1) == 24)
            {
                hour = 0;
                AdvanceDay();
                
            }
            else
            {
                hour++;
            }
        }


        private void AdvanceDay()
        {
           
            day++;
            if (day > totalDaysPerWeek)
            {
                day = 1;
                
            }

            date++;

            if (date % (totalDaysPerMonth + 1) == 0)
            {
                AdvanceSeason();
                date = 1;
            }

            totalNumDays++;

        }

        private void AdvanceSeason()
        {
            if (month == totalMonthsPerYear)
            {
                month = 0;
                AdvanceYear();
            }
            else month++;
        }

        private void AdvanceYear()
        {
            date = 1;
            year++;
        }


        //To Strings

        public override string ToString()
        {
            return $"Date: {DateToString()} Time: {TimeToString()} " +
                $"\nTotal Days: {totalNumDays}";
        }

        public string DateToString()
        {
            return $"{GameObject.FindObjectOfType<TimeManagerScript>().DaysInAWeek[day]} {Date} {Year.ToString("D2")}";

        }

        public string MonthToString()
        {
            return $"{GameObject.FindObjectOfType<TimeManagerScript>().MonthsInAYear[month]}";
        }

        public string TimeToString()
        {
            int adjustedHour = 0;

            if (hour == 0)
            {
                adjustedHour = 12;
            }
            else if (hour >= 12)
            {
                adjustedHour = hour - 12;
            }
            else
            {
                adjustedHour = hour;
            }


            string AmPm = hour < 12 ? "AM" : "PM";

            return $"{adjustedHour.ToString("D2")}:{minutes.ToString("D2")} {AmPm}";
        }
    }
}


