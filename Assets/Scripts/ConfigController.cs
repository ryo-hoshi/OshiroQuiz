using Common;
using QuizCollections;
using UnityEngine;
using UnityEngine.UI;

namespace QuizManagement
{
    public class ConfigController : MonoBehaviour
    {
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Button okButton;
        [SerializeField]
        private Slider masterVolumeSlider;
        [SerializeField]
        private Slider seVolumeSlider;
        [SerializeField]
        private Slider voiceVolumeSlider;

        SaveData saveData;

        // Start is called before the first frame update
        void Start()
        {
            init();

            cancelButton.onClick.AddListener(() => close());

            okButton.onClick.AddListener(() => save());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

		/// <summary>初期化
        /// </summary>
        private void init() {

            saveData = new SaveData();
            
            ConfigInfo configInfo = saveData.GetConfigData();
            masterVolumeSlider.value = configInfo.MasterVolume;
            seVolumeSlider.value = configInfo.SeVolume;
            voiceVolumeSlider.value = configInfo.VoiceVolume;
        }

        private void save() {

            saveData.SaveConfigData(masterVolumeSlider.value, seVolumeSlider.value, voiceVolumeSlider.value);
            
            SoundController.instance.SetAudioMixerVolume(masterVolumeSlider.value, seVolumeSlider.value, voiceVolumeSlider.value);

            Destroy(gameObject);
        }

        private void close() {
            Destroy(gameObject);
        }
    }
}