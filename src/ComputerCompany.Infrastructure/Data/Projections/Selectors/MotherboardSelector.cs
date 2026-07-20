using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class MotherboardSelector
{
    internal static Expression<Func<MotherboardEntity, MotherboardModel>> ToMotherboardModel = motherboard =>
    new MotherboardModel
    (
        motherboard.Id,
        motherboard.Name,
        motherboard.Description,
        motherboard.Price,
        motherboard.Count,
        motherboard.Model,
        motherboard.Chipset,
        motherboard.Socket
    );
}