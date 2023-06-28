using System.Text;
using System.Text.Json;
using gcp.api.auth.domain;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;

namespace gcp.api.cloud.storage;

public class GCPBucketService : IGCPBucketService
{
    private readonly StorageClient _storageClient;
    
    public GCPBucketService(IOptions<GCPServiceAccountKey> options)
    {
        var credentials = GoogleCredential.FromJson(JsonSerializer.Serialize(options.Value));
        _storageClient = StorageClient.Create(credentials);
    }

    public IEnumerable<string> List(string bucketName)
    {
        // Listar todos los objetos en el bucket 
        return  _storageClient.ListObjects(bucketName, "").Select(obj => obj.Name).ToList();
    }
    
    public MemoryStream Get(string bucketName, string fileName)
    {
        // Descargar imagen desde el bucket
        var stream = new MemoryStream();
        _storageClient.DownloadObject(bucketName, fileName, stream);
        stream.Position = 0;
        return stream;
    }
    
    public string? Upload(string bucketName, IFormFile image)
    {
        try
        {
            /* TODO - Method replaces image if already existing name is provided... to discuss... */
            using var stream = new MemoryStream();
            image.CopyTo(stream);
            _storageClient.UploadObject(bucketName, image.FileName, image.ContentType, stream);

            return null;
        }
        catch (Exception e)
        {
            Log.Error("Error Saving Image");
            Log.Error(e.Message);
            return e.Message;
        }
    }
}