#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoAngNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoAngMouEntries> getCharacterCaoAngMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact) : IMouCaoAngNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public void Use(ProgramSkillFrame f) => getCharacterCaoAngMouRuntime().IssueMouCaUse(f);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterCaoAngMouRuntime().MouCaoAng.Valid(f) || f.MouCaoAng is not { Stage: MouCaoAngStage.DiscardChildren, ActualDiscard: 0 } r || r.Equipment.Count == 0 || r.Equipment.Any(c => getCardZoneStore().GetLocation(c.Id) != c.From))
            throw new InvalidOperationException("Zhiwang requires its real current equipment group once.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Equipment.Select(c => c.Id).ToArray(), new(MouCaoAngRuntime.DiscardReason), (_, moves) =>
        {
            var current = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = current.MouCaoAng!;
            if (!getCharacterCaoAngMouRuntime().MouCaoAng.Valid(current) || moves.Count != r.Equipment.Count || moves.Any(m => !r.Equipment.Any(c => c.Id == m.CardId && c.Kind == m.CardKind && c.From == m.From) || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Zhiwang lost its actual native logical discard group.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, current with { MouCaoAng = paid with { After = moves.Max(m => m.Sequence), ActualDiscard = moves.Count } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouCaDiscardedEvent(f.Id, m.CardId, m.CardKind, m.To, m.Sequence));
        });
    }
}
