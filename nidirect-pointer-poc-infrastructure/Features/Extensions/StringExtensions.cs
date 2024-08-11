namespace nidirect_pointer_poc_infrastructure.Features.Extensions;

public static class StringExtensions
{
    private const int MaxPostCodeLength = 7;

    public static string FormatPostCode(string postcode)
    {
        string formattedPostcode = postcode.Replace("-", "").Replace(" ", "").ToUpper();

        switch (formattedPostcode.Length)
        {
            case 6:
                formattedPostcode = formattedPostcode.Insert(3, " ");
                break;
            case MaxPostCodeLength:
                formattedPostcode = formattedPostcode.Insert(4, " ");
                break;
        }

        return formattedPostcode;
    }
}