using TMPro;
using UnityEngine;

namespace _Assets.BoardSystem
{
    public class DefenseItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private GameObject container;


        private void OnEnable()
        {
            BoardEventsHandler.DefenseItemHovered += HandleHovered;
            BoardEventsHandler.DefenseItemUnhovered += HandleUnhovered;
        }


        private void OnDisable()
        {
            BoardEventsHandler.DefenseItemHovered -= HandleHovered;
            BoardEventsHandler.DefenseItemUnhovered -= HandleUnhovered;
        }


        private void HandleHovered(DefenseItem item)
        {
            container.SetActive(true);
            SetStatsText(item.GetDamage(), item.GetRange(), item.GetInterval(), item.GetAttackingDirection());
        }


        private void HandleUnhovered(DefenseItem item)
        {
            container.SetActive(false);
        }


        private void SetStatsText(float damage, float range, float interval, string attackDirection)
        {
            statsText.text = "Damage: " + damage + "\nRange: " + range + "\nInterval: " + interval + "\nAttack Direction: " + attackDirection;
        }
    }
}