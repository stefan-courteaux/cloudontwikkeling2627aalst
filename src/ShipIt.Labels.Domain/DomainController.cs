using QuestPDF.Fluent;
using ShipIt.Labels.Domain.DTO;
using ShipIt.Labels.Domain.Model;
using ShipIt.Labels.Storage;

namespace ShipIt.Labels.Domain;

public class DomainController(ILabelRepository repo) : IDomainController
{
    private readonly ILabelRepository _repo = repo;

    public async Task<string> CreateLabelAsync(LabelCreationDTO labelCreation)
    {
        // init domain model objects
        var addressModel = new Address(labelCreation);
        var labelModel = new Label(addressModel, labelCreation.ShipmentPrice);

        // generate pdf
        var pdf = labelModel.GeneratePdf();

        // write file to storage
        await _repo.WriteAsync(pdf, labelModel.TrackingNumber);

        // Return tn for created label
        return labelModel.TrackingNumber;
    }

    public Task<byte[]> GetLabelAsync(string trackingNumber){
        return _repo.ReadAsync(trackingNumber);
    }
}
