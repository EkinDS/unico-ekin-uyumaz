using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Assets.BoardSystem
{
    [CreateAssetMenu(fileName = "DefenseArrangementData", menuName = "Scriptable Objects/DefenseArrangementData", order = 1)]
    public class DefenseArrangementData : ScriptableObject
    {
        public List<DefenseArrangement> arrangements;
    }


    [Serializable]
    public class DefenseArrangement
    {
        [SerializeField] public List<Vector3> localPositions;
    }
}