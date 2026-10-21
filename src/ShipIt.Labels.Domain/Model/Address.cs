using System;
using ShipIt.Labels.Domain.DTO;

namespace ShipIt.Labels.Domain.Model;

internal class Address
{
    public string Name { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Country { get; set; }

    public Address(){

    }

    public Address(LabelCreationDTO labelCreation)
    {
        Name = labelCreation.RecipientName;
        AddressLine1 = labelCreation.RecipientAddressLine1;
        AddressLine2 = labelCreation.RecipientAddressLine2;
        Country = labelCreation.RecipientCountry;
    }
}
