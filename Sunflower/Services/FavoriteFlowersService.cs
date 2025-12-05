public class FavoriteFlowersService
{
    private List<PlantDetailsResponse> favorites = new();

    public IReadOnlyList<PlantDetailsResponse> Favorites => favorites;

    public void ToggleFavorite(PlantDetailsResponse plant)
    {
        if (favorites.Any(f => f.Id == plant.Id))
        {
            favorites.RemoveAll(f => f.Id == plant.Id);
        }
        else
        {
            favorites.Add(plant);
        }
    }

    public bool IsFavorite(int plantId)
    {
        return favorites.Any(f => f.Id == plantId);
    }
}
