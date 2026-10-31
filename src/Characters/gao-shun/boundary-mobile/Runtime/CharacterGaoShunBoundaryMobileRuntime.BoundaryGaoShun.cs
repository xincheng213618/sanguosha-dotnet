#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGaoShunBoundaryMobileRuntime
{
    internal static CardConversionSource BoundaryGaoShunSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    void IBoundaryGaoShunHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteBoundaryGaoShun(f, op);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 57300)]
    internal void CaptureBoundaryGaoShunPindian(PindianResultDeterminedEvent fact)
    {
        if (fact.SkillId != BoundaryGaoShunComposition.Xianzhen || _resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianFrame native || native.Id != fact.FrameId || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame f || f.Id != native.ParentFrameId || f.SkillId != fact.SkillId || f.ActivationId != "contest" || f.InstructionIndex != 1 || f.SelectedTargetSeats is not [var target] || target != fact.Result.OpponentSeat || f.OwnerSeat != fact.Result.SourceSeat || native.ProgramResultBind != "__active-pindian-result" || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryGaoShunPindianCapturedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Mobile Xianzhen cannot capture its native comparison twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryGaoShunPindianCapturedEvent(f.Id, BoundaryGaoShunSource(f), f.GameplayHash, native.Id, target, fact.Result.SourceCardId, _coreCardAttackStateOperations.GetAttackCard(fact.Result.SourceCardId).Kind, fact.Result.SourceRank, fact.Result.OpponentRank, phase, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId));
    }

    internal void ExecuteBoundaryGaoShun(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash)
        {
            if (f.SkillId != BoundaryGaoShunComposition.Jinjiu || f.WindowContext is not { } context || !TryBoundaryGaoShunAlcoholBonus(context, f.OwnerSeat, out var bonus))
                throw new InvalidOperationException("Mobile Jinjiu lost its original Slash's captured Alcohol consumption.");
            _coreProgramDamageReductionOperations.ReduceProgramCurrentDamage(f, bonus);
            return;
        }

        if (f.SkillId != BoundaryGaoShunComposition.Xianzhen || f.InstructionIndex != 2 || f.ActivationId != "contest" || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryGaoShunPindianCapturedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var receipt] || receipt.Source != BoundaryGaoShunSource(f) || receipt.GameplayHash != f.GameplayHash || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(receipt.Phase) || f.PindianResultBindings is not [var binding] || binding.Name != "__active-pindian-result" || binding.SourceSeat != f.OwnerSeat || binding.OpponentSeat != receipt.TargetSeat || binding.SourceRank != receipt.SourceRank || binding.OpponentRank != receipt.OpponentRank || _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == receipt.PindianFrameId && e.SkillId == f.SkillId && e.Result.SourceSeat == f.OwnerSeat && e.Result.OpponentSeat == receipt.TargetSeat && e.Result.SourceCardId == receipt.SourceCardId && e.Result.SourceRank == receipt.SourceRank && e.Result.OpponentRank == receipt.OpponentRank && e.Result.SourceWon == binding.SourceWon) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryGaoShunXianzhenIssuedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Mobile Xianzhen must issue once from its exact completed native comparison and original Play.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber != receipt.Phase.TurnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != receipt.Phase.ActualTurnOwnerSeat)
            return;
        var exempt = CoreNativeCardClassificationsRuntime.IsSlashCard(receipt.SourceCardKind);
        if (exempt)
            _coreTurnHandLimitKindExemptionsOperations.GrantProgramTurnHandLimitCardKindExemption(f, CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryGaoShunXianzhenIssuedEvent(f.Id, receipt.Source, receipt.GameplayHash, receipt.PindianFrameId, receipt.TargetSeat, binding.SourceWon, exempt, receipt.Phase, receipt.DebitPhase));
    }

    internal BoundaryGaoShunXianzhenIssuedEvent? ActiveBoundaryGaoShunXianzhen(int actor)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != actor)
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryGaoShunXianzhenIssuedEvent>().SingleOrDefault(e => e.Source.OwnerSeat == actor && e.Phase == phase && e.DebitPhase == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    }

    internal bool BoundaryGaoShunWonTarget(int actor, int target) => ActiveBoundaryGaoShunXianzhen(actor)is { Won: true } grant && grant.TargetSeat == target;
    internal bool HasBoundaryGaoShunArmorBypass(int actor, int target, CardKind kind) => BoundaryGaoShunWonTarget(actor, target);
    [ProgramTargetRule(ProgramTargetRulePoint.QuotaAllowance, 57300)]
    internal bool BoundaryGaoShunQuota(ProgramTargetQuotaQuery q) => BoundaryGaoShunWonTarget(q.ActorSeat, q.TargetSeat);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 57300)]
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 57300)]
    internal bool BoundaryGaoShunSlashQuota(ProgramSlashQuotaQuery q) => BoundaryGaoShunWonTarget(q.Actor.Seat, q.Target.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 57300)]
    internal bool BoundaryGaoShunDistance(ProgramCardDistanceQuery q) => BoundaryGaoShunWonTarget(q.Actor.Seat, q.Target.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 57300)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 57300)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 57300)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 57300)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 57300)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 57300)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 57300)]
    internal bool BoundaryGaoShunSlashDistance(ProgramSlashDistanceQuery q) => BoundaryGaoShunWonTarget(q.Actor.Seat, q.Target.Seat);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeIntrinsicUseBan, 57300)]
    internal bool BoundaryGaoShunSlashBan(ProgramCardUseQuery q) => q.ActionType == CardActionType.Use && CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && ActiveBoundaryGaoShunXianzhen(q.ActorSeat)is { Won: false };
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 57301, SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash)]
    internal bool CanRunBoundaryGaoShunJinjiu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryGaoShunComposition.Jinjiu && TryBoundaryGaoShunAlcoholBonus(context, c.OwnerSeat, out _);
    internal bool TryBoundaryGaoShunAlcoholBonus(ProgramSkillWindowContext context, int owner, out int bonus)
    {
        bonus = 0;
        if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || context.TargetSeat != owner || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.TargetSeat != owner || attack.IsSourceLess || attack.IsDelayedJudgmentDamage || attack.IsProgramJudgmentDamage || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ConsumedAlcoholBoost: true, ConsumedAlcoholBonus: > 0, Action: { Type: CardActionType.Use } action } use || use.SourceSeat != attack.CardUserSeat || action.ActorSeat != attack.CardUserSeat || action.EffectiveKind != kind || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Prevented: false } window || window.TargetSeat != owner || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseAlcoholConsumptionCapturedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray()is not [var consumed] || consumed.ActionId != action.ActionId || consumed.ActorSeat != action.ActorSeat || consumed.AlcoholBonus != use.ConsumedAlcoholBonus)
            return false;
        bonus = use.ConsumedAlcoholBonus.Value;
        return true;
    }
}
