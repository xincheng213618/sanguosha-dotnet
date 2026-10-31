#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDongChengNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CharacterDongChengMouEntries> getCharacterDongChengMouRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, MouDongChengFactsFactory createMouDongChengFacts, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, MouDcLightningFactsFactory createMouDcLightningFacts, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IMouDongChengNative
{
    public void Finish(ProgramSkillFrame f)
    {
        getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
        if (f.MouDongChengSettlement is { } s)
            getCharacterDongChengMouRuntime().MouDongCheng.DelayedSettlementReturned(s);
    }

    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Link(ProgramSkillFrame f, int other)
    {
        if (!getCharacterDongChengMouRuntime().MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.Opening } r || other == f.OwnerSeat || !matchParticipantsState.ReferenceToPlayers[other].IsAlive)
            throw new InvalidOperationException("Baojia requires one actual other living opening role.");
        var owner = matchParticipantsState.ReferenceToPlayers[f.OwnerSeat];
        owner.MouDongChengLinks = Array.AsReadOnly((owner.MouDongChengLinks ?? []).Where(l => l.Source.SkillInstanceId != f.SkillInstanceId).Append(new(r.Source, other)).ToArray());
        advanceEventRulesAndQueueFact(new MouDcLinkedEvent(f.Id, r.Source, other));
    }

    public void Protect(ProgramSkillFrame f) => getCharacterDongChengMouRuntime().MouDcProtect(f);
    public void Claim(ProgramSkillFrame f) => getCharacterDongChengMouRuntime().MouDcClaim(f);
    public void Use(ProgramSkillFrame f) => getCharacterDongChengMouRuntime().IssueMouDcUse(f);
    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterDongChengMouRuntime().MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.CostChildren } r || !createMouDongChengFacts().Costs(f.OwnerSeat).Any(c => c.Id == r.CardId && c.Kind == r.Kind))
            throw new InvalidOperationException("Douwei requires its exact real discardable damage-card cost.");
        var from = getCardZoneStore().GetLocation(r.CardId);
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, f.OwnerSeat, [r.CardId], new("skill-program.mou:douwei.cost"), (_, moves) =>
        {
            if (moves is not [var m] || m.CardId != r.CardId || m.From != from || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Douwei's real logical discard invoice changed.");
            var own = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, own with { MouDongCheng = own.MouDongCheng!with { Actual = 1, After = m.Sequence } });
            advanceEventRulesAndQueueFact(new MouDcMovementPaidEvent(f.Id, r.CardId, 1, r.Before, m.Sequence));
        });
    }

    public void Recover(ProgramSkillFrame f)
    {
        if (!getCharacterDongChengMouRuntime().MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.RecoveryChildren, Returned: true, Dying.Count: 1 } r || (f.MouDongChengSettlement is { Reward: true } s ? !createMouDcLightningFacts().SettlementParent(f) || s.DyingId != r.Dying[0] : r.Native is not { } n || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouDcDyingEvent>().Count(x => x.FrameId == f.Id && x.UseId == n.UseId) != 1))
            throw new InvalidOperationException("Douwei's once-per-whole-use reward needs its actual first causal dying return.");
        createProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
    }

    public void Restore(ProgramSkillFrame f)
    {
        if (!getCharacterDongChengMouRuntime().MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.RestoreChildren, Slot: { } slot } || !getCoreGameEngineAdvancedLifecycleRuntime().RestoreEquipmentSlot(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], slot))
            throw new InvalidOperationException("Douwei requires one actual abolished equipment slot.");
        advanceEventRulesAndQueueFact(new MouDcRestoredEvent(f.Id, slot));
    }

    public void Disable(ProgramSkillFrame f)
    {
        if (!getCharacterDongChengMouRuntime().MouDongCheng.Valid(f) || f.MouDongCheng is not { Stage: MouDongChengStage.NativeUse, Dying.Count: 1 })
            throw new InvalidOperationException("Only the first real causal dying may disable Douwei.");
        matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].MouDongChengDisabledTurn = actualTurnProgressState.ReferenceToTurnNumber;
    }
}
