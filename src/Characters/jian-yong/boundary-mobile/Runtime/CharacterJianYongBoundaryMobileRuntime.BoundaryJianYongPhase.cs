#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJianYongBoundaryMobileRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryJianYongHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveBoundaryJianYongZongshi ? BeginBoundaryJianYongZongshiChoice(f) : RecordBoundaryJianYongFailure(f);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 56500)]
    internal void TrackBoundaryJianYongGrant(NextActualUseTargetAdjustmentGrantedEvent e)
    {
        if (e.Source.SkillId == BoundaryJianYongComposition.Qiaoshui)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryJianYongPhaseEvent(e.ProgramFrameId, e.Source, e.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, true));
    }

    internal SkillProgramStepOutcome RecordBoundaryJianYongFailure(ProgramSkillFrame f)
    {
        var paid = f.PindianResultBindings.Single(b => b.Name == "__active-pindian-result");
        if (f.SkillId != BoundaryJianYongComposition.Qiaoshui || f.InstructionIndex != 4 || paid.SourceWon || paid.SourceSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat)
            throw new InvalidOperationException("Mobile Qiaoshui needs its exact failed Play-phase Pindian.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryJianYongPhaseEvent(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, false));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool BoundaryJianYongPhaseCurrent(BoundaryJianYongPhaseEvent e) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Actor == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.Phase == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;
    internal bool IsBoundaryJianYongAdjustmentCurrent(NextActualUseTargetAdjustmentGrantedEvent grant) => grant.Source.SkillId != BoundaryJianYongComposition.Qiaoshui || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryJianYongPhaseEvent>().Any(e => e.ProgramFrameId == grant.ProgramFrameId && e.Source == grant.Source && e.Hash == grant.GameplayHash && e.Won && BoundaryJianYongPhaseCurrent(e));
    internal bool BoundaryJianYongAdjustmentConsumes(NextActualUseTargetAdjustmentGrantedEvent grant, CardKind kind) => grant.Source.SkillId != BoundaryJianYongComposition.Qiaoshui || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Basic || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 56500)]
    internal bool BoundaryJianYongTrickForbidden(ProgramCardUseQuery q) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(q.Kind) == SkillProgramCardCategory.Trick && (q.ActionType == CardActionType.Use || q.Kind == CardKind.Nullification) && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryJianYongPhaseEvent>().Any(e => !e.Won && e.Source.OwnerSeat == q.ActorSeat && BoundaryJianYongPhaseCurrent(e));
}
