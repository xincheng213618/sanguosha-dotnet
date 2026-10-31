#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJiangWeiAnniversaryNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterJiangWeiMouEntries> getCharacterJiangWeiMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, Func<DeterministicRandom> getDeterministicRandom, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, MouJiangWeiAnniversaryFactsFactory createMouJiangWeiAnniversaryFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, ProgramSkillHostFactory createProgramSkillHost) : IMouJiangWeiAnniversaryNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void PublishState() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void State(ProgramSkillFrame f, string state, bool value)
    {
        if (!getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { } r || state is not ("yin" or "modified"))
            throw new InvalidOperationException("Juemou state requires its original accepted branch or Fuzhan.");
        getCoreGameEngineProgramSkillChangesRuntime().SetProgramBooleanState2(f, MouJiangWeiAnniversaryRuntime.Juemou, r.JuemouInstance, state, value);
    }

    public MouJiangWeiAnniversaryCard RandomHand(ProgramSkillFrame f, IReadOnlyList<MouJiangWeiAnniversaryCard> cards)
    {
        if (!getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.Discard } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.DamageSource)))
            throw new InvalidOperationException("Juemou random Hand choice requires the actual payable hidden Hand group.");
        var before = getDeterministicRandom().State;
        var selected = cards[getDeterministicRandom().Next(cards.Count)];
        advanceEventRulesAndQueueFact(new MouJiangWeiAnniversaryHandChosenEvent(f.Id, selected.Id, before, getDeterministicRandom().State, cards.Count));
        return selected;
    }

    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.DiscardChildren, Card: { } card } r || !createMouJiangWeiAnniversaryFacts().Discardable(r.Victim, r.DamageSource).Contains(card))
            throw new InvalidOperationException("Juemou discard requires the chosen actual other-role HE entity.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Victim, [card.Id], new(MouJiangWeiAnniversaryRuntime.CostReason), (_, records) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiangWeiAnniversary!;
            if (!getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(parent) || records is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= paid.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Juemou lost its primary real foreign discard receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJiangWeiAnniversary = paid with { After = m.Sequence } });
            advanceEventRulesAndQueueFact(new MouJiangWeiAnniversaryDiscardedEvent(f.Id, paid.Victim, paid.DamageSource, card.Id, card.From, m.Sequence));
        });
    }

    public void Damage(ProgramSkillFrame f)
    {
        if (!getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.Damage } r)
            throw new InvalidOperationException("Juemou damage requires its accepted real Yin/Yang branch.");
        getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, r.Victim, 1, explicitSourceSeat: r.DamageSource);
    }

    public void Recover(ProgramSkillFrame f, int amount)
    {
        if (!getCharacterJiangWeiMouRuntime().MouJiangWeiAnniversary.Valid(f) || f.MouJiangWeiAnniversary is not { Stage: MouJiangWeiAnniversaryStage.RecoveryChildren } r || r.Requested != amount)
            throw new InvalidOperationException("Juemou/Fuzhan recovery requires its real native recovery cursor.");
        createProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
    }
}
