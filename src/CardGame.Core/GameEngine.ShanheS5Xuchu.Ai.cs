namespace CardGame.Core;

internal static class ShanheS5LuoyiAi
{
    private static readonly double UnknownGainFraction = Enum.GetValues<Suit>().Average(suit =>
        ProgramCardSetFilter.PriorForSuit(suit, [], [SkillProgramCardCategory.Basic], [EquipmentSlot.Weapon], [CardKind.Duel]));

    internal static double UnknownChoiceScore(PlayerSkillContext owner, ProgramAiPublicContext context)
    {
        // This is used before activating/revealing, when the branch lookahead has no
        // actual faces. Match the shared type prior and compare against the real public
        // normal-draw budget. Existing hand count gives a larger combat opportunity;
        // an empty hand still has the unknown reveal's potential attacks.
        var hp = Math.Max(0, owner.Hp);
        var expectedGain = hp * UnknownGainFraction;
        var bonusOpportunity = owner.HandCount > 0 ? 11d : 6d;
        return expectedGain * 8d + hp * bonusOpportunity - Math.Max(0, context.NormalDrawCount) * 8d;
    }
}
