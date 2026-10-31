#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlcoholSlashSuppressionsRuntime
{
    internal void CaptureProgramAlcoholConsumption(long cardUseFrameId, CharacterState actor)
    {
        var permanent = _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReplaceOwnSkillsAfterAlcoholSlashDamage);
        var captureAmount = permanent || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash);
        if (!actor.HasAlcoholEffect || !captureAmount && !_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesConsumedAlcoholSlash)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(cardUseFrameId);
        if (use?.Action is not { Type: CardActionType.Use } action || action.ActorSeat != actor.Seat || use.SourceSeat != actor.Seat || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || use.CardKind != action.EffectiveKind)
            throw new InvalidOperationException("Alcohol consumption requires its original real Slash use and actor.");
        if (use.ConsumedAlcoholBoost)
            throw new InvalidOperationException("A Slash cannot consume its Alcohol effect twice.");
        int? bonus = captureAmount ? actor.AlcoholDamageBonus : null;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ConsumedAlcoholBoost = true, ConsumedAlcoholBonus = bonus });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAlcoholConsumptionCapturedEvent(use.Id, action.ActionId, actor.Seat) { AlcoholBonus = bonus });
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 940)]
    internal bool CanRunAlcoholSlashSuppression(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => !trigger.RequiresConsumedAlcoholSlash || TryAlcoholSlashSuppressionUse(context, ownerSeat, out _);
    internal bool TryAlcoholSlashSuppressionUse(ProgramSkillWindowContext context, int ownerSeat, out CardUseFrame use)
    {
        use = null !;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.OwnerSeat != ownerSeat || context.SourceSeat != ownerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().LastOrDefault(f => f.Id == context.ParentFrameId)is not { } window || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || window.SourceSeat != ownerSeat || _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger?.Id != window.Id)
            return false;
        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
        var original = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        if (attack.IsSourceLess || attack.IsChainPropagation || attack.SourceSeat != ownerSeat || attack.CardUserSeat != ownerSeat || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || original is not { ConsumedAlcoholBoost: true, Action: { Type: CardActionType.Use } action } || action.ActorSeat != ownerSeat || action.EffectiveKind != kind || original.SourceSeat != ownerSeat || original.CardKind != kind || window.TargetSeat != attack.TargetSeat || context.TargetSeat != attack.TargetSeat)
            return false;
        use = original;
        return true;
    }

    SkillProgramStepOutcome IAlcoholSlashSuppressionProgramHost.StartOwnedDamagePointJudgment(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreOwnedDamagePointJudgmentsOperations.Value.BeginOwnedDamagePointJudgment(frame, effect);
}
