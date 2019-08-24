using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx.Async;
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

		//private int textBlinkSpeed = (int)TextBlinkType.Init;
		//private float time = 0.0f;

		[SerializeField]
		private InputField rankStarEdit;
		[SerializeField]
		private InputField rankEdit;
		[SerializeField]
		private InputField rankExpEdit;
		[SerializeField]
		private InputField careerEdit;
		[SerializeField]
		private InputField careerExpEdit;

        [SerializeField]
        private Button helpButton;

        [SerializeField]
		private Button titleButton;
		[SerializeField]
		private Text loadingText;
        //        [SerializeField]
        //        private HowToPlayController howToPlayPref;

        private Animator titleAnimator;
//		[SerializeField]
//		private Text testText;
		/*
		enum TextBlinkType {
			Init = 3,
			Load = 12,
		}
		*/

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
			this.titleAnimator = titleButton.GetComponent<Animator>();
			outputStatusEdit();

            //			StartCoroutine(GetText());

            titleButton.onClick.AddListener(() => gameStart());

            helpButton.onClick.AddListener(() => helpOpen());
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
        /*
		IEnumerator GetText() {
			Debug.Log("API実行");
			UnityWebRequest request = UnityWebRequest.Get("https://us-central1-oshiroquiz.cloudfunctions.net/hello");
			// リクエスト送信
			yield return request.Send();

			// 通信エラーチェック
			if (request.isHttpError) {
				Debug.Log("■API結果がエラー:"+request.error);
			} else {
				if (request.responseCode == 200) {
					// UTF8文字列として取得する
					string text = request.downloadHandler.text;
					Debug.Log("■API結果のtext:"+text);
					testText.text = text;

					// バイナリデータとして取得する
					byte[] results = request.downloadHandler.data;
					foreach(byte a in results) {
						Debug.Log("■API結果のbyte:"+a);
					}
				}
			}
		}
		*/

        private async UniTask gameStart()
        {
            this.titleAnimator.SetTrigger("blinkEnd");

            await UniTask.Delay(650);

            var loadingColor = loadingText.color;
            loadingColor.a = 255;
            loadingText.color = loadingColor;

            await UniTask.DelayFrame(1);


            // ゲームシーンロード
            SceneManager.LoadScene("GameScene");
        }

        private void helpOpen()
        {
            var helpWindow = GameObject.FindWithTag("Help");

            if (helpWindow == null) {
                var canvas = GameObject.Find("Canvas");
                // await UniTask.Delay(650);
                var helpPref = (GameObject)Resources.Load("Prefabs/HowToPlay");
                var help = Instantiate(helpPref);
                help.tag = "Help";
                help.transform.SetParent(canvas.transform, false);
            }
        }


        /*
        private IEnumerator loadGameScene () {

//			this.textBlinkSpeed = (int)TextBlinkType.Load;
			
			float progressTime = 0.0f;
			float blinkVal = 0.0f;

			while (progressTime < LOAD_GAME_SCENE_TIME) {

				var textColor = this.gameStartText.color;
				blinkVal += Time.deltaTime * 10.0f;
				textColor.a = Mathf.Sin(blinkVal) * 0.5f *+ 0.5f;

//				startText.color.a = Mathf.Sin(progressTime);
				this.gameStartText.color = textColor;

				progressTime += Time.deltaTime;

				yield return null;
			}

			this.titleAnimator.SetTrigger("blinkEnd");

			yield return new WaitForSeconds(1f);

			var loadingColor = loadingText.color;
			loadingColor.a = 255;
			loadingText.color = loadingColor;


			// ゲームシーンロード
			SceneManager.LoadScene("GameScene");
		}
        */

        public void DataClear() {

//			if (EditorUtility.DisplayDialog("警告", "データをクリアしますか？", "OK", "CANCEL"))
			//			{
				SaveData saveData = new SaveData();
				saveData.ClearStatusInfo();
			Debug.Log("データクリア！");
			outputStatusEdit();
			//}
		}


		public void StatusEdit() {
			SaveData saveData = new SaveData();

//			saveData.SaveRankInfo(int.Parse(this.rankStarEdit.text), int.Parse(this.rankEdit.text), int.Parse(this.rankExpEdit.text));

			saveData.SaveStatusInfo(int.Parse(this.rankStarEdit.text), 
				int.Parse(this.rankEdit.text), 
				int.Parse(this.rankExpEdit.text),
				0.0f,
				int.Parse(this.careerEdit.text), 
				int.Parse(this.careerExpEdit.text),
				0.0f,
				0 // 石高
			);


			outputStatusEdit();
		}

		private void outputStatusEdit() {
			SaveData saveData = new SaveData();
			StatusInfo statusInfo = saveData.GetStatusInfo();

			this.rankStarEdit.text = statusInfo.RankStar.ToString();
			this.rankEdit.text = statusInfo.Rank.ToString();
			this.rankExpEdit.text = statusInfo.RankExp.ToString();
			this.careerEdit.text = statusInfo.Career.ToString();
			this.careerExpEdit.text = statusInfo.CareerExp.ToString();
		}
	}
}
