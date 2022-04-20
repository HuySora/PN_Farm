namespace FarmGame
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public partial class LoadScene : MonoBehaviour {
        [field: SerializeField] public int BuildIndex { get; private set; }
        public void Load() => SceneManager.LoadScene(BuildIndex);
    }
}

