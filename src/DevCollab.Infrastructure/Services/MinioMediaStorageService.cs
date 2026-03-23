using DevCollab.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace DevCollab.Infrastructure.Services;

public class MinioMediaStorageService : IMediaStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioMediaStorageService(IConfiguration config)
    {
        var endpoint = config["Minio:Endpoint"] ?? throw new InvalidOperationException("Minio Endpoint missing");
        var accessKey = config["Minio:AccessKey"] ?? throw new InvalidOperationException("Minio AccessKey missing");
        var secretKey = config["Minio:SecretKey"] ?? throw new InvalidOperationException("Minio SecretKey missing");
        _bucketName = config["Minio:BucketName"] ?? "devcollab-media";

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            // .WithSSL() // Uncomment if Minio has SSL
            .Build();
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        var beArgs = new BucketExistsArgs().WithBucket(_bucketName);
        bool found = await _minioClient.BucketExistsAsync(beArgs, cancellationToken);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _minioClient.MakeBucketAsync(mbArgs, cancellationToken);
        }
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        // Returning the file name, in a real scenario you would have a URL builder or CDN link
        return fileName;
    }

    public async Task<string> GetUrlAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var statObjectArgs = new StatObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName);

        await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);

        // Generate a presigned URL valid for 1 hour
        var presignedArgs = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithExpiry(3600);

        return await _minioClient.PresignedGetObjectAsync(presignedArgs);
    }

    public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }
}
