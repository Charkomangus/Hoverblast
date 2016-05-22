using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.MAIN_MANAGERS;
using Assets.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Players
{
    public class AiPlayer : Player
    {

        //Setting Variables for more leggible code below
        private BattleManager _bm;
        private List<Player> _players;
        private Player _currPlayer;
        private List<List<Tile>> _map;




        public override void TurnUpdate()
        {
            _bm = BattleManager.Instance;
            _players = _bm.ReturnPlayers();
            _currPlayer = _bm.ReturnCurrentPlayer();
            _map = BattleManager.Instance.ReturnMap();


            if (PositionQueue.Count > 0)
            {
                transform.position += (PositionQueue[0] - transform.position).normalized * MoveSpeed * Time.deltaTime;

                if (!(Vector3.Distance(PositionQueue[0], transform.position) <= 0.1f)) return;
                transform.position = PositionQueue[0];
                PositionQueue.RemoveAt(0);
                if (PositionQueue.Count == 0)
                {
                    ActionPoints--;
                }
            }
            else
            {
                //Set up the possible paths that the AI player will use to move or attack
                var attacktiles = TileHighlight.FindHighlight(_map[(int)GridPosition.x][(int)GridPosition.y], AttackRange, false, false);
                var movementTiles = TileHighlight.FindHighlight(_map[(int)GridPosition.x][(int)GridPosition.y], BattleManager.Instance.ReturnMapSize() * BattleManager.Instance.ReturnMapSize(), false, true);
             

                //Attack if enemy is in range. If more than one enemy present, attack the one with lowest HP
                if (attacktiles.Any(target => _players.Any(player => player.GetType() != typeof(AiPlayer) && player.IsAlive()  && player.GridPosition == target.GridPosition)))
                {
                    //Make a list of all opponents that are within range
                    var opponentsInRange = attacktiles.Select(targetPlayer => _players.Any(player => player.GetType() != typeof(AiPlayer) && player.IsAlive() && player.GridPosition == targetPlayer.GridPosition) ? _players.First(player => player.GridPosition == targetPlayer.GridPosition) : null).ToList();
                    //Select the opponment with the least health as the target
                    var opponent = opponentsInRange.OrderBy(player => player != null ? -player.ReturnHp() : 1000).First();
                    _bm.RemoveTileHighlights();
                    _currPlayer.SetMode("Attack");
                    _bm.HighlightTiles(GridPosition, new Color(255, 0, 0, 0.6f), AttackRange, false);
                    _bm.AttackPlayer(_map[(int)opponent.GridPosition.x][(int)opponent.GridPosition.y]);
                }

                //move toward nearest opponent
                else if (Mode != "Move" && movementTiles.Any(tile => _players.Any(player => player.GetType() != typeof(AiPlayer) && player.IsAlive() && player.GridPosition == tile.GridPosition)))
                {
                    var opponentsInRange = movementTiles.Select(tile =>_players.Any(player => player.GetType() != typeof(AiPlayer) && player.IsAlive() && player.GridPosition == tile.GridPosition)? _players.First(y => y.GridPosition == tile.GridPosition)
                                    : null).ToList();



                    var opponent = opponentsInRange.OrderBy(x => x != null ? -x.ReturnHp() : 1000).ThenBy(player =>player != null ? TilePathFinder.FindPath(_map[(int)GridPosition.x][(int)GridPosition.y],
                                            _map[(int)player.GridPosition.x][(int)player.GridPosition.y]).Count()                                        : 1000)
                            .First();
                    _bm.RemoveTileHighlights();
                    _bm.ReturnCurrentPlayer().SetMode("Move");
                    _bm.HighlightTiles(GridPosition, Color.grey, BattleManager.Instance.ReturnMapSize() * BattleManager.Instance.ReturnMapSize(), false, false, true);
                  





                    var path = TilePathFinder.FindPath(_map[(int)GridPosition.x][(int)GridPosition.y], _map[(int)opponent.GridPosition.x][(int)opponent.GridPosition.y],_players.Where(player => player.GridPosition != GridPosition && player.GridPosition != opponent.GridPosition).Select(x => x.GridPosition).ToArray());

                    if (path.Any())
                    {
                        var actualMovement = TileHighlight.FindHighlight(_map[(int)GridPosition.x][(int)GridPosition.y], MovementPerActionPoint, BattleManager.Instance.ReturnPlayers().Where(player => player.GridPosition != GridPosition).Select(player => player.GridPosition).ToArray());
                        path.Reverse();


                        //If the path contains tiles move the unit to the first one
                        if (path.Any(tile => actualMovement.Contains(tile)))
                            _bm.MovePlayer(path.First(tile => actualMovement.Contains(tile)));
                        else
                        {
                            Debug.Log("No Path");
                            SetIdle();

                            
                        }
                    }
                }
              
                SetIdle();
                base.TurnUpdate();

            }

            
        }

        private void SetIdle()
        {
            if (ActionPoints <= 1 && (Mode == "Move" || Mode == "Attack"))
            {
                Mode = "Idle";

            }

        }
    }
}