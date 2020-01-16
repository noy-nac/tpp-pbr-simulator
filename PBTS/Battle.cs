using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTS {
	public class Battle {

		private Random rand;

		private Team blue;
		private Team red;

		private bool doub;
		private bool switching;
		private bool inputctrl;
		private bool randomorder;

		private double friendlyfire;

		public Battle(Team blue, Team red, bool doub, bool switching, bool inputctrl, bool randomorder, double friendlyfire) {
			rand = new Random();

			this.blue = blue;
			this.red = red;

			this.doub = doub;
			this.switching = switching;
			this.inputctrl = inputctrl;
			this.randomorder = randomorder;

			this.friendlyfire = friendlyfire;
		}

		public double Run() {
			if(randomorder) {
				Team bluerand = new Team("BLUE", (byte)blue.Pokemon.Length, blue[0].Level);
				Team redrand = new Team(" RED", (byte)red.Pokemon.Length, red[0].Level);

				bool[] doneb = new bool[bluerand.Pokemon.Length];
				bool[] doner = new bool[redrand.Pokemon.Length];

				for(byte i = 0; i < bluerand.Pokemon.Length; i++) {
					if(doneb[i]) {
						i--;
					}
					else {
						bluerand[i] = blue[(byte)(rand.NextDouble() * (blue.Pokemon.Length - 1))];
						doneb[i] = true;
					}
				}
				for(byte i = 0; i < redrand.Pokemon.Length; i++) {
					if(doner[i]) {
						i--;
					}
					else {
						redrand[i] = red[(byte)(rand.NextDouble() * (red.Pokemon.Length - 1))];
						doner[i] = true;
					}
				}
				return Sim(bluerand, redrand);
			}
			if(switching) {
				double[] trials = new double[blue.Pokemon.Length * red.Pokemon.Length];
				double[] weight = new double[blue.Pokemon.Length * red.Pokemon.Length];

				for(byte i = 0; i < blue.Pokemon.Length; i++) {
					for(byte j = 0; j < red.Pokemon.Length; j++) {
						Team blues = new Team("BLUE", (byte)(blue.Pokemon.Length - i), blue[0].Level);
						Team reds = new Team(" RED", (byte)(red.Pokemon.Length - j), red[0].Level);

						bool[] doneb = new bool[blues.Pokemon.Length];
						bool[] doner = new bool[reds.Pokemon.Length];

						for(byte k = 0; k < blues.Pokemon.Length; k++) {
							blues[k] = blue[(byte)(i + k)];
						}
						for(byte k = 0; k < reds.Pokemon.Length; k++) {
							reds[k] = blue[(byte)(j + k)];
						}
						byte pos = (byte)(i * red.Pokemon.Length + j);

						trials[pos] = Sim(blues, reds);
						if(trials[pos] > 0) {
							
							//trials[pos] *= (double)blue.Pokemon.Length / blues.Pokemon.Length;
							trials[pos] -= (reds.Pokemon.Length - blues.Pokemon.Length);

							trials[pos] = trials[pos] > blue.Pokemon.Length ? blue.Pokemon.Length : trials[pos];
							trials[pos] = trials[pos] < -1 * red.Pokemon.Length ? -1 * red.Pokemon.Length : trials[pos];
						}
						else if(trials[pos] < 0) {
							//trials[pos] *= (double)red.Pokemon.Length / reds.Pokemon.Length;
							trials[pos] += (blues.Pokemon.Length - reds.Pokemon.Length);

							trials[pos] = trials[pos] > blue.Pokemon.Length ? blue.Pokemon.Length : trials[pos];
							trials[pos] = trials[pos] < -1 * red.Pokemon.Length ? -1 * red.Pokemon.Length : trials[pos];
						}
						double bias = RandGuass(Math.Abs(trials[pos]), 0.5);

						//bias = bias > 1 ? 1 : bias; // bias should be > -1
						bias = bias < -1 ? -1 : bias;

						weight[pos] = ((blue.Pokemon.Length - i) + (red.Pokemon.Length - j));//* (1 + bias);
					}
				}
				double sum = weight.Sum();

				double[] partialsum = new double[weight.Length];

				double result = 0;

				for(byte i = 0; i < weight.Length; i++) {
					result += trials[i] * weight[i] / sum;
					weight[i] = weight[i] / sum;
					partialsum[i] = (i > 0 ? partialsum[i-1] : 0) + weight[i];
				}
				//byte rtrial;
				double seed = rand.NextDouble();

				double w = RandGuass(0.50, 0.10);

				w = w > 1 ? 1 : w;
				w = w < 0 ? 0 : w;

				for(byte i = 0; i < partialsum.Length; i++) {
					if((i > 0 ? partialsum[i - 1] : 0) <= seed && seed < partialsum[i])
						return w * trials[i] + (1-w)*Sim(blue, red);
				}

				//return trials[(byte)rand.Next(0,trials.Length)];
				return result * w + Sim(blue, red) * (1-w);
			}
			if(doub) {
				Team blue1 = new Team("BLUE", 1, blue[0].Level);
				Team red1 = new Team(" RED", 1, red[0].Level);

				Team blue2 = new Team("BLUE", 2, blue[0].Level);
				Team red2 = new Team(" RED", 2, red[0].Level);
			}
			else {
				return Sim(blue, red);
			}
			return Sim(blue, red);
		}

		public double Sim(Team blue, Team red) {
			byte b = 0;
			byte r = 0;

			ushort bhpdiff = 0;
			ushort rhpdiff = 0;

			short[] hp = new short[] { 0, 0 };

			while(b < blue.Pokemon.Length && r < red.Pokemon.Length) {
				hp = SimChain(blue.Pokemon[b], red.Pokemon[r], bhpdiff, rhpdiff);

				if(hp[0] > 0) {
					bhpdiff = (ushort)(blue.Pokemon[b].Stats[(byte)Pokemon.Stat.HP] - hp[0]);
				}
				else {
					b++;
					bhpdiff = 0;
				}
				if(hp[1] > 0) {
					rhpdiff = (ushort)(red.Pokemon[r].Stats[(byte)Pokemon.Stat.HP] - hp[1]);
				}
				else {
					r++;
					rhpdiff = 0;
				}
			}
			double result = (hp[0] > 0 ? 1 : -1) * (double)(blue.Pokemon.Length - b + red.Pokemon.Length - r);

			result -= (hp[0] > 0 ?
				1 - (double)hp[0] / blue.Pokemon[b].Stats[(byte)Pokemon.Stat.HP] :
				-1 *(1 - (double)hp[1] / red.Pokemon[r].Stats[(byte)Pokemon.Stat.HP]));

			//

			/*
			long[] score = new long[] { 0, 0 };
			long[] temp;

			for(byte i = 0; i < blue.Pokemon.Length; i++) {
				for(byte j = 0; j < red.Pokemon.Length; j++) {
					temp = SimScore(blue.Pokemon[i], red.Pokemon[j]);

					score[0] += temp[0] / (1 + Math.Abs(i - j));
					score[1] += temp[1] / (1 + Math.Abs(i - j));
				}
			}

			// muddying the data honestly
			if((score[0] - score[1] < 0 && result < 0) || (score[0] - score[1] > 0 && result > 0)) {
				return (score[0] - score[1]) * Math.Abs(result);
			}
			else {
				return 0;
			}*/

			return result;
		}

		public long[] SimScore(Pokemon blue, Pokemon red) {
			long[] bdmg = new long[Pokemon.MOVEMAX];
			long[] rdmg = new long[Pokemon.MOVEMAX];

			long bluetot = 0;
			long redtot = 0;

			for(byte i = 0; i < Pokemon.MOVEMAX; i++) {
				bdmg[i] = TrialDamage(blue.Level, blue.Type, blue.Moves[i % blue.Moves.Length], red.Type,
					blue.Stats[(byte)Pokemon.Stat.Attack], blue.Stats[(byte)Pokemon.Stat.SpAttack],
					red.Stats[(byte)Pokemon.Stat.Defense], red.Stats[(byte)Pokemon.Stat.SpDefense], doub);

				rdmg[i] = TrialDamage(red.Level, red.Type, red.Moves[i % red.Moves.Length], blue.Type,
					red.Stats[(byte)Pokemon.Stat.Attack], red.Stats[(byte)Pokemon.Stat.SpAttack],
					blue.Stats[(byte)Pokemon.Stat.Defense], blue.Stats[(byte)Pokemon.Stat.SpDefense], doub);

				bluetot += (long)Math.Ceiling((double)blue.Stats[(byte)Pokemon.Stat.HP] / (rdmg[i] + 1));
				redtot += (long)Math.Ceiling((double)red.Stats[(byte)Pokemon.Stat.HP] / (bdmg[i] + 1));

				if(blue.Stats[(byte)Pokemon.Stat.Speed] > red.Stats[(byte)Pokemon.Stat.Speed]) {
					bluetot++;
				}
				else if(blue.Stats[(byte)Pokemon.Stat.Speed] < red.Stats[(byte)Pokemon.Stat.Speed]) {
					redtot++;
				}
				else { } // blue speed == red speed
			}
			return new long[] { bluetot, redtot };
		}

		public short[] SimChain(Pokemon blue, Pokemon red, ushort bhpdiff, ushort rhpdiff) {
			short bhp = (short)(blue.Stats[(byte)Pokemon.Stat.HP] - bhpdiff);
			short rhp = (short)(red.Stats[(byte)Pokemon.Stat.HP] - rhpdiff);

			long[] bdmg = new long[Pokemon.MOVEMAX];
			long[] rdmg = new long[Pokemon.MOVEMAX];

			double[] bprob = new double[Pokemon.MOVEMAX];
			double[] rprob = new double[Pokemon.MOVEMAX];

			Type bmove = Type.None;
			Type rmove = Type.None;

			// fetch move damage with random variation
			for(byte i = 0; i < Pokemon.MOVEMAX; i++) {
				bdmg[i] = TrialDamage(blue.Level, blue.Type, blue.Moves[i % blue.Moves.Length], red.Type,
					blue.Stats[(byte)Pokemon.Stat.Attack], blue.Stats[(byte)Pokemon.Stat.SpAttack],
					red.Stats[(byte)Pokemon.Stat.Defense], red.Stats[(byte)Pokemon.Stat.SpDefense], doub);

				rdmg[i] = TrialDamage(red.Level, red.Type, red.Moves[i % red.Moves.Length], blue.Type,
					red.Stats[(byte)Pokemon.Stat.Attack], red.Stats[(byte)Pokemon.Stat.SpAttack],
					blue.Stats[(byte)Pokemon.Stat.Defense], blue.Stats[(byte)Pokemon.Stat.SpDefense], doub);
			}
			// bias for most powerful move, bias for non status moves
			for(byte i = 0; i < Pokemon.MOVEMAX; i++) {
				if(inputctrl) {
					bprob[i] = RandGuass(0.25, 0.1) + RandGuass(1, 0.1) * bdmg[i] / bdmg.Sum() *
						(1 - (double)TypeInfo.STATUS[(byte)blue.Moves[i % blue.Moves.Length]] /
						(TypeInfo.ATK[(byte)blue.Moves[i % blue.Moves.Length]] +
						TypeInfo.STATUS[(byte)blue.Moves[i % blue.Moves.Length]]));
					rprob[i] = RandGuass(0.25, 0.1) + RandGuass(1, 0.2) * rdmg[i] / rdmg.Sum() *
						(1 - (double)TypeInfo.STATUS[(byte)red.Moves[i % red.Moves.Length]] /
						(TypeInfo.ATK[(byte)red.Moves[i % red.Moves.Length]] +
						TypeInfo.STATUS[(byte)red.Moves[i % red.Moves.Length]]));
				}
				else {
					bprob[i] = 1;
					rprob[i] = 1;
				}
			}

			double bsum = bprob.Sum();
			double rsum = rprob.Sum();

			double brand = rand.NextDouble();
			double rrand = rand.NextDouble();

			// make probabilities add up to 1
			for(byte i = 0; i < Pokemon.MOVEMAX; i++) {
				bprob[i] = bprob[i] / bsum;
				rprob[i] = rprob[i] / rsum;
			}
			ushort length = 0;

			while(bhp > 0 && rhp > 0 && length <= 8) {
				double bps = 0;
				double rps = 0;

				// select random move based on probabilities
				for(byte i = 0; i < bprob.Length; i++) {
					if(brand >= bps && brand < bps + bprob[i]) {
						bmove = blue.Moves[i % blue.Moves.Length];
						break;
					}
					bps += bprob[i];

				}
				for(byte i = 0; i < rprob.Length; i++) {
					if(rrand >= rps && rrand < rps + rprob[i]) {
						rmove = red.Moves[i % red.Moves.Length];
						break;
					}
					rps += rprob[i];
				}

				//Console.WriteLine(length + " " + bhp + " " + rhp);

				// most speed goes first
				// when any hp <= 0, battle ends
				if(blue.Stats[(byte)Pokemon.Stat.Speed] > red.Stats[(byte)Pokemon.Stat.Speed]) {
					rhp -= (short)TrialDamage(blue.Level, blue.Type, bmove, red.Type,
						blue.Stats[(byte)Pokemon.Stat.Attack], blue.Stats[(byte)Pokemon.Stat.SpAttack],
						red.Stats[(byte)Pokemon.Stat.Defense], red.Stats[(byte)Pokemon.Stat.SpDefense], doub);
					if(rhp <= 0)
						break;

					bhp -= (short)TrialDamage(red.Level, red.Type, rmove, blue.Type,
						red.Stats[(byte)Pokemon.Stat.Attack], red.Stats[(byte)Pokemon.Stat.SpAttack],
						blue.Stats[(byte)Pokemon.Stat.Defense], blue.Stats[(byte)Pokemon.Stat.SpDefense], doub);
					if(bhp <= 0)
						break;
				}
				else if(blue.Stats[(byte)Pokemon.Stat.Speed] < red.Stats[(byte)Pokemon.Stat.Speed]) {
					bhp -= (short)TrialDamage(red.Level, red.Type, rmove, blue.Type,
						red.Stats[(byte)Pokemon.Stat.Attack], red.Stats[(byte)Pokemon.Stat.SpAttack],
						blue.Stats[(byte)Pokemon.Stat.Defense], blue.Stats[(byte)Pokemon.Stat.SpDefense], doub);
					if(bhp <= 0)
						break;

					rhp -= (short)TrialDamage(blue.Level, blue.Type, bmove, red.Type,
						blue.Stats[(byte)Pokemon.Stat.Attack], blue.Stats[(byte)Pokemon.Stat.SpAttack],
						red.Stats[(byte)Pokemon.Stat.Defense], red.Stats[(byte)Pokemon.Stat.SpDefense], doub);
					if(rhp <= 0)
						break;
				}
				else {
					double r = rand.NextDouble();

					if(r < 0.5) {
						rhp -= (short)TrialDamage(blue.Level, blue.Type, bmove, red.Type,
							blue.Stats[(byte)Pokemon.Stat.Attack], blue.Stats[(byte)Pokemon.Stat.SpAttack],
							red.Stats[(byte)Pokemon.Stat.Defense], red.Stats[(byte)Pokemon.Stat.SpDefense], doub);
						if(rhp <= 0)
							break;

						bhp -= (short)TrialDamage(red.Level, red.Type, rmove, blue.Type,
							red.Stats[(byte)Pokemon.Stat.Attack], red.Stats[(byte)Pokemon.Stat.SpAttack],
							blue.Stats[(byte)Pokemon.Stat.Defense], blue.Stats[(byte)Pokemon.Stat.SpDefense], doub);
						if(bhp <= 0)
							break;
					}
					else {
						bhp -= (short)TrialDamage(red.Level, red.Type, rmove, blue.Type,
							red.Stats[(byte)Pokemon.Stat.Attack], red.Stats[(byte)Pokemon.Stat.SpAttack],
							blue.Stats[(byte)Pokemon.Stat.Defense], blue.Stats[(byte)Pokemon.Stat.SpDefense], doub);
						if(bhp <= 0)
							break;

						rhp -= (short)TrialDamage(blue.Level, blue.Type, bmove, red.Type,
							blue.Stats[(byte)Pokemon.Stat.Attack], blue.Stats[(byte)Pokemon.Stat.SpAttack],
							red.Stats[(byte)Pokemon.Stat.Defense], red.Stats[(byte)Pokemon.Stat.SpDefense], doub);
						if(rhp <= 0)
							break;
					}
				}

				// status moves


				length++;
				//Console.WriteLine(length+ " " +  bhp + " " + rhp);
			}
			return new short[] { bhp, rhp };
		}

		public double RandGuass(double mean, double sd) {
			return mean + sd * Math.Sqrt(-2 * Math.Log(1 - rand.NextDouble())) * Math.Sin(2 * Math.PI * (1 - rand.NextDouble()));
		}

		public ushort TrialDamage(byte lvl, Type[] atkpkmn, Type move, Type[] defpkmn, ushort atk, ushort spatk, ushort def, ushort spdef, bool doub) {
			return (ushort)(((0.4 * lvl + 2) * POWER(move) * ATKDEF(move, atk, spatk, def, spdef) / 50) * ACC(move)
				* M_STAB(atkpkmn, move) * M_EFF(move, defpkmn) * M_TGT() * M_CRIT() * M_RAND() * M_WEATHER() * M_BURN());
		}

		public double POWER(Type type) {
			const double SD_PROP = 0.35;

			double pwr = RandGuass(TypeInfo.AVGPOWER[(byte)type], RandGuass(SD_PROP, 0.05) * TypeInfo.ATK[(byte)type]);
			// round to nearest 5.
			return 5 * Math.Round(pwr / 5);
		}

		public double ATKDEF(Type type, ushort atk, ushort spatk, ushort def, ushort spdef) {
			//double phy = (double)TypeInfo.PHYSICAL[(byte)type] / TypeInfo.ATK[(byte)type];
			//double spe = (double)TypeInfo.SPECIAL[(byte)type] / TypeInfo.ATK[(byte)type];

			return rand.NextDouble() < (double)TypeInfo.PHYSICAL[(byte)type] / TypeInfo.ATK[(byte)type] ?
				(double)atk / def : (double)spatk / spdef;
		}

		public double ACC(Type type) {
			return 100 * rand.NextDouble() < RandGuass(TypeInfo.AVGACCURACY[(byte)type], 3.8) ? 1 : 0;
		}

		public double M_STAB(Type[] ptype, Type mtype) {
			return ptype.Contains(mtype) ? 1.5 : 1;
		}

		public double M_EFF(Type mtype, Type[] ptype) {
			return ptype.Length == 1 ?
				TypeInfo.EFFECT[(byte)mtype, (byte)ptype[0]] :
				TypeInfo.EFFECT[(byte)mtype, (byte)ptype[0]] * TypeInfo.EFFECT[(byte)mtype, (byte)ptype[1]];
		}

		public double M_TGT() {
			return doub && rand.NextDouble() < RandGuass(0.20, 0.05) ? 0.75 : 1;
		}

		public double M_CRIT() {
			return rand.NextDouble() < 1.0 / 16 ? 2 : 1;
		}

		public double M_RAND() {
			return 0.85 + 0.15 * rand.NextDouble();
		}

		public double M_WEATHER() {
			return 1;
		}

		public double M_BURN() {
			return 1;
		}

	}
}
