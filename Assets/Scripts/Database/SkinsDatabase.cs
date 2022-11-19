using UnityEngine;
using System.Collections.Generic;

namespace PingPong.Database
{
    [CreateAssetMenu(fileName = "SkinsDatabase", menuName = "PingPong/CreateSkinsDatabase")]
    public sealed class SkinsDatabase : ScriptableObject
    {
        public int SkinsCount => _sprites == null ? 0 : _sprites.Count;


        [SerializeField] private Sprite _default;
        [SerializeField] private List<Sprite> _sprites;

       
        public Sprite GetSkin(int index)
        {
            if (index < 0)
            {
                Debug.LogWarning($"Try get skin of ball with index {index}");
                return _default;
            }

            if (_sprites == null || index >= _sprites.Count)
            {
                Debug.LogWarning($"Try get skin of ball with index {index}. Count skins {SkinsCount}");
                return _default;
            }
                

            return _sprites[index];
        }
    }
}
