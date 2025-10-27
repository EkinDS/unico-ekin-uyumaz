using System.Collections.Generic;
using UnityEngine;

namespace _Assets.BoardSystem
{
    public class PlayerBoardGrid : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private DefenseArrangementData arrangementData;
        [SerializeField] private Material detectionMaterial;
        [SerializeField] private Material defaultMaterial;

        private List<DefenseItem> defenseItems = new List<DefenseItem>();

        private const int MaximumDefenseItemCount = 9;


        public void AcceptDefenseItem(DefenseItem defenseItem)
        {
            defenseItems.Add(defenseItem);

            RearrangeAllDefenseItems();
        }


        public void RemoveDefenseItem(DefenseItem defenseItem)
        {
            defenseItems.Remove(defenseItem);

            RearrangeAllDefenseItems();
        }


        public bool IsFull()
        {
            return defenseItems.Count >= MaximumDefenseItemCount;
        }


        public void UseDetectionMaterial()
        {
            meshRenderer.material = detectionMaterial;
        }


        public void UseDefaultMaterial()
        {
            meshRenderer.material = defaultMaterial;
        }


        private void RearrangeAllDefenseItems()
        {
            DefenseArrangement arrangement = arrangementData.arrangements[defenseItems.Count];

            if (arrangement != null)
            {
                List<Vector3> positions = arrangement.localPositions;

                for (var i = 0; i < defenseItems.Count; i++)
                {
                    defenseItems[i].transform.localPosition = transform.position + positions[i];
                }
            }
        }
    }
}