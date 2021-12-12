using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace LocalizationDemo {

    public class LocalizationWindow : MonoBehaviour
    {

        #region Fields

        [SerializeField] private Button _russianButton;
        [SerializeField] private Button _englishButton;

        #endregion

        #region Unity Events

        private void Start()
        {
            
            _russianButton.onClick.AddListener(() => ChangeLanguage(1));
            _englishButton.onClick.AddListener(() => ChangeLanguage(0));

        }

        private void OnDestroy()
        {
            
            _russianButton.onClick.RemoveAllListeners();
            _englishButton.onClick.RemoveAllListeners();
        
        }

        #endregion

        #region Methods

        private void ChangeLanguage(int index)
        {

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        }

        #endregion

    }

}