namespace AndreVeiculos.Address.PostalCodeServices
{
    public interface IPostalCodeService
    {
        Task<IAddressResult?> Fetch(string postalCode);
    }
}
