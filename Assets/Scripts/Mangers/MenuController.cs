using System;
using System.Collections.Generic;
using DG.Tweening;


using TMPro;
using UnityEngine;

namespace GameSystems.Core
{
    public class MenuController : MonoBehaviour
    {

        [Header("--------------------------------Level SelectionPanel---")] [SerializeField]
        private RectTransform LevelSelctionParrent;


        [SerializeField] private RectTransform RaycastBlocker;
        [SerializeField] private RectTransform LevelSelctionMenu;

        [Header("--------------------------------Level Name---")] [SerializeField]
        private TextMeshProUGUI CurrentLevelName;


        private IPoolService poolService;
        private IGameManger gameManger;
        private IAssetLoaderService assetLoaderService;

        private void Start()
        {
            poolService = ServiceLocator.Instance.GetService<IPoolService>();
            gameManger = ServiceLocator.Instance.GetService<IGameManger>();
            assetLoaderService = ServiceLocator.Instance.GetService<IAssetLoaderService>();
        }

        public void ShowLevelMenu(List<Level> levels)
        {

            // Util.ShowMessage($"Show Level menu",TextColor.Yellow);
            RaycastBlocker.gameObject.SetActive(true);
            Debug.Log($"Level count is{levels.Count}");
            AddLevelIcons(levels);
            UpdateLevelnameText(string.Empty);
        }

        public void UpdateLevelnameText(string name)
        {
            CurrentLevelName.text = name;
        }

        private void ClearLevelIcons()
        {
            foreach (var item in LevelSelctionParrent.GetComponentsInChildren<LevelSelectButton>())
            {
                item.transform.SetParent(poolService.GetGameObject().transform, false);
                item.gameObject.SetActive(false);
            }
        }

        private void AddLevelIcons(List<Level> gameDataLevels)
        {


            foreach (var item in gameDataLevels)
            {
                Debug.Log($"we have {gameDataLevels.Count} levels");
                //Just for Simplify
                var passed = true;

                var number = item.number + 1;

                var levelButtonObject = poolService.Get("LevelButton");
                levelButtonObject.SetActive(true);
                levelButtonObject.transform.SetParent(LevelSelctionParrent, false);
                var icon = Resources.Load<Sprite>(passed ? "Unlocked" : "Lock");
                assetLoaderService.LoadAddressableAsset<Sprite>($"icons/{(passed ? "Unlocked" : "Lock")}", (icon) =>
                {
                    levelButtonObject.GetComponent<LevelSelectButton>().Setup(new LevelSelectButtonData(number, passed,
                        icon,
                        () =>
                        {
                            gameManger.StartLevel(item);
                            Hide();

                        }));
                });

            }

            Show();
        }

        private void Show()
        {
            LevelSelctionMenu.transform.position = new Vector3(Screen.width * 4, Screen.height / 2, 0);
            LevelSelctionMenu.gameObject.SetActive(true);
            LevelSelctionMenu.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), .5f).SetEase(Ease.InOutQuint);
        }

        private void Hide()
        {

            LevelSelctionMenu.transform.DOMove(new Vector3(Screen.width * 2, Screen.height / 2, 0), .5f)
                .SetEase(Ease.InOutQuint).OnComplete(() =>
                {
                    ClearLevelIcons();
                    RaycastBlocker.gameObject.SetActive(false);
                });
        }

    }
}