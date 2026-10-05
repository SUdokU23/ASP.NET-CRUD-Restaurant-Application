namespace Resto.Service.Upload;

public class UploadService
{
    private readonly int _maxSize = 5 * (1024 * 1024);
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" , ".svg" };
    private readonly string[] _allowedMimeTypes = { "image/jpeg", "image/png", "image/webp", "image/svg+xml" };
    
    private bool DoesFileHaveContent(IFormFile file)
    {
        Console.WriteLine(file.Length);
        return file.Length != 0 ;
    }

    private bool IsFileTooBig(IFormFile file)
    { 
        Console.WriteLine(file.Length - _maxSize);
        return file.Length > _maxSize;
    }

    private bool IsFileExtensionAllowed(IFormFile file)
    {
        Console.WriteLine(Path.GetExtension(file.FileName));
        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        string fileMimeType = file.ContentType.ToLower();
        return _allowedExtensions.Contains(fileExtension) || _allowedMimeTypes.Contains(fileMimeType);
    }

    public bool IsFileValid(IFormFile file)
    {
        return !IsFileTooBig(file) && IsFileExtensionAllowed(file) && DoesFileHaveContent(file);
    }

    public async Task<string> UploadFile(IFormFile file)
    {
        string uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ItemLogos");

        string filePath = Path.Combine(folderPath, uniqueFileName);

        Directory.CreateDirectory(folderPath);

        await using var fileStream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(fileStream);

        if (File.Exists(filePath))
        {
            return "/ItemLogos/" + uniqueFileName;
        }

        return "Error";
    }

    public async Task<bool> DeleteFile(string relativePath)
    {
        try
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false; // File doesn't exist
        }
        catch
        {
            return false; // Something went wrong (e.g., permissions issue)
        }
    }
}