#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouLiuXieNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterLiuXieMouEntries> getCharacterLiuXieMouRuntime, MouLiuXieFactsFactory createMouLiuXieFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<FrameStore> getFrameStore, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IMouLiuXieNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Damage(ProgramSkillFrame f, int target) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(f, target, 1);
    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterLiuXieMouRuntime().MouLiuXie.Valid(f) || f.MouLiuXie is not { Stage: MouLiuXieStage.DiscardChildren } r || r.Paying.Count == 0 || r.Paying.Any(id => !createMouLiuXieFacts().Discardable(r.Payer).Contains(id)))
            throw new InvalidOperationException("Zhanban must discard this payer's exact legal Hand entities.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Payer, r.Paying, new(MouLiuXieRuntime.DiscardReason), (_, records) =>
        {
            var parent = getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
            if (parent?.MouLiuXie is not { Stage: MouLiuXieStage.DiscardChildren } paid || !getCharacterLiuXieMouRuntime().MouLiuXie.Valid(parent) || paid.Payer != r.Payer || !paid.Paying.SequenceEqual(r.Paying) || records.Count != r.Paying.Count || records.Any(m => !cardMovementJournalState.ReferenceToCardMovements.Contains(m) || !r.Paying.Contains(m.CardId) || m.From != CardLocation.Hand(r.Payer) || m.Sequence <= paid.Before || m.Reason.Value != MouLiuXieRuntime.DiscardReason || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Zhanban's native discard lost its original parent or primary movement receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent = parent with { MouLiuXie = paid with { After = records.Max(m => m.Sequence) } });
            foreach (var moved in records)
                advanceEventRulesAndQueueFact(new MouLiuXieDiscardPaidEvent(f.Id, paid.Payer, moved.CardId, moved.Sequence, paid.IsAdjustment));
        });
    }
}
