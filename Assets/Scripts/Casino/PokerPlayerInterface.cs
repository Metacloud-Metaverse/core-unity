using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Casino.Poker
{
	public class PokerPlayerInterface : MonoBehaviour
	{
		public TextMeshProUGUI ownBet;
		public TextMeshProUGUI availableCash;
		public List<Image> cards = new List<Image>();
		public Image turnMark;
		public CanvasGroup cGroup;
		public JoinTableButton joinTableButton;

		public float timeScale = 0.4f;
		public iTween.EaseType easeScale = iTween.EaseType.easeInOutBounce;
		public GameObject panelRootPlayer;
		public GameObject panelButton;
		private Coroutine coroutineMoveAlpha;
		private bool buttonIsPressed = false;
        private void Start()
        {
			panelRootPlayer.transform.localScale = Vector3.zero;
		}
        public void OnButtonJoin()
        {
			if (buttonIsPressed)
				return;
			StartCoroutine(OpenPanels());
		}
		public IEnumerator OpenPanels()
        {
			buttonIsPressed = true;
			var hashButton = iTween.Hash("x", 0, "y", 0, "z", 0, "time", timeScale, "easeType", easeScale);
			iTween.ScaleTo(panelButton, hashButton);
			yield return Yielders.Seconds(timeScale);
			panelButton.SetActive(false);
			panelRootPlayer.SetActive(true);
			var hashPanelPlayer = iTween.Hash("x", 1, "y", 1, "z", 1, "time", timeScale, "easeType", easeScale);
			iTween.ScaleTo(panelRootPlayer, hashPanelPlayer);
			yield return Yielders.Seconds(timeScale);
			joinTableButton.OnButtonJoinPress();
		}

		public void OnSetCurrentPlayerPlaying()
		{
			if (coroutineMoveAlpha != null)
				StopCoroutine(coroutineMoveAlpha);
			coroutineMoveAlpha =StartCoroutine(MoveAlphaGroup(1));
			turnMark.gameObject.SetActive(true);
		}
		public void OnSetPlayerWaitingForNextTurn()
		{
			turnMark.gameObject.SetActive(false);
			//	var h = iTween.Hash("from",1,"to",0.7,"onupdate", "OnUpdateAlpha");
			//iTween.ValueTo(gameObject, h);
			if (coroutineMoveAlpha != null)
				StopCoroutine(coroutineMoveAlpha);
			coroutineMoveAlpha =StartCoroutine(MoveAlphaGroup(0.7f));
		}
		public IEnumerator MoveAlphaGroup(float targetValue)
        {

			float v = cGroup.alpha;
			while (v != targetValue)
			{
				v = Mathf.MoveTowards(v, targetValue, 2 * Time.deltaTime);
				cGroup.alpha = v;
				yield return null;
			}
		}
	}
}