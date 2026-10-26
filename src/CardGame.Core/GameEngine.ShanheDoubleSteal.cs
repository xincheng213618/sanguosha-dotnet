using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Both grants use the same native extra-effect slot. Their card kinds are
    // disjoint, while Fengzi retains its separate paid ExtraCardEffect owner.
    private static bool IsShanheNativeTrickRepeatSource(CardKind kind, string skillId) =>
        skillId == ShanheSchoolEffects.TrickRepeatSkillId && IsShanheSchoolDamageTrick(kind) ||
        skillId == ShanheDoubleStealPrograms.SkillId && kind == CardKind.Snatch;

    private bool TryGetShanheDoubleStealUse(int owner, ProgramSkillWindowContext? context,
        out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null!; window = null!;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } identity } ||
            _resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId) is not ProgramCardTriggerWindowFrame w ||
            w.Action is not { Type: CardActionType.Use } action || action.ActorSeat != owner ||
            w.TrickContinuation is not { ActionKind: LegalActionKind.Snatch } || w.CompletedResponseReturn is not null ||
            LifecycleCardUse(w.ParentFrameId) is not { Action: { } actual, ShanheSchoolTrick: null } current ||
            current.SourceSeat != owner || actual.ActionId != action.ActionId ||
            identity.CardActionId != actual.ActionId || identity.ParentCardUseFrameId != current.Id ||
            current.CardKind != CardKind.Snatch || actual.EffectiveKind != CardKind.Snatch) return false;
        use = current; window = w; return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62027, SkillProgramEffectOp.RepeatCurrentSnatch)]
    private bool CanOfferShanheDoubleSteal(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger,
        ProgramSkillWindowContext context) => candidate.SkillId == ShanheDoubleStealPrograms.SkillId &&
        TryGetShanheDoubleStealUse(candidate.OwnerSeat, context, out _, out _);

    [ProgramCapability]
    private sealed class ShanheDoubleStealHost(GameEngine engine) : IShanheDoubleStealHost
    {
        public void RepeatCurrentSnatch(ProgramSkillFrame frame) => engine.IssueShanheDoubleSteal(frame);
    }

    private void IssueShanheDoubleSteal(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheDoubleStealPrograms.SkillId || frame.InstructionIndex != 1 ||
            !TryGetShanheDoubleStealUse(frame.OwnerSeat, frame.WindowContext, out var use, out var window))
            throw new InvalidOperationException("Double Steal requires its exact accepted original Snatch use.");
        var source = new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var native = new ExtraCardEffectState { Source = source, ProgramFrameId = frame.Id,
            ActionId = use.Action!.ActionId, AutomaticGameplayHash = frame.GameplayHash, Trick = window.TrickContinuation };
        ReplaceRuntimeFrame(use.Id, use with { ShanheSchoolTrick =
            new(use.Action.ActionId, frame.Id, window.Id, frame.GameplayHash, native) });
        AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickIssuedEvent(use.Id, use.Action.ActionId, frame.Id,
            window.Id, source, frame.GameplayHash));
    }
}
