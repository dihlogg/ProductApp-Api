﻿using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public interface ICartInfoRepository : IGenericRepository<CartInfo>
{
    Task<CartInfo?> GetCartInfoDetail(Guid cartInfoId);
    Task<IEnumerable<CartHistoryDto>> GetTransactions(Guid? userId = null, string? status = null);
    Task<CartHistoryDto?> GetTransactionByIdAsync(Guid id);
    Task<List<CartHistoryDto>> SearchTransactionsByUserNameAsync(string userName);
}
