using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PayByPhoneInterview
{
    public class TwitterDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            const string twitterFormat = "ddd MMM dd HH:mm:ss zzzz yyyy";
            return DateTime.ParseExact(reader.Value.ToString(), twitterFormat, CultureInfo.InvariantCulture);
        }
    }
}