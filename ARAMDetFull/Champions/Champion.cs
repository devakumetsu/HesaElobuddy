using SharpDX;
using EloBuddy;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    abstract class Champion
    {
        protected Champion()
        {
            Chat.Print(player.ChampionName + " plugin loaded!");
        }

        public bool safeGap(Obj_AI_Base target)
        {
            return MapControl.safeGap(target);
        }

        public bool safeGap(Vector2 position)
        {
            return MapControl.safeGap(position);
        }

        public static AIHeroClient player = ObjectManager.Player;

        public SpellBase Q = new Active(SpellSlot.Q), W = new Active(SpellSlot.W), E = new Active(SpellSlot.E), R = new Active(SpellSlot.R);

        /* Skill Use */
        public abstract void useQ(Obj_AI_Base target);
        public abstract void useW(Obj_AI_Base target);
        public abstract void useE(Obj_AI_Base target);
        public abstract void useR(Obj_AI_Base target);

        public abstract void setUpSpells();

        public abstract void useSpells();

        public virtual void escape(){ }

        public virtual void farm()
        {
            MapControl.myControler.useSpellsOnMinions();
        }
        public virtual void killSteal() { }
        public virtual void alwaysCheck() { }

        public virtual void kiteBack(Vector2 pos) { }
    }
}