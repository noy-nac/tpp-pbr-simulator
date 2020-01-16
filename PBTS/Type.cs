using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTS {
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

		Error = byte.MaxValue
	}

	public class TypeInfo {

		public const double REG = 1.0;
		public const double NVE = 0.5;
		public const double SUP = 2.0;
		public const double NON = 0.0;

		// [type]
		public static readonly double[] AVGPOWER = {
			0,
			81.17, 92.70, 76.38, 78.55,
			85.65, 90.69, 80.80, 67.50,
			75.89, 80.13, 105.01, 64.90,
			94.67, 66.18, 85.42, 78.48,
			77.35, 76.43
		};

		// [type]
		public static readonly double[] AVGACCURACY = {
			100,
			97.66, 93.75, 94.38, 96.58,
			99.35, 94.38, 95.86, 99.33,
			94.71, 98.68, 101.67, 98.44,
			91.67, 104.55, 94.17, 105.00,
			97.69, 104.29
		};

		public static readonly byte[] ATK = {
			1,
			88, 31, 27, 24,
			26, 19, 38, 12,
			20, 20, 22, 19,
			13, 17, 12, 23,
			17, 10
		};

		public static readonly byte[] PHYSICAL = {
			0,
			68, 10, 8, 11,
			13, 9, 33, 6,
			16, 13, 4, 13,
			11, 12, 6, 20,
			14, 1
		};

		public static readonly byte[] SPECIAL = {
			0,
			20, 21, 19, 13,
			13, 10, 5, 9,
			4, 7, 18, 6,
			2, 5, 9, 3,
			3, 9
		};

		public static readonly byte[] EXTRA = {
			0,
			5, 7, 3, 7,
			2, 6, 6, 5,
			5, 3, 2, 3,
			2, 1, 2, 4,
			2, 1
		};


		//


		public static readonly byte[] STATUS = {
			1,
			90, 2, 5, 8,
			14, 4, 4, 12,
			4, 4, 37, 9,
			4, 7, 1, 14,
			5, 10
		};

		public static readonly byte[] STAUSATK = {
			1,
			37, 1, 2, 4,
			8, 2, 0, 9,
			3, 2, 19, 5,
			2, 7, 0, 12,
			1, 4

		};

		public static readonly double[] STATUSDEF = {
			1,
			53, 1, 3, 4,
			6, 2, 4, 3,
			1, 2, 18, 4,
			2, 0, 1, 2,
			4, 6
		};

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
