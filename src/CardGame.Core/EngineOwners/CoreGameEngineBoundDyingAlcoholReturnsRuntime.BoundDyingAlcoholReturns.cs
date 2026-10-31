#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBoundDyingAlcoholReturnsRuntime
{
    internal void IssueBoundDyingAlcoholReturn(long useId, ProgramSkillFrame program, DyingFrame dying, Card card, CardLocation from, CardMoveReason reason)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Bound Alcohol lost its issued native use.");
        if (use.BoundDyingAlcoholUse is not null || use.Action is not { } action)
            throw new InvalidOperationException("Bound Alcohol cannot issue a second native return.");
        var token = new BoundDyingAlcoholUseReceipt(program.Id, program.InstructionIndex, new(program.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId), program.GameplayHash, dying.Id, dying.VictimSeat, useId, action.ActionId, card.Id, from, reason, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { BoundDyingAlcoholUse = token });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBoundDyingAlcoholIssuedEvent(token));
    }

    internal void RecordBoundDyingAlcoholPayment(long useId, int recovered, long? recoveryAttemptId = null)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Bound Alcohol payment lost its native use.");
        var token = use.BoundDyingAlcoholUse ?? throw new InvalidOperationException("Bound Alcohol payment has no owned return.");
        RequireBoundDyingAlcoholUse(use, requirePayment: false, requireFinished: false);
        if (use.BoundDyingAlcoholPayment is not null || recovered < 0)
            throw new InvalidOperationException("Bound Alcohol recovery was already paid or has an invalid actual amount.");
        var payment = new BoundDyingAlcoholPaymentReceipt(recoveryAttemptId, recovered, _coreMatchParticipantsPlayersAccess._players[token.VictimSeat].Hp);
        _coreRuntimeOperations.ReplaceRuntimeFrame(useId, use with { BoundDyingAlcoholPayment = payment });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBoundDyingAlcoholPaidEvent(useId, token.ProgramFrameId, payment));
        // This existing fact describes actual recovery payment. It precedes
        // completion observers, while the use token owns their eventual return.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDyingRescueEvent(token.DyingFrameId, token.Source.SkillId, token.Source.OwnerSeat, token.VictimSeat, token.CardId, recovered, payment.RemainingHp));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(token.Source.SkillId);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[token.Source.OwnerSeat].Name)} 发动【{(skill.Name)}】，令 {(_coreMatchParticipantsPlayersAccess._players[token.VictimSeat].Name)} 视为使用【酒】并回复{(recovered)}点体力。", token.Source.OwnerSeat, token.VictimSeat);
    }

    internal void RequireBoundDyingAlcoholUse(CardUseFrame use, bool requirePayment, bool requireFinished)
    {
        if (use.BoundDyingAlcoholUse is not { } t)
            throw new InvalidOperationException("Bound Alcohol has no native return token.");
        var program = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == t.ProgramFrameId);
        var dying = _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(f => f.Id == t.DyingFrameId);
        var programIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == t.ProgramFrameId);
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (program is null || dying is null || program.InstructionIndex != t.InstructionIndex || programIndex < 1 || _resolutionStackOperations._resolutionStack[programIndex - 1].Id != dying.Id || (useIndex >= 0 ? useIndex != programIndex + 1 : !requireFinished || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != program.Id) || t.Source != new CardConversionSource(program.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId) || t.GameplayHash != program.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program is not { } definition || definition.GameplayHash != t.GameplayHash || program.WindowContext is not { Window: SkillProgramTriggerWindow.DyingResponse } context || context.ParentFrameId != dying.Id || context.TargetSeat != t.VictimSeat || context.OwnerSeat != program.OwnerSeat || dying.VictimSeat != t.VictimSeat || dying.ResponderSeat != program.OwnerSeat || t.InstructionIndex < 1 || t.CardUseFrameId != use.Id || use.CardKind != CardKind.Alcohol || use.CardId != t.CardId || use.SourceSeat != t.VictimSeat || use.DyingResponse is not null || !use.TargetSeats.SequenceEqual([t.VictimSeat]) || !(use.PhysicalCardIds ?? [use.CardId]).SequenceEqual([t.CardId]) || t.From.OwnerSeat != program.OwnerSeat || t.From.Zone is not (CardZoneKind.WoodenOxGrain or CardZoneKind.BuquWound or CardZoneKind.Authority or CardZoneKind.Chunlao) || t.Reason.Value != $"skill-program.{(program.SkillId)}.{(SkillProgramEffectOp.UseBoundCardAsDyingAlcohol)}" || use.Action is not { Type: CardActionType.Use } action || action.ActionId != t.ActionId || action.ActorSeat != t.VictimSeat || action.ProviderSeat != program.OwnerSeat || action.EffectiveKind != CardKind.Alcohol || !action.TargetSeats.SequenceEqual(use.TargetSeats) || !action.EffectiveDesignatedTargetSeats.SequenceEqual(use.TargetSeats) || !action.ConversionChain.SequenceEqual([t.Source]) || action.PhysicalCards is not [var cost] || cost.CardId != t.CardId || cost.From != t.From || ProgramInstructionResolver.Default.Resolve(program, definition).GetPausedInstruction(program.InstructionIndex).Effect is not { Op: SkillProgramEffectOp.UseBoundCardAsDyingAlcohol, SourceBind: { } bind } || program.CardSetBindings.SingleOrDefault(b => b.Name == bind)is not { CardIds: [var bound], SourceLocations: [var location] } || bound != t.CardId || location != t.From || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == program.Id && e.SkillId == program.SkillId && e.BindingId == program.TriggerId && e.SkillInstanceId == program.SkillInstanceId && e.OwnerSeat == program.OwnerSeat && e.Window == SkillProgramTriggerWindow.DyingResponse) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBoundDyingAlcoholIssuedEvent>().Count(e => e.Receipt == t) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == t.CardId && e.CardKind == CardKind.Alcohol && e.SourceSeat == t.VictimSeat) != 1 || t.SequenceBefore < 0 || t.SequenceBefore > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > t.SequenceBefore && m.CardId == t.CardId && m.From == t.From && m.To == CardLocation.Processing && m.Reason == t.Reason) != 1)
            throw new InvalidOperationException("Bound Alcohol lost its exact source, program, dying occurrence, native use or physical payment.");
        var payments = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBoundDyingAlcoholPaidEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (use.BoundDyingAlcoholPayment is { } p)
        {
            if (p.Recovered < 0 || payments is not [var payment] || payment.ProgramFrameId != t.ProgramFrameId || payment.Payment != p || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(e => e is not ProgramBoundDyingAlcoholIssuedEvent issued || issued.Receipt != t).Skip(1).OfType<ProgramDyingRescueEvent>().Count(e => e.DyingFrameId == t.DyingFrameId && e.SkillId == t.Source.SkillId && e.OwnerSeat == t.Source.OwnerSeat && e.VictimSeat == t.VictimSeat && e.CardId == t.CardId && e.RecoveredHp == p.Recovered && e.VictimHp == p.RemainingHp) != 1)
                throw new InvalidOperationException("Bound Alcohol lost its once-paid native recovery fact.");
        }
        else if (requirePayment || payments.Length != 0)
            throw new InvalidOperationException("Bound Alcohol cannot return without its actual recovery payment.");
        if (requireFinished && (_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == t.CardId && e.CardKind == CardKind.Alcohol) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > t.SequenceBefore && m.CardId == t.CardId && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == t.Reason) != 1))
            throw new InvalidOperationException("Bound Alcohol must finish its original use and physical discard before returning its program.");
    }

    [NativeUseValidation(275)]
    internal void ValidateBoundDyingAlcoholReturn(CardUseFrame use)
    {
        if (use.BoundDyingAlcoholUse is not null)
            RequireBoundDyingAlcoholUse(use, requirePayment: true, requireFinished: true);
    }

    [NativeUseReturn(275)]
    internal bool ReturnBoundDyingAlcoholUse(CardUseFrame use)
    {
        if (use.BoundDyingAlcoholUse is not { } t)
            return false;
        RequireBoundDyingAlcoholUse(use, requirePayment: true, requireFinished: true);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame program || program.Id != t.ProgramFrameId)
            throw new InvalidOperationException("Bound Alcohol returned outside its exact paused program.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(program.Id);
        return true;
    }
}
