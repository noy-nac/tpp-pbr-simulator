using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Newtonsoft.Json;

namespace PBTS {
	public class Program {

		public static byte level;

		public static byte teamsize;

		public static bool doub;
		public static bool switching;
		public static bool inputctrl;
		public static bool randomorder;

		public static double friendlyfire;

		public static Team blue; 
		public static Team red;

		public static Battle battle;

		public static double[] results;


		// shoe-horned in
		public static WebClient api;
		public static dynamic tree;

		public static void Main() {
			api = new WebClient();

			while(true) {
				Go();
			}
		}

		public static void Go() {

			SetConsts();

			tree = JsonConvert.DeserializeObject(api.DownloadString("https://twitchplayspokemon.tv/api/current_match"));

			blue = new Team("BLUE", teamsize, level);
			red = new Team(" RED", teamsize, level);

			Fill();

			battle = new Battle(blue, red, doub, switching, inputctrl, randomorder, friendlyfire);

			GenerateResults(10000);
			Console.ReadLine();
			Console.Write("> ");
			string xtra = Console.ReadLine();
			
			if(xtra == "tsw") { // toggle switching
				battle = new Battle(blue, red, doub, !switching, inputctrl, randomorder, friendlyfire);

				GenerateResults(10000);
			}
			else if(xtra == "tic") { // toggle input control
				battle = new Battle(blue, red, doub, switching, !inputctrl, randomorder, friendlyfire);

				GenerateResults(10000);
			}

			Console.WriteLine("\n*  PRESS ENTER TO USE AGAIN  *");
			Console.ReadLine();
		}

		public static void SetConsts() {
			Console.Write("*  PKMN LEVEL    (100)  *\t>");
			try {
				level = Convert.ToByte(Console.ReadLine());
			}
			catch {
				level = 100;
			}

			Console.Write("*  TEAM SIZE       (3)  *\t>");
			try {
				teamsize = Convert.ToByte(Console.ReadLine());
			}
			catch {
				teamsize = 3;
			}

			Console.Write("*  DOUBLE BATTLE   (F)  *\t>");
			try {
				doub = Convert.ToBoolean(Console.ReadLine());
			}
			catch {
				doub = false;
			}

			Console.Write("*  SWITCHING       (T)  *\t>");
			try {
				switching = Convert.ToBoolean(Console.ReadLine());
			}
			catch {
				switching = true;
			}
			  
			Console.Write("*  INPUT CONTROL   (T)  *\t>");
			try {
				inputctrl = Convert.ToBoolean(Console.ReadLine());
			}
			catch {
				inputctrl = true;
			}

			Console.Write("*  RANDOM ORDER    (F)  *\t>");
			try {
				randomorder = Convert.ToBoolean(Console.ReadLine());
			}
			catch {
				randomorder = false;
			}

			Console.Write("*  FRIENDLY FIRE % (0)  *\t>");
			try {
				friendlyfire = Convert.ToDouble(Console.ReadLine()) /100;
			}
			catch {
				friendlyfire = 0;
			}

			Console.WriteLine();
			}

		public static void Fill() {
			blue.FillType();
			red.FillType();

			blue.FillMoves();
			red.FillMoves();

			blue.FillStats();
			red.FillStats();
		}

		public static double N(double x, double u, double s) {
			return Math.Exp(-1 * (x - u) * (x - u) / (2 * s * s)) / (s * Math.Sqrt(2 * Math.PI));
		}

		public static void GenerateResults(ushort count) {
			results = new double[count];

			for(ushort i = 0; i < results.Length; i++) {
				Console.Title = (i + 1) + " / " + count;

				results[i] = battle.Run();
			}
			double mean = results.Sum() / count;
			double sd = 0;

			for(ushort i = 0; i < count; i++) {
				sd += (mean - results[i]) * (mean - results[i]);
			}
			sd = Math.Sqrt(sd / (count - 1));

			double bluearea = 0;
			double redarea = 0;

			for(double i = 0; i < mean + 6 * sd; i += 0.0001) {
				bluearea += N(i, mean, sd) * .01;
			}
			for(double i = 0; i > mean - 6 * sd; i -= 0.0001) {
				redarea += N(i, mean, sd) * .01;
			}

			Console.WriteLine("\n*  BLUE: {0}%", Math.Round(100 * bluearea) / 100);
			Console.WriteLine("*   RED: {0}%", Math.Round(100 * redarea) / 100);

			Console.WriteLine("*   BET: {0}% {1}", Math.Round(100 * Math.Abs(bluearea - redarea) / 6) / 100, mean >= 0 ? "BLUE" : "RED");

			Console.WriteLine("\n*  MEAN: {0} +/- {1} for {2}", Math.Round(100 * Math.Abs(mean)) / 100, Math.Round(100 * sd / Math.Sqrt(count)) / 100, mean >= 0 ? "BLUE" : "RED");
			Console.WriteLine("*  STDV: {0}\n", Math.Round(100 * sd) / 100);

			Array.Sort(results);

			// histagram

			double[] hist = new double[96 + 1];

			double max = teamsize;
			double min = -1 * teamsize;

			for(ushort i = 0; i < results.Length; i++) {
				hist[(byte)Math.Floor((results[i] - min) * (hist.Length - 1) / (max - min))]++;
			}

			double maxc = hist.Max();

			byte len = 10;

			for(byte i = 0; i < len; i++) {
				//Console.Write(hist[i] + " ");
				for(sbyte j = 0; j < hist.Length; j++) {

					if(j < (hist.Length) / 2) {
						if(j % (hist.Length / (4 * teamsize)) == 0) {
							Console.ForegroundColor = ConsoleColor.Magenta;
						}
						else {
							Console.ForegroundColor = ConsoleColor.Red;
						}
					}
					else if(j > (hist.Length) / 2) {
						if(j % (hist.Length / (4 * teamsize)) == 0) {
							Console.ForegroundColor = ConsoleColor.Cyan;
						}
						else {
							Console.ForegroundColor = ConsoleColor.Blue;
						}
					}
					else {
						Console.ForegroundColor = ConsoleColor.Green;
					}

					if(hist[j] / maxc >= (len - i - 0.25) / len) {
						Console.Write('█');
					}
					else if(hist[j] / maxc >= (len - i - 0.5) / len) {
						Console.Write('▓');
					}
					else if(hist[j] / maxc >= (len - i - .75) / len) {
						Console.Write('▒');
					}
					else {
						Console.Write('░');
					}
					Console.ResetColor();
				}
				Console.WriteLine();
			}
			sbyte ctr = (sbyte)(-1 * teamsize);

			for(sbyte i = 0; i < hist.Length; i++) {
				if(i % (hist.Length / (2 * teamsize)) == 0) {
					Console.Write(Math.Abs(ctr));
					ctr++;
				}
				else {
					Console.Write(' ');
				}
			}
			//Console.Write(ctr);
			//

			bluearea = 0;
			double bluebet = 0;
			redarea = 0;
			double redbet = 0;

			for(byte i = 0; i < hist.Length / 2; i++) {
				redarea += 100*hist[i] / count;
				redbet += (1 - N((double)(hist.Length / 2 - i) / (hist.Length / 2), 0, 1/3.0)) * 100 * hist[i] / count;
				//redbet += Math.Pow((double)(hist.Length / 2 - i) / (hist.Length / 2),2) * 100 * hist[i] / count;
			}
			for(byte i = (byte)(hist.Length / 2); i < hist.Length; i++) {
				bluearea += 100*hist[i] / count;
				bluebet += (1 - N((double)(i - hist.Length / 2) / (hist.Length - hist.Length / 2), 0, 1/3.0)) * 100 * hist[i] / count;
				//bluebet += Math.Pow((double)(i - hist.Length / 2) / (hist.Length - hist.Length / 2),2) * 100 * hist[i] / count;
			}

			Console.WriteLine("\n\n*  BLUE: {0}%", Math.Round(100 * bluearea) / 100);
			Console.WriteLine("*   RED: {0}%", Math.Round(100 * redarea) / 100);

			Console.WriteLine("\n*   BET: {0}% {1}", Math.Round(100 * Math.Abs(bluearea - redarea) / 6) / 100, bluearea - redarea >= 0 ? "BLUE" : "RED");
			Console.WriteLine("*  WBET: {0}% {1}", Math.Round(100 * Math.Abs(bluebet - redbet)) / 100, bluebet - redbet >= 0 ? "BLUE" : "RED");

			Console.WriteLine("*  %BET: {0}% {1}", Math.Round(Math.Abs(Math.Max(bluearea, redarea) * (bluebet - redbet)))/100, bluebet - redbet >= 0 ? "BLUE" : "RED");

			double[] deciles = new double[10];

			for(byte i = 0; i < 10; i++) {
				for(ushort j = (ushort)(i * count / 10); j < (ushort)((i + 1) * count / 10); j++) {
					deciles[i] += results[j];
				}
				//Console.WriteLine(deciles[i]);
				deciles[i] /= (ushort)((i + 1) * count / 10) - (ushort)(i * count / 10);
				//Console.WriteLine(deciles[i]);
			}

			Console.Write("\n*  DIST: {0} R ", Math.Abs(Math.Round(100 * results[0]) / 100));
			for(byte i = 0; i < deciles.Length; i++) {
				if(i >= 1 && deciles[i - 1] < 0 && deciles[i] > 0)
					Console.Write("|| ");
				Console.Write("{0} ", (Math.Round(100 * Math.Abs(deciles[i])) / 100).ToString("0.00"));
			}
			Console.Write("B {0}", Math.Abs(Math.Round(100 * results[count - 1]) / 100) + " ");

			//Console.WriteLine("\n*  DEC: {0} {1} {2} {3} {4} {5} {6} {7} {8}", Math.Round(100*mravg)/100);
		}

	}
}
