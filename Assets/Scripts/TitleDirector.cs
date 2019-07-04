using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor;

namespace QuizManagement
{
	public class TitleDirector : MonoBehaviour
	{
//		[SerializeField]
//		private Text careerText;
//		[SerializeField]
//		private Text startText;

//		[SerializeField]
		//		private Text dataClear;

		private float loadGameSceneTime = 1.0f;
		private int textBlinkSpeed = (int)TextBlinkType.Init;
		private float time = 0.0f;

		[SerializeField]
		private InputField rankStarEdit;
		[SerializeField]
		private InputField rankEdit;
		[SerializeField]
		private InputField rankExpEdit;

		enum TextBlinkType {
			Init = 3,
			Load = 12,
		}

		// Start is called before the first frame update
		void Start()
		{
			/*
			SaveData saveData = new SaveData();

			StatusInfo statusInfo = saveData.GetStatusInfo();
			Debug.Log("出世経験値(Exp):"+statusInfo.CareerExp);
			Debug.Log("身分(Career):"+statusInfo.Career);
*/
//			int currentExp = careerData.Exp;
//			this.careerText.text = careerData.Career;
			outputStatusEdit();
		}

		// Update is called once per frame
		void Update()
		{
			/*
			time += Time.deltaTime * textBlinkSpeed;

			var textColor = startText.color;
//			textColor.a = Mathf.Sin(time) * this.textBlinkSpeed;
			textColor.a = Mathf.Sin(time) * 0.5f + 0.5f;
			startText.color = textColor;

			if (textBlinkSpeed == (int)TextBlinkType.Init) {
				if (Input.GetMouseButtonDown(0)) {
					StartCoroutine(loadGameScene());
				}	
			}
			*/
		}

		private IEnumerator loadGameScene () {

//			this.textBlinkSpeed = (int)TextBlinkType.Load;

//			while (time < loadGameSceneTime) {
				


//				color.a = Mathf.Sin(time) * 0.5f + 0.5f;

				//	startText.color.a = Mathf.Sin(time);

				yield return new WaitForSeconds(1.0f);
			//				yield return null;
//			}
			SceneManager.LoadScene("GameScene");
		}

		public void DataClear() {

//			if (EditorUtility.DisplayDialog("警告", "データをクリアしますか？", "OK", "CANCEL"))
			//			{
				SaveData saveData = new SaveData();
				saveData.ClearStatusInfo();
			Debug.Log("データクリア！");
			outputStatusEdit();
			//}
		}

		public void GameStart() {
			StartCoroutine(loadGameScene());
		}


		public void StatusEdit() {
			SaveData saveData = new SaveData();

			saveData.SaveRankInfo(int.Parse(this.rankStarEdit.text), int.Parse(this.rankEdit.text), int.Parse(this.rankExpEdit.text));

			outputStatusEdit();
		}

		private void outputStatusEdit() {
			SaveData saveData = new SaveData();
			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.rankStarEdit.text = statusInfo.RankStar.ToString();
			this.rankEdit.text = statusInfo.Rank.ToString();
			this.rankExpEdit.text = statusInfo.RankExp.ToString();
		}
	}
}
