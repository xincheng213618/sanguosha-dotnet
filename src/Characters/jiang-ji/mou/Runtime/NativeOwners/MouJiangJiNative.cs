#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJiangJiNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangJiMouEntries> getCharacterJiangJiMouRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, global::CardGame.Core.NativeOwners.MouJiangJiFactsFactory createMouJiangJiFacts, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime) : IMouJiangJiNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Give(ProgramSkillFrame f)
    {
        if (!getCharacterJiangJiMouRuntime().MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.GiveChildren, Card: { } card } r || getCardZoneStore().GetLocation(card.Id) != card.From)
            throw new InvalidOperationException("Shiju requires its own committed actual HE entity.");
        if (r.Giver == f.OwnerSeat && card.From == CardLocation.Hand(r.Giver))
        {
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, f with { MouJiangJi = r with { Delivered = true } });
            advanceEventRulesAndQueueFact(new MouJiangJiSelfCardCommittedEvent(f.Id, card.Id));
            return;
        }

        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(MouJiangJiRuntime.GiveReason), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiangJi!;
            if (!getCharacterJiangJiMouRuntime().MouJiangJi.Valid(parent) || moves is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Shiju lost its primary true gift receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJiangJi = paid with { Delivered = m.To == CardLocation.Hand(f.OwnerSeat), After = m.Sequence } });
            if (m.To == CardLocation.Hand(f.OwnerSeat))
                advanceEventRulesAndQueueFact(new MouJiangJiGivenEvent(f.Id, r.Giver, f.OwnerSeat, card.Id, m.Sequence));
            getCharacterHuangQuanMouRuntime().RecordNativeGivenCards(f.Id, r.Giver, f.OwnerSeat, [card.Id], r.Before);
        });
    }

    public void Use(ProgramSkillFrame f) => getCharacterJiangJiMouRuntime().IssueMouJjUse(f);
    public void Repeat(ProgramSkillFrame f) => getCharacterJiangJiMouRuntime().IssueMouJjRepeat(f);
    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterJiangJiMouRuntime().MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.DiscardChildren } r || r.Selected.Any(id => !createMouJiangJiFacts().Cards(r.Target, true).Any(c => c.Id == id)))
            throw new InvalidOperationException("Yingshi requires its actual target's selected payable HE.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Target, r.Selected, new(MouJiangJiRuntime.DiscardReason), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiangJi!;
            if (!getCharacterJiangJiMouRuntime().MouJiangJi.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Selected.Contains(m.CardId) || m.From.OwnerSeat != r.Target || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Yingshi lost its actual native logical discard.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJiangJi = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouJiangJiDiscardPaidEvent(f.Id, r.Target, m.CardId, m.Sequence));
        });
    }

    public void Disable(ProgramSkillFrame f)
    {
        if (!getCharacterJiangJiMouRuntime().MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.DiscardChildren } r)
            throw new InvalidOperationException("Yingshi cannot disable before its issued discard branch returns.");
        getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(f.OwnerSeat, MouJiangJiRuntime.Yingshi, f.SkillInstanceId, "disabled", true);
        advanceEventRulesAndQueueFact(new MouJiangJiDisabledEvent(f.Id, r.Source, r.Turn));
    }
}
