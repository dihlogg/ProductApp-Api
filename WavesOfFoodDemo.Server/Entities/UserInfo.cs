namespace WavesOfFoodDemo.Server.Entities;

public class UserInfo : BaseEntities
{
    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public string? UserFullName { get; set; }

    public string? UserAddress { get; set; }

    public string? UserPhone { get; set; }

    public ICollection<CartInfo> CartInfos { get; set; }
}
