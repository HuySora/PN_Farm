namespace FarmGame.UI
{
    using UnityEngine;

    public partial class ViewManager : SingletonBehaviour<ViewManager> {
        #region Static ----------------------------------------------------------------------------------------------------
        public static Canvas Canvas => Current.m_Canvas;
        public static void SwitchToMain() => Current.InnerSwitchToMain();
        public static void SwitchToShop() => Current.InnerSwitchToShop();
        public static void SwitchToBuild() => Current.InnerSwitchToBuild();
        #endregion

        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private ViewBehaviour m_MainView;
        [SerializeField] private ViewBehaviour m_ShopView;
        [SerializeField] private ViewBehaviour m_BuildView;

        private void Start()
        {
            m_MainView.Open();
        }

        private void InnerSwitchToMain()
        {
            m_ShopView.Close();
            m_BuildView.Close();
            m_MainView.Open();
        }
        
        private void InnerSwitchToShop()
        {
            m_MainView.Close();
            m_BuildView.Close();
            m_ShopView.Open();
        }

        private void InnerSwitchToBuild()
        {
            m_MainView.Close();
            m_ShopView.Close();
            m_BuildView.Open();
        }
    }
}

