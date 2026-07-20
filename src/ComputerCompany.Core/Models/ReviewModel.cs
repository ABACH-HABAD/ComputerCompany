using System.Text;

namespace ComputerCompany.Core.Models;

public record ReviewModel(Guid Id, AccountModel Sender, string Message, int Stars) : BaseModel(Id)
{
    public const char ActiveStar = '★';
    public const char NotActiveStar = '☆';

    private static readonly string[] _starsStrings = new string[6];

    public string DisplayStars => StarsToString(Stars);

    public static string StarsToString(int Stars)
    {
        if (_starsStrings[Stars] != null && _starsStrings[Stars] != string.Empty) return _starsStrings[Stars];
        else
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(ActiveStar, Stars);
            stringBuilder.Append(NotActiveStar, (5 - Stars));
            _starsStrings[Stars] = stringBuilder.ToString();
            return _starsStrings[Stars];
        }
    }
}