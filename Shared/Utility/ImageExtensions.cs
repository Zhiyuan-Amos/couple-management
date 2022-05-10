namespace Couple.Shared.Utility;

public static class ImageExtensions
{
    // See https://www.filesignatures.net/
    private static readonly Dictionary<string, List<byte[]>> ImageSignatures =
        new()
        {
            {
                ".jpg",
                new()
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
                }
            },
            {
                ".jpeg",
                new()
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
                }
            },
            { ".png", new() { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } }
        };

    // See https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0#file-signature-validation
    public static bool IsImage(Stream fileData)
    {
        using var reader = new BinaryReader(fileData);

        var signatures = ImageSignatures.Values
            .SelectMany(sigs => sigs.ToList())
            .ToList();
        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

        return signatures.Any(signature =>
            headerBytes.Take(signature.Length).SequenceEqual(signature));
    }

    // Less precise validation compared to IsImage(Stream)
    public static bool IsImage(string fileExtension) => ImageSignatures.ContainsKey(fileExtension);
}
