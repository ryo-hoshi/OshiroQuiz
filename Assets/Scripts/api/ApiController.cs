using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace QuizManagement.Api
{
	public class ApiController : MonoBehaviour
	{
		public const string QUIZ_LOAD_URL = "https://us-central1-oshiroquiz.cloudfunctions.net/QuizLoad";

		public const string RANKING_URL = "https://us-central1-oshiroquiz.cloudfunctions.net/Ranking";


		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}


		public IEnumerator CareerQuizLoad() {
			Debug.Log("■CareerQuizLoad API実行");
			UnityWebRequest request = UnityWebRequest.Get(QUIZ_LOAD_URL);
			// リクエスト送信
			yield return request.SendWebRequest();

			// 通信エラーチェック
			if (request.isHttpError || request.isNetworkError) {
				Debug.Log("・API結果がエラー:"+request.error);
			} else {
				// UTF8文字列として取得する
				string text = request.downloadHandler.text;
				Debug.Log("・API結果のtext:"+text);
				//testText.text = text;

				CareerQuiz careerQuizData = JsonUtility.FromJson<CareerQuiz>(text);

				if (careerQuizData == null) {
					Debug.Log("・レスポンスがNULL");
				} else {
					Debug.Log("・レスポンスのサイズ：" + careerQuizData.value.Count);
					Debug.Log("・レスポンスの値：" + careerQuizData.value[0].question);
				}
			}
		}

		/*
	public IEnumerator RankingLoad() {
		Debug.Log("API実行");
		UnityWebRequest request = RANKING_URL;
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
	}
}
