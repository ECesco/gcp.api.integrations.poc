using Microsoft.AspNetCore.Http;

namespace gcp.api.cloud.storage;

public interface IGCPBucketService
{
    public IEnumerable<string> List(string bucketName);
    public MemoryStream Get(string bucketName, string fileName);
    public string? Upload(string bucketName, IFormFile image);

}