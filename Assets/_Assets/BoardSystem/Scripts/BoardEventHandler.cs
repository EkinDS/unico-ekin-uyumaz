using System;

namespace _Assets.BoardSystem
{
    public static class BoardEventsHandler
    {
        public static event Action<DefenseItem> DefenseItemHovered;
        public static event Action<DefenseItem> DefenseItemUnhovered;
        public static event Action<int> SwitchLevelButtonClicked;
        public static event Action<Enemy> EnemyDied;
        public static event Action PlayerWon;
        public static event Action PlayerFailed;
        public static event Action LevelStarted;
        public static event Action StartEnemyAdvancesButtonClicked;

        public static void RaiseHovered(DefenseItem item)
        {
            DefenseItemHovered?.Invoke(item);
        }

        public static void RaiseUnhovered(DefenseItem item)
        {
            DefenseItemUnhovered?.Invoke(item);
        }


        public static void RaisePlayerWon()
        {
            PlayerWon?.Invoke();
        }


        public static void RaisePlayerFailed()
        {
            PlayerFailed?.Invoke();
        }
    
    
        public static void RaiseLevelStarted()
        {
            LevelStarted?.Invoke();
        }


        public static void RaiseSwitchLevelButtonClicked(int levelId)
        {
            SwitchLevelButtonClicked?.Invoke(levelId);
        }


        public static void RaiseStartEnemyAdvancesButtonClicked()
        {
            StartEnemyAdvancesButtonClicked?.Invoke();
        }
    
    
        public static void RaiseEnemyDied(Enemy enemy)
        {
            EnemyDied?.Invoke(enemy);
        }
    }
}