using Newtonsoft.Json;

namespace Carsties.Shared.Helpers;

public class FileReader<T> where T : class
{
    public static List<T> SeederFileReader(string filename, string path)
    {
        var data = File.ReadAllText($"{path}/{filename}.json");
        var jsonData = JsonConvert.DeserializeObject<List<T>>(data);
#pragma warning disable CS8603 // Possible null reference return.
        return jsonData;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
