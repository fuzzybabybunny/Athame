using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athame.PluginAPI.Downloader
{
    public class FileType
    {
        /// <summary>
        /// The extension of the file. This should be used only for writing to disk; for URLs, you should check
        /// the response's Content-Type header then match it to the <see cref="MimeType"/> property.
        /// Note also the extension must not begin with a period.
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// The file type's MIME type.
        /// </summary>
        public string MimeType { get; set; }
    }

    public class MediaFileTypes
    {
        /// <summary>
        /// Represents the MP3 filetype, with the extension .mp3 and the MIME type "audio/mpeg".
        /// </summary>
        public static readonly FileType Mpeg3Audio = new FileType {Extension = "mp3", MimeType = "audio/mpeg"};
        /// <summary>
        /// Represents the AAC filetype, with the extension .aac and the MIME type "audio/aac".
        /// </summary>
        public static readonly FileType AdvancedAudioCoding = new FileType {Extension = "aac", MimeType = "audio/aac"};
        /// <summary>
        /// Represents the MPEG 4 audio filetype, with the extension .m4a and the MIME type "audio/mp4".
        /// </summary>
        public static readonly FileType Mpeg4Audio = new FileType { Extension = "m4a", MimeType = "audio/mp4"};
        /// <summary>
        /// Represents the Ogg Vorbis filetype, with the extension .ogg and the MIME type "audio/ogg".
        /// </summary>
        public static readonly FileType OggVorbis = new FileType {Extension = "ogg", MimeType = "audio/ogg"};
        /// <summary>
        /// Represents the FLAC filetype, with the extension .flac and the MIME type "audio/x-flac".
        /// </summary>
        public static readonly FileType FreeLosslessAudioCodec = new FileType
        {
            Extension = "flac",
            MimeType = "audio/x-flac"
        };

        private static readonly List<FileType> AllTypes = new List<FileType> {Mpeg3Audio, AdvancedAudioCoding, Mpeg4Audio, OggVorbis, FreeLosslessAudioCodec};

        /// <summary>
        /// Retrieves an extension for the specified MIME type, or null if none exists.
        /// Uses the static readonly members of this class to resolve the extension.
        /// </summary>
        /// <param name="mimeType">A MIME type.</param>
        /// <returns>The preferred extension for the MIME type, or null.</returns>
        public static string ExtensionByMimeType(string mimeType)
        {
            return (from ft in AllTypes
                where ft.MimeType == mimeType
                select ft.Extension).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the MIME type for the specified extension type, or null if none exists.
        /// Uses the static readonly members of this class to resolve the MIME type.
        /// </summary>
        /// <param name="extension">A file extension.</param>
        /// <returns>The preferred MIME type for the extension, or null.</returns>
        public static string MimeTypeByExtension(string extension)
        {
            return (from ft in AllTypes
                where ft.Extension == extension
                select ft.MimeType).FirstOrDefault();
        }
    }
}
