namespace FarmGame.Economy
{
    using UnityEngine;

#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Currency", menuName = "Economy/Currency")]
    public partial class CurrencyAsset { }
#endif

    public partial class CurrencyAsset : ScriptableObject {
        [field: SerializeField] public string Name { get; private set; } = "New Currency";
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}

