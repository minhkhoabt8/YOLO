using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Forms;
using Aspose.Pdf.Text;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.AccountMapping;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLib.Core.Enums;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

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
            
            try
            {

                if (documentFile == null || Path.GetExtension(documentFile.FileName)?.ToLower() != ".pdf")
                {
                    throw new InvalidOperationException("Invalid file format. Please provide a PDF file.");
                }

                var validUser = await _authService.GetAccountByIdAsync(userId);

                if (validUser == null)
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

                string certificatePath = Path.Combine(userCertificatePath, $"{userId}.pfx");

                if (!File.Exists(certificatePath))
                {
                    throw new FileNotFoundException($"Certificate file not found for user [{userId}].");
                }
                PKCS7 pkcs = new PKCS7(certificatePath, signaturePassword);


                // Load the PDF document
                var pdfDoc = new Document();

                MemoryStream memoryStream = new MemoryStream();

                documentFile.CopyTo(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);

                pdfDoc = new Document(memoryStream);


                // Specify the name of the field
                string fieldName = "*Created By Yolo Team";

                // Create TextFragmentAbsorber object to search text
                TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber(fieldName);

                if (textFragmentAbsorber == null || textFragmentAbsorber.TextFragments == null)
                {
                    throw new InvalidActionException($"Cannot Specify Field [{fieldName}] For Signing. ");
                }

                // Accept the absorber for all pages
                pdfDoc.Pages.Accept(textFragmentAbsorber);

                var textFragments = textFragmentAbsorber.TextFragments;

                if (textFragments != null && textFragments.Count > 0)
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

                    var signedPdfFileStream = new FileStream(signedPdfFilePath, FileMode.Create);

                    pdfSignature.Save(signedPdfFileStream);

                    signedPdfFileStream.Close();

                    // Read the file content for ExportFileDTO
                    byte[] signedPdfFileBytes = File.ReadAllBytes(signedPdfFilePath);

                    pdfSignature.Close();

                    // Delete the signed PDF file
                    //File.Delete(signedPdfFilePath);

                    return new ExportFileDTO
                    {
                        FileName = signedPdfFileName,
                        FileByte = signedPdfFileBytes,
                        FileType = "application/pdf"
                    };
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidActionException($"File not found");
            }
            catch (Exception ex)
            {
                //File.Delete(filepath);
                throw new InvalidActionException("Cannot Sign Document");
            }

            return null;
        }

        public async Task<ExportFileDTO> SignDocumentWithPictureAsync(string userId, IFormFile documentFile, string signaturePassword, bool replaceSignatureWithPicture = true)
        {
            try
            {
                if (documentFile == null || Path.GetExtension(documentFile.FileName)?.ToLower() != ".pdf")
                {
                    throw new InvalidOperationException("Invalid file format. Please provide a PDF file.");
                }
                var validUser = await _authService.GetAccountByIdAsync(userId);
                if (validUser == null)
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
                string certificatePath = Path.Combine(userCertificatePath, $"{userId}.pfx");
                if (!File.Exists(certificatePath))
                {
                    throw new FileNotFoundException($"Certificate file not found for user [{userId}].");
                }
                PKCS7 pkcs = new PKCS7(certificatePath, signaturePassword);
                TimestampSettings timestampSettings = new TimestampSettings("https://freetsa.org/tsr", string.Empty); // User/Password can be omitted

                pkcs.TimestampSettings = timestampSettings;
                // Load the PDF document
                var pdfDoc = new Document();
                MemoryStream memoryStream = new MemoryStream();
                documentFile.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                pdfDoc = new Document(memoryStream);
                // Specify the name of the field
                string fieldName = "Người ký";
                // Iterate through all pages in the PDF document
                for (int pageIndex = 1; pageIndex <= pdfDoc.Pages.Count; pageIndex++)
                {
                    // Create TextFragmentAbsorber object to search text
                    TextFragmentAbsorber textFragmentAbsorber = new TextFragmentAbsorber(fieldName);
                    // Accept the absorber for the current page only
                    pdfDoc.Pages[pageIndex].Accept(textFragmentAbsorber);
                    var textFragments = textFragmentAbsorber.TextFragments;
                    if (textFragments != null && textFragments.Count > 0)
                    {
                        TextFragment firstTextFragment = textFragments[1];
                        
                        // Instantiate the PdfFileSignature for the loaded PDF document
                        Aspose.Pdf.Facades.PdfFileSignature pdfSignature = new Aspose.Pdf.Facades.PdfFileSignature(pdfDoc);
                        // Assign the access permissions
                        DocMDPSignature docMdpSignature = new DocMDPSignature(pkcs, DocMDPAccessPermissions.FillingInForms);
                        
                        // Sign the PDF file with the certify method
                        if (replaceSignatureWithPicture)
                        {
                            pdfSignature.BindPdf(pdfDoc);
                            System.Drawing.Rectangle stampRect = new System.Drawing.Rectangle(
                            Convert.ToInt32(firstTextFragment.Rectangle.LLX),
                            Convert.ToInt32(firstTextFragment.Rectangle.LLY - 110),
                            Convert.ToInt32(firstTextFragment.Rectangle.Width + 100),
                            Convert.ToInt32(firstTextFragment.Rectangle.Height) + 100);
                            
                            pdfSignature.SignatureAppearance = Path.Combine(_storagePath, "yolo-signature.png");
       
                            pdfSignature.Certify(pageIndex, $"Sign {Path.GetFileName(documentFile.FileName)}", $"{validUser.Phone}", "Binh-Dinh", true, stampRect, docMdpSignature);
                        }
                        else
                        {
                            // Get the position of the field
                            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                                Convert.ToInt32(firstTextFragment.Rectangle.LLX),
                                Convert.ToInt32(firstTextFragment.Rectangle.LLY - 100),
                                Convert.ToInt32(firstTextFragment.Rectangle.Width + 1000),
                                Convert.ToInt32(firstTextFragment.Rectangle.Height) + 88);
                            pdfSignature.Certify(pageIndex, $"Sign {Path.GetFileName(documentFile.FileName)}", $"{validUser.Phone}", "Binh-Dinh", true, rect, docMdpSignature);
                        }
                        // Set the certificate
                        pdfSignature.SetCertificate(certificatePath, signaturePassword);
                        // Save digitally signed PDF file in the user's folder
                        string userFolderPath = Path.Combine(userCertificatePath, "SignedPDFs");
                        Directory.CreateDirectory(userFolderPath); // Ensure the folder exists
                        string signedPdfFileName = $"DigitallySignedPDF-{Path.GetFileNameWithoutExtension(documentFile.FileName)}-{Guid.NewGuid()}.pdf";
                        string signedPdfFilePath = Path.Combine(userFolderPath, signedPdfFileName);
                        var signedPdfFileStream = new FileStream(signedPdfFilePath, FileMode.Create);
                        pdfSignature.Save(signedPdfFileStream);
                        signedPdfFileStream.Close();
                        // Read the file content for ExportFileDTO
                        byte[] signedPdfFileBytes = File.ReadAllBytes(signedPdfFilePath);
                        pdfSignature.Close();
                        // Delete the signed PDF file if needed
                        // File.Delete(signedPdfFilePath);
                        return new ExportFileDTO
                        {
                            FileName = signedPdfFileName,
                            FileByte = signedPdfFileBytes,
                            FileType = "application/pdf"
                        };
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidActionException($"File not found");
            }
            catch (Exception ex)
            {
                throw new InvalidActionException("Cannot Sign Document");
            }
            return null;
        }


        public async Task GenerateSignerCertificateAsync(string signerId, string secretPassword)
        {
            var signer = await _authService.GetAccountByIdAsync(signerId);

            if (signer == null)
            {
                throw new EntityWithIDNotFoundException<AccountMappping>(signerId);
            }

            if (signer == null || signer.Role.Id != ((int)AuthRoleEnum.Approval).ToString())
            {
                throw new CannotAssignSignerException();
            }

            // Generate RSA key pair
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                
                // Get or create the folder based on the user's GUID
                string folderPath = Path.Combine(_storagePath,"YOLO-Certificates", signer.Id);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (Directory.Exists(folderPath) && Directory.EnumerateFiles(folderPath).Any())
                {
                    throw new InvalidActionException("User Already Has Certificate");
                }

                // Export private key
                string privateKey = rsa.ToXmlString(true);
                string privateKeyPath = Path.Combine(folderPath, $"{signer.Id}.key");
                SaveToFile(privateKeyPath, privateKey);

                // Export public key
                string publicKey = rsa.ToXmlString(false);
                string publicKeyPath = Path.Combine(folderPath, $"{signer.Id}.crt");
                SaveToFile(publicKeyPath, publicKey);


                // Create a self-signed certificate with user information
                X509Certificate2 certificate = CreateSelfSignedCertificate(rsa, signer.Name, signer.Email ?? "", "YOLO", "Binh Dinh", "Binh Dinh", "Viet Nam");

                // Export certificate to the folder
                string certFileName = Path.Combine(folderPath, $"{signer.Id}.crt");

                SaveToFile(certFileName, certificate.Export(X509ContentType.Cert));

                // Export PFX to the folder with a password
                string pfxFileName = Path.Combine(folderPath, $"{signer.Id}.pfx");
                byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, secretPassword);
                SaveToFile(pfxFileName, pfxBytes);
            }
        }

        static X509Certificate2 CreateSelfSignedCertificate(RSACryptoServiceProvider rsa, string userName, string userEmail, string organization, string locality, string state, string country)
        {
            // Create a certificate request
            CertificateRequest request = new CertificateRequest(
                $"CN={userName}, Email={userEmail}, O={organization}, L={locality}, S={state}, C={country}",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            // Set the validity period of the certificate to 1 year
            DateTimeOffset notBefore = DateTimeOffset.Now;
            DateTimeOffset notAfter = notBefore.AddYears(1);

            // Add user information to the certificate
            request.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
                    false));

            // Create the self-signed certificate
            X509Certificate2 certificate = request.CreateSelfSigned(notBefore, notAfter);

            return certificate;
        }

        static void SaveToFile(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }

        static void SaveToFile(string fileName, byte[] content)
        {
            File.WriteAllBytes(fileName, content);
        }

        public async Task<bool> VerifySignedDocument(IFormFile signedFile)
        {
            PdfFileSignature pdfSign = new PdfFileSignature();
            try
            {
                using (var fileStream = signedFile.OpenReadStream())
                {
                    pdfSign.BindPdf(fileStream);

                    if (!pdfSign.ContainsSignature())
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                // Ensure that pdfSign.Close() is always called
                pdfSign.Close();
            }

            return true;
        }

        public async Task<bool> VerifySignerSignatureExistAsync(string signerId)
        {
            // Get or create the folder based on the user's GUID
            string folderPath = Path.Combine(_storagePath, "YOLO-Certificates", signerId);

            if (Directory.Exists(folderPath) && Directory.EnumerateFiles(folderPath).Any())
            {
                throw new InvalidActionException("User Already Has Certificate");
            }
            return true;
        }

    }
}
