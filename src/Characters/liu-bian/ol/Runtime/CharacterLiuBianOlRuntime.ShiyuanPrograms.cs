#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBianOlRuntime
{
    internal bool TracksShiYuanLedger => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ShiYuanTargetDraw) == true;

    internal int ShiYuanDrawCountFor(int ownerHp, int sourceHp) => sourceHp > ownerHp ? 3 : sourceHp == ownerHp ? 2 : 1;
    internal int ShiYuanTurnLimit(int ownerSeat) => IsYuWeiLimitDoubled(ownerSeat) ? 2 : 1;
    internal int ShiYuanOptionUsesThisTurn(int ownerSeat, int drawCount) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramShiYuanTargetDrawEvent>().Count(e => e.OwnerSeat == ownerSeat && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.DrawCount == drawCount);
    internal bool ShiYuanDrawAvailable(int ownerSeat, int sourceHp) => ShiYuanOptionUsesThisTurn(ownerSeat, ShiYuanDrawCountFor(_coreMatchParticipantsPlayersAccess._players[ownerSeat].Hp, sourceHp)) < ShiYuanTurnLimit(ownerSeat);
    // The optional prompt only appears while the matching option still has a
    // remaining use this turn; an exhausted option never reaches the chooser.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 130)]
    internal bool CanRunShiYuanTargetDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ShiYuanTargetDraw))
            return true;
        if (!TracksShiYuanLedger || _coreMatchOutcomeWinnerAccess._winner != Winner.None || context.SourceSeat is not { } sourceSeat || sourceSeat == candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Hp <= 0)
            return false;
        return ShiYuanDrawAvailable(candidate.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[sourceSeat].Hp);
    }

    internal SkillProgramStepOutcome ShiYuanProgramTargetDraw(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || frame.WindowContext?.SourceSeat is not { } sourceSeat || sourceSeat == owner.Seat || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var drawCount = ShiYuanDrawCountFor(owner.Hp, _coreMatchParticipantsPlayersAccess._players[sourceSeat].Hp);
        if (ShiYuanOptionUsesThisTurn(owner.Seat, drawCount) >= ShiYuanTurnLimit(owner.Seat))
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 诗怨：本回合摸{(drawCount)}张牌的机会已用完。", owner.Seat);
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return SkillProgramStepOutcome.Continue;
        }

        var actionId = frame.WindowContext?.CardUse?.CardActionId ?? 0;
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(owner, drawCount, true, new($"skill-program.{(active.SkillId)}.target-draw"));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，" + $"因成为 {(_coreMatchParticipantsPlayersAccess._players[sourceSeat].Name)} 使用牌的目标（体力值{((drawCount == 3 ? "大于" : drawCount == 2 ? "等于" : "小于"))}你），摸 {(drawn.Count)} 张牌。", owner.Seat, sourceSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramShiYuanTargetDrawEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, sourceSeat, drawCount, actionId));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
