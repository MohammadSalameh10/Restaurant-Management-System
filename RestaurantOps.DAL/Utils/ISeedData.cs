namespace RestaurantOps.DAL.Utils
{
    public interface ISeedData
    {
        Task DataSeedingAsync();
        Task IdentityDataSeedingAsync();
    }
}
