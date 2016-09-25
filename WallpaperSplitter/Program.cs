using System;
using System.Drawing;
using System.IO;

namespace WallpaperSplitter {
	class Program {
		static void Main(string[] args) {
			Console.Title = "Wallpaper Splitter - Initializing";
			ConsoleWriteLine("Welcome to Wallpaper Splitter.\n");
			
			string targetDir = "";
			while (!Directory.Exists(targetDir)) {
				ConsoleWriteLine("Please enter the target directory: ");
				targetDir = ConsoleReadLine(ConsoleColor.Cyan);
			}

			ConsoleWriteLine("\nCreating output directory...", ConsoleColor.Yellow);
			string outputDir = targetDir;
			outputDir = outputDir.Substring(outputDir.Length - 1, 1) == "\\" || outputDir.Substring(outputDir.Length - 1, 1) == "/" ? outputDir : outputDir + "\\";
			outputDir += "Splitter output";
			if (!Directory.Exists(outputDir)) {
				Directory.CreateDirectory(outputDir);
			}

			ConsoleWriteLine("Loading files...", ConsoleColor.Yellow);
			string[] files = Directory.GetFiles(targetDir);
			ConsoleWriteLine($"{files.Length} files loaded.", ConsoleColor.Yellow);

			ConsoleWriteLine($"Processing files...\n", ConsoleColor.Yellow);
			for (int i = 0; i < files.Length; i++) {
				Console.Title = $"Wallpaper Splitter - Processing ({i + 1} / {files.Length})";

				try {
					Image target = Image.FromFile(files[i]);

					Bitmap original = new Bitmap(target);
					Rectangle leftHalf = new Rectangle(0, 0, target.Width / 2, target.Height);
					Bitmap leftCrop = original.Clone(leftHalf, original.PixelFormat);
					Rectangle rightHalf = new Rectangle(target.Width / 2, 0, target.Width / 2, target.Height);
					Bitmap rightCrop = original.Clone(rightHalf, original.PixelFormat);

					Bitmap result = new Bitmap(target.Width, target.Height);
					using (Graphics g = Graphics.FromImage(result)) {
						g.DrawImage(rightCrop, 0, 0);
						g.DrawImage(leftCrop, leftCrop.Width, 0);
					}
					
					result.Save(outputDir + "\\" + Path.GetFileName(files[i]));

					ConsoleWriteLine($"{Path.GetFileName(files[i])} processed", ConsoleColor.Green);
				} catch {
					ConsoleWriteLine($"Exception in file {Path.GetFileName(files[i])}", ConsoleColor.Red);
				}
			}

			Console.Title = "Wallpaper Splitter - Finished";
			ConsoleWriteLine("\nFinished executing!", ConsoleColor.Green);
			ConsoleWriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		static string ConsoleReadLine(ConsoleColor col = ConsoleColor.White) {
			Console.ForegroundColor = col;
			string result = Console.ReadLine();
			Console.ForegroundColor = ConsoleColor.White;
			return result;
		}

		static void ConsoleWriteLine(string input, ConsoleColor col = ConsoleColor.White) {
			Console.ForegroundColor = col;
			Console.WriteLine(input);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
