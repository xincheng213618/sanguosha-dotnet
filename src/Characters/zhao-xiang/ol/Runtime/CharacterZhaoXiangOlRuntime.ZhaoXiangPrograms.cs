#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoXiangOlRuntime
{
    internal const string ZhaoXiangDrawReason = "program.zhao-xiang.conversion-draw";
    internal bool DrainZhaoXiangChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 22601)]
    internal bool ResumeZhaoXiang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhaoXiang: not null } f || f.Id != id)
            return false;
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnZhaoXiangMovement(f);
            return true;
        }

        if (DrainZhaoXiangChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.ZhaoXiang!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishZhaoXiang(f);
            return true;
        }

        if (r.Stage == "selection")
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishZhaoXiangExchange(f);
            return true;
        }

        if (r.Stage == "replacement-children")
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var request = owner.HasHpValue ? Math.Min(1, Math.Max(0, owner.MaxHp - owner.Hp)) : 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoXiang = r with { Stage = "recovery-children", RecoveryRequested = request } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoXiangExchangeRecoveryIssuedEvent(f.Id, f.OwnerSeat, request));
            if (request > 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, request, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (r.Stage == "recovery-children")
        {
            FinishZhaoXiang(f);
            return true;
        }

        throw new InvalidOperationException("Zhao Xiang resumed an unrelated native stage.");
    }

    internal SkillProgramEffect? ZhaoXiangPausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool IsZhaoXiangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.DrawAfterPaidMeiYingConversion && f.ZhaoXiang is { Stage: "movement", ActionId: not null } && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Movement, 22602)]
    internal bool ReturnZhaoXiangMovement(ProgramSkillFrame f)
    {
        if (f.ZhaoXiang is null)
            return false;
        if (f.PendingMovementContinuation is not { } p || !IsZhaoXiangMovement(f, ZhaoXiangPausedEffect(f), p))
            throw new InvalidOperationException("MeiYing draw consumed a foreign movement return.");
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd() || _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        FinishZhaoXiang(f);
        return true;
    }

    internal void FinishZhaoXiang(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoXiang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool ValidZhaoXiang(ProgramSkillFrame f)
    {
        if (f.ZhaoXiang is not { } r || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || r.Source != new CardConversionSource(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.ParentFrameId != f.WindowContext?.ParentFrameId || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            return false;
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (r.ActionId is { } actionId)
        {
            if (r.Stage != "movement" || ZhaoXiangPausedEffect(f)?.Op != SkillProgramEffectOp.DrawAfterPaidMeiYingConversion || ZhaoXiangActionPayment(f)?.ActionId != actionId || r.Before > r.After)
                return false;
            return events.OfType<ZhaoXiangConversionDrawPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid.ActionId == actionId && paid.OwnerSeat == f.OwnerSeat && paid.Before == r.Before && paid.After == r.After && paid.Actual == r.ActualDraw && r.ActualDraw is >= 0 and <= 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhaoXiangDrawReason) == r.ActualDraw;
        }

        if (!ZhaoXiangPreparationParentMatches(f) || ZhaoXiangPausedEffect(f)?.Op != SkillProgramEffectOp.ExchangeFiveShuBySpentMeiYing || r.Generals.Count != 5 || r.Generals.Distinct(StringComparer.Ordinal).Count() != 5 || r.Generals.Any(id => !_contentRegistryOperations._contentRegistry.Generals.TryGetValue(id, out var general) || general.FactionId != "shu" || id == r.OriginalGeneralId) || r.Stage is not ("selection" or "replacement-children" or "recovery-children"))
            return false;
        if (events.OfType<ZhaoXiangExchangePaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var cost] || cost != new ZhaoXiangExchangePaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.Turn, r.Removed, r.CumulativeRemoved, ZhaoXiangPoolFingerprint(r.Generals)) || r.Removed < 0 || r.CumulativeRemoved < r.Removed || ZhaoXiangCumulativeRemoved(f.OwnerSeat) != r.CumulativeRemoved)
            return false;
        var selected = events.OfType<ZhaoXiangExchangeSelectedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == "selection")
            return r.SelectedGeneralId is null && selected.Length == 0;
        if (r.SelectedGeneralId is not { } chosen || !r.Generals.Contains(chosen) || r.MaximumAfter != Math.Clamp(r.CumulativeRemoved, 2, 8) || selected is not [var choice] || choice != new ZhaoXiangExchangeSelectedEvent(f.Id, f.OwnerSeat, r.OriginalGeneralId, chosen, r.MaximumBefore, r.MaximumAfter))
            return false;
        var recovery = events.OfType<ZhaoXiangExchangeRecoveryIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return r.Stage == "replacement-children" ? recovery.Length == 0 : recovery is [var request] && request.OwnerSeat == f.OwnerSeat && request.Requested == r.RecoveryRequested && r.RecoveryRequested is >= 0 and <= 1;
    }

    void IZhaoXiangProgramHost.GainMeiYingAfterActualSlashDamage(ProgramSkillFrame f) => GainMeiYingAfterActualSlashDamage(f);
    SkillProgramStepOutcome IZhaoXiangProgramHost.ExchangeFiveShuBySpentMeiYing(ProgramSkillFrame f) => ExchangeFiveShuBySpentMeiYing(f);
    SkillProgramStepOutcome IZhaoXiangProgramHost.DrawAfterPaidMeiYingConversion(ProgramSkillFrame f) => DrawAfterPaidMeiYingConversion(f);
}
