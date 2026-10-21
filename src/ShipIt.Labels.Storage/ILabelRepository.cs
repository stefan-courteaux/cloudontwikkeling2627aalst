using System;
using Azure.Storage.Blobs;

namespace ShipIt.Labels.Storage;

public interface ILabelRepository
{
    Task WriteAsync(byte[] content, string trackingNumber);
    Task<byte[]> ReadAsync(string trackingNumber);
}

public class LabelRepository : ILabelRepository
{
    public async Task<byte[]> ReadAsync(string trackingNumber)
    {
        var blob = GetBlobClient($"{trackingNumber}.pdf");
        var download = await blob.DownloadContentAsync();
        return download.Value.Content.ToArray();
    }

    public async Task WriteAsync(byte[] content, string trackingNumber)
    {
        var blob = GetBlobClient($"{trackingNumber}.pdf");
        await blob.UploadAsync(new BinaryData(content));
    }

    private BlobClient GetBlobClient(string fileName)
    {
        var client = new BlobServiceClient("connectionstring");
        var containerClient = client.GetBlobContainerClient("labelsaalst");
        return containerClient.GetBlobClient(fileName);
    }
}