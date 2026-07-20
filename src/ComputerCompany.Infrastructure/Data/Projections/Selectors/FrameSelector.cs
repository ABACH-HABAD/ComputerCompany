using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class FrameSelector
{
    internal static Expression<Func<FrameEntity, FrameModel>> ToFrameModel = frame =>
    new FrameModel(frame.Id, frame.Name, frame.Description, frame.Price, frame.Count, frame.Model, frame.FormFactor);
}