using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using QuizManagement;
using UniRx.Async;
using System.Threading.Tasks;
using Firebase.Functions;

namespace QuizManagement.Api
{
	public class ApiController : MonoBehaviour
	{

		public const string RANKING_URL = "https://us-central1-oshiroquiz.cloudfunctions.net/Ranking";


		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}


		public async UniTask CareerQuizLoad(CareerQuizMaker quizMaker, int career) {
			Debug.Log("■CareerQuizLoad API実行");
			// UnityWebRequest request = UnityWebRequest.Get(Const.QUIZ_LOAD_URL);
			// // リクエスト送信
			// yield return request.SendWebRequest();

			string result = null;

			FirebaseAuth auth = FirebaseAuth.DefaultInstance;

			await CareerQuizLoadAsync(career).ContinueWith((task) => {
				if (task.IsFaulted) {
					foreach (var inner in task.Exception.InnerExceptions) {
						if (inner is FunctionsException) {
							var e = (FunctionsException) inner;
							// Function error code, will be INTERNAL if the failure
							// was not handled properly in the function call.
							var code = e.ErrorCode;
							var message = e.Message;

							Debug.Log("・API結果がエラー[code:"+code + "][message:" + message + "]");
						}
					}
				} else {
					result = task.Result;
				}
			});

			if (result != null)
			{
				Debug.Log("・API結果のtext:"+result);
				//testText.text = text;

				CareerQuizData loadCareerQuizData = JsonUtility.FromJson<CareerQuizData>(result);

				if (loadCareerQuizData == null || loadCareerQuizData.value.Count < GameDirector.QUIZ_MAX_NUM) {

					if (loadCareerQuizData == null) {
						Debug.Log("・レスポンスがNULL");
					} else {
						Debug.Log("・レスポンスのサイズが少ない（サイズ）：" + loadCareerQuizData.value.Count);						
					}

					// TODO exception

				} else {
					Debug.Log("・レスポンスのサイズ：" + loadCareerQuizData.value.Count);

					List<int> typeList = new List<int>();

					Dictionary<int, CareerLoadData> careerQuizDatas = new Dictionary<int, CareerLoadData>();
					for (int i = 0; i < loadCareerQuizData.value.Count; i++) {
						careerQuizDatas.Add(loadCareerQuizData.value[i].type, loadCareerQuizData.value[i]);

						typeList.Add(loadCareerQuizData.value[i].type);
					}

					quizMaker.SetCareerQuizDatas(careerQuizDatas, typeList.ToArray());
				}
			}
		}

		// private async UniTask<string> CareerQuizLoadFunction(int career) {
		// 	var data = new Dictionary<string, object>();
		// 	data["career"] = career;

		// 	var functions = Firebase.Functions.FirebaseFunctions.GetInstance("asia-northeast1");
		// 	var function = functions.GetHttpsCallable("careerQuizLoad");

		// 	string result = null;
		// 	await function.CallAsync(data).ContinueWith((task) => {
		// 		result = (string) task.Result.Data;
		// 	});

		// 	return result;
		// }
		private async Task<string> CareerQuizLoadAsync(int career) {
			var data = new Dictionary<string, object>();
			data["career"] = career;

			var functions = Firebase.Functions.FirebaseFunctions.GetInstance("asia-northeast1");
			var function = functions.GetHttpsCallable("careerQuizLoad");

			return await function.CallAsync(data).ContinueWith((task) => {
				Debug.Log("!!!!!!!!!!!!! oncall呼び出し結果"+(string) task.Result.Data);
				return (string) task.Result.Data;
			});
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
