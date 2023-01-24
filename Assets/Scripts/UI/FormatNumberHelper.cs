using UnityEngine;

public static class FormatNumberHelper
{
    private const int ZeroNumber = 0;
    private const int NumberToRound = 1000;

    private static string[] names = { "", "K", "M", "B", "T" };

    public static string FormatNumber(double number)
    {
        if (number == ZeroNumber) return "0";

        number = Mathf.Round((float)number);

        int i = 0;

        while (i + 1 < names.Length && number >= NumberToRound)
        {
            number /= NumberToRound;
            i++;
        }

        return number.ToString("#.#") + names[i];
    }
}
