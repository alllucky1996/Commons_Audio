using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Audio
{
    public class BassHelper
    {
        //public static void AddBass(string pathAudio)
        //{
        //    // init BASS using the default output device
        //    if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
        //    {
        //        // create a stream channel from a file
        //        int stream = Bass.BASS_StreamCreateFile("test.mp3", 0, 0, BASSFlag.BASS_DEFAULT);
        //        if (stream != 0)
        //        {
        //            // play the stream channel
        //            Bass.BASS_ChannelPlay(stream, false);
        //        }
        //        else
        //        {
        //            // error creating the stream
        //            Console.WriteLine("Stream error: {0}", Bass.BASS_ErrorGetCode());
        //        }

        //        // wait for a key
        //        Console.WriteLine("Press any key to exit");
        //        Console.ReadKey(false);

        //        // free the stream
        //        Bass.BASS_StreamFree(stream);
        //        // free BASS
        //        Bass.BASS_Free();
        //    }
        //}
    }
}
