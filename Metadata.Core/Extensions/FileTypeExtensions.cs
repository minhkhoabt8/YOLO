using Metadata.Core.Enums;
using Metadata.Core.Exceptions;

namespace Metadata.Core.Extensions;

public static class FileTypeExtensions
{
    public static FileTypeEnum ToFileTypeEnsureSupported(this string type)
    {
        return type switch
        {
            "application/msword" => FileTypeEnum.doc,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => FileTypeEnum.docx,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => FileTypeEnum.xlsx,
            "application/vnd.ms-excel" => FileTypeEnum.xls,
            "application/pdf" => FileTypeEnum.pdf,
            "image/png" => FileTypeEnum.img,
            "image/bmp" => FileTypeEnum.bmp,
            "image/jpeg" => FileTypeEnum.jpeg,
            "text/plain" => FileTypeEnum.txt,
            "application/vnd.ms-powerpoint" => FileTypeEnum.ppt,
            "application/vnd.openxmlformats-officedocument.presentationml.presentation" => FileTypeEnum.pptx,
            _ => throw new UnsupportedFileTypeException()
        };
    }


    public static string ToFileMimeTypeString(FileTypeEnum? fileType)
    {
        return fileType switch
        {
            FileTypeEnum.doc => "application/msword",
            FileTypeEnum.docx => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            FileTypeEnum.xls => "application/vnd.ms-excel",
            FileTypeEnum.xlsx => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileTypeEnum.pdf => "application/pdf",
            FileTypeEnum.img => "image/png",
            FileTypeEnum.txt => "text/plain",
            FileTypeEnum.ppt => "application/vnd.ms-powerpoint",
            FileTypeEnum.pptx => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            _ => throw new UnsupportedFileTypeException()
        };
    }
}