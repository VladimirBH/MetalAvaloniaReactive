using Newtonsoft.Json.Converters;

namespace WebServer.Classes;

public class OnlyDateConverter: IsoDateTimeConverter
{
    public OnlyDateConverter()
    {
        DateTimeFormat = "yyyy-MM-dd";
    }
}