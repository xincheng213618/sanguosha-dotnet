#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaXiongMouMobileRuntime
{
    internal bool MouYangweiCooling(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongYangweiReservedEvent>().Any(e => e.OwnerSeat == owner && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongCooldownRecoveredEvent>().Any(returned => returned.FrameId == e.FrameId));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 29100, SkillProgramEffectOp.ResolveMouHuaXiongYangwei)]
    internal bool CanActivateMouYangwei(ProgramActivationLegalityContext c) => c.SkillId == MouYangwei && MouHuaXiongComposition.IsYangwei(c.Program) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && c.Owner.IsAlive && !MouYangweiCooling(c.OwnerSeat);
    internal bool MouYangweiRoot(ProgramSkillFrame f) => f.SkillId == MouYangwei && f.ActivationId == "draw-and-wei" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is { } p && p.GameplayHash == f.GameplayHash && MouHuaXiongComposition.IsYangwei(p);
    internal SkillProgramStepOutcome ResolveMouYangwei(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MouYangweiRoot(f))
            throw new InvalidOperationException("Yangwei needs its exact complete zero-card Play activation.");
        if (f.InstructionIndex == 1)
        {
            if (f.MouHuaXiongYangwei is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber < 1 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || MouYangweiCooling(f.OwnerSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongYangweiReservedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Yangwei cannot repeat an issued phase activation or bypass its owner-turn cooldown.");
            var receipt = new MouHuaXiongYangweiReceipt(MouHuaXiongSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouHuaXiongYangwei = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuaXiongYangweiReservedEvent(f.Id, f.OwnerSeat, receipt.ActualTurn, receipt.PlayPhaseId, receipt.Source, receipt.GameplayHash));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.InstructionIndex != 3 || f.MouHuaXiongYangwei is not { WeiResolved: false } r || !ValidMouYangweiReceipt(f) || f.NativeDraw is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongWeiGrantedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Yangwei's Wei grant must follow its once-paid Draw and all native children.");
        var expired = !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != r.ActualOwner || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber != r.ActualTurn || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId != r.PlayPhaseId;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouHuaXiongYangwei = r with { WeiResolved = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuaXiongWeiGrantedEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.PlayPhaseId, expired));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidMouYangweiReceipt(ProgramSkillFrame f) => MouYangweiRoot(f) && f.MouHuaXiongYangwei is { } r && r.Source == MouHuaXiongSource(f) && r.GameplayHash == f.GameplayHash && r.ActualOwner == f.OwnerSeat && r.ActualTurn > 0 && r.PlayPhaseId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongYangweiReservedEvent>().Count(e => e == new MouHuaXiongYangweiReservedEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.PlayPhaseId, r.Source, r.GameplayHash)) == 1;
    internal bool HasMouHuaXiongWei(int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongWeiGrantedEvent>().Any(e => e.OwnerSeat == owner && !e.ExpiredBeforeGrant && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.PlayPhaseId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongYangweiReservedEvent>().Any(r => r.FrameId == e.FrameId && r.OwnerSeat == owner && r.ActualTurn == e.ActualTurn && r.PlayPhaseId == e.PlayPhaseId));
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMouHuaXiongWei(int owner, IReadOnlyList<PlayerMarkerSnapshot>? ordinary) => HasMouHuaXiongWei(owner) ? Array.AsReadOnly((ordinary ?? []).Where(m => m.Kind != PlayerMarkerKind.MouHuaXiongWei).Append(new PlayerMarkerSnapshot(PlayerMarkerKind.MouHuaXiongWei, "威", 1)).OrderBy(m => m.Kind).ToArray()) : ordinary;
    internal void RecoverMouYangweiAtEnding(int owner)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber < 1 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != owner || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != owner || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("Yangwei cooldown recovery requires the actual turn's native Ending entry.");
        foreach (var issued in _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongYangweiReservedEvent>().Where(e => e.OwnerSeat == owner && e.ActualTurn < _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuaXiongCooldownRecoveredEvent>().Any(returned => returned.FrameId == e.FrameId)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuaXiongCooldownRecoveredEvent(issued.FrameId, owner, issued.ActualTurn, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber));
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 20)]
    internal bool DistanceCardUseExemption20HasMouHuaXiongWei(ProgramCardDistanceQuery q) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && HasMouHuaXiongWei(q.Actor.Seat);
}
