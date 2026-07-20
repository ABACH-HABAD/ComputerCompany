using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class PowerUnitSelector
{
    internal static Expression<Func<PowerUnitEntity, PowerUnitModel>> ToPowerUnitModel = powerUnit =>
    new PowerUnitModel(powerUnit.Id, powerUnit.Name, powerUnit.Description, powerUnit.Price, powerUnit.Count, powerUnit.Model, powerUnit.Certification, powerUnit.FormFactor, powerUnit.Power);
}