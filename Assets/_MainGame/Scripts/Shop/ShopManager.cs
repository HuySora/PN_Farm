namespace FarmGame.Shop
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
    
    public partial class ShopManager
    {
        [field: SerializeField] public string[] Editor_FindAssetsPath { get; private set; }
        [field: SerializeField] public bool Editor_DoNullCheck { get; private set; } = true;


        [ContextMenu("Populate")]
        private void Editor_Populate()
        {
            EditorUtility.SetDirty(this);
            // Search for assets and apply it to the field
            string[] guids = AssetDatabase.FindAssets($"t: {typeof(ShopCategoryAsset)}", Editor_FindAssetsPath);
            m_CategoryAssets = new ShopCategoryAsset[guids.Length];
            for(int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                m_CategoryAssets[i] = AssetDatabase.LoadAssetAtPath<ShopCategoryAsset>(path);

                if (!Editor_DoNullCheck) continue;

                if (m_CategoryAssets[i].Sprite == null)
                {
                    Debug.LogWarning($"{m_CategoryAssets[i].name} has null(s)! (click to trace)", m_CategoryAssets[i]);
                }
            }

            // Search for assets and apply it to the field
            guids = AssetDatabase.FindAssets($"t: {typeof(ShopItemAsset)}", Editor_FindAssetsPath);
            m_ItemAssets = new ShopItemAsset[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                m_ItemAssets[i] = AssetDatabase.LoadAssetAtPath<ShopItemAsset>(path);

                if (!Editor_DoNullCheck) continue;

                bool iconIsNull = m_ItemAssets[i].Icon == null;
                bool categoryAssetIsNull = m_ItemAssets[i].ShopCategoryAsset == null;
                bool currencyAssetIsNull = m_ItemAssets[i].CurrencyAsset == null;
                if (iconIsNull || categoryAssetIsNull || currencyAssetIsNull)
                {
                    Debug.LogWarning($"{m_ItemAssets[i].name} has null(s)! (click to trace)", m_ItemAssets[i]);
                }
            }
        }
    }
#endif

    public partial class ShopManager : SingletonBehaviour<ShopManager> {
        [Header("Shop")]
        [SerializeField] private RectTransform m_CategoryPanelTransform;
        [SerializeField] private ToggleGroup m_CategoryToggleGroup;

        [SerializeField] private ShopCategoryToggle m_CategoryTogglePrefab;
        [SerializeField] private ShopCategoryPanel m_CategoryPanelPrefab;
        [SerializeField] private ShopItemHolder m_ItemHolderPrefab;

        [Header("Use 'Populate' in context menu")]
        [SerializeField] private ShopCategoryAsset[] m_CategoryAssets;
        [SerializeField] private ShopItemAsset[] m_ItemAssets;
        
        private Dictionary<ShopCategoryAsset, RectTransform> m_CategoryContentTransforms;

        private void Awake()
        {
            m_CategoryContentTransforms = new Dictionary<ShopCategoryAsset, RectTransform>();
        }

        private void Start()
        {
            // Populating the UI. We could use Resources.Load, not sure if it boost performance or not, or
            // Addressables package too, still we try to keep it simple and use pre-populate lists
            foreach (var category in m_CategoryAssets)
            {
                // NULLCHECK: Game designer error
                if (category.TryNullCheckAndLog("Detect null value in list of ShopCategoryAsset.", this)) continue;

                var panel = Instantiate(m_CategoryPanelPrefab, m_CategoryPanelTransform);
                panel.Title.text = category.Name;
                m_CategoryContentTransforms[category] = panel.ContentTransform;

                var toggle = Instantiate(m_CategoryTogglePrefab, m_CategoryToggleGroup.transform);
                toggle.Title.text = category.Name;
                toggle.Image.sprite = category.Sprite;
                toggle.Toggle.onValueChanged.AddListener((value) => panel.gameObject.SetActive(value));
                // We do '.isOn = false' to make the first toggle as an active one and also trigger onValueChanged
                // event and make the panel inactive
                toggle.Toggle.isOn = false;
                toggle.Toggle.group = m_CategoryToggleGroup;
            }

            foreach (var item in m_ItemAssets)
            {
                // NULLCHECK: Game designer error
                if (item.TryNullCheckAndLog("Detect null value in list of ShopItemAsset.", this)) continue;
                if (item.ShopCategoryAsset.TryNullCheckAndLog("ShopCategoryAsset is null.", item)) continue;
                
                Instantiate(m_ItemHolderPrefab, m_CategoryContentTransforms[item.ShopCategoryAsset]).Inject(item);
            }
        }
    }
}

