using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace QuizManagement
{
	public class ResultDirector : MonoBehaviour
	{
		[SerializeField]
		private GameObject charactor;

		private CharactorController charactorController;

		private bool isResultAnimEnd = false;

		// Start is called before the first frame update
		void Start()
		{
			this.charactorController = this.charactor.GetComponent<CharactorController>();
			StartCoroutine(ResultAnimation());
		}

		// Update is called once per frame
		void Update()
		{
			if (this.isResultAnimEnd && Input.GetMouseButtonDown(0)) {
				SceneManager.LoadScene("GameScene");
			}
		}


		private IEnumerator ResultAnimation() {

			charactorController.ResultTrigger();
			//			long resultWait = 0.0f;

			yield return new WaitForSeconds(0.5f);

			// TODO パラメーター表示の演出
			yield return new WaitForSeconds(2.0f);

			bool isStatusUpdate = false;
			Debug.LogError("ステータス更新があるか："+isStatusUpdate);

			// ランク(ウデマエ)上下の時のアニメーション
			//			if (GamePlayInfo.Result.RankUp == statusResult) {
			if (GamePlayInfo.Result.RankUp == GamePlayInfo.QuizResult) {
				this.charactorController.RankUpTrigger();
				isStatusUpdate = true;

				//			} else if (GamePlayInfo.Result.RankDown == statusResult) {
			} else if (GamePlayInfo.Result.RankDown == GamePlayInfo.QuizResult) {
				this.charactorController.RankDownTrigger();
				isStatusUpdate = true;
			}

			if (isStatusUpdate) {
				yield return new WaitForSeconds(2.0f);
			}

			this.isResultAnimEnd = true;
			Debug.LogError("画面遷移可能！！");
		}
	}
}
