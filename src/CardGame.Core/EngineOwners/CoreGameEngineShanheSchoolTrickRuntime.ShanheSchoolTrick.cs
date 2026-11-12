#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSchoolTrickRuntime
{
    internal static bool IsShanheSchoolDamageTrick(CardKind kind) => CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && (CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || kind == CardKind.UnexpectedAssault);
    internal bool TryGetShanheSchoolTrickUse(int owner, ProgramSkillWindowContext? context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } identity } || !_coreShanheSchoolsOperations.HasShanheSchoolEffect(owner, 9) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.Action is not { Type: CardActionType.Use } action || action.ActorSeat != owner || w.TrickContinuation is null || w.CompletedResponseReturn is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { } actual } current || current.SourceSeat != owner || actual.ActionId != action.ActionId || identity.CardActionId != actual.ActionId || identity.ParentCardUseFrameId != current.Id || current.CardKind != actual.EffectiveKind || !IsShanheSchoolDamageTrick(current.CardKind) || current.EffectEpisodes?.Episodes.Any(e => e.Issue.Source.SkillId == ShanheSchoolEffects.TrickRepeatSkillId) == true)
            return false;
        use = current;
        window = w;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62024, SkillProgramEffectOp.RepeatCurrentDamageTrick)]
    internal bool CanOfferShanheSchoolTrick(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => candidate.SkillId == ShanheSchoolEffects.TrickRepeatSkillId && TryGetShanheSchoolTrickUse(candidate.OwnerSeat, context, out _, out _);
    void IShanheSchoolTrickHost.RepeatCurrentDamageTrick(ProgramSkillFrame frame) => IssueShanheSchoolTrick(frame);
    internal void IssueShanheSchoolTrick(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheSchoolEffects.TrickRepeatSkillId || frame.InstructionIndex != 1 || !TryGetShanheSchoolTrickUse(frame.OwnerSeat, frame.WindowContext, out var use, out var window))
            throw new InvalidOperationException("A school trick repeat requires its exact accepted original damage-trick use.");
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        _effectEpisodes.Issue(use.Id, new(frame.Id, frame.InstructionIndex - 1, 0, source, frame.GameplayHash,
            new(80)), window.TrickContinuation);
    }
}
