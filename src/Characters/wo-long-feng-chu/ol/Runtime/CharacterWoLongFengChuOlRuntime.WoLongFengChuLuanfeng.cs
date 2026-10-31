#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWoLongFengChuOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26101)]
    internal bool CanOfferLuanfeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveLuanfengDyingRecovery) || context is { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: { } victim } && _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.VictimSeat == victim && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _coreMatchParticipantsPlayersAccess._players[victim].Hp <= 0 && _coreMatchParticipantsPlayersAccess._players[victim].MaxHp >= _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].MaxHp;
    SkillProgramStepOutcome IWoLongFengChuProgramHost.ResolveLuanfeng(ProgramSkillFrame frame) => ResolveLuanfeng(frame);
    internal SkillProgramStepOutcome ResolveLuanfeng(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.Luanfeng is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: { } victim } || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.VictimSeat != victim || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive || _coreMatchParticipantsPlayersAccess._players[victim].Hp > 0 || _coreMatchParticipantsPlayersAccess._players[victim].MaxHp < _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp)
            throw new InvalidOperationException("Luanfeng requires the exact current dying victim and its qualifying maximum HP.");
        var receipt = new LuanfengReceipt(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, dying.FrameId, victim, LuanfengStage.RecoveryChild, Math.Min(3, _coreMatchParticipantsPlayersAccess._players[victim].MaxHp) - _coreMatchParticipantsPlayersAccess._players[victim].Hp);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Luanfeng = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuanfengIssuedEvent(frame.Id, receipt.Source, dying.FrameId, victim));
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, frame.OwnerSeat, victim, 3 - _coreMatchParticipantsPlayersAccess._players[victim].Hp, null, null);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id)
            ResumeLuanfeng(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26101)]
    internal bool ResumeLuanfeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Luanfeng: { } receipt } frame || frame.Id != id)
            return false;
        if (DrainEquipmentSlotNamedChildren(frame.Id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.Luanfeng!;
        var victim = _coreMatchParticipantsPlayersAccess._players[receipt.VictimSeat];
        if (!victim.IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Luanfeng = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (receipt.Stage == LuanfengStage.RecoveryChild)
        {
            var restored = Enum.GetValues<EquipmentSlot>().Count(slot => _coreAdvancedLifecycleOperations.RestoreEquipmentSlot(victim, slot));
            var draw = Math.Max(0, 6 - restored - _corePhysicalOwnedCardQueriesOperations.GetHand(victim).Count);
            receipt = receipt with
            {
                Stage = LuanfengStage.DrawChildren,
                RestoredSlots = restored,
                RequestedDraw = draw,
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Luanfeng = receipt, PendingMovementContinuation = draw > 0 ? new(frame.OwnerSeat, 0, null) : null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuanfengSlotsRestoredEvent(frame.Id, victim.Seat, restored, draw));
            if (draw > 0)
            {
                _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, victim.Seat, draw, null, null, SkillProgramCardSetVisibility.Private, new("program.luanfeng.draw-to"));
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { Luanfeng = receipt with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            }
        }

        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.Luanfeng!;
        if (DrainEquipmentSlotNamedChildren(frame.Id))
            return true;
        if (receipt.VictimSeat == frame.OwnerSeat)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentSlotNamedNamesResetEvent(frame.OwnerSeat, frame.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Luanfeng = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26101)]
    internal bool ReturnLuanfengMovement(ProgramSkillFrame frame) => frame.Luanfeng is not null && ResumeLuanfeng(frame.Id);
}
