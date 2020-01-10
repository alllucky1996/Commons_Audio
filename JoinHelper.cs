using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Audio
{
    public class JoinHelper
    {
        public string OutFile { get; set; }
        public List<DFileInfo> SourcePath { get; set; }
        public JoinHelper() { }
        public JoinHelper(string outFile, List<DFileInfo> sourcePath)
        {
            OutFile = outFile;
            SourcePath = sourcePath;
        }
        public JoinHelper(string outFile, string sourceFolder)
        {
            OutFile = outFile;
            SourcePath = new List<DFileInfo>();
            var files = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                DFileInfo file = new DFileInfo() { Name = Path.GetFileName(f), PathFile = f };
                SourcePath.Add(file);
            }
        }
        public void Join2Audio(string inputFile1, string inputFile2, string outFile)
        {
            using (var reader1 = new AudioFileReader(inputFile1))
            using (var reader2 = new AudioFileReader(inputFile2))
            {
                var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                WaveFileWriter.CreateWaveFile16(outFile, mixer);
            }
        }
        public static void MixAudio(List<string> inputFiles, string outFile)
        {
            List<ISampleProvider> sources = new List<ISampleProvider>();
            inputFiles.Sort();
            foreach (var item in inputFiles)
            {
                sources.Add(new AudioFileReader(item));
            }
            var mixer = new MixingSampleProvider(sources);
            WaveFileWriter.CreateWaveFile16(outFile, mixer);
        }
        public static void JoinAudio(List<string> inputFiles, Stream output)
        {
            foreach (string file in inputFiles)
            {
                Mp3FileReader reader = new Mp3FileReader(file);
                if ((output.Position == 0) && (reader.Id3v2Tag != null))
                {
                    output.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                }
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    output.Write(frame.RawData, 0, frame.RawData.Length);
                }
            }
        }
        public void JoinAudio(string outFile)
        {
            List<ISampleProvider> sources = new List<ISampleProvider>();
            foreach (var item in SourcePath.OrderBy(o=>o.Name))
            {
                sources.Add(new AudioFileReader(item.PathFile));
            }
            var mixer = new MixingSampleProvider(sources);
            WaveFileWriter.CreateWaveFile16(outFile, mixer);
        }

        /// <summary>
        /// Bên dưới chưa thử
        /// </summary>
        public void Run()
        {
            byte[] buffer = new byte[1024];
            WaveFileWriter waveFileWriter = null;

            try
            {
                foreach (string sourceFile in SourcePath.Select(o => o.PathFile))
                {
                    using (WaveFileReader reader = new WaveFileReader(sourceFile))
                    {
                        if (waveFileWriter == null)
                        {
                            // first time in create new Writer
                            waveFileWriter = new WaveFileWriter(OutFile, reader.WaveFormat);
                        }
                        else
                        {
                            if (!reader.WaveFormat.Equals(waveFileWriter.WaveFormat))
                            {
                                throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
                            }
                        }

                        int read;
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            waveFileWriter.WriteData(buffer, 0, read);
                        }
                    }
                }

            }

            finally
            {
                if (waveFileWriter != null)
                {
                    waveFileWriter.Dispose();
                }

            }
        }
        public void Run(Func<DFileInfo, bool> func)
        {
            byte[] buffer = new byte[1024];
            WaveFileWriter waveFileWriter = null;

            try
            {
                foreach (string sourceFile in SourcePath.Where(func).Select(o => o.PathFile))
                {
                    using (WaveFileReader reader = new WaveFileReader(sourceFile))
                    {
                        if (waveFileWriter == null)
                        {
                            // first time in create new Writer
                            waveFileWriter = new WaveFileWriter(OutFile, reader.WaveFormat);
                        }
                        else
                        {
                            if (!reader.WaveFormat.Equals(waveFileWriter.WaveFormat))
                            {
                                throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
                            }
                        }

                        int read;
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            waveFileWriter.WriteData(buffer, 0, read);
                        }
                    }
                }

            }

            finally
            {
                if (waveFileWriter != null)
                {
                    waveFileWriter.Dispose();
                }

            }
        }
    }

}
