using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public enum ArmorSlotType {
        Armour,
        Accesory
    }

    public enum ArmorKey {
        //Armour
        MagnesiumPlating,
        ExoSkeleton,
        MedicSuit,
        StealthSuit,
        
        //Accesories
        Teleportation,
        Scope,
        Medpack,
        ForceField
        
       
        
    }

    public class Armor : Item
    {
        public ArmorSlotType Type;
        public string ArmorName;
        public string ArmorExplanation;
        public string ArmorStats;
       

        public static Armor FromKey(ArmorKey key)
        {

            Armor armor;
            switch (key)
            {
                case ArmorKey.MagnesiumPlating:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Armour,
                        ArmorName = "Magnesium Plate",
                        ArmorExplanation =
                            "This armor is made using New Magnesium, an alloy that is light as aluminum, but as strong as titanium alloys. " +
                            "This has allowed multiple layers making an amazing defensive plate. While this material has the" +
                            " highest strength-to-weight ratio known to mankind all the layers render it extremely heavy making movement " +
                            "difficult.",
                        ArmorStats =
                            "Pros"+ Environment.NewLine +
                            "+ 8 Damage Resistance" + Environment.NewLine +
                            "Cons" + Environment.NewLine +
                            "- 5 % Hit chance" + Environment.NewLine +
                            "- 10 % Evade Chance" + Environment.NewLine +
                            "- 2 Movement Per Action Point",
                        //PROS
                        AlterDamageReduction = 6,
                        //CONS
                        AlterMovementPerActionPoint = -2,
                        AlterEvade = -0.10f,
                    };
                    break;
                case ArmorKey.ExoSkeleton:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Armour,
                        ArmorName = "Exo-Skeleton",
                        ArmorExplanation =
                            "A web of flexible yet durable alloys surround this unit. Possessing its own set of smart boosters, " +
                            "it can enable a unit to move much faster and also put much more force if it's using a melee weapon. " +
                            "Due to the stabilization provided by the skeleton the unit can increase both its range and chance to hit.",
                        ArmorStats =
                            "Pros" + Environment.NewLine +
                            "+ 5 Melee Damage" +  Environment.NewLine +
                            "+ 1 Attack Range" + Environment.NewLine +
                            "+ 5 % Hit chance" + Environment.NewLine +
                            "+ 1 Damage Resistance" + Environment.NewLine +
                            "+ 2 Movement Per Action Point" + Environment.NewLine +
                            "Cons" + Environment.NewLine +
                            "N/A",
                        AlterMeleeDamage = 5,
                        AlterAttackRange = 1,
                        AlterDamageReduction = 1,
                        AlterMovementPerActionPoint = 2,
                    };
                    break;
                case ArmorKey.MedicSuit:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Armour,
                        ArmorName = "Medic Suit",
                        ArmorExplanation =
                            "This highly advanced suit uses nanites to regulate and self-repair any damaged components. " +
                            "The metal plating, while not the best out there provides some degree of protection. " +
                            "The technology is still experimental and it is found that interference from the " +
                            "nanites might cause problems with the onboard targeting system. Additionally, it's quite bulky " +
                            "slowing the unit down.",
                        ArmorStats =
                            "Pros" + Environment.NewLine +
                            "Each turn will heal up to 5 health points if the unit is damaged at the end of its turn." + Environment.NewLine +
                            "+2 Damage Resistance" + Environment.NewLine +
                            "Cons" + Environment.NewLine +
                            "- 5 % Hit chance" + Environment.NewLine +
                            "- 1 Movement Per Action Point",
                        //PROS
                        AlterDamageReduction = 2,
                        //CONS
                        AlterMovementPerActionPoint = -1,
                        AlterEvade = -0.05f
                    };
                    break;
                    ;
                case ArmorKey.StealthSuit:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Armour,
                        ArmorName = "Stealth Suit",
                        ArmorExplanation =
                            "Smart materials scan their surrounding and blend the unit with its environment. " +
                            "In addition, it possesses radar and satellite blockers making tracing or hitting the wearer a very " +
                            "arduous task. Because of the nature of the camouflage the unit is required to move relatively slowly to " +
                            "allow the material to properly adapt.",
                        ArmorStats =
                            "Pros" + Environment.NewLine +
                            "+ 2 Melee Damage" + Environment.NewLine +
                            "+ 10 % Evade Chance" + Environment.NewLine +
                            "+ 1 Damage Reduction" + Environment.NewLine +
                            "Cons" + Environment.NewLine +
                            "- 1 Movement Per Action Point",
                        //PROS
                        AlterMeleeDamage = 2,
                        AlterEvade = 0.10f,
                        AlterDamageReduction = 1,
                        //CONS
                        AlterMovementPerActionPoint = -1,
                    };
                    break;
                    ;
                case ArmorKey.Teleportation:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Accesory,
                        ArmorName = "Teleportation",
                        ArmorExplanation = "Using little understood quantum events the unit " +
                                           "can divert its processing power to this device enabling it to " +
                                           "jump to a nearby location through foes and obstacles alike. " +
                                           "Even while passive it makes the carrier harder to hit by subtly shifting it.",
                        ArmorStats = "Pros" + Environment.NewLine +
                                     "+ 2 % Evade Chance" + Environment.NewLine +
                                     "Cons" + Environment.NewLine +
                                     "N/A",
                        AlterEvade = 0.05f

                    };
                    break;
                    
                case ArmorKey.Scope:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Accesory,
                        ArmorName = "Scope",
                        ArmorExplanation = "Using a sequence of factory made lenses and an extremely powerful " +
                                           "processor this scope is able to enhance a unit’s weapon range and accuracy. Of course trying to " +
                                           "use it with a melee weapon might not be the smartest idea.",
                        ArmorStats = "Pros" + Environment.NewLine +
                                     "+10 % Hit chance" + Environment.NewLine +
                                     "+ 2 Attack Range" + Environment.NewLine +
                                     "Cons" + Environment.NewLine +
                                     "- 5 Melee Damage",
                        

                    };
                    break;
                    
                case ArmorKey.Medpack:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Accesory,
                        ArmorName = "Medpack",
                        ArmorExplanation = "A small factory of nanites that manufactures packages capable of " +
                                           "repairing multiple units at once. It can also regenerate itself so it can be used multiple times.",
                        ArmorStats = "Pros" + Environment.NewLine +
                                     "Heals up to 30 health of the unit using it and 20 of all the surrounding friendly units." + Environment.NewLine +
                                      "1 + Damage Reduction" + Environment.NewLine +
                                     "Cons" + Environment.NewLine +
                                     "N/A",
                        AlterDamageReduction = 1
                    };
                    break;
                    
                case ArmorKey.ForceField:
                    armor = new Armor()
                    {
                        Type = ArmorSlotType.Accesory,
                        ArmorName = "Force Field",
                        ArmorExplanation =
                            "Using Advanced prediction algorithms this barrier can neutralize any sub-light speed attack " +
                            "before it reaches the target. It can overheat after a certain amount of damage.",
                        ArmorStats =
                            "Pros" + Environment.NewLine +
                            "Protects Unit until the Force Field is destroyed" + Environment.NewLine +
                            "1 + Damage Reduction" + Environment.NewLine +
                            "Cons" + Environment.NewLine +
                            "N / A",
                        AlterDamageReduction = 1,

                    };
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException("key", key, null);
            }
            return armor;
        }


    }

}
