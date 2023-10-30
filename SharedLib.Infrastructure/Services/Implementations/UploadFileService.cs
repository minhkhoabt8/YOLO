using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLib.Core.Extensions;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Infrastructure.Services.Implementations
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IConfiguration _configuration;
        private readonly string _awsAccessKeyId;
        private readonly string _awsSecretAccessKey;
        private readonly string _awsBucketName;
        private readonly string _awsRegion;
        private readonly string _awsLink = $"https://s3.amazonaws.com/";

        public UploadFileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _awsAccessKeyId = _configuration["AwsConfig:AccessKey"]!;
            _awsSecretAccessKey = _configuration["AwsConfig:SecretKey"]!;
            _awsBucketName = _configuration["AwsConfig:BucketName"]!;
            _awsRegion = _configuration["AwsConfig:Region"]!;
        }

        public async Task<string> UploadFileAsync(UploadFileDTO file)
        {
            var fileUpload = new TransferUtilityUploadRequest
            {
                InputStream = new MemoryStream(file.File),
                Key = file.FileName,
                BucketName = _awsBucketName,
                CannedACL = S3CannedACL.PublicRead,
                ContentType = file.FileType
            };
            var client = new AmazonS3Client(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.APSoutheast1);
            {
                var transferUtility = new TransferUtility(client);
                await transferUtility.UploadAsync(fileUpload);
            }

            var request = new GetPreSignedUrlRequest
            {
                BucketName = fileUpload.BucketName,
                Key = fileUpload.Key,
                Expires = DateTime.Now.SetKindUtc().AddYears(10),
            };
            return client.GetPreSignedURL(request);
        }
    }
}
