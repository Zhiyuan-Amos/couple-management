using System.Text.RegularExpressions;

namespace Couple.Client.States.Image;

public class CreateUpdateImageStateContainer
{
    private string _day;

    private string _month;

    private string _year;

    public CreateUpdateImageStateContainer()
    {
        var today = DateTime.Today;
        Day = today.Day.ToString();
        Month = today.Month.ToString();
        Year = today.Year.ToString()[2..];
        IsFavourite = false;
    }

    public CreateUpdateImageStateContainer(DateOnly date, bool isFavourite, byte[] data)
    {
        Day = date.Day.ToString();
        Month = date.Month.ToString();
        Year = date.Year.ToString()[2..];
        IsFavourite = isFavourite;
        Data = data;
    }

    public string Day
    {
        get => _day;
        set
        {
            if (value.Length != 0 && !Regex.IsMatch(value, "^[0-9]{1,2}$"))
            {
                return;
            }

            _day = value;
        }
    }

    public string Month
    {
        get => _month;
        set
        {
            if (value.Length != 0 && !Regex.IsMatch(value, "^[0-9]{1,2}$"))
            {
                return;
            }

            _month = value;
        }
    }

    public string Year
    {
        get => _year;
        set
        {
            if (value.Length != 0 && !Regex.IsMatch(value, "^[0-9]{1,2}$"))
            {
                return;
            }

            _year = value;
        }
    }

    public bool IsFavourite { get; set; }
    public byte[] Data { get; set; }

    public DateOnly GetDate() => new(int.Parse($"20{Year}"), int.Parse(Month), int.Parse(Day));
}
