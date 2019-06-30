using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace QuizManagement
{
	public class TitleDirector : MonoBehaviour
	{
//		[SerializeField]
//		private Text careerText;
		[SerializeField]
		private Text startText;

		private float loadGameSceneTime = 1.0f;
		private int textBlinkSpeed = (int)TextBlinkType.Init;
		private float time = 0.0f;

		enum TextBlinkType {
			Init = 3,
			Load = 12,
		}

		// Start is called before the first frame update
		void Start()
		{
			SaveData saveData = new SaveData();

			StatusInfo statusInfo = saveData.GetStatusInfo();
			Debug.Log("出世経験値(Exp):"+statusInfo.CareerExp);
			Debug.Log("身分(Career):"+statusInfo.Career);

//			int currentExp = careerData.Exp;
//			this.careerText.text = careerData.Career;

		}

		// Update is called once per frame
		void Update()
		{
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
		}

		private IEnumerator loadGameScene () {

			this.textBlinkSpeed = (int)TextBlinkType.Load;

//			while (time < loadGameSceneTime) {
				


//				color.a = Mathf.Sin(time) * 0.5f + 0.5f;

				//	startText.color.a = Mathf.Sin(time);

				yield return new WaitForSeconds(1.0f);
			//				yield return null;
//			}
			SceneManager.LoadScene("GameScene");
		}
	}
}
