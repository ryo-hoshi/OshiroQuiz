using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

namespace QuizManagement
{
	public class CharactorController : MonoBehaviour
	{
		private Animator animator;
		private FaceUpdate faceUpdate;

		public enum AnimationTag {
			Idle,
			Answer,
			StatusUpdate,
		}

		// Start is called before the first frame update
		void Start()
		{
			this.animator = GetComponent<Animator>();
			this.faceUpdate = GetComponent<FaceUpdate>(); 
		}

		// Update is called once per frame
		void Update()
		{
//			Debug.LogError("タグ："+ animator.GetCurrentAnimatorStateInfo(0).tagHash +"ループするかどうか："+animator.GetCurrentAnimatorStateInfo(0).loop);
		}

		public void Wait() {

//			this.faceUpdate.OnCallChangeFace("default");
//			int waitPtn = (int)UnityEngine.Random.Range(1, 4);
//			this.animator.SetInteger("WaitPtn", waitPtn);

//			this.animator.SetTrigger("QuestionTrigger");
		}

		public void CorrectAnswerTrigger() {
//			this.faceUpdate.OnCallChangeFace("smile");
//			int correctPtn = (int)UnityEngine.Random.Range(1, 3);
//			this.animator.SetInteger("CorrectPtn", correctPtn);
			this.animator.SetInteger("CorrectPtn", 1);
			this.animator.SetInteger("InCorrectPtn", 0);

			this.animator.SetTrigger("AnswerTrigger");
		}

		public void CorrectAnswerAnotherTrigger() {
			this.animator.SetInteger("CorrectPtn", 2);
			this.animator.SetInteger("InCorrectPtn", 0);

			this.animator.SetTrigger("AnswerTrigger");
		}

		public void InCorrectAnswerTrigger() {
//			this.faceUpdate.OnCallChangeFace("angry");

			this.animator.SetInteger("CorrectPtn", 0);
			this.animator.SetInteger("InCorrectPtn", 1);
//			int inCorrectPtn = (int)UnityEngine.Random.Range(1, 3);
//			this.animator.SetInteger("InCorrectPtn", inCorrectPtn);

			this.animator.SetTrigger("AnswerTrigger");
		}

		public void InCorrectAnswerAnotherTrigger() {

			this.animator.SetInteger("CorrectPtn", 0);
			this.animator.SetInteger("InCorrectPtn", 2);
		
			this.animator.SetTrigger("AnswerTrigger");
		}

		public void WarmUp1Trigger() {
			
			this.animator.SetTrigger("WarmUp1Trigger");
		}

		public void WarmUp2Trigger() {

			this.animator.SetTrigger("WarmUp2Trigger");
		}

		public void QuizStartTrigger() {
			this.animator.SetTrigger("QuizStartTrigger");
		}

		/**
		 * ステータス計算結果アニメーション
		 */
		public bool IsResultAnimation() {
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			int hashResult = Animator.StringToHash("Result");
			print(Time.time);
			if (stateInfo.tagHash == hashResult) {
				Debug.Log("Resultタグ");

				return true;
			} else {
				Debug.Log("Resultタグ以外");

				return false;
			}
		}

		/*
		public bool IsAnswerAnimation() {
			
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			int hashResult = Animator.StringToHash("Answer");
			print(Time.time);
			if (stateInfo.tagHash == hashResult) {
				Debug.LogError("Answerタグ");

				return true;
			} else {
				Debug.LogError("Answerタグ以外");

				return false;
			}

		}
		*/

		public bool IsAnimation(string animTagName) {
			Debug.LogWarning("確認するタグ："+animTagName);

			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(animTagName)) {
				Debug.LogWarning(animTagName+"のアニメーション中！");

				return true;
			} else {
				Debug.LogWarning(animTagName+"のアニメーション以外！");

				if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) {
					Debug.LogWarning("Idleのアニメーション中！");
				}
				return false;
			}
		}

		public void ResultTrigger() {
			this.animator.SetTrigger("ResultTrigger");
		}

		public void RankUpTrigger() {
			this.animator.SetTrigger("RankUpTrigger");
		}

		public void RankDownTrigger() {
			this.animator.SetTrigger("RankDownTrigger");
		}

		public bool IsTransitionPossible() {
			return true;
		}
	}
}
