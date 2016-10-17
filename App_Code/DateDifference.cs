using System;
using System.Text;

public class DateDifference
{
    // Global variables
    private readonly int _year;
    private readonly int _month;
    private readonly int _day;

    // Public properties
    public int Years
    {
        get { return _year; }
    }

    public int Months
    {
        get { return _month; }
    }

    public int Days
    {
        get { return _day; }
    }

    /// <summary>
    /// defining Number of days in month; index 0=> january and 11=> December
    /// february contain either 28 or 29 days, that's why here value is -1
    /// which wil be calculate later.
    /// </summary>
    private readonly int[] _monthDay = { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    public DateDifference(DateTime d1, DateTime d2)
    {
        DateTime toDate;
        DateTime fromDate;
        if (d1 > d2)
        {
            fromDate = d2;
            toDate = d1;
        }
        else
        {
            fromDate = d1;
            toDate = d2;
        }

        // Day Calculation
        var increment = 0;

        if (fromDate.Day > toDate.Day)
        {
            increment = _monthDay[fromDate.Month - 1];
        }
        // if it is february month if it's to day is less then from day
        if (increment == -1)
        {
            increment = DateTime.IsLeapYear(fromDate.Year) ? 29 : 28;
        }
        if (increment != 0)
        {
            _day = (toDate.Day + increment) - fromDate.Day;
            increment = 1;
        }
        else
        {
            _day = toDate.Day - fromDate.Day;
        }

        //month calculation
        if ((fromDate.Month + increment) > toDate.Month)
        {
            _month = (toDate.Month + 12) - (fromDate.Month + increment);
            increment = 1;
        }
        else
        {
            _month = (toDate.Month) - (fromDate.Month + increment);
            increment = 0;
        }

        // year calculation
        _year = toDate.Year - (fromDate.Year + increment);
    }

    public override string ToString()
    {
        var builder = new StringBuilder("Zero days");

        if (_year > 0)
        {
            if (_month > 0)
            {
                builder.Clear();
                builder.Append(_year).Append(" year").Append(", ").Append(_month).Append(" month");

                if (_day > 0)
                {
                    builder.Append(" and ").Append(_day).Append(" days");
                }
            }
            else
            {
                if (_day <= 0) return builder.ToString();
                builder.Clear();
                builder.Append(" and ").Append(_day).Append(" days");
            }
        }
        else if (_year == 0)
        {
            if (_month > 0)
            {
                builder.Clear();
                builder.Append(_month).Append(" month");

                if (_day > 0)
                {
                    builder.Append(" and ").Append(_day).Append(" days");
                }
            }
            else
            {
                if (_day <= 0) return builder.ToString();
                builder.Clear();
                builder.Append(_day).Append(" days");
            }
        }

       return builder.ToString();
    }
}