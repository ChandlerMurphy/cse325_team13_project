using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class PlantService
{
    private readonly HttpClient _http = new HttpClient();
    private readonly string _apiKey;

    public PlantService()
    {
        _apiKey = Environment.GetEnvironmentVariable("PERENUAL_API_KEY")
            ?? throw new Exception("API key not found!");
    }

    public async Task<List<PlantData>> GetPlantListAsync(int page = 1)
    {
        var url = $"https://perenual.com/api/v2/species-list?key={_apiKey}&indoor=1&page={page}";
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<PlantListResponse>(json, options);

        return result?.Data ?? new List<PlantData>();
    }

    public async Task<PlantDetailsResponse> GetPlantDetailsAsync(int id)
    {
        var apiKey = Environment.GetEnvironmentVariable("PERENUAL_API_KEY") 
            ?? throw new Exception("API key not found!");

        var url = $"https://perenual.com/api/v2/species/details/{id}?key={apiKey}";
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var result = JsonSerializer.Deserialize<PlantDetailsResponse>(json, options) ?? throw new Exception("Failed to deserialize PlantDetailsResponse");
        return result;
    }

}