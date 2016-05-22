using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class PrefabHolder : MonoBehaviour {
        public static PrefabHolder Instance;

        public GameObject BaseTilePrefab;

        public GameObject TileNormalPrefab;
        public GameObject TileDifficultPrefab;
        public GameObject TileVeryDifficultPrefab;
        public GameObject TileImpassiblePrefab;

        private void Awake() {
            Instance = this;
        }
    }
}
