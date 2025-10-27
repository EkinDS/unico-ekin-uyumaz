using UnityEngine;

namespace _Assets.BoardSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int[] enemyCounts; //Needs to have have 3 elements
        public int[] defenseItemCounts; //Needs to have 3 elements
    }
}