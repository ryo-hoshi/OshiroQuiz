using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class HowToPlayController : MonoBehaviour
    {
        [SerializeField]
        RectTransform rectTran;

        [SerializeField]
        private Button closeButton;

        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(() => close());

            open();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void open()
        {
            rectTran.localScale = new Vector3(0.0f, 0.0f, 1.0f);

            rectTran.DOScale (
                new Vector3(1.0f, 1.0f, 1.0f),
                0.2f
            );
        }

        private void close() {
            SoundController.instance.Cancel();

            Destroy(gameObject);
        }
    }
}
