using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class ForceUpdateController : MonoBehaviour
    {
        [SerializeField]
        private Button okButton;

        // Start is called before the first frame update
        void Start()
        {
            okButton.onClick.AddListener(() => Ok());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Ok() {
            
            Application.OpenURL(Constants.M_OSHIRO_QUIZ_URL);

            okButton.onClick.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}
