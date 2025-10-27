using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Assets.BoardSystem
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private GameObject baseBoardGridPrefab;
        [SerializeField] private GameObject playerBoardGridPrefab;
        [SerializeField] private GameObject boardBase;
        [SerializeField] private EnemySpawnPositionList enemySpawnPositionList;
        [SerializeField] private List<LevelData> levelDataList;
        [SerializeField] private List<Enemy> enemyPrefabs;
        [SerializeField] private List<DefenseItem> defenseItemPrefabs;
    
        private List<GameObject> spawnedBoardGrids = new List<GameObject>();
        private List<Enemy> spawnedEnemies = new List<Enemy>();
        private List<DefenseItem> spawnedDefenseItems = new List<DefenseItem>();

        private const int BoardWidth = 4;
        private const int BoardHeight = 8;
        private const int PlayerHeight = 4;


        private void SpawnLevel(int levelId)
        {
            ResetBoard();
            CreateBoard();
            SpawnDefenseItems(levelDataList[levelId - 1]);
            SpawnEnemies(levelDataList[levelId - 1]);

            BoardEventsHandler.RaiseLevelStarted();
        }


        private void StartEnemyAdvances()
        {
            foreach (var spawnedDefenseItem in spawnedDefenseItems)
            {
                spawnedDefenseItem.StartCombat(spawnedEnemies);
            }

            foreach (var spawnedEnemy in spawnedEnemies)
            {
                spawnedEnemy.StartAdvancing();
            }
        }


        private void CreateBoard()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    float xPosition = x - BoardWidth / 2F + 0.5F;
                    float yPosition = 0.1F;
                    float zPosition = y + 0.5f;
                    Vector3 position = new Vector3(xPosition, yPosition, zPosition);
                    GameObject prefab = y < PlayerHeight ? playerBoardGridPrefab : baseBoardGridPrefab;

                    spawnedBoardGrids.Add(Instantiate(prefab, position, Quaternion.identity, transform));
                }
            }

            Vector3 centerPosition =
                (spawnedBoardGrids[0].transform.position + spawnedBoardGrids.Last().transform.position) / 2F;
            boardBase.transform.position = new Vector3(centerPosition.x, 0F, centerPosition.z);
            boardBase.transform.localScale = new Vector3(BoardWidth, 0.15F, BoardHeight);
        }


        private void SpawnEnemies(LevelData levelData)
        {
            float yPosition = 0.1F;
            float zPosition = BoardHeight;

            int totalEnemyCount = 0;
            foreach (var enemyCount in levelData.enemyCounts)
            {
                totalEnemyCount += enemyCount;
            }

            List<int> positionIndicesForEnemiesToSpawn = Enumerable.Range(0, BoardWidth * 4).OrderBy(_ => Random.value)
                .Take(totalEnemyCount).ToList();

            for (int i = 0; i < levelData.enemyCounts.Length; i++)
            {
                for (int j = 0; j < levelData.enemyCounts[i]; j++)
                {
                    float xPosition;
                    if (spawnedEnemies.Count < positionIndicesForEnemiesToSpawn.Count)
                    {
                        xPosition = enemySpawnPositionList.possibleEnemySpawnXPositions[
                            positionIndicesForEnemiesToSpawn[spawnedEnemies.Count]];
                    }
                    else
                    {
                        xPosition = enemySpawnPositionList.possibleEnemySpawnXPositions[
                            positionIndicesForEnemiesToSpawn[Random.Range(0, positionIndicesForEnemiesToSpawn.Count)]];
                    }

                    Enemy newEnemy = Instantiate(enemyPrefabs[i], new Vector3(xPosition, yPosition, zPosition), Quaternion.Euler(0F, 180F, 0F), transform);

                    newEnemy.Initialize(this);
                    spawnedEnemies.Add(newEnemy);
                }
            }
        }


        private void SpawnDefenseItems(LevelData levelData)
        {
            float yPosition = 0.1F;

            for (int i = 0; i < levelData.defenseItemCounts.Length; i++)
            {
                for (int j = 0; j < levelData.defenseItemCounts[i]; j++)
                {
                    float xPosition = 2.25F + j * 0.3F;
                    float zPosition = 0.5F + i * 0.5F;

                    spawnedDefenseItems.Add(Instantiate(defenseItemPrefabs[i], new Vector3(xPosition, yPosition, zPosition),
                        Quaternion.identity, transform));
                }
            }
        }


        private void OnEnemyDied(Enemy enemy)
        {
            spawnedEnemies.Remove(enemy);

            foreach (var spawnedDefenseItem in spawnedDefenseItems)
            {
                spawnedDefenseItem.OnEnemyDied(enemy);
            }

            if (spawnedEnemies.Count <= 0)
            {
                BoardEventsHandler.RaisePlayerWon();
            }
        }


        private void ResetBoard()
        {
            foreach (var spawnedEnemy in spawnedEnemies)
            {
                DestroyImmediate(spawnedEnemy.gameObject);
            }

            foreach (var spawnedDefenseItem in spawnedDefenseItems)
            {
                DestroyImmediate(spawnedDefenseItem.gameObject);
            }

            foreach (var boardGrid in spawnedBoardGrids)
            {
                DestroyImmediate(boardGrid.gameObject);
            }

            spawnedEnemies = new List<Enemy>();
            spawnedDefenseItems = new List<DefenseItem>();
            spawnedBoardGrids = new List<GameObject>();
        }
    
    
        private void OnEnable()
        {
            BoardEventsHandler.StartEnemyAdvancesButtonClicked += StartEnemyAdvances;
            BoardEventsHandler.SwitchLevelButtonClicked += SpawnLevel;
            BoardEventsHandler.EnemyDied += OnEnemyDied;
        }


        private void OnDisable()
        {
            BoardEventsHandler.StartEnemyAdvancesButtonClicked -= StartEnemyAdvances;
            BoardEventsHandler.SwitchLevelButtonClicked -= SpawnLevel;
            BoardEventsHandler.EnemyDied -= OnEnemyDied;
        }
    }
}