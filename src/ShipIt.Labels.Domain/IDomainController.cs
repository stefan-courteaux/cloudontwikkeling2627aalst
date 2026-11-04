using System;
using ShipIt.Labels.Domain.DTO;

namespace ShipIt.Labels.Domain;

public interface IDomainController
{
    Task<string> CreateLabelAsync(LabelCreationDTO labelCreation);

    Task<byte[]> GetLabelAsync(string trackingNumber);
}
