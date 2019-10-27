using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

	public class StatusCalcBasis
    {
        // ランク計算の設定値
        private const int RANK_CALC_INIT = 5;
        private const int RANK_EXP_UP_STEP = 5;

        // 該当の身分になるために必要な経験値
        private const int DAIMYOU_THRESHOLD = 60;
		private const int SYUKUROU_THRESHOLD = 44;
		private const int KAROU_THRESHOLD = 31;
		private const int SAMURAI_DAISYOU_THRESHOLD = 21;
		private const int ASHIGARU_DAISYOU_THRESHOLD = 13;
		private const int ASHIGARU_KUMIGASHIRA_THRESHOLD = 6;

		public enum Career {
			大名 = 7,
			宿老 = 6,
			家老 = 5,
			侍大将 = 4,
			足軽大将 = 3,
			足軽組頭 = 2,
			足軽 = 1,
		}

        /// <summary>次の身分に上がるための経験値の情報（大名は身分は最大なので大名に上がった時の値のまま）
        /// </summary>
		public static Dictionary<int, int> NextCareerUpExps = new Dictionary<int, int>()
		{
            {(int)Career.大名, DAIMYOU_THRESHOLD},
            {(int)Career.宿老, DAIMYOU_THRESHOLD},
			{(int)Career.家老, SYUKUROU_THRESHOLD},
			{(int)Career.侍大将, KAROU_THRESHOLD},
			{(int)Career.足軽大将, SAMURAI_DAISYOU_THRESHOLD},
			{(int)Career.足軽組頭, ASHIGARU_DAISYOU_THRESHOLD},
			{(int)Career.足軽, ASHIGARU_KUMIGASHIRA_THRESHOLD}
		};

        private const int REIWA_BAKUFU_THRESHOLD = 600;
        private const int TENGABITO_THRESHOLD = 450;
        private const int JYOURAKU_THRESHOLD = 300;
        private const int CHIHOU_TOUITSU_THRESHOLD = 150;
        private const int MEIMON_DAIMYOU_THRESHOLD = 60;
        private const int IKKOKU_TOUITSU_THRESHOLD = 30;
        private const int SYOU_DAIMYOU_THRESHOLD = 6;

        public enum DaimyouClass
        {
            その他 = 99,
            令和幕府 = 8,
            天下人 = 7,
            上洛 = 6,
            地方統一 = 5,
            名門大名 = 4,
            一国統一 = 3,
            小大名 = 2,
            滅亡危機 = 1,
        }

        // public Dictionary<int, int> NextDaimyouClassUpCastles = new Dictionary<int, int>()
        // {
        //     {(int)DaimyouClass.天下人, REIWA_BAKUFU_THRESHOLD},
        //     {(int)DaimyouClass.上洛, TENGABITO_THRESHOLD},
        //     {(int)DaimyouClass.地方統一, JYOURAKU_THRESHOLD},
        //     {(int)DaimyouClass.名門大名, CHIHOU_TOUITSU_THRESHOLD},
        //     {(int)DaimyouClass.一国統一, MEIMON_DAIMYOU_THRESHOLD},
        //     {(int)DaimyouClass.小大名, IKKOKU_TOUITSU_THRESHOLD},
        //     {(int)DaimyouClass.滅亡危機, SYOU_DAIMYOU_THRESHOLD}
        // };
        
        /// <summary>身分が大名の時のメーターの色
        /// </summary>
        public static Dictionary<int, string> DaimyouClassColor = new Dictionary<int, string>()
        {
            {(int)DaimyouClass.令和幕府, "#9400d3"},
            {(int)DaimyouClass.天下人, "#da70d6"},
            {(int)DaimyouClass.上洛, "#0000ff"},
            {(int)DaimyouClass.地方統一, "#4169e1"},
            {(int)DaimyouClass.名門大名, "#ff0000"},
            {(int)DaimyouClass.一国統一, "#ff4500"},
            {(int)DaimyouClass.小大名, "#ffff00"},
            {(int)DaimyouClass.滅亡危機, "#f0e68c"},
            {(int)DaimyouClass.その他, "#00ff7f"}
        };

        /// <summary>身分経験値に対応する身分を取得する
        /// <param name="careerExp">身分経験値</param>
        /// </summary>
        public static Career CareerFromCareerExp(int careerExp)
        {
            if (DAIMYOU_THRESHOLD <= careerExp)
            {
                return Career.大名;
            }
            else if (SYUKUROU_THRESHOLD <= careerExp)
            {
                return Career.宿老;
            }
            else if (KAROU_THRESHOLD <= careerExp)
            {
                return Career.家老;
            }
            else if (SAMURAI_DAISYOU_THRESHOLD <= careerExp)
            {
                return Career.侍大将;
            }
            else if (ASHIGARU_DAISYOU_THRESHOLD <= careerExp)
            {
                return Career.足軽大将;
            }
            else if (ASHIGARU_KUMIGASHIRA_THRESHOLD <= careerExp)
            {
                return Career.足軽組頭;
            }
            else
            {
                return Career.足軽;
            }
        }

        /// <summary>城支配数に対応する大名格を取得する
        /// <param name="castleDominance">城支配数</param>
        /// </summary>
        public static DaimyouClass DaimyouClassFromCastleNum(int castleDominance)
        {
            if (REIWA_BAKUFU_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.令和幕府;
            }
            else if (TENGABITO_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.天下人;
            }
            else if (JYOURAKU_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.上洛;
            }
            else if (CHIHOU_TOUITSU_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.地方統一;
            }
            else if (MEIMON_DAIMYOU_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.名門大名;
            }
            else if (IKKOKU_TOUITSU_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.一国統一;
            }
            else if (SYOU_DAIMYOU_THRESHOLD <= castleDominance)
            {
                return DaimyouClass.小大名;
            }
            else
            {
                return DaimyouClass.滅亡危機;
            }
        }
        
        /// <summary>次のランクアップに必要な経験値を計算する
        /// </summary>
		public static int CalcNextRankUpExp(int rank)
        {
            Debug.Log("次のランクアップに必要な経験値："+ RANK_CALC_INIT + (rank / RANK_EXP_UP_STEP));
            Debug.Log("rank："+ rank);
            Debug.Log("次のランクアップに必要な経験値：");
            return RANK_CALC_INIT + (rank / RANK_EXP_UP_STEP);
        }

        /// <summary>メーター表示に使用する値を計算する（小数点2位までの値）
        /// メーターMax値に対する現在地の割合
        /// </summary>
        public static float CalcMeter(int nowValue, int nextStateUpValue)
        {
            Debug.Log("CalcMeter値："+ (float)Math.Round((float)nowValue / nextStateUpValue, 2, MidpointRounding.AwayFromZero));
            return (float)Math.Round((float)nowValue / nextStateUpValue, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>キャリアの数値からキャリアのEnum値を取得
        /// </summary>
        public static Career CareerFromNum(int careerNum)
        {
            return (Career)Enum.ToObject(typeof(Career), careerNum);
        }

        /// <summary>大名格の数値から大名格のEnum値を取得
        /// </summary>
        public static DaimyouClass DaimyouClassFromNum(int daimyouClassNum)
        {
            return (DaimyouClass)Enum.ToObject(typeof(DaimyouClass), daimyouClassNum);
        }

        /// <summary>身分と大名格から身分メーターの色を取得する
        /// </summary>
        public static Color CareerMeterColorCode(int career, int daimyouClass)
        {

            int daimyouClassNum = career < (int)Career.大名 ? (int)DaimyouClass.その他 : daimyouClass;

            string colorCode = DaimyouClassColor[daimyouClassNum];

			Color meterColor;
			if (ColorUtility.TryParseHtmlString(colorCode, out meterColor))
            {
                return meterColor;
            }
            else
            {
                // 万が一取得できなかった時のデフォルト値
                return Color.magenta;
            }
        }
    }
}
