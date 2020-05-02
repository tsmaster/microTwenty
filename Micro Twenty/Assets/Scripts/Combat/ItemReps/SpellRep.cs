using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MicroTwenty
{
    public class SpellRep
    {
        public readonly string Name;
        public readonly Action<CombatUnit, CombatUnit> HitAction;
        public readonly Func<int, int, WorldRep, WorldRep> ApplyFunc;

        public bool AllowFriendly;
        public bool AllowHostile;

        public readonly SpriteId Sprite;
        public readonly Color SpriteColor;

        // TODO track mana
        // TODO verify hostile spells are targeted at enemy
        // TODO verify beneficial spells are targeted at friends

        public SpellRep (string name, SpriteId sprite, Color spriteColor, 
            bool allowFriendly, bool allowHostile,
            Action<CombatUnit, CombatUnit> hitAction,
            Func<int, int, WorldRep, WorldRep> applyFunc)
        {
            Name = name;
            Sprite = sprite;
            SpriteColor = spriteColor;
            HitAction = hitAction;
            AllowFriendly = allowFriendly;
            AllowHostile = allowHostile;
            ApplyFunc = applyFunc;
        }

        public static SpellRep MakeFireballSpell ()
        {
            return new SpellRep ("Fireball", SpriteId.SPRITE_BALL, Color.red, 
                false, true,
                (caster, target) => {
                    var dmg = Random.Range (1, 6);
                    target.currentHP -= dmg;
                },
                (casterIndex, targetIndex, before) => {
                    var after = before.IncrementHealthOfTarget (targetIndex, -3);
                    return after;
                });
        }

        public static SpellRep MakeHealSpell ()
        {
            return new SpellRep ("Heal", SpriteId.SPRITE_BALL, Color.blue, 
                true, false,
                (caster, target) => {
                    var dmg = Random.Range (1, 6);
                    target.currentHP += dmg;
                    target.currentHP = Math.Min (target.currentHP, target.maxHP);
                },
                (casterIndex, targetIndex, before) => {
                    var after = before.IncrementHealthOfTarget (targetIndex, 3);
                    return after;
                });
        }
    }
}
