using Newtonsoft.Json;

namespace Carsties.Shared.Helpers;

public class FileReader<T> where T : class
{
    public static List<T> SeederFileReader(string filename, string path)
    {
        var data = File.ReadAllText($"{path}/{filename}.json");
        var jsonData = JsonConvert.DeserializeObject<List<T>>(data);
        return jsonData;
    }
}
