using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Assets.BoardSystem
{
    public class DefenseItem : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private int range;
        [SerializeField] private float interval;
        [SerializeField] private AttackingDirection attackingDirection;
        [SerializeField] private Projectile projectile;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material hoverMaterial;
        [SerializeField] private List<MeshRenderer> meshRenderers;

        private PlayerBoardGrid assignedPlayerBoardGrid;
        private PlayerBoardGrid playerBoardGridBelow;
        private Vector3 originalPosition;
        private float nextShootingTime;
        private List<Enemy> relatedEnemies = new List<Enemy>();
        private DefenseItemState defenseItemState = DefenseItemState.Precombat;
        private Plane dragPlane = new Plane(Vector3.up, new Vector3(0F, 0.1F, 0F));
        private Camera mainCamera;
        private Tween scaleTween;
        private bool isBeingDragged;


        private void Awake()
        {
            originalPosition = transform.position;
            mainCamera = Camera.main;
        }


        private void Update()
        {
            if (defenseItemState != DefenseItemState.Combat)
            {
                return;
            }

            if (Time.time >= nextShootingTime && assignedPlayerBoardGrid)
            {
                ShootIfPossible();
            }
        }


        private void OnMouseDown()
        {
            if (defenseItemState != DefenseItemState.Precombat)
            {
                return;
            }

            isBeingDragged = true;

            scaleTween = transform.DOScale(1.2F, 0.4F).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = defaultMaterial;
            }

            BoardEventsHandler.RaiseUnhovered(this);
        }


        private void OnMouseDrag()
        {
            if (defenseItemState != DefenseItemState.Precombat)
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!dragPlane.Raycast(ray, out float enter))
            {
                return;
            }

            transform.position = new Vector3(ray.GetPoint(enter).x, 0.1f, ray.GetPoint(enter).z);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, 1 << LayerMask.NameToLayer("PlayerBoardGrid")))
            {
                PlayerBoardGrid hitPlayerBoard = hit.collider.gameObject.GetComponent<PlayerBoardGrid>();

                hitPlayerBoard.UseDetectionMaterial();

                if (hitPlayerBoard != playerBoardGridBelow)
                {
                    if (playerBoardGridBelow)
                    {
                        playerBoardGridBelow.UseDefaultMaterial();
                    }
                }

                playerBoardGridBelow = hitPlayerBoard;
            }
            else
            {
                if (playerBoardGridBelow)
                {
                    playerBoardGridBelow.UseDefaultMaterial();
                }

                playerBoardGridBelow = null;
            }
        }


        private void OnMouseUp()
        {
            if (defenseItemState != DefenseItemState.Precombat)
            {
                return;
            }

            if (playerBoardGridBelow != null && !playerBoardGridBelow.IsFull())
            {
                GetAssignedToPlayerBoardGrid();
            }
            else
            {
                ReturnToOriginalPosition();
            }

            isBeingDragged = false;
            playerBoardGridBelow?.UseDefaultMaterial();
            scaleTween?.Kill();
            scaleTween = null;
            transform.localScale = Vector3.one;
        }


        private void OnMouseEnter()
        {
            if (isBeingDragged || defenseItemState != DefenseItemState.Precombat)
            {
                return;
            }

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = hoverMaterial;
            }

            BoardEventsHandler.RaiseHovered(this);
        }


        private void OnMouseExit()
        {
            if (isBeingDragged || defenseItemState != DefenseItemState.Precombat)
            {
                return;
            }

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = defaultMaterial;
            }

            BoardEventsHandler.RaiseUnhovered(this);
        }


        public float GetDamage()
        {
            return damage;
        }


        public float GetRange()
        {
            return range;
        }


        public string GetAttackingDirection()
        {
            return attackingDirection.ToString();
        }


        public float GetInterval()
        {
            return interval;
        }


        public void StartCombat(List<Enemy> spawnedEnemies)
        {
            defenseItemState = DefenseItemState.Combat;

            if (!assignedPlayerBoardGrid)
            {
                return;
            }

            switch (attackingDirection)
            {
                case AttackingDirection.Forward:
                    relatedEnemies = new List<Enemy>();
                    foreach (var spawnedEnemy in spawnedEnemies)
                    {
                        if (Mathf.Abs(spawnedEnemy.transform.position.x - assignedPlayerBoardGrid.transform.position.x) <
                            0.5F)
                        {
                            relatedEnemies.Add(spawnedEnemy);
                        }
                    }

                    break;
                case AttackingDirection.All:
                    relatedEnemies = new List<Enemy>(spawnedEnemies);
                    break;
            }
        }


        public void FinishCombat()
        {
            defenseItemState = DefenseItemState.Postcombat;
        }


        public void OnEnemyDied(Enemy enemy)
        {
            relatedEnemies.Remove(enemy);
        }


        private void ShootIfPossible()
        {
            Enemy targetEnemy = null;
            Vector3 assignedPlayerBoardGridPosition = assignedPlayerBoardGrid.transform.position;

            switch (attackingDirection)
            {
                case AttackingDirection.Forward:
                    foreach (var relatedEnemy in relatedEnemies)
                    {
                        Vector3 relatedEnemyPosition = relatedEnemy.transform.position;

                        if (relatedEnemyPosition.z - assignedPlayerBoardGridPosition.z < range + 0.5F &&
                            relatedEnemyPosition.z > assignedPlayerBoardGridPosition.z)
                        {
                            if (!targetEnemy || targetEnemy.transform.position.z > relatedEnemyPosition.z)
                            {
                                targetEnemy = relatedEnemy;
                            }
                        }
                    }

                    break;
                case AttackingDirection.All:
                    foreach (var relatedEnemy in relatedEnemies)
                    {
                        Vector3 relatedEnemyPosition = relatedEnemy.transform.position;

                        if (Mathf.Abs(relatedEnemyPosition.z - assignedPlayerBoardGridPosition.z) < range + 0.5F &&
                            Mathf.Abs(relatedEnemyPosition.x - assignedPlayerBoardGridPosition.x) < range + 0.5F)
                        {
                            if (!targetEnemy || targetEnemy.transform.position.z > relatedEnemyPosition.z)
                            {
                                targetEnemy = relatedEnemy;
                            }
                        }
                    }

                    break;
            }

            if (targetEnemy)
            {
                scaleTween = transform.DOPunchScale(Vector3.one * 0.1F, 0.2F, 1, 0);

                projectile.GetLaunched(targetEnemy, damage);

                nextShootingTime = Time.time + interval;
            }
        }


        private void GetAssignedToPlayerBoardGrid()
        {
            if (assignedPlayerBoardGrid)
            {
                assignedPlayerBoardGrid.RemoveDefenseItem(this);
            }

            playerBoardGridBelow.AcceptDefenseItem(this);
            assignedPlayerBoardGrid = playerBoardGridBelow;
        }


        private void ReturnToOriginalPosition()
        {
            if (assignedPlayerBoardGrid)
            {
                assignedPlayerBoardGrid.RemoveDefenseItem(this);
            }

            transform.position = originalPosition;
        }
    
    
        private void OnEnable()
        {
            BoardEventsHandler.PlayerFailed += FinishCombat;
        }


        private void OnDisable()
        {
            BoardEventsHandler.PlayerFailed -= FinishCombat;
        }
    }
}