using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTSNet {
	public static class Type {

		public enum Name : byte {
			Null = 0,

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
			Fairy
		}

		//

		public const double REG = 1.0;
		public const double NVE = 0.5;
		public const double SUP = 2.0;
		public const double NON = 0.0;

		// [attacking, defending]
		public static readonly double[,] EFFECT = {
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

	}
}
