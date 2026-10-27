namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string KunlunMirrorSkill = "shanhe:kunlun-mirror-14080";

    private bool HasEffectiveKunlunMirror(int owner, string instance) => IsValidPlayerSeat(owner) && _players[owner].IsAlive &&
        KongwuEffectiveEquipment(_players[owner]).Any(card => card.Kind == CardKind.KunlunMirror &&
            IsActiveProgramSourceEquipmentCard(owner, KunlunMirrorSkill, instance, card));

    private bool KnowsKunlunMirrorName(int owner, string name) => ProgramEventHistory<KunlunMirrorTrickLearnedEvent>()
        .Any(fact => fact.Receipt.Source.OwnerSeat == owner && string.Equals(fact.Receipt.Origin.CardName, name, StringComparison.Ordinal));

    // Native attack identity retains the effective trick through payment,
    // virtual uses, delayed judgment and subsequent elemental chain recipients.
    private bool TryKunlunMirrorOrigin(IDamageAttempt? attack, out KunlunMirrorDamageOrigin origin)
    {
        origin = null!;
        if (attack?.EffectiveCardKind is not { } kind ||
            CardUseCategoryCatalog.Get(kind) is not (CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick)) return false;
        var use = LifecycleCardUse(attack.ResolutionId);
        var action = use?.Action;
        if (action is not null && (action.Type != CardActionType.Use || action.EffectiveKind != kind)) return false;
        origin = new(attack.ResolutionId, action?.ActionId, attack.Card?.Id, kind, CardCatalog.Get(kind).DisplayName,
            attack.CardUserSeat, attack.IsSourceLess ? null : attack.SourceSeat, attack.TargetSeat,
            attack.IsChainPropagation, attack.IsDelayedJudgmentDamage);
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62056, SkillProgramEffectOp.ResolveKunlunMirror)]
    private bool CanOfferKunlunMirror(ProgramTriggerLegalityContext context)
    {
        if (context.SkillId != KunlunMirrorSkill || !HasEffectiveKunlunMirror(context.OwnerSeat, context.Candidate.SkillInstanceId) ||
            context.WindowContext is not { Amount: > 0 } window || window.TargetSeat != context.OwnerSeat ||
            !TryKunlunMirrorOrigin(CurrentDamageAttempt, out var origin) || origin.TargetSeat != context.OwnerSeat) return false;
        var learned = KnowsKunlunMirrorName(context.OwnerSeat, origin.CardName);
        return window.Window == SkillProgramTriggerWindow.BeforeDamageApplied ? learned :
            window.Window == SkillProgramTriggerWindow.DamageAppliedBeforeDying && !learned;
    }

    [ProgramCapability]
    private sealed class KunlunMirrorCapability(GameEngine engine) : IKunlunMirrorHost
    { public void Resolve(ProgramSkillFrame frame) => engine.ResolveKunlunMirror(frame); }

    private void ResolveKunlunMirror(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != KunlunMirrorSkill || frame.KunlunMirror is not null || frame.InstructionIndex != 1 ||
            frame.WindowContext is not { Amount: > 0 } context || context.TargetSeat != frame.OwnerSeat ||
            !HasEffectiveKunlunMirror(frame.OwnerSeat, frame.SkillInstanceId))
            throw new InvalidOperationException("Kunlun Mirror lost its enabled treasure source or exact positive damage owner.");
        IDamageAttempt attack; long? damageId = null;
        var learning = context.Window == SkillProgramTriggerWindow.DamageAppliedBeforeDying;
        if (learning)
        {
            if (frame.TriggerId != "remember-real-trick-damage" || _resolutionStack.Count < 3 ||
                _resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != context.ParentFrameId ||
                window.TriggerWindow != context.Window || window.ParentFrameId != context.DamageFrameId ||
                _resolutionStack[^3] is not DamageFrame damage || damage.Id != window.ParentFrameId ||
                damage.TargetSeat != frame.OwnerSeat || damage.Amount != context.Amount ||
                window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
                !MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
                throw new InvalidOperationException("Kunlun Mirror cannot learn before a real positive applied-damage window.");
            attack = GetDamageTriggerAttack(window); damageId = damage.Id;
            if (attack.ResolutionId != damage.ParentFrameId || attack.SourceSeat != damage.SourceSeat ||
                ProgramEventHistory<DamageRequestedEvent>().Count(fact => fact.ResolutionId == damage.Id &&
                    fact.SourceSeat == damage.SourceSeat && fact.TargetSeat == frame.OwnerSeat && fact.Amount == damage.Amount &&
                    fact.SourceCard == attack.EffectiveCardKind && fact.SourceLess == attack.IsSourceLess) != 1)
                throw new InvalidOperationException("Kunlun Mirror lost the original actual damage receipt.");
        }
        else
        {
            if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || frame.TriggerId != "prevent-remembered-trick" ||
                _resolutionStack.Count < 2 || _resolutionStack[^2] is not BeforeDamageProgramWindowFrame before ||
                before.Id != context.ParentFrameId || before.TargetSeat != frame.OwnerSeat || before.Prevented ||
                before.Amount != context.Amount || before.RedirectedTargetSeat is not null ||
                before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count ||
                !MountObserverCandidateMatches(frame, before.Candidates[before.CandidateIndex].Candidate) ||
                LastDamageSourceOriginalAttack(before.ContinuationAttackResolutionId ?? before.ParentFrameId) is not { } original)
                throw new InvalidOperationException("Kunlun Mirror prevention lost its original live damage attempt.");
            attack = original;
        }
        if (!TryKunlunMirrorOrigin(attack, out var origin) || origin.TargetSeat != frame.OwnerSeat ||
            origin.SourceSeat != context.SourceSeat || KnowsKunlunMirrorName(frame.OwnerSeat, origin.CardName) == learning)
            throw new InvalidOperationException("Kunlun Mirror cannot exchange its original effective trick name or learned-name branch.");
        var receipt = new KunlunMirrorReceipt(frame.InstructionIndex, context.ParentFrameId, damageId,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            origin, context.Amount, learning);
        ReplaceRuntimeTop(frame with { KunlunMirror = receipt });
        if (learning) AdvanceEventRulesAndQueueFact(new KunlunMirrorTrickLearnedEvent(frame.Id, receipt));
        else AdvanceEventRulesAndQueueFact(new KunlunMirrorPreventionIssuedEvent(frame.Id, receipt));
        // The next ordinary PreventCurrentDamage instruction owns actual prevention.
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62056)]
    private void AssertKunlunMirrorReceipt(ProgramSkillFrame frame)
    {
        if (frame.KunlunMirror is not { } receipt) return;
        if (receipt.InstructionIndex != 1 || frame.InstructionIndex < 1 || frame.InstructionIndex > (receipt.Learning ? 1 : 2) ||
            receipt.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            receipt.GameplayHash != frame.GameplayHash || receipt.WindowFrameId != frame.WindowContext?.ParentFrameId ||
            receipt.Origin.TargetSeat != frame.OwnerSeat || receipt.Amount <= 0 ||
            CardUseCategoryCatalog.Get(receipt.Origin.EffectiveKind) is not (CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick) ||
            string.IsNullOrWhiteSpace(receipt.Origin.CardName))
            throw new InvalidOperationException("Kunlun Mirror lost its frozen treasure/damage/name receipt.");
    }
}
