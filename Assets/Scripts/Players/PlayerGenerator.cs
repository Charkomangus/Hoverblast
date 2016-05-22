using System.Collections.Generic;
using Assets.Scripts.SaveLoad;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerGenerator : MonoBehaviour
    {
        public GameObject Player1; // Empty Container
        public GameObject Player2; // Empty Container
        

        //List of all players, including destroyed or dead
        private readonly List<Player> _players = new List<Player>();
        private readonly List<Vector2> _positions = new List<Vector2>();
        private GameObject _body;
        //Primary Generator
        public void GeneratePlayers(bool pvp, int mapSize, GameObject aiTank, GameObject aiJet, GameObject player1Tank,
            GameObject player1Jet, GameObject player2Tank, GameObject player2Jet, List<CreatedCharacter> characterList,
            List<Player> originalPlayers, bool isLoaded)
        {
#region Standard Positions
            //Team 1 Position
            _positions.Add(new Vector2(2, 2));
                _positions.Add(new Vector2(mapSize/4f, 2));
                _positions.Add(new Vector2(mapSize/2f + mapSize/4f, 2));
                _positions.Add(new Vector2(mapSize - 2, 2));
                //Team 2 Position
                _positions.Add(new Vector2(2, mapSize - 2));
                _positions.Add(new Vector2(mapSize/4f, mapSize - 2));
                _positions.Add(new Vector2(mapSize/2f + mapSize/4f, mapSize - 2));
                _positions.Add(new Vector2(mapSize - 2, mapSize - 2));
#endregion
#region PVP
            if (pvp)
                {
                    for (var i = 0; i < characterList.Count/2; i++)
                    {
                        switch (characterList[i].GetAppearance())
                        {
                            case "Tank":
                                _body = player1Tank;
                                break;
                            case "Jet":
                                _body = player1Jet;
                                break;
                            default:
                                _body = player1Tank;
                                break;
                        }

                        GeneratePlayer(true, true, _body, mapSize, Player1, _positions[i], characterList[i].GetName(), 0,
                            characterList[i].GetArmor(), characterList[i].GetItem(), characterList[i].GetWeapon());

                    }
                    for (var i = characterList.Count/2; i < characterList.Count; i++)
                    {
                        switch (characterList[i].GetAppearance())
                        {
                            case "Tank":
                                _body = player2Tank;
                                break;
                            case "Jet":
                                _body = player2Jet;
                                break;
                            default:
                                _body = player2Tank;
                                break;
                        }

                        GeneratePlayer(false, true, _body, mapSize, Player2, _positions[i], characterList[i].GetName(), 1,
                            characterList[i].GetArmor(), characterList[i].GetItem(), characterList[i].GetWeapon());


                    }

                }
                #endregion
#region Campaign
                else
                {
                    for (var i = 0; i < characterList.Count; i++)
                    {
                        switch (characterList[i].GetAppearance())
                        {
                            case "Tank":
                                _body = player1Tank;
                                break;
                            case "Jet":
                                _body = player1Jet;
                                break;
                            default:
                                _body = player1Tank;
                                break;
                        }

                        GeneratePlayer(true, true, _body, mapSize, Player1, _positions[i], characterList[i].GetName(), 0,
                            characterList[i].GetArmor(), characterList[i].GetItem(), characterList[i].GetWeapon());

                    }




                    {
                        //Players Controlled by AI
                        GeneratePlayer(false, false, aiTank, mapSize, Player2, _positions[4], "AI-'Dave'", 1,
                            Armor.FromKey(ArmorKey.MagnesiumPlating), Armor.FromKey(ArmorKey.Medpack),
                            Weapon.FromKey(WeaponKey.LaserTurret));
                        GeneratePlayer(false, false, aiJet, mapSize, Player2, _positions[5], "AI-'L33T'", 1,
                            Armor.FromKey(ArmorKey.StealthSuit), Armor.FromKey(ArmorKey.Scope),
                            Weapon.FromKey(WeaponKey.SniperRifle));
                        GeneratePlayer(false, false, aiJet, mapSize, Player2, _positions[6], "AI-'Cogs'", 1,
                            Armor.FromKey(ArmorKey.ExoSkeleton), Armor.FromKey(ArmorKey.Teleportation),
                            Weapon.FromKey(WeaponKey.PlasmaSpear));
                        GeneratePlayer(false, false, aiTank, mapSize, Player2, _positions[7], "AI-'Doomsday'", 1,
                            Armor.FromKey(ArmorKey.MedicSuit), Armor.FromKey(ArmorKey.Medpack),
                            Weapon.FromKey(WeaponKey.BeamCannon));
                    }
                }
            }
        

#endregion


        /// <summary>
        /// Takes all the needed information and adds a new unit to the players list
        /// </summary>
        private void GeneratePlayer(bool rotate, bool user, Object playerPrefab, int mapSize, GameObject container, Vector2 position,
            string playername, int team, Armor armour, Armor accesory, Weapon weapon)
        {
            Player player;
            if (user)
                player =
                    ((GameObject)
                        Instantiate(playerPrefab,
                            new Vector3(position.x - Mathf.Floor(mapSize/2F), 1.5f,
                                -position.y + Mathf.Floor(mapSize/2F)), Quaternion.Euler(new Vector3())))
                        .GetComponent<UserPlayer>();
            else
                player =
                    ((GameObject)
                        Instantiate(playerPrefab,
                            new Vector3(position.x - Mathf.Floor(mapSize/2F), 1.5f,
                                -position.y + Mathf.Floor(mapSize/2F)), Quaternion.Euler(new Vector3())))
                        .GetComponent<AiPlayer>();

            if(rotate)
                player.transform.localEulerAngles = new Vector3(0, 180,0);
            player.SetPosition(position);
            player.SetTeam(team);
            player.EquipPlayer(armour, accesory, weapon);
            player.SetName(playername);
            _players.Add(player);
            player.transform.SetParent(container.transform);
        }




        #region Sets & returns
        /// <summary>
        /// Return a List of all players
        /// </summary>
        /// <returns></returns>
        public List<Player> ReturnPlayers()
        {
            return _players;
        }
        #endregion
    }
}
