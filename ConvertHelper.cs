using FFMpegSharp;
using FFMpegSharp.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Audio
{
    public class ConvertHelper
    {
        public static void ConvertMp4ToMp3(string inputFile, string outPutFile)
        {
            var cv = new NReco.VideoConverter.FFMpegConverter();
            cv.ConvertMedia(inputFile, outPutFile, "mp3");
        }
        public static void AudioInVideo(string inPutVideo, string outFile, bool deleteOldFile = false)
        {
            if (deleteOldFile == true && File.Exists(outFile))
            {
                File.Delete(outFile);
            }
            new FFMpeg().ExtractAudio(
                    VideoInfo.FromPath(inPutVideo),
                    new System.IO.FileInfo(outFile)
                );
        }
        /// <summary>
        /// lấy ra audio từ video; tạo ra luồng mới để thao tác
        /// </summary>
        /// <param name="inPutVideo">video vào</param>
        /// <param name="outFile">âm thanh ra</param>
        /// <param name="deleteOldFile"> xóa file âm thanh nếu đã đc xuất trc đó </param>
        public static async Task<bool> AudioInVideoAsync(string inPutVideo, string outFile, bool deleteOldFile = false)
        {
            try
            {
                if (deleteOldFile == true && File.Exists(outFile))
                {
                    File.Delete(outFile);
                }
                await Task.Run(() =>
                {
                    new FFMpeg().ExtractAudio(
                        VideoInfo.FromPath(inPutVideo),
                        new System.IO.FileInfo(outFile)
                    );
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// lấy ra âm thanh trong 1 folder
        /// Nếu có file video sẽ chuyển đổi video trong đó thành audio 
        /// </summary>
        /// <param name="folder">đường đẫn folder</param>
        /// <returns>danh sách các đường dẫn đến các file</returns>
        public static async Task<List<string>> AudioInFolderAsync(string folder)
        {
            List<string> audios = new List<string>();
            audios.AddRange(Commons.FileAndFolders.FileFolder.AudioInFolder(folder));
            var videos = Commons.FileAndFolders.FileFolder.VideoInFolder(folder);
            foreach (var v in videos)
            {
                await Task.Run(() =>
                {
                    string extens = Path.GetExtension(v);
                    // sinh file âm thanh ngay cạnh file video
                    string newFile = v.Replace(extens, ".mp3");
                    AudioInVideo(v, newFile);
                    audios.Add(newFile);
                });
            }
            return audios;
        }
        public static async Task<bool> AudioInFolderByAsync(string folder, string outFile, bool deleteOldFile = true)
        {
            try
            {
                List<string> audios = new List<string>();
                audios.AddRange(Commons.FileAndFolders.FileFolder.AudioInFolder(folder));
                var videos = Commons.FileAndFolders.FileFolder.VideoInFolder(folder);
                foreach (var v in videos)
                {
                    string extens = Path.GetExtension(v);
                    // sinh file âm thanh ngay cạnh file video
                    string newFile = v.Replace(extens, ".mp3");
                    if (!File.Exists(newFile))
                    {
                        await AudioInVideoAsync(v, newFile, deleteOldFile);
                        audios.Add(newFile);
                    }
                }
                if (File.Exists(outFile) && deleteOldFile == true)
                    File.Delete(outFile);
                var outPut = new FileStream(outFile, FileMode.Create);
                JoinHelper.JoinAudio(audios, outPut);
                outPut.Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public static List<string> AudioInFolder(string folder)
        {
            List<string> audios = new List<string>();
            audios.AddRange(Commons.FileAndFolders.FileFolder.AudioInFolder(folder));
            var videos = Commons.FileAndFolders.FileFolder.VideoInFolder(folder);


            foreach (var v in videos)
            {
                string extens = Path.GetExtension(v);
                // sinh file âm thanh ngay cạnh file video
                string newFile = v.Replace(extens, ".mp3");
                AudioInVideo(v, newFile);
                audios.Add(newFile);

            }
            return audios;
        }
    }

   // 491404451814539498842029147538338665738015101264610825457542163
}
