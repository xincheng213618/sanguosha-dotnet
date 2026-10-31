#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlcoholSlashSuppressionsRuntime
{
    internal void CaptureProgramAlcoholConsumption(long cardUseFrameId, CharacterState actor)
    {
        var permanent = _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReplaceOwnSkillsAfterAlcoholSlashDamage);
        var captureAmount = permanent || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash);
        if (!actor.HasAlcoholEffect || !captureAmount && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SuppressOwnSkillAfterAlcoholSlashDamage))
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(cardUseFrameId);
        if (use?.Action is not { Type: CardActionType.Use } action || action.ActorSeat != actor.Seat || use.SourceSeat != actor.Seat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || use.CardKind != action.EffectiveKind)
            throw new InvalidOperationException("Alcohol consumption requires its original real Slash use and actor.");
        if (use.ConsumedAlcoholBoost)
            throw new InvalidOperationException("A Slash cannot consume its Alcohol effect twice.");
        int? bonus = captureAmount ? actor.AlcoholDamageBonus : null;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ConsumedAlcoholBoost = true, ConsumedAlcoholBonus = bonus });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAlcoholConsumptionCapturedEvent(use.Id, action.ActionId, actor.Seat) { AlcoholBonus = bonus });
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 940)]
    internal bool CanRunAlcoholSlashSuppression(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.SuppressOwnSkillAfterAlcoholSlashDamage) || TryAlcoholSlashSuppressionUse(context, ownerSeat, out _);
    internal bool TryAlcoholSlashSuppressionUse(ProgramSkillWindowContext context, int ownerSeat, out CardUseFrame use)
    {
        use = null !;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.OwnerSeat != ownerSeat || context.SourceSeat != ownerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().LastOrDefault(f => f.Id == context.ParentFrameId)is not { } window || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || window.SourceSeat != ownerSeat || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id != window.Id)
            return false;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        var original = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        if (attack.IsSourceLess || attack.IsChainPropagation || attack.SourceSeat != ownerSeat || attack.CardUserSeat != ownerSeat || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || original is not { ConsumedAlcoholBoost: true, Action: { Type: CardActionType.Use } action } || action.ActorSeat != ownerSeat || action.EffectiveKind != kind || original.SourceSeat != ownerSeat || original.CardKind != kind || window.TargetSeat != attack.TargetSeat || context.TargetSeat != attack.TargetSeat)
            return false;
        use = original;
        return true;
    }

    internal void SuppressOwnSkillAfterAlcoholSlashDamage(ProgramSkillFrame frame, string skillId)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (!ReferenceEquals(active, _resolutionStackOperations._resolutionStack.LastOrDefault()) || active.WindowContext is not { } context || !TryAlcoholSlashSuppressionUse(context, active.OwnerSeat, out var use) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId) || !_contentRegistryOperations._contentRegistry.Skills.ContainsKey(skillId) || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Single(f => f.Id == context.ParentFrameId)is not { } window || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex].ToProgramCandidate()is not { } candidate || candidate.OwnerSeat != active.OwnerSeat || candidate.SkillId != active.SkillId || candidate.BindingId != active.TriggerId || candidate.SkillInstanceId != active.SkillInstanceId || candidate.GameplayHash != active.GameplayHash)
            throw new InvalidOperationException("An own-skill suppression requires the exact live source instance and paid Alcohol Slash damage candidate.");
        var plan = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!);
        if (active.InstructionIndex < 1 || active.InstructionIndex > plan.Instructions.Count || plan.Instructions[active.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.SuppressOwnSkillAfterAlcoholSlashDamage } effect || !effect.SkillIds.SequenceEqual([skillId]) || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat)
            throw new InvalidOperationException("An own-skill suppression lost its instruction or actual turn.");
        var issued = new CurrentTurnOwnSkillSuppression(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, active.Id, active.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(active), skillId, use.Id, use.Action!.ActionId);
        var prior = _coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.SingleOrDefault(s => s.ParentFrameId == issued.ParentFrameId && s.EffectIndex == issued.EffectIndex);
        if (prior is not null)
        {
            if (prior != issued)
                throw new InvalidOperationException("An own-skill suppression issue changed identity.");
            return;
        }

        _coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.Add(issued);
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnOwnSkillSuppressionIssuedEvent(issued));
    }

    internal bool IsCurrentTurnOwnSkillGrantQualified(CharacterState owner, SkillGrant grant) => !_coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.Any(s => s.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && s.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && s.Source.OwnerSeat == owner.Seat && s.SuppressedSkillId == grant.SkillId);
    internal void ExpireCurrentTurnOwnSkillSuppressions(int turnNumber, int turnOwnerSeat)
    {
        if (_coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.RemoveAll(s => s.TurnNumber == turnNumber && s.TurnOwnerSeat == turnOwnerSeat) == 0)
            return;
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnOwnSkillSuppressionsExpiredEvent(turnNumber, turnOwnerSeat));
    }

    internal void AssertCurrentTurnOwnSkillSuppressions()
    {
        if (_coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.Any(s => s.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || s.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || s.ParentFrameId <= 0 || s.EffectIndex < 0 || s.CardUseFrameId <= 0 || s.CardActionId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s.Source.OwnerSeat) || string.IsNullOrWhiteSpace(s.Source.SkillId) || string.IsNullOrWhiteSpace(s.Source.BindingId) || string.IsNullOrWhiteSpace(s.Source.SkillInstanceId) || !_contentRegistryOperations._contentRegistry.Skills.ContainsKey(s.SuppressedSkillId)) || _coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.GroupBy(s => (s.ParentFrameId, s.EffectIndex)).Any(group => group.Count() != 1))
            throw new InvalidOperationException("An own-skill suppression lost its frozen identity or actual-turn expiry.");
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(f => f.ConsumedAlcoholBoost || f.ConsumedAlcoholBonus is not null))
            if (!use.ConsumedAlcoholBoost || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.ConsumedAlcoholBonus is { } bonus && (bonus <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseAlcoholConsumptionCapturedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == action.ActionId && e.ActorSeat == use.SourceSeat && e.AlcoholBonus == bonus) != 1))
                throw new InvalidOperationException("An Alcohol consumption receipt lost its original Slash action.");
    }

    void global::CardGame.Core.IAlcoholSlashSuppressionProgramHost.SuppressOwnSkillAfterAlcoholSlashDamage(ProgramSkillFrame frame, string skillId) => SuppressOwnSkillAfterAlcoholSlashDamage(frame, skillId);
    SkillProgramStepOutcome global::CardGame.Core.IAlcoholSlashSuppressionProgramHost.StartOwnedDamagePointJudgment(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreOwnedDamagePointJudgmentsOperations.BeginOwnedDamagePointJudgment(frame, effect);
}
