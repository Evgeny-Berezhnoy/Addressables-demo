using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace AddressablesDemo
{

    public class Demo : MonoBehaviour
    {

        #region Fields

        [SerializeField] private Button _loadAsssetsButton;
        
        [SerializeField] private DataSpriteBundle[] _dataSpriteBundles;
        [SerializeField] private DataAudioBundle _dataAudioBundle;

        private Stack<AsyncOperationHandle> _addressableOperationHandles;

        #endregion

        #region Unity Events

        private void Start()
        {

            _addressableOperationHandles = new Stack<AsyncOperationHandle>();

            _loadAsssetsButton.onClick.AddListener(LoadAsset);
            
        }

        private void OnDestroy()
        {

            StopAllCoroutines();

            _loadAsssetsButton.onClick.RemoveAllListeners();

            while(_addressableOperationHandles.Count > 0)
            {

                Addressables.ReleaseInstance(_addressableOperationHandles.Pop());

            };

        }

        #endregion

        #region Methods

        private void LoadAsset()
        {
            
            _loadAsssetsButton.interactable = false;

            StartCoroutine("LoadResourcesFromNetwork");
        
        }

        private void LoadResourcesFromNetwork()
        {

            var spriteResourcesOperation = Addressables.LoadResourceLocationsAsync("Sprites", typeof(Sprite));

            _addressableOperationHandles.Push(spriteResourcesOperation);

            spriteResourcesOperation.Completed += (resourceOperation) =>
            {

                var spritesLocations = resourceOperation.Result;

                for (int i = 0; i < spritesLocations.Count; i++)
                {

                    var location = spritesLocations[i];

                    var spriteAssetLoadOperation = Addressables.LoadAssetAsync<Sprite>(location);

                    _addressableOperationHandles.Push(spriteAssetLoadOperation);

                    spriteAssetLoadOperation.Completed += (assetOperation) =>
                    {

                        var spriteAsset = assetOperation.Result;

                        var dataSpriteBundle = _dataSpriteBundles.FirstOrDefault(x => x.NameAssetBundle.editorAsset.Equals(spriteAsset.texture));

                        dataSpriteBundle.Image.sprite = spriteAsset;

                    };

                };

            };

            var musicResourcesOperation = Addressables.LoadResourceLocationsAsync("Audioclip", typeof(AudioClip));

            _addressableOperationHandles.Push(musicResourcesOperation);

            musicResourcesOperation.Completed += (resourceOperation) =>
            {

                var musicLocations = resourceOperation.Result;

                var audioclipAssetLoadOperation = Addressables.LoadAssetAsync<AudioClip>(musicLocations[0]);

                _addressableOperationHandles.Push(audioclipAssetLoadOperation);

                audioclipAssetLoadOperation.Completed += (assetOperation) =>
                {

                    var audioClip = assetOperation.Result;

                    if (_dataAudioBundle.NameAssetBundle.editorAsset.Equals(audioClip))
                    {

                        _dataAudioBundle.AudioSource.clip = audioClip;
                        _dataAudioBundle.AudioSource.Play();

                    };

                };

            };

        }

        #endregion

    }

}