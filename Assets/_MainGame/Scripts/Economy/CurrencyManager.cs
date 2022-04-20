namespace FarmGame.Economy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    public partial class CurrencyManager
    {
        [field: SerializeField] public string[] EditorFindAssetsPath { get; private set; }

        [ContextMenu("Populate")]
        private void EditorPopulate()
        {
            // Search for assets
            string[] guids = AssetDatabase.FindAssets($"t: {typeof(CurrencyAsset)}", EditorFindAssetsPath);

            EditorUtility.SetDirty(this);
            m_CurrencyAssets = new CurrencyAsset[guids.Length];
            for(int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<CurrencyAsset>(path);
                m_CurrencyAssets[i] = asset;
            }
        }
    }
#endif    

    public partial class CurrencyManager : SingletonBehaviour<CurrencyManager>
    {
        #region Static ----------------------------------------------------------------------------------------------------
        public static int Get(CurrencyAsset type) => Current.InnerGet(type);
        //public static Sprite GetSprite(CurrencyType type) => Current.InnerGetSprite(type);
        public static bool CanConsume(CurrencyAsset type, int amount) => Current.InnerCanConsume(type, amount);
        public static bool TryConsume(CurrencyAsset type, int amount) => Current.InnerTryConsume(type, amount);
        #endregion

        [SerializeField] private CurrencyAsset[] m_CurrencyAssets;
        private Dictionary<CurrencyAsset, int> m_CurrencyValues;

        private void Awake()
        {
            m_CurrencyValues = new Dictionary<CurrencyAsset, int>();
        }
        
        private int InnerGet(CurrencyAsset asset) => m_CurrencyValues[asset];
        private bool InnerCanConsume(CurrencyAsset type, int amount) => m_CurrencyValues[type] >= amount;
        private bool InnerTryConsume(CurrencyAsset type, int amount)
        {
            if (!InnerCanConsume(type, amount)) return false;

            m_CurrencyValues[type] -= amount;
            return true;
        }
    }
}

