using EloBuddy;
using Settings = KickassSeries.Activator.Maps.Twistedtreeline.Config.Types.DeffensiveItems;

// ReSharper disable once ConvertIfStatementToReturnStatement

namespace KickassSeries.Activator.Maps.Twistedtreeline.Items
{
    internal static class Extensions
    {
        public static bool HasCCs(this AIHeroClient target)
        {
            if (target.HasBuffOfType(BuffType.Polymorph) && Settings.Polymorphs)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Snare) && Settings.Snares)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Taunt) && Settings.Taunts)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Silence) && Settings.Silences)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Charm) && Settings.Charms)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Stun) && Settings.Stuns)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Blind) && Settings.Blinds)
            {
                return true;
            }

            if (target.HasBuffOfType(BuffType.Fear) && Settings.Fears)
            {
                return true;
            }

            if (target.HasBuff("zedulttargetmark") && Settings.ZedUlt)
            {
                return true;
            }

            if (target.HasBuff("VladimirHemoplague") && Settings.VladUlt)
            {
                return true;
            }

            if (target.HasBuff("MordekaiserChildrenOfTheGrave") && Settings.MordeUlt)
            {
                return true;
            }
            return false;
        }
    }
}
