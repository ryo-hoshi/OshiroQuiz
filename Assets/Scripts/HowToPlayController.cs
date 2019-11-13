using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class HowToPlayController : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;

        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(() => close());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void close() {
            SoundController.instance.Cancel();

            Destroy(gameObject);
        }
    }
}
