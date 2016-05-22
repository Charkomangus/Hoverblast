using Assets.Scripts.MAIN_MANAGERS;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    
    public class Actions : MonoBehaviour {

        public Text SpecialAbilityName;
        public Text SpecialAbilityExplanation;
        public Text SpecialAbilityStats;
        public Image SpecialAbilityImage;
        public Sprite BarrierImage;
        public Sprite ScopeImage;
        public Sprite MedpackImage;
        public Sprite TeleportationImage;
        public Sprite DefaultImage;
        /// <summary>
        /// Set player to attack mode if they are not else do nothing
        /// </summary>
        public void Attack()
        {
            var bm = BattleManager.Instance;
            var player = bm.ReturnCurrentPlayer();

            if (player.ReturnMode() != "Attack")
            {
                bm.RemoveTileHighlights();
                player.SetMode("Attack");
                bm.HighlightTiles(player.ReturnPosition(), new Color(1f, 0.3f, 0.3f, 1), player.ReturnAttackRange(), false);
            }
        else
            {
                player.SetMode("Idle");
                bm.RemoveTileHighlights();
            }
        }

        /// <summary>
        /// Set player to move mode if they are not else do nothing
        /// </summary>
        public void Move()
        {
            var bm = BattleManager.Instance;
            var player = bm.ReturnCurrentPlayer();

            if (player.ReturnMode() != "Move")
            {
                bm.RemoveTileHighlights();
                player.SetMode("Move");
                bm.HighlightTiles(player.ReturnPosition(), Color.grey, player.ReturnMovement(), false, false,true);
            }
            else
            {
                player.SetMode("Idle");
                bm.RemoveTileHighlights();
            }
        }

        /// <summary>
        /// Activate accesory item
        /// </summary>
        public void SpecialAbility()
        {
            var bm = BattleManager.Instance;
            var player = BattleManager.Instance.ReturnCurrentPlayer();


           
            //Sets proper image depending on special ability
            switch (player.ReturnAccesory().ArmorName)
            {
                case "Teleportation":
                    SpecialAbilityImage.sprite = TeleportationImage;
                    if (player.ReturnMode() != "Teleportation")
                    {
                        bm.RemoveTileHighlights();
                        player.SetMode("Teleport");
                        bm.HighlightTiles(player.ReturnPosition(), new Color(0.3f, 0.3f, 1f, 1), 12, false, false, false);
                    }
                    else
                    {
                        player.SetMode("Idle");
                        bm.RemoveTileHighlights();
                    }
                    break;
                case "Medpack":
                    SpecialAbilityImage.sprite = MedpackImage;
                    if (player.ReturnMode() != "Medpack")
                    {
                        bm.RemoveTileHighlights();
                        player.SetMode("Medpack");
                        bm.HighlightTiles(player.ReturnPosition(), new Color(0.3f, 0.3f, 1f, 1), 0, true, true, true);
                    }
                    else
                    {
                        player.SetMode("Idle");
                        bm.RemoveTileHighlights();
                    }
                    break;
                case "Scope":
                    SpecialAbilityImage.sprite = ScopeImage;
                    if (player.ReturnMode() != "ScopeImage")
                    {
                        bm.RemoveTileHighlights();
                        player.SetMode("Scope");
                        bm.HighlightTiles(player.ReturnPosition(), new Color(1f, 0.2f, 0.2f, 1), player.ReturnAttackRange() + 2, false);
                    }
                    else
                    {
                        player.SetMode("Idle");
                        bm.RemoveTileHighlights();
                    }
                   
                    break;
                case "Force Field":
                    SpecialAbilityImage.sprite = BarrierImage;
                    if (player.ReturnMode() != "Force Field")
                    {
                        bm.RemoveTileHighlights();
                        player.SetMode("Force Field");
                        bm.HighlightTiles(player.ReturnPosition(), new Color(0.3f, 0.3f, 1f, 1), 0, true, true, true);
                    }
                    else
                    {
                        player.SetMode("Idle");
                        bm.RemoveTileHighlights();
                    }
                    break;
                default:
                    SpecialAbilityImage.sprite = DefaultImage;
                    break;
            }
        }

        /// <summary>
        /// Activate accesory item
        /// </summary>
        public void SetSpecial()
        {
            var player = BattleManager.Instance.ReturnCurrentPlayer();
            //Sets proper text depending on special ability
            SpecialAbilityName.text = player.ReturnAccesory().ArmorName;
            SpecialAbilityExplanation.text = player.ReturnAccesory().ArmorExplanation;
            SpecialAbilityStats.text = player.ReturnAccesory().ArmorStats;
            //Sets proper image depending on special ability
            switch (player.ReturnAccesory().ArmorName)
            {
                case "Teleportation":
                    SpecialAbilityImage.sprite = TeleportationImage;
                    break;
                case "Medpack":
                    SpecialAbilityImage.sprite = MedpackImage;
                    break;
                case "Scope":
                    SpecialAbilityImage.sprite = ScopeImage;
                    break;
                case "Force Field":
                    SpecialAbilityImage.sprite = BarrierImage;
                    break;
                default:
                    SpecialAbilityImage.sprite = DefaultImage;
                    break;
            }
        }

        /// <summary>
        /// End Players turn and reset their atributes
        /// </summary>
        public void SkipTurn()
        {
            var bm = BattleManager.Instance;
            var player = bm.ReturnCurrentPlayer();
            bm.RemoveTileHighlights();
            player.SetActionPoints(0);
        }
    }
}


		
           
		
     