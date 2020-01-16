using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTSNet {
	public class Battle {

		public enum DamageMode : byte {
			Sim = 0,
			Trial,

			Smart,
			Dumb
		}

		//

		private Random rand;

		private Team blue;
		private Team red;

		//

		public Battle(Team blue, Team red) {
			rand = new Random();

			this.red = red;
			this.blue = blue;
		}

		public Pkmn[] Turn(Pkmn blue, Pkmn red) {

		} 

		// sims the battle of two opposing pokemon
		// returns the winner
		public Pkmn SimSingle(Pkmn blue, Pkmn red) {

			// move damage bias
			short[] bdmg = new short[blue.Moves.Length];
			short[] rdmg = new short[red.Moves.Length];

			// move use bias
			// positive for (sp) attack moves
			// negative for status moves
			sbyte[] buse = new sbyte[blue.Moves.Length];
			sbyte[] ruse = new sbyte[red.Moves.Length];

			// move use probabilities
			double[] bpr = new double[blue.Moves.Length];
			double[] rpr = new double[red.Moves.Length];

			double bsum;
			double rsum;

			DamageMode dmgmode = DamageMode.Trial;

			double rand;

			// 20% dumb, 50% trial, 30% smart
			rand = this.rand.NextDouble();

			dmgmode = (0 <= rand && rand < 0.2 ? DamageMode.Dumb : (0.2 <= rand && rand < 0.7 ? DamageMode.Trial : DamageMode.Smart));

			// fill damage for probability calculations
			for(byte i = 0; i < bdmg.Length; i++) {
				bdmg[i] = (short)Damage(blue, i, red, dmgmode);
			}
			for(byte i = 0; i < rdmg.Length; i++) {
				rdmg[i] = (short)Damage(red, i, blue, dmgmode);
			}

			while(red.HP > 0 && blue.HP > 0) {
				bsum = 0;
				rsum = 0;

				// determine relative prob. weights
				for(byte i = 0; i < bpr.Length; i++) {
					bpr[i] = (10 + bdmg[i]) * (10.0 + (buse[i] > 0 ? buse[i] : 0)) / (10.0 - (buse[i] < 0 ? buse[i] : 0));
					bsum += bpr[i];
				}
				for(byte i = 0; i < rpr.Length; i++) {
					rpr[i] = (10 + rdmg[i]) * (10.0 + (ruse[i] > 0 ? ruse[i] : 0)) / (10.0 - (ruse[i] < 0 ? ruse[i] : 0));
					rsum += rpr[i];
				}

				// fit prob. to 0 - 1
				for(byte i = 0; i < bpr.Length; i++) {
					bpr[i] = bpr[i] / bsum;
				}
				for(byte i = 0; i < rpr.Length; i++) {
					rpr[i] = rpr[i] / rsum;
				}
			}

		}

		public void SimDouble() {
		
		}

		// uses a box-mueller transform
		// random paramaters are 1 - rand b/c rand is on [0, 1)
		// log(rand) could error, but log(1 - rand) will not
		public double RandomGuassian(double mean, double sd) {
			return mean + sd * Math.Sqrt(-2 * Math.Log(1 - rand.NextDouble())) * Math.Sin(2 * Math.PI * (1 - rand.NextDouble()));
		}

		public double Damage(Pkmn atk, byte mid, Pkmn def, DamageMode dmgmode) {
			switch(dmgmode) {
				case DamageMode.Sim:
					return ((0.4 * atk.Level + 2) * atk[mid].Power * AtkDefRatio(atk.Stats, atk[mid], def.Stats) / 50) 
						* Accuracy(atk[mid]) * STAB(atk, atk[mid]) * Effect(atk[mid], def);
				case DamageMode.Trial:
					return ((0.4 * atk.Level + 2) * atk[mid].Power * AtkDefRatio(atk.Stats, atk[mid], def.Stats) / 50)
						* STAB(atk, atk[mid]) * Effect(atk[mid], def);
				case DamageMode.Smart:
					return ((0.4 * atk.Level + 2) * atk[mid].Power * AtkDefRatio(atk.Stats, atk[mid], def.Stats) / 50)
						* (1 - Math.Pow(1 - (double)atk[mid].Accuracy / Move.MAX_ACC, 1 + rand.NextDouble())) // risk tolerance for accuracy: rand ~ risk
						* STAB(atk, atk[mid]) * Effect(atk[mid], def); 
				case DamageMode.Dumb:
					return ((0.4 * atk.Level + 2) * atk[mid].Power / 50)
						* STAB(atk, atk[mid]) * Effect(atk[mid], def);
				default:
					return 0;
			}

		}

		//

		public double Accuracy(Move move) {
			return Move.MAX_ACC * rand.NextDouble() < move.Accuracy ? 1 : 0;
		}

		public double AccuracyWeight(Move move) {
			return (double)move.Accuracy / Move.MAX_ACC;
		}

		public double STAB(Pkmn atk, Move move) {
			return atk.Types.Contains(move.Type) ? 1.5 : 1;
		}

		public double Effect(Move move, Pkmn def) {
			double effect = 1;

			for(byte i = 0; i < def.Types.Length; i++) {
				effect *= Type.EFFECT[(byte)move.Type, (byte)def.Types[i]];
			}
			return effect;
		}

		public double AtkDefRatio(Stats atk, Move move, Stats def) {
			switch(move.DamageType) {
				case Move.DmgType.Physical:
					return (double)atk.Attack / def.Defense;
				case Move.DmgType.Special:
					return (double)atk.SpAttack / def.SpDefense;
				case Move.DmgType.Status:
					return 0;
				default:
					return 1;
			}
		}


	}
}
