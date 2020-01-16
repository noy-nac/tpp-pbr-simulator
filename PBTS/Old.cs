/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTS {
	public class Old {
		public enum Type : byte {
			None = 0,

			Normal,
			Fire,
			Water,
			Electric,
			Grass,
			Ice,
			Fighting,
			Poison,
			Ground,
			Flying,
			Psychic,
			Bug,
			Rock,
			Ghost,
			Dragon,
			Dark,
			Steel,
			Fairy,
		}

		public const float MUL = 1.0f;
		public const float ADD = 0.0f;

		public const float REG = 1.0f * MUL + ADD;
		public const float NVE = 0.5f * MUL + ADD;
		public const float SUP = 2.0f * MUL + ADD;
		public const float NON = 0.0f * MUL + ADD;

		// [attacking, defending]
		public static readonly float[,] EFFECT = {
			{ REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG },
			{ REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, NVE, NON, REG, REG, NVE, REG },
			{ REG, REG, NVE, NVE, REG, SUP, SUP, REG, REG, REG, REG, REG, SUP, NVE, REG, NVE, REG, SUP, REG },
			{ REG, REG, SUP, NVE, REG, NVE, REG, REG, REG, SUP, REG, REG, REG, SUP, REG, NVE, REG, REG, REG },
			{ REG, REG, REG, SUP, NVE, NVE, REG, REG, REG, NON, SUP, REG, REG, REG, REG, NVE, REG, REG, REG },
			{ REG, REG, NVE, SUP, REG, NVE, REG, REG, NVE, SUP, NVE, REG, NVE, SUP, REG, NVE, REG, NVE, REG },
			{ REG, REG, NVE, NVE, REG, SUP, NVE, REG, REG, SUP, SUP, REG, REG, REG, REG, SUP, REG, NVE, REG },
			{ REG, SUP, REG, REG, REG, REG, SUP, REG, NVE, REG, NVE, NVE, NVE, SUP, NON, REG, SUP, SUP, NVE },
			{ REG, REG, REG, REG, REG, SUP, REG, REG, NVE, NVE, REG, REG, REG, NVE, NVE, REG, REG, NON, SUP },
			{ REG, REG, SUP, REG, SUP, NVE, REG, REG, SUP, REG, NON, REG, NVE, SUP, REG, REG, REG, SUP, REG },
			{ REG, REG, REG, REG, NVE, SUP, REG, SUP, REG, REG, REG, REG, SUP, NVE, REG, REG, REG, NVE, REG },
			{ REG, REG, REG, REG, REG, REG, REG, SUP, SUP, REG, REG, NVE, REG, REG, REG, REG, NON, NVE, REG },
			{ REG, REG, NVE, REG, REG, SUP, REG, NVE, NVE, REG, NVE, SUP, REG, REG, NVE, REG, SUP, NVE, NVE },
			{ REG, REG, SUP, REG, REG, REG, SUP, NVE, REG, NVE, SUP, REG, SUP, REG, REG, REG, REG, NVE, REG },
			{ REG, NON, REG, REG, REG, REG, REG, REG, REG, REG, REG, SUP, REG, REG, SUP, REG, NVE, REG, REG },
			{ REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, SUP, REG, NVE, NON },
			{ REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, REG, SUP, REG, REG, SUP, REG, NVE, REG, NVE },
			{ REG, REG, NVE, NVE, NVE, REG, SUP, REG, REG, REG, REG, REG, REG, SUP, REG, REG, REG, NVE, SUP },
			{ REG, REG, NVE, REG, REG, REG, REG, SUP, NVE, REG, REG, REG, REG, REG, REG, SUP, SUP, NVE, REG },
		};

		public static byte TEAM_SIZE = 3;
		//public static byte TEAM_SIZE = 4;

		// quick input
		public static byte[][] _BLUE = new byte[][] {
			new byte[]{       2            },
			new byte[]{      9,10                  },
			new byte[]{     11                   },
		};
		public static byte[][] _RED = new byte[][] {
			new byte[]{    4                },
			new byte[]{    11                 },
			new byte[]{     5,8                       },
		};
		public static byte[][] _BLUE_M = new byte[][] {
			new byte[]{      2,2,7,13                       },
			new byte[]{      9,16,13,1                      },
			new byte[]{      11,5,4,11          },
		};
		public static byte[][] _RED_M = new byte[][] {
			new byte[]{    4,4,6,1            },
			new byte[]{   11,7,2,1              },
			new byte[]{    5,8,5,8                   },
		};

		public static int[][] _BLUE_S = new int[][] {
			new int[]{   298,158,191,317,205,327                  },
			new int[]{   353,226,286,113,238,262         },
			new int[]{    444,158,372,186,297,206             },
		};
		public static int[][] _RED_S = new int[][] {
			new int[]{   272,123,155,319,226,394         },
			new int[]{   359,276,194,382,216,394               },
			new int[]{  262,157,146,348,246,305               },
		};

		public static Type[][] T_B = new Type[TEAM_SIZE][];
		public static Type[][] T_R = new Type[TEAM_SIZE][];
		public static Type[][] M_B = new Type[TEAM_SIZE][];
		public static Type[][] M_R = new Type[TEAM_SIZE][];
		public static int[][] S_B = new int[TEAM_SIZE][];
		public static int[][] S_R = new int[TEAM_SIZE][];

		public static void Format() {
			for(byte i = 0; i < _BLUE_M.Length; i++) {
				M_B[i] = new Type[4];

				for(byte j = 0; j < _BLUE_M[i].Length; j++) {
					M_B[i][j] = (Type)_BLUE_M[i][j];
				}
			}

			for(byte i = 0; i < _RED_M.Length; i++) {
				M_R[i] = new Type[4];

				for(byte j = 0; j < _RED_M[i].Length; j++) {
					M_R[i][j] = (Type)_RED_M[i][j];
				}
			}

			for(byte i = 0; i < _RED.Length; i++) {
				T_R[i] = new Type[2];

				for(byte j = 0; j < _RED[i].Length; j++) {
					T_R[i][j] = (Type)_RED[i][j];

					if(_RED[i].Length < T_R[i].Length) {
						T_R[i][j + 1] = T_R[i][j];
					}
				}
			}

			for(byte i = 0; i < _BLUE.Length; i++) {
				T_B[i] = new Type[2];

				for(byte j = 0; j < _BLUE[i].Length; j++) {
					T_B[i][j] = (Type)_BLUE[i][j];

					if(_BLUE[i].Length < T_B[i].Length) {
						T_B[i][j + 1] = T_B[i][j];
					}
				}
			}

			for(byte i = 0; i < _BLUE_S.Length; i++) {
				S_B[i] = new int[6];

				for(byte j = 0; j < _BLUE_S[i].Length; j++) {
					S_B[i][j] = _BLUE_S[i][j];

					if(S_B[i][j] > 1000)
						throw new Exception("Unrealistic Input - Blue Stats");
				}
			}

			for(byte i = 0; i < _RED_S.Length; i++) {
				S_R[i] = new int[6];

				for(byte j = 0; j < _RED_S[i].Length; j++) {
					S_R[i][j] = _RED_S[i][j];

					if(S_B[i][j] > 1000)
						throw new Exception("Unrealistic Input - Red Stats");
				}
			}
		}

		public static void Input() {
		}

		public static void Main() {
			Input();
			Format();

			ulong[] _00 = MatchScore(S_B[0], S_R[0], T_B[0], T_R[0], M_B[0], M_R[0]);
			ulong[] _11 = MatchScore(S_B[1], S_R[1], T_B[1], T_R[1], M_B[1], M_R[1]);
			ulong[] _22 = MatchScore(S_B[2], S_R[2], T_B[2], T_R[2], M_B[2], M_R[2]);

			ulong[] _01 = MatchScore(S_B[0], S_R[1], T_B[0], T_R[1], M_B[0], M_R[1]);
			ulong[] _12 = MatchScore(S_B[1], S_R[2], T_B[1], T_R[2], M_B[1], M_R[2]);
			ulong[] _20 = MatchScore(S_B[2], S_R[0], T_B[2], T_R[0], M_B[2], M_R[0]);

			ulong[] _10 = MatchScore(S_B[1], S_R[0], T_B[1], T_R[0], M_B[1], M_R[0]);
			ulong[] _21 = MatchScore(S_B[2], S_R[1], T_B[2], T_R[1], M_B[2], M_R[1]);
			ulong[] _02 = MatchScore(S_B[0], S_R[2], T_B[0], T_R[2], M_B[0], M_R[2]);

			ulong W1 = 1, W2 = 2, W3 = 4;

			ulong blue = (_00[2] + _11[2] + _22[2]) / W1 + (_01[2] + _10[2] + _12[2] + _21[2]) / W2 + (_20[2] + _02[2]) / W3;
			ulong red = (_00[3] + _11[3] + _22[3]) / W1 + (_01[3] + _10[3] + _12[3] + _21[3]) / W2 + (_20[3] + _02[3]) / W3;

			long net = (long)(blue - red);
			long total = (long)(blue + red);

			double x = 100*(double)net / total / 3;

			double w = 50 * (N(x, -6, 3.5) + N(x,6, 3.5));
			double l = 100 * N(x, 0, 4);
			double a = 100 * N(x, 0, 6);

			double pct = 100 * w / (a * l + 4 * w);

			if(Math.Abs(x) >= 7.75) {
				pct += Math.Log(x * x * x * x / 3607.5);
			}

			pct *= 0.75;

			Console.WriteLine("B: {0}, R: {1}", blue, red);
			Console.WriteLine("{0} / {1} = {2}", net, total, (float)net / total);
			Console.WriteLine("\n{0}%", 100*(float)net / total / 3);
			Console.WriteLine("\n\n * * * * *    {0}%    * * * * *", Math.Round(pct*100)/100);
			Console.WriteLine("               {0}", x >= 0 ? "BLUE" : "RED");
			Console.ReadLine();
		}

		public static double N(double x, double u, double s) {
			return Math.Exp(-1 * (x - u) * (x - u) / (2 * s * s)) / (s*Math.Sqrt(2 * Math.PI));
		}

		// Stats: 0 = HP, 1 = Atk, 2 = Def, 3 = Sp Atk, 4 = Sp Def, 5 = Spe
		// POSITIVE: ADVATAGE BLUE, NEGATIVE: ADVANTAGE RED!
		public static ulong[] MatchScore(int[] bsta, int[] rsta, Type[] bty, Type[] rty, Type[] bmvty, Type[] rmvty) {
			float[] bdmg = new float[4];
			float[] rdmg = new float[4];

			ulong[] br = new ulong[2];

			for(byte i = 0; i < 4; i++) {
				bdmg[i] = Damage(3*Math.Max(bsta[1], bsta[3])/4+ Math.Min(bsta[1], bsta[3])/4, (rsta[2] + rsta[4]) / 2, bmvty[i], rty, bmvty[i] == bty[0] || bmvty[i] == bty[1]);
				rdmg[i] = Damage(3 * Math.Max(rsta[1], rsta[3]) / 4 + Math.Min(rsta[1], rsta[3]) / 4, (bsta[2] + bsta[4]) / 2, rmvty[i], bty, bmvty[i] == rty[0] || bmvty[i] == rty[1]);
			}
			for(byte i = 0; i < 4; i++) {
				br[0] += (ulong)Math.Ceiling(bsta[0] / rdmg[i]);
				br[1] += (ulong)Math.Ceiling(rsta[0] / bdmg[i]);

				// give +1 advantage to highest speed (tiebreaker)
				if(bsta[5] != rsta[5]) br[bsta[5] > rsta[5] ? 0 : 1]++;
			}
			return new ulong[] { br[0] - br[1], br[0] + br[1], br[0], br[1] };
		}

		public 

		public static float Damage(int atk, int def, Type mov, Type[] pkmn, bool stab) {
			return (float)((60 * atk / def + 2) * EFFECT[(byte)mov, (byte)pkmn[0]] * EFFECT[(byte)mov, (byte)pkmn[1]] * (stab ? 1.5 : 1));
		}

		/*public static void Main(string[] args) {
			Format();

			float tb = TotalPower(M_B, T_R, true);
			float tr = TotalPower(M_R, T_B, true);

			float xtb = TotalPower(M_B, T_R, false);
			float xtr = TotalPower(M_R, T_B, false);

			Console.WriteLine("\nX BL: " + xtb);
			Console.WriteLine("X RD: " + xtr);

			Console.WriteLine("\nBLUE: " + tb);
			Console.WriteLine(" RED: " + tr);

			float total = tb + tr;
			float diff = tb - tr;

			float bet = diff / total;
			float abet = (xtb - xtr) / (xtb + xtr);

			double pct = 0.15 / (1 + 14 * Math.Exp(-1.35 * (25*Math.Abs(bet) - 2.25)));
			double pctx = 0.15 / (1 + 14 * Math.Exp(-1.35 * (25*Math.Abs(abet) - 2.25)));

			Console.WriteLine("\n  DO: " + (Math.Round(10000*pct)/100) + "% on " + (bet > 0 ? "BLUE" : "RED"));

			Console.WriteLine("\nX DO: " + (Math.Round(10000 * pctx) / 100) + "% on " + (abet > 0 ? "BLUE" : "RED"));

			Console.WriteLine("\nD-DX: " + (Math.Round(10000 * pct) / 100 - Math.Round(10000 * pctx) / 100));

			//Console.WriteLine("\nBET1: " + Math.Round(Math.Abs(bet)) + "% on " + (bet > 0 ? "BLUE" : "RED"));
			Console.WriteLine(" - - - - - \n\nBET2: " + (Math.Round(100*Math.Abs(25*bet))/100) + "% on " + (bet > 0 ? "BLUE" : "RED"));

			//Console.WriteLine("\nBET3: " + Math.Round(Math.Abs(50*bet)) + "% on " + (bet > 0 ? "BLUE" : "RED"));
			//Console.WriteLine("\nBET4: " + Math.Round(Math.Abs(100*bet)) + "% on " + (bet > 0 ? "BLUE" : "RED"));
			Console.WriteLine("      "+bet);
			Console.WriteLine("\nALT2: " + (Math.Round(100*Math.Abs(25 * abet))/100) + "% on " + (abet > 0 ? "BLUE" : "RED"));

			Console.WriteLine("      " + abet);

			Console.ReadLine();
		}

		public static float TotalPower(Type[][] moves, Type[][] pkmn, bool order) {
			float power = 0;

			for(byte i = 0; i < moves.Length; i++) {
				for(byte j = 0; j < pkmn.Length; j++) {
					if(order) {
						if(TEAM_SIZE == 4) {
							power += Power(moves[i], pkmn[j]) * (1.0f / (j >= 3 ? 2 : 1));
						}
						else {
							power += Power(moves[i], pkmn[j]) * (1.0f / (j + 1));
						}
					}
					else {
						power += Power(moves[i], pkmn[j]);
					}
				}
			}
			return power;
		}

		public static float Power(Type[] moves, Type[] pkmn) {
			float power = 0;

			for(byte i = 0; i < moves.Length; i++) {
				power += EFFECT[(byte)moves[i], (byte)pkmn[0]] * EFFECT[(byte)moves[i], (byte)pkmn[1]] - REG;
			}
			return power;
		}*//*
	}
}*/
