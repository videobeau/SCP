using System;
using System.Drawing;
using System.IO;

class Program
{
    const int ALPHA_CLIP = 128; // below this alpha value is transparent

    static int Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("-- SCP --");
        Console.ResetColor();
        if (args.Length < 1)
        {
            Console.WriteLine("Usage:\nscp [image_folder]");
            return -1;
        }

        string folderPath = args[0];
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("directory " + folderPath + " does not exist");
            return -1;
        }

        var files = Directory.GetFiles(folderPath, "*.png");
        if (files.Length <= 0)
        {
            Console.Write("no .png images found within the folder: " + folderPath);
            return -1;
        }

        foreach (var file in files)
        {
            Console.WriteLine("Processing " + new FileInfo(file).Name);
            PNGToSCP(file);
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Output to " + folderPath + "\\SCP\\");
        Console.ResetColor();
        return 0;
    }

    private static void PNGToSCP(string filepath)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
        Bitmap bitmap = new Bitmap(filepath);

        byte[] singleChannelValues = new byte[bitmap.Width * bitmap.Height];

        uint byteIndex = 0;
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                if (bitmap.GetPixel(x, y).A < ALPHA_CLIP)
                {
                    singleChannelValues[byteIndex] = 0;
                    continue;
                }
                else
                {
                    // change the range from 0-255 to 1-255, saving 0 for alpha
                    var val = bitmap.GetPixel(x, y).R / 255.0;
                    val *= 244.0;
                    val += 1;
                    singleChannelValues[byteIndex] = (byte)val;
                }

                byteIndex += 1;
            }
        }

        // Write the .scp
        writer.Write((uint)bitmap.Width);
        writer.Write((uint)bitmap.Height);
        writer.Write(singleChannelValues);

        byte[] buffer = memoryStream.ToArray();
        memoryStream.Dispose();

        //Console.WriteLine(BitConverter.ToString(buffer));

        var outputDirectory = new FileInfo(filepath).Directory.FullName + "\\SCP\\";
        Directory.CreateDirectory(outputDirectory);
        File.WriteAllBytes(outputDirectory + Path.GetFileNameWithoutExtension(new FileInfo(filepath).Name) + ".scp", buffer);

        writer.Dispose();
    }
}