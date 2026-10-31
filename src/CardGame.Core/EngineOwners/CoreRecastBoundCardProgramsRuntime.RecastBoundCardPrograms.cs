#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreRecastBoundCardProgramsRuntime
{
    SkillProgramStepOutcome IRecastBoundCardProgramEffectHost.RecastBoundCard(ProgramSkillFrame frame, string sourceBind) => BeginProgramBoundCardRecast(frame.Id, sourceBind);
    internal SkillProgramStepOutcome BeginProgramBoundCardRecast(long frameId, string sourceBind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.BoundCardRecast is not null)
            throw new InvalidOperationException("A bound-card recast cannot repay its completed cost.");
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(frame, sourceBind);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || source.CardIds.Count != 1 || source.SourceLocations.Count != 1 || source.SourceLocations[0].OwnerSeat != owner.Seat || source.SourceLocations[0].Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(source.CardIds[0]) != source.SourceLocations[0])
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "重铸牌或技能来源已失效，未支付成本。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(source.SourceLocations[0]).Single(c => c.Id == source.CardIds[0]);
        frame = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(frame, owner.Seat, [card.Id]);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardRecast = new(frame.InstructionIndex, sourceBind, card.Id, card.Kind, source.SourceLocations[0]) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, source.SourceLocations[0], CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        // The paid instruction yields to a fresh dispatcher. Its exact program
        // return owns both cost and reward observers before the executor resumes.
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 780)]
    internal bool ResumeProgramBoundCardRecast(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.BoundCardRecast is not { } receipt)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        if (!receipt.DrawApplied)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardRecast = receipt with { DrawApplied = true } });
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var drawn = _coreMatchOutcomeWinnerAccess._winner == Winner.None && owner.IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) ? _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, CardMoveReasons.RecastDraw) : Array.Empty<int>();
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardRecast = frame.BoundCardRecast!with { DrawCount = drawn.Count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(owner.Seat, receipt.CardId, receipt.CardKind, drawn.Count));
            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), drawn.Count);
            return ResumeProgramBoundCardRecast(frame.Id);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardRecast = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "游戏已结束，重铸后续收益已取消。");
            return true;
        }

        return false;
    }
}
