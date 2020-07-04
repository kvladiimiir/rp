using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using StackExchange.Redis;

namespace TextRancCalc
{
    public class TextRanc
    {
        public static void Calculate(string id, string currText)
        {
            int vowels = 0;
            int consonants = 0;
            string vowelsPattern = @"[aeiou]";
            string consonantsPattern = @"[bcdfghjklmnprrstvwxyz]";

            vowels = Regex.Matches(currText, vowelsPattern).Count;
            consonants = Regex.Matches(currText, consonantsPattern).Count;
            string result = "value = ";
            if (consonants == 0)
            {
                result += "null";
            }
            else
            {
                result += ((float)vowels/(float)consonants).ToString();
            }

            Console.WriteLine(result);

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect($"localhost:6379");
            IDatabase db = redis.GetDatabase();
            db.StringSet("resultRanc" + id, result);
        }
    }
}