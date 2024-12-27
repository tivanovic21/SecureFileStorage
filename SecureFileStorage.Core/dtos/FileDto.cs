using SecureFileStorage.Core.Entities;

namespace SecureFileStorage.Core.Dtos
{
    public class FileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string EncryptedUrl { get; set; }
        public string PublicKey { get; set; }
        public string Signature {get; set;}
        public int UploaderId {get; set;}
        public DateTime UploadedAt { get; set; }

        public User? Uploader {get; set;} = null;
    }
}