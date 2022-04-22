namespace FarmGame
{
    using UnityEngine;

    public class GameManager : SingletonBehaviour<GameManager>
    {
        #region Static ----------------------------------------------------------------------------------------------------
        #endregion

        private void Awake()
        {
            Input.simulateMouseWithTouches = false;
        }
    }
}

