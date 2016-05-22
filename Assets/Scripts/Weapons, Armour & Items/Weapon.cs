using System;

namespace Assets.Scripts
{
    public enum WeaponSlotType {
        OneHanded,
        TwoHanded
    }

    public enum WeaponKey {
        LaserTurret,
        BeamCannon,
        SniperRifle,
        SRiuS,
        

        //Melee
        Taser,
        PlasmaSpear
    }

    public class Weapon : Item {
        public WeaponSlotType Type;
        public string WeaponName;
        public string WeaponExplanation;
        public string WeaponStats;
        public static Weapon FromKey (WeaponKey key) {
            Weapon weapon;
            switch (key)
            {
                case WeaponKey.LaserTurret:
                    weapon = new Weapon()
                    {
                        Type = WeaponSlotType.OneHanded,
                        WeaponName = "Laser Turret",
                        WeaponExplanation = "A weapons platform completely separate from the main unit, " +
                                            "it only requires that it is fed targets which it will pepper with laser beams. " +
                                            "This separation enables the unit to focus on survival.",
                        WeaponStats =
                        "Pros" + Environment.NewLine +
                        "Damage: 14-22" + Environment.NewLine +
                        "Range: 5" + Environment.NewLine +
                        "95% Hit chance" + Environment.NewLine +
                        "+ 5% chance to Evade" + Environment.NewLine +
                        "Cons" + Environment.NewLine +
                        "N/A",
                        AlterAttackChance = 0.10f,
                        AlterAttackRange = 5,
                        AlterDamageReduction = 2,
                        AlterEvade = 0.05f,
                        AlterDamageBase = 4,
                        AlterDamageRollSides = 8,
                        AlterMovementPerActionPoint = 0,
                        
                    };
                    break;
                case WeaponKey.BeamCannon:
                    weapon = new Weapon()
                    {
                        Type = WeaponSlotType.TwoHanded,
                        WeaponName = "Beam Cannon",
                        WeaponExplanation = "Using enough energy to power a city block for a week the beam that this cannon produces" +
                                            " can pierce the toughest armor easily. Unfortunately the power involved makes it unstable, hard to aim and " +
                                            "produces unpredictable results.",
                        WeaponStats =
                        "Pros" + Environment.NewLine +
                        "Damage: 20-50" + Environment.NewLine +
                        "Range: 4" + Environment.NewLine +
                        "80% Hit chance" + Environment.NewLine +
                        "Cons" + Environment.NewLine +
                        "Costs two Action points to Operate" + Environment.NewLine +
                        "- 1 Movement Per Action Point" + Environment.NewLine +
                        "Low chance to Hit",
                        AlterAttackChance = -0.05f,
                        AlterAttackRange = 4,
                        AlterDamageReduction = 0,
                        AlterDamageBase = 10,
                        AlterDamageRollSides = 30,
                        AlterEvade = 0,
                        AlterMovementPerActionPoint = -1,
                        AlterActionCost = 1
                    };
                    break;
                case WeaponKey.SniperRifle:
                    weapon = new Weapon()
                    {
                        Type = WeaponSlotType.TwoHanded,
                        WeaponName = "Sniper Rifle",
                        WeaponExplanation = "Less a rifle and more of a Superconducting railgun capable of launching mass at supersonic" +
                                            " speeds over massive distances this is a truly powerful weapon that requires most of a Unit’s processing power.",
                        WeaponStats =
                        "Pros" + Environment.NewLine +
                        "Damage: 25-31" + Environment.NewLine +
                        "Range: 7" + Environment.NewLine +
                        "99% Hit chance" + Environment.NewLine +
                        "Cons" + Environment.NewLine +
                        "Costs two Action points to Operate" + Environment.NewLine +
                        "- 1 Movement Per Action Point" + Environment.NewLine +
                        "- 5% Evade chance",
                        AlterAttackChance = 0.14f,
                        AlterAttackRange = 6,
                        AlterDamageBase = 15,
                        AlterDamageRollSides = 6,
                        AlterEvade = -0.05f,
                        AlterMovementPerActionPoint = -1,
                        AlterActionCost = 1
                        
                    };
                    break;
                case WeaponKey.SRiuS:
                    weapon = new Weapon()
                    {
                        Type = WeaponSlotType.TwoHanded,
                        WeaponName = "SRiuS",
                        WeaponExplanation = "The Short Range (intelligent uni-componment) Shotgun is a device that launches globs of Ultra hot plasma upon an enemy. " +
                                            "While theretotecially it's range is huge, the plasma cools down rapidly making it no more than a nuisance in medium to long ranges.",
                        WeaponStats = 
                        "Pros" + Environment.NewLine +
                        "Damage: 10-30" + Environment.NewLine +
                        "Range: 3" + Environment.NewLine +
                        "90% Hit chance" + Environment.NewLine +
                        "Cons" + Environment.NewLine +
                        "Unpredictable Damage" + Environment.NewLine +
                        "Short Range",
                        AlterAttackChance = 0.05f,
                        AlterAttackRange = 2,
                        AlterDamageRollSides = 20
                    };
                    break;

                case WeaponKey.Taser:
                    weapon = new Weapon()
                    {
                       
                        Type = WeaponSlotType.OneHanded,
                        WeaponName = "Shock Taser",
                        WeaponExplanation = "The Taser, usually used for crowd control, has found its place in the battlefield. " +
                                            "Channelling immense electrical currents it can damage a unit and perhaps disable some of it’s systems. As " +
                                            "it does not take much processing power to operate the unit operating it can focus on defence.",
                        WeaponStats = "Pros" + Environment.NewLine +
                        "Damage: 14-18" + Environment.NewLine +
                        "Range: 1" + Environment.NewLine +
                        "100% Hit chance" + Environment.NewLine +
                        "+ 2 to Damage Reduction" + Environment.NewLine +
                        "+ 5% Evade chance" + Environment.NewLine +
                        "Can potentially disable enemy's ability" + Environment.NewLine +
                        "Cons" + Environment.NewLine +
                        "Low Damage" + Environment.NewLine +
                        "Short Range",

                        AlterAttackChance = 0.15f,
                        AlterEvade = 0.05f,
                        AlterAttackRange = 0,
                        AlterDamageReduction = 2,
                        AlterDamageBase = 4,
                        AlterDamageRollSides = 4
                    };
                    break; ;
                case WeaponKey.PlasmaSpear:
                    weapon = new Weapon()
                    {
                        Type = WeaponSlotType.TwoHanded,
                        WeaponName = "Plasma Spear",
                        WeaponExplanation = "The Plasma Spear is a containment field that is filled with Plasma on the moment of the strike. " +
                                            "While the range is extremely poor, the speed of the strike can bypass a units defences destroying critical components with ease.",
                        WeaponStats =
                        "Pros" + Environment.NewLine +
                        "Damage: 15-23" + Environment.NewLine +
                        "Range: 2" + Environment.NewLine +
                        "95% Hit chance" + Environment.NewLine +
                        "Pierces Armor" + Environment.NewLine +
                        "Cons" + Environment.NewLine +
                        "Short Range",
                        
                        AlterAttackChance = 0.10f,
                        AlterAttackRange = 1,
                        AlterDamageBase = 5,
                        AlterDamageRollSides = 8
                    };
                    break; 
                default:
                    throw new ArgumentOutOfRangeException("key", key, null);
            }
            return weapon;
        }
    }
}