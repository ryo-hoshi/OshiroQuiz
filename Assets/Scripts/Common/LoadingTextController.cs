using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class LoadingTextController : MonoBehaviour
    {
        private Text loadingText;

		void Start()
		{
            loadingText = this.GetComponent<Text>();
        }

        public void Display()
        {
            var loadingColor = loadingText.color;
            loadingColor.a = 255;
            loadingText.color = loadingColor;
        }

        public void Hidden()
        {
            var loadingColor = loadingText.color;
            loadingColor.a = 0;
            loadingText.color = loadingColor;
        }
    }
}
