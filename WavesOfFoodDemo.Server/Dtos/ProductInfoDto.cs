using System;

namespace WavesOfFoodDemo.Server.Dtos;

public class ProductInfoDto : ProductInfoCreateDto
{
    public Guid Id { get; set; }
}