namespace Pharmacy_Inventory_System.Data
{
    public class MedicationImageService
    {
        private readonly IWebHostEnvironment _environment;

        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg", ".jpeg", ".png", ".webp"
            };

        public MedicationImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string? Validate(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return "Please select an image.";

            if (file.Length > 5 * 1024 * 1024)
                return "Image size must not exceed 5 MB.";

            string extension =
                Path.GetExtension(Path.GetFileName(file.FileName));

            if (!AllowedExtensions.Contains(extension))
                return "Only JPG, JPEG, PNG and WebP images are allowed.";

            return null;
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            string extension =
                Path.GetExtension(Path.GetFileName(file.FileName))
                    .ToLowerInvariant();

            string folder = Path.Combine(
                _environment.WebRootPath, "images", "medications");

            Directory.CreateDirectory(folder);

            string fileName = $"{Guid.NewGuid():N}{extension}";

            string fullPath = Path.Combine(folder, fileName);

            await using var stream = new FileStream(
                fullPath, FileMode.CreateNew, FileAccess.Write);

            await file.CopyToAsync(stream);

            return $"images/medications/{fileName}";
        }

        public void Delete(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            string prefix = "images/medications/";

            if (!relativePath.StartsWith(
                prefix, StringComparison.OrdinalIgnoreCase))
                return;

            string fileName = relativePath[prefix.Length..];

            if (string.IsNullOrWhiteSpace(fileName) ||
                fileName != Path.GetFileName(fileName))
                return;

            string folder = Path.GetFullPath(Path.Combine(
                _environment.WebRootPath, "images", "medications"));

            string fullPath = Path.GetFullPath(
                Path.Combine(folder, fileName));

            if (!fullPath.StartsWith(
                folder + Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase))
                return;

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}
