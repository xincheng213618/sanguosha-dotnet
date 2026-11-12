namespace CardGame.Core.EngineOwners;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandLimitCountingRuntime(
    IHandLimitExemptions _exemptions,
    RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _cards,
    RuntimeEntries.CoreGameEngineNationalSkillsEntries _skills,
    RuntimeEntries.CoreNativeSkillContextQueriesEntries _contexts,
    RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _roles,
    RuntimeEntries.CoreGameEngineWushuangEntries _mode,
    RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _history) : IHandLimitCounting
{
    private readonly HandLimitMatch _intrinsic = new(CardKinds: [CardKind.MantianGuohai]);

    public IReadOnlyList<Card> GetCountedHand(CharacterState owner)
    {
        var hand = _cards.GetHand(owner);
        if (hand.Count == 0) return [];
        var declarations = new List<HandLimitMatch> { _intrinsic };
        if (_mode.WushuangFeature?.HandLimitExemptCardKinds(owner.Seat) is { Count: > 0 } modeKinds)
            declarations.Add(new(CardKinds: modeKinds));
        PlayerSkillContext? context = null;
        bool Qualifies(IndexedSkillProgramInstance instance, SkillProgramCardPolicy policy) =>
            (policy.OwnerRole is null || _roles.HasSkillRoleQualification(owner, instance.SkillId,
                instance.SkillInstanceId, policy.OwnerRole.Value)) &&
            (policy.Condition.Kind == SkillProgramConditionKind.Always ||
                policy.Condition.Evaluate(context ??= _contexts.CreateSkillContext(owner), null));
        var shard = _skills.GetSkillBindingShard(owner);
        foreach (var (instance, policy) in shard.GetCardPolicies(SkillProgramCardPolicyKind.HandLimitCardKindExemption))
            if (Qualifies(instance, policy))
                declarations.Add(policy.HandLimitMatch ?? new(CardKinds: policy.CardKinds,
                    Face: HandLimitCardFace.LegacyHandIdentity));
        var exempt = _exemptions.GetExemptCardIds(owner.Seat, hand, declarations);
        var ignoresObtained = shard.GetCardPolicies(SkillProgramCardPolicyKind.IgnoreTurnObtainedHandCardsForDiscard)
            .Any(binding => Qualifies(binding.Source, binding.Policy));
        if (!ignoresObtained)
            return Array.AsReadOnly(hand.Where(card => !exempt.Contains(card.Id)).ToArray());
        // Fulin samples physical entries after the actual public TurnStarted
        // boundary. Leaving and reentering does not revoke that turn's benefit.
        var obtained = _history.EventsSinceLastBoundary(fact => fact is TurnStartedEvent).OfType<CardMovedEvent>()
            .Where(move => move.To == CardLocation.Hand(owner.Seat) && move.From != move.To)
            .Select(move => move.CardId).ToHashSet();
        return Array.AsReadOnly(hand.Where(card => !exempt.Contains(card.Id) && !obtained.Contains(card.Id)).ToArray());
    }
}
