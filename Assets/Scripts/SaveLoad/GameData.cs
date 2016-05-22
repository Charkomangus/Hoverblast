using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.Players;


namespace Assets.Scripts.SaveLoad
{

    


    [Serializable]
    public class GameData
    {
        public static GameData CurrentGameData;
        private List<Player> _players;
        private int _currentIndex;

        public void Update()
        {
            _players = BattleManager.Instance.ReturnPlayers();
            _currentIndex = BattleManager.Instance.CurrentPlayerIndex;
        }

    }
}
