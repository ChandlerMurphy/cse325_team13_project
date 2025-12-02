public class PlantDetailsResponse
{
    public int Id { get; set; }
    public string? Common_Name { get; set; }
    public string? Family { get; set; }
    public string? Type { get; set; }
    public string? Watering { get; set; }
    public List<string>? Sunlight { get; set; }
    public bool Flowers { get; set; }
    public string? Flowering_Season { get; set; }
    public List<string>? Soil { get; set; }
    public List<string>? Pest_Susceptibility { get; set; }
    public bool Fruits { get; set; }
    public string? Fruiting_Season { get; set; }
    public string? Harvest_Season { get; set; }
    public string? Harvest_Method { get; set; }
    public bool Leaf { get; set; }
    public bool Edible_Leaf { get; set; }
    public string? Maintenance { get; set; }
    public bool Medicinal { get; set; }
    public bool Poisonous_To_Humans { get; set; }
    public bool Poisonous_To_Pets { get; set; }
    public bool Indoor { get; set; }
    public string? Care_Level { get; set; }
    public string? Description { get; set; }
    public PlantImage? Default_Image { get; set; }
}
