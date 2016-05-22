using System.Collections.Generic;
using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.MAIN_MANAGERS
{
    /// <summary>
    /// Responsible for changing the turn
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        /// <summary>
        /// End current turn and start a new one.
        /// </summary>
        public int NextTurn(int playerIndex, List<Player> players)
        {
            if (playerIndex + 1 < players.Count)
                playerIndex++;
            else
                playerIndex = 0;

            return playerIndex;
        }
    }
}
