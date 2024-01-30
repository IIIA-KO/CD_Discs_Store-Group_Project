using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Options
{
    public record DefaultImageNames(string Disc);

    public record CloudUrls(string StorageUrl, string FilmCoversUrl, string MusicCoversUrl);

    public class StorageOptions
    {
        private DefaultImageNames DefaultImageStorageNames { get; }
        private CloudUrls CloudUrls { get; }

        public StorageOptions(DefaultImageNames defaultImageStorageNames, CloudUrls cloudUrls)
        {
            DefaultImageStorageNames = defaultImageStorageNames ?? throw new ArgumentNullException(nameof(defaultImageStorageNames));
            CloudUrls = cloudUrls ?? throw new ArgumentNullException(nameof(cloudUrls));
        }

        public string DefaultImageExtension => ".jpg";

        public string DefaultDiscCoverImagePath =>
            CloudUrls.StorageUrl + DefaultImageStorageNames.Disc;

        public string DefaultDiscImageStorageName =>
            DefaultImageStorageNames.Disc;

        public string FilmCoversPath =>
            CloudUrls.StorageUrl + CloudUrls.FilmCoversUrl;

        public string MusicCoversPath =>
            CloudUrls.StorageUrl + CloudUrls.MusicCoversUrl;

        public string GetDefaultFilmCoverImagePath(Film film) =>
            FilmCoversPath + GetDefaultFilmImageStorageName(film);

        public string GetDefaultFilmImageStorageName(Film film) =>
            film.Genre + DefaultImageExtension;

        public string GetDefaultMusicCoverImagePath(Music music) =>
            MusicCoversPath + GetDefaultMusicImageStorageName(music);

        public string GetDefaultMusicImageStorageName(Music music) =>
            music.Genre + DefaultImageExtension;
    }
}
