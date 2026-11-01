namespace CardGame.Core;

internal static class CardSuitRewrites
{
    internal static Suit Apply(Suit suit, IReadOnlyList<SkillProgramCardPolicy> policies)
    {
        Validate(policies);
        return ApplyCore(suit, policies);
    }

    internal static int CountEquipmentSuits(IEnumerable<Card> cards, IReadOnlyList<SkillProgramCardPolicy> policies)
    {
        Validate(policies);
        return cards.Select(card => ApplyCore(card.Suit, policies)).Where(suit => suit != Suit.None).Distinct().Count();
    }

    private static Suit ApplyCore(Suit suit, IReadOnlyList<SkillProgramCardPolicy> policies)
    {
        foreach (var policy in policies)
            if (policy.CardKinds.Count == 0 && policy.RequiredCardKinds.Count == 0 && policy.InputSuit == suit)
                suit = policy.OutputSuit!.Value;
        return suit;
    }

    private static void Validate(IReadOnlyList<SkillProgramCardPolicy> policies)
    {
        foreach (var policy in policies)
            if (policy.Condition.Kind != SkillProgramConditionKind.Always || policy.OwnerRole is not null)
                throw new InvalidOperationException("Equipment suit qualification does not support context-dependent suit rewrites.");
    }
}
