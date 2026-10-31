#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangChengYanOlRuntime
{
    internal SkillProgramStepOutcome YinshiProgramPreventSourcelessDamage(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var attempt = _coreProgramAttackAttemptOperations.CurrentDamageAttempt;
        if (attempt is null || attempt.TargetSeat != active.OwnerSeat || attempt.IsSourceLess == false && attempt.Card is not null)
            return SkillProgramStepOutcome.Continue;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramYinshiDamagePreventedEvent>().Any(fact => fact.OwnerSeat == active.OwnerSeat && fact.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber))
            return SkillProgramStepOutcome.Continue;
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame parent || parent.Id != frame.WindowContext?.ParentFrameId || parent.Prevented)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYinshiDamagePreventedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, frame.WindowContext?.Amount ?? 0));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"【隐世】防止了 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 本回合首次受到的无来源伤害。", active.OwnerSeat, active.OwnerSeat);
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome YinshiProgramClaimBaguaJudgmentCard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var context = frame.WindowContext;
        var judgment = context?.Judgment;
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment;
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (context is null || judgment is null || pending is null || context.Window != SkillProgramTriggerWindow.JudgmentFinalized || pending.Id != judgment.JudgmentFrameId || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)is not { } card || card.Id != judgment.CardId || active.PendingMovementContinuation is not null || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var from = CardLocation.Judgment(judgment.SubjectSeat);
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != from)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new ProgramMovementContinuation(active.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(active.OwnerSeat), new CardMoveReason($"skill-program.{(active.SkillId)}.yinshi-claim"));
        _coreMatchLogOperations.AddLog("CardGained", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 获得【八卦阵】生效的判定牌【{(card.DisplayName)}】。", active.OwnerSeat, judgment.SubjectSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYinshiJudgmentClaimedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, judgment.SubjectSeat, card.Id));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
