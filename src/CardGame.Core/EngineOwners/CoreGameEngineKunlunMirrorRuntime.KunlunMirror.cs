#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineKunlunMirrorRuntime
{
    internal const string KunlunMirrorSkill = "shanhe:kunlun-mirror-14080";
    internal bool HasEffectiveKunlunMirror(int owner, string instance) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _characterHuCheErMouOperations.KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).Any(card => card.Kind == CardKind.KunlunMirror && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, KunlunMirrorSkill, instance, card));
    internal bool KnowsKunlunMirrorName(int owner, string name) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<KunlunMirrorTrickLearnedEvent>().Any(fact => fact.Receipt.Source.OwnerSeat == owner && string.Equals(fact.Receipt.Origin.CardName, name, StringComparison.Ordinal));
    internal bool TryKunlunMirrorOrigin(GameEngine.IDamageAttempt? attack, out KunlunMirrorDamageOrigin origin)
    {
        origin = null !;
        if (attack?.EffectiveCardKind is not { } kind || CardUseCategoryCatalog.Get(kind)is not (CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        var action = use?.Action;
        if (action is not null && (action.Type != CardActionType.Use || action.EffectiveKind != kind))
            return false;
        origin = new(attack.ResolutionId, action?.ActionId, attack.Card?.Id, kind, CardCatalog.Get(kind).DisplayName, attack.CardUserSeat, attack.IsSourceLess ? null : attack.SourceSeat, attack.TargetSeat, attack.IsChainPropagation, attack.IsDelayedJudgmentDamage);
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62056, SkillProgramEffectOp.ResolveKunlunMirror)]
    internal bool CanOfferKunlunMirror(ProgramTriggerLegalityContext context)
    {
        if (context.SkillId != KunlunMirrorSkill || !HasEffectiveKunlunMirror(context.OwnerSeat, context.Candidate.SkillInstanceId) || context.WindowContext is not { Amount: > 0 } window || window.TargetSeat != context.OwnerSeat || !TryKunlunMirrorOrigin(_coreProgramAttackAttemptOperations.CurrentDamageAttempt, out var origin) || origin.TargetSeat != context.OwnerSeat)
            return false;
        var learned = KnowsKunlunMirrorName(context.OwnerSeat, origin.CardName);
        return window.Window == SkillProgramTriggerWindow.BeforeDamageApplied ? learned : window.Window == SkillProgramTriggerWindow.DamageAppliedBeforeDying && !learned;
    }

    void IKunlunMirrorHost.Resolve(ProgramSkillFrame frame) => ResolveKunlunMirror(frame);
    internal void ResolveKunlunMirror(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != KunlunMirrorSkill || frame.KunlunMirror is not null || frame.InstructionIndex != 1 || frame.WindowContext is not { Amount: > 0 } context || context.TargetSeat != frame.OwnerSeat || !HasEffectiveKunlunMirror(frame.OwnerSeat, frame.SkillInstanceId))
            throw new InvalidOperationException("Kunlun Mirror lost its enabled treasure source or exact positive damage owner.");
        GameEngine.IDamageAttempt attack;
        long? damageId = null;
        var learning = context.Window == SkillProgramTriggerWindow.DamageAppliedBeforeDying;
        if (learning)
        {
            if (frame.TriggerId != "remember-real-trick-damage" || _resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId || window.TriggerWindow != context.Window || window.ParentFrameId != context.DamageFrameId || _resolutionStackOperations._resolutionStack[^3] is not DamageFrame damage || damage.Id != window.ParentFrameId || damage.TargetSeat != frame.OwnerSeat || damage.Amount != context.Amount || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
                throw new InvalidOperationException("Kunlun Mirror cannot learn before a real positive applied-damage window.");
            attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
            damageId = damage.Id;
            if (attack.ResolutionId != damage.ParentFrameId || attack.SourceSeat != damage.SourceSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageRequestedEvent>().Count(fact => fact.ResolutionId == damage.Id && fact.SourceSeat == damage.SourceSeat && fact.TargetSeat == frame.OwnerSeat && fact.Amount == damage.Amount && fact.SourceCard == attack.EffectiveCardKind && fact.SourceLess == attack.IsSourceLess) != 1)
                throw new InvalidOperationException("Kunlun Mirror lost the original actual damage receipt.");
        }
        else
        {
            if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || frame.TriggerId != "prevent-remembered-trick" || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame before || before.Id != context.ParentFrameId || before.TargetSeat != frame.OwnerSeat || before.Prevented || before.Amount != context.Amount || before.RedirectedTargetSeat is not null || before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, before.Candidates[before.CandidateIndex].Candidate) || _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(before.ContinuationAttackResolutionId ?? before.ParentFrameId)is not { } original)
                throw new InvalidOperationException("Kunlun Mirror prevention lost its original live damage attempt.");
            attack = original;
        }

        if (!TryKunlunMirrorOrigin(attack, out var origin) || origin.TargetSeat != frame.OwnerSeat || origin.SourceSeat != context.SourceSeat || KnowsKunlunMirrorName(frame.OwnerSeat, origin.CardName) == learning)
            throw new InvalidOperationException("Kunlun Mirror cannot exchange its original effective trick name or learned-name branch.");
        var receipt = new KunlunMirrorReceipt(frame.InstructionIndex, context.ParentFrameId, damageId, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, origin, context.Amount, learning);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { KunlunMirror = receipt });
        if (learning)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KunlunMirrorTrickLearnedEvent(frame.Id, receipt));
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KunlunMirrorPreventionIssuedEvent(frame.Id, receipt));
    // The next ordinary PreventCurrentDamage instruction owns actual prevention.
    }
}
