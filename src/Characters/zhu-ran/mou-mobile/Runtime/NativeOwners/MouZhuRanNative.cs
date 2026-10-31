#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouZhuRanNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhuRanMouMobileRuntime>> getCharacterZhuRanMouMobileRuntime, MouZhuRanFactsFactory createMouZhuRanFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<FrameStore> getFrameStore, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IMouZhuRanNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Damage(ProgramSkillFrame f, int target) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, target, 1);
    public void Discard(ProgramSkillFrame f, int payer, IReadOnlyList<MouZhuRanCard> cards)
    {
        var reason = f.MouZhuRan!.Operation == SkillProgramEffectOp.ResolveMouZhuRanZhenwei ? MouZhuRanRuntime.CostReason : MouZhuRanRuntime.HeyuanCostReason;
        if (!getCharacterZhuRanMouMobileRuntime().Value.MouZhuRan.Valid(f) || f.MouZhuRan is not { Stage: MouZhuRanStage.CostChildren } r || r.Payer != payer || !r.Paying.SequenceEqual(cards) || cards.Count == 0 || cards.Any(c => !createMouZhuRanFacts().Discardable(payer).Any(l => l.CardId == c.CardId && l.From == c.From)))
            throw new InvalidOperationException("Mou Zhu Ran's native discard must pay precisely this participant's sealed real HE materials.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, payer, cards.Select(c => c.CardId).ToArray(), new(reason), (_, records) =>
        {
            var parent = getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
            if (parent?.MouZhuRan is not { Stage: MouZhuRanStage.CostChildren } paid || !getCharacterZhuRanMouMobileRuntime().Value.MouZhuRan.Valid(parent) || paid.Payer != payer || !paid.Paying.SequenceEqual(cards) || records.Count != cards.Count || records.Any(m => !cardMovementJournalState.ReferenceToCardMovements.Contains(m) || m.Sequence <= paid.Before || m.Reason.Value != reason || !cards.Any(c => c.CardId == m.CardId && c.From == m.From) || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Zhu Ran's discard callback must seal its exact ancestor and native primary movement records.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent = parent with { MouZhuRan = paid with { After = records.Max(m => m.Sequence) } });
            foreach (var m in records)
            {
                var material = cards.Single(c => c.CardId == m.CardId);
                advanceEventRulesAndQueueFact(new MouZhuRanCostPaidEvent(f.Id, payer, m.CardId, m.From, material.Suit, m.Sequence));
            }
        });
    }
}
