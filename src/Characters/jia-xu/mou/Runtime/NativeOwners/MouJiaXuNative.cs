#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJiaXuNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CharacterJiaXuMouEntries> getCharacterJiaXuMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineCharacterHostEntries> getCoreGameEngineCharacterHostRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreSnapshotValueProjectionEntries> getCoreSnapshotValueProjectionRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, MouJiaXuFactsFactory createMouJiaXuFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime) : IMouJiaXuNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void State(ProgramSkillFrame f, bool yin)
    {
        if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(f))
            throw new InvalidOperationException("Fumou's state requires its own paid source and frozen instance.");
        getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(f.OwnerSeat, MouJiaXuRuntime.Fumou, f.MouJiaXu!.FumouInstance, "yin", yin);
    }

    public void Grant(ProgramSkillFrame f)
    {
        if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.GrantChildren, Record: { } record } || record.FrameId != f.Id || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouJiaXuDerivedGrantedEvent>().Any(g => g.FrameId == f.Id))
            throw new InvalidOperationException("Sushen must record real current values before actually granting derived Rushi once.");
        getCoreGameEngineCharacterHostRuntime().AcquireRuntimeSkills(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], $"sushen:{f.OwnerSeat}:{f.SkillInstanceId}:{f.Id}", [MouJiaXuRuntime.Rushi]);
        advanceEventRulesAndQueueFact(new MouJiaXuDerivedGrantedEvent(f.Id, f.OwnerSeat));
    }

    public void Reveal(ProgramSkillFrame f)
    {
        if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.Use or MouJiaXuStage.Recipient } r || r.Frozen.Count > (r.View.Count + 1) / 2 || r.Frozen.Any(id => !r.View.Any(c => c.Id == id) || getCardZoneStore().GetLocation(id) != CardLocation.Hand(r.Holder)))
            throw new InvalidOperationException("Fumou may reveal only the exact chosen half of its privately viewed real Hand.");
        if (r.Frozen.Count > 0)
            advanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.ActivationId, f.OwnerSeat, "fumou-shown", Array.AsReadOnly(r.Frozen.Select(id => getCoreSnapshotValueProjectionRuntime().ToSnapshot(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id))).ToArray())));
        foreach (var id in r.Frozen)
            advanceEventRulesAndQueueFact(new MouJiaXuShownEvent(f.Id, r.Holder, id));
    }

    public void Transfer(ProgramSkillFrame f)
    {
        if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.TransferChildren, Yin: false } r || r.Recipient == f.OwnerSeat || r.Recipient == r.Holder || !matchParticipantsState.ReferenceToPlayers[r.Recipient].IsAlive)
            throw new InvalidOperationException("Fumou Yang needs its selected third living recipient and exact shown entities.");
        var ids = r.Frozen.Where(id => getCardZoneStore().GetLocation(id) == CardLocation.Hand(r.Holder)).ToArray();
        if (ids.Length == 0)
            return;
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(r.Recipient), new(MouJiaXuRuntime.TransferReason), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiaXu!;
            if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(parent) || moves.Count != ids.Length || moves.Any(m => !ids.Contains(m.CardId) || m.From != CardLocation.Hand(r.Holder) || m.Sequence <= r.Before || m.To != CardLocation.Hand(r.Recipient) && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Fumou lost the primary actual shown-card obtain receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJiaXu = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves.Where(m => m.To == CardLocation.Hand(r.Recipient)))
                advanceEventRulesAndQueueFact(new MouJiaXuTransferredEvent(f.Id, r.Holder, r.Recipient, m.CardId, m.Sequence));
        });
    }

    public void Use(ProgramSkillFrame f) => getCharacterJiaXuMouRuntime().IssueMouJxUse(f);
    public void SetHealth(ProgramSkillFrame f)
    {
        if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(f) || f.MouJiaXu is not { Operation: SkillProgramEffectOp.RestoreMouJiaXuRushi, Stage: MouJiaXuStage.HealthChildren, Record: { } record } r || matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].Hp != r.HpBefore || r.HpAfter != Math.Min(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat].MaxHp, record.Hp))
            throw new InvalidOperationException("Rushi requires the actual recorded HP and current native maximum.");
        var owner = matchParticipantsState.ReferenceToPlayers[f.OwnerSeat];
        owner.Hp = r.HpAfter;
        var change = getCoreGameEngineHpChangeProgramsRuntime().RecordHpChange(f.Id, f.OwnerSeat, f.OwnerSeat, r.HpBefore, r.HpAfter, HpChangeKind.Setting);
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, f with { MouJiaXu = r with { HpChangeId = change?.Id } });
        advanceEventRulesAndQueueFact(new MouJiaXuHpSetEvent(f.Id, record.FrameId, r.HpBefore, r.HpAfter, change?.Id));
    }

    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(f) || f.MouJiaXu is not { Stage: MouJiaXuStage.DiscardChildren, Record: not null } r || r.Selected.Any(id => !createMouJiaXuFacts().Hand(f.OwnerSeat, true).Any(c => c.Id == id)))
            throw new InvalidOperationException("Rushi requires its selected real discardable Hand entities.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(MouJiaXuRuntime.DiscardReason), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiaXu!;
            if (!getCharacterJiaXuMouRuntime().MouJiaXu.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Selected.Contains(m.CardId) || m.From != CardLocation.Hand(f.OwnerSeat) || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Rushi lost its actual logical Hand discard.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJiaXu = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouJiaXuDiscardPaidEvent(f.Id, m.CardId, m.Sequence));
        });
    }
}
