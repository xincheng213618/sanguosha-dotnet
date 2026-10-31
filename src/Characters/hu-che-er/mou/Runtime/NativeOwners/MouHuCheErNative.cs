#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouHuCheErNative(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries> getCoreSnapshotValueProjectionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::CardGame.Core.NativeOwners.MouHuCheErFactsFactory createMouHuCheErFacts, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime) : IMouHuCheErNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public bool CanSlash(ProgramSkillFrame f) => getCharacterHuCheErMouRuntime().CanMouHuCheErSlash(f);
    public void Slash(ProgramSkillFrame f) => getCharacterHuCheErMouRuntime().IssueMouHuCheErSlash(f);
    public void Reveal(ProgramSkillFrame f)
    {
        if (!getCharacterHuCheErMouRuntime().ValidMouHuCheEr(f) || f.MouHuCheEr is not { Stage: MouHuCheErStage.DiscardChildren } r || getCardZoneStore().GetLocation(r.Shown) != CardLocation.Hand(r.Target))
            throw new InvalidOperationException("Kongwu reveal requires its actual selected Hand entity.");
        advanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, [getCoreSnapshotValueProjectionRuntime().ToSnapshot(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(r.Shown))]));
        advanceEventRulesAndQueueFact(new MouHuCheErShownEvent(f.Id, r.Target, r.Shown, r.Suit, r.Category));
    }

    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterHuCheErMouRuntime().ValidMouHuCheEr(f) || f.MouHuCheEr is not { Stage: MouHuCheErStage.DiscardChildren } r || !r.Discards.SequenceEqual(createMouHuCheErFacts().SameSuitDiscards(r.Target, r.Suit)))
            throw new InvalidOperationException("Kongwu discards require its actual payable whole same-suit Hand group.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Target, r.Discards, new(MouHuCheErRuntime.MouHuCheErDiscardReason), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var receipt = parent.MouHuCheEr!;
            if (!moves.Select(m => m.CardId).SequenceEqual(r.Discards) || moves.Any(m => m.From != CardLocation.Hand(r.Target) || m.Sequence <= receipt.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Kongwu lost its real same-suit discard entities.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouHuCheEr = receipt with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouHuCheErDiscardedEvent(f.Id, r.Target, m.CardId, m.Sequence));
        });
    }
}
