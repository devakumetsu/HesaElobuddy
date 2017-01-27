using System;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Activator
{
    internal class EventsManager
    {
        public static bool CanAACancel;
        public static bool AAingEnemy;
        public static bool SpellAA;

        public static void Initialize()
        {
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.ApplyAttackEffect)
            {
                SpellAA = true;
                Core.DelayAction(() => SpellAA = false, 50);
            }
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (!target.IsEnemy) return;
            AAingEnemy = true;
            Core.DelayAction(() => AAingEnemy = false, 50);
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            CanAACancel = true;
            Core.DelayAction(() => CanAACancel =false, 50);
        }
    }
}
