namespace DevInSales.Core.Interfaces
{
    public interface IRoleManager
    {
        public Task<bool> RoleExistsAsync(string roleName);
    }
}