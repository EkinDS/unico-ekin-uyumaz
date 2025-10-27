using System.Collections.Generic;
using UnityEngine;

namespace _Assets.BoardSystem
{
    [CreateAssetMenu(fileName = "EnemySpawnPositionList", menuName = "Scriptable Objects/EnemySpawnPositionList", order = 1)]
    public class EnemySpawnPositionList : ScriptableObject
    {
        public List<float> possibleEnemySpawnXPositions;
    }
}