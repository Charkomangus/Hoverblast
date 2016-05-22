using System.Collections.Generic;
using Assets.Asset_Store.BasicBeamShot.Script;
using UnityEngine;

namespace Assets.Scripts.Players
{
    //Manages the setting of the shooters
    public class ShootingManager : MonoBehaviour
    {

        public List<GameObject> ShooterRotates;
        public List<MultiShooter> _shot;
        public List<BeamParam> _shotBeamParametres;

        // Use this for initialization
        private void Start()
        {
            _shot = new List<MultiShooter>();
            _shotBeamParametres = new List<BeamParam>();
            //Set lists of scripts up
            for (int i = 0; i < ShooterRotates.Count; i++)
            {
                _shot.Add(ShooterRotates[i].GetComponentInChildren<MultiShooter>());
                _shotBeamParametres.Add(ShooterRotates[i].GetComponentInChildren<BeamParam>());
            }
                
            
            //Set initial color
            foreach (var parametre in _shotBeamParametres)
            {
                parametre.BeamColor.r = 1.1f;
                parametre.BeamColor.g = 0.1f;
                parametre.BeamColor.b = 0.1f;
            }

        }

        /// <summary>
        /// SHOOT!
        /// </summary>
        public void ShootNow(Player attacker, Player target)
        {
            var type = 0;
            var dist = Vector3.Distance(target.transform.position, attacker.transform.position);
            
          

            switch (attacker.ReturnWeapon().WeaponName)
            {
                case "Sniper Rifle":
                    SetEverything(0.1f, dist, 3, Color.blue);
                    type = 1;
                    break;
                case "Laser Turret":
                    SetEverything(1f, dist, 2, Color.red);
                    type = 1;
                    break;
                case "Beam Cannon":
                    SetEverything(0.01f, dist, 7, Color.yellow);
                    type = 1;
                    break;
                case "SRiuS":
                    SetEverything(0.2f, dist, 4, Color.magenta);
                    type = 0;
                    break;
                case "Shock Taser":
                    SetEverything(0.3f, dist, 2, Color.red);
                    type = 0;
                    break;
                case "Plasma Spear":
                    SetEverything(0.3f, dist, 1, Color.green);
                    type = 0;
                    break;
            }


            //Set initial color
            foreach (var shooter in _shot)
            {
                shooter.Shoot(type);
            }

        }

        #region GET & SETS
        /// <summary>
        /// Set everything
        /// </summary>
        public void SetEverything(float speed, float length, float scale, Color color)
        {
            //Set initial color
            foreach (var parametre in _shotBeamParametres)
            {
                parametre.Scale = scale;
                parametre.MaxLength = length;
                parametre.BeamColor = color;
                parametre.AnimationSpd = speed;
            }

        }


        /// <summary>
        /// Get shooting scale
        /// </summary>
        public float GetLength()
        {
            return _shotBeamParametres[0].MaxLength;

        }

        /// <summary>
        /// Set shooting speed
        /// </summary>
        public void SetLength(float length)
        {
            //Set initial color
            foreach (var parametre in _shotBeamParametres)
            {
                parametre.MaxLength = length;
            }
        }

        /// <summary>
        /// Get shooting scale
        /// </summary>
        public float GetScale()
        {
            return _shotBeamParametres[0].Scale;

        }

        /// <summary>
        /// Set shooting speed
        /// </summary>
        public void SetScale(float scale)
        {
            //Set initial color
            foreach (var parametre in _shotBeamParametres)
            {
                parametre.Scale = scale;
            }
        }

        /// <summary>
        /// Get shooting color
        /// </summary>
        public Color GetColor()
        {
            return _shotBeamParametres[0].BeamColor;

        }

        /// <summary>
        /// Set shooting color
        /// </summary>
        public void SetColour(float r, float g, float b)
        {
            //Set initial color
            foreach (var parametre in _shotBeamParametres)
            {
                parametre.BeamColor.r = r;
                parametre.BeamColor.g = g;
                parametre.BeamColor.b = b;
            }
        }

        /// <summary>
        /// Get shooting speed
        /// </summary>
        public float GetSpeed()
        {
            return _shotBeamParametres[0].AnimationSpd;

        }

        /// <summary>
        /// Set shooting speed
        /// </summary>
        public void SetSpeed(float speed)
        {
            //Set initial color
            foreach (var parametre in _shotBeamParametres)
            {
                parametre.AnimationSpd = speed;
            }
        }
#endregion
    }

}
