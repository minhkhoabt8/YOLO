using Aspose.Pdf;
using Aspose.Pdf.Forms;
using Aspose.Pdf.Text;
using Metadata.Infrastructure.DTOs.AccountMapping;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class DigitalSignatureService : IDigitalSignatureService
    {
        private readonly string _storagePath;
        private readonly IAuthService _authService;

        public DigitalSignatureService(IConfiguration configuration, IAuthService authService)
        {
            _storagePath = configuration["StoragePath"]!;
            _authService = authService;
        }

        public async Task<ExportFileDTO> SignDocumentAsync(string userId, IFormFile documentFile, string signaturePassword)
        {
            if (documentFile == null || Path.GetExtension(documentFile.FileName)?.ToLower() != ".pdf")
            {
                throw new InvalidOperationException("Invalid file format. Please provide a PDF file.");
            }

            var validUser = await _authService.GetAccountByIdAsync(userId);

            if(validUser == null)
            {
                throw new EntityWithIDNotFoundException<AccountMappping>(userId);
            }

            // Retrieve the storage path from configuration
            string storagePath = _storagePath 
                ?? throw new InvalidOperationException("Storage Path not configured.");

            // Build the path for the user's certificate folder
            string userCertificatePath = Path.Combine(storagePath, "YOLO-Certificates", userId);

            // Check if the user's certificate folder exists, create it if not
            if (!Directory.Exists(userCertificatePath))
            {
                throw new DirectoryNotFoundException($"Certificate folder not found for user [{userId}].");
            }

            // Load the PDF document
            Document pdfDoc = new Document();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await documentFile.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                pdfDoc = new Document(memoryStream);
            }

            // Specify the name of the field
            string fieldName = "*Created By Yolo Team";

            // Create TextFragmentAbsorber object to search text
            TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber(fieldName);

            if(textFragmentAbsorber == null)
            {
                throw new InvalidActionException($"Cannot Specify Field [{fieldName}] For Signing. ");
            }

            // Accept the absorber for all pages
            pdfDoc.Pages.Accept(textFragmentAbsorber);

            TextFragmentCollection textFragments = textFragmentAbsorber.TextFragments;

            if (textFragments.Count > 0)
            {
                TextFragment firstTextFragment = textFragments[1];

                // Get the position of the field
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                    Convert.ToInt32(firstTextFragment.Rectangle.LLX),
                    Convert.ToInt32(firstTextFragment.Rectangle.LLY - 100),
                    Convert.ToInt32(firstTextFragment.Rectangle.Width + 1000),
                    Convert.ToInt32(firstTextFragment.Rectangle.Height) + 88);

                // Instantiate the PdfFileSignature for the loaded PDF document
                Aspose.Pdf.Facades.PdfFileSignature pdfSignature = new Aspose.Pdf.Facades.PdfFileSignature(pdfDoc);

                // Load the certificate file along with the password
                string certificatePath = Path.Combine(userCertificatePath, $"{userId}.pfx");

                if (!File.Exists(certificatePath))
                {
                    throw new FileNotFoundException($"Certificate file not found for user [{userId}].");
                }
                PKCS7 pkcs = new PKCS7(certificatePath, signaturePassword);

                // Assign the access permissions
                DocMDPSignature docMdpSignature = new DocMDPSignature(pkcs, DocMDPAccessPermissions.FillingInForms);

                // Sign the PDF file with the certify method
                pdfSignature.Certify(1, "Sign Plan", "0834102453", "Binh-Dinh", true, rect, docMdpSignature);

                // Set the certificate
                pdfSignature.SetCertificate(certificatePath, signaturePassword);

                // Save digitally signed PDF file in the user's folder
                string userFolderPath = Path.Combine(userCertificatePath, "SignedPDFs");
                Directory.CreateDirectory(userFolderPath); // Ensure the folder exists

                string signedPdfFileName = $"DigitallySignedPDF-{Guid.NewGuid()}.pdf";
                string signedPdfFilePath = Path.Combine(userFolderPath, signedPdfFileName);

                using (FileStream signedPdfFileStream = new FileStream(signedPdfFilePath, FileMode.Create))
                {
                    pdfSignature.Save(signedPdfFileStream);
                }

                // Read the file content for ExportFileDTO
                byte[] signedPdfFileBytes = File.ReadAllBytes(signedPdfFilePath);

                // Delete the signed PDF file
                File.Delete(signedPdfFilePath);

                return new ExportFileDTO
                {
                    FileName = signedPdfFileName,
                    FileByte = signedPdfFileBytes,
                    FileType = "application/pdf"
                };
            }
            return null;
        }

    }
}
