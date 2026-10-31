#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualHandGainProgramsEntries
{
    internal IReadOnlyList<PromptChoice> ActualHandGainChoices(ProgramSkillFrame f) => _resolve().ActualHandGainChoices(f);
    internal ActualHandGainEntity[] ActualHandGainEntities(CardMovementBatchContext batch, int owner) => _resolve().ActualHandGainEntities(batch, owner);
    internal CardLocation ActualHandGainOriginalSource(CardMovementRecord first, CardMovementBatchContext? batch = default) => _resolve().ActualHandGainOriginalSource(first, batch);
    internal SkillProgramEffect? ActualHandGainPausedEffect(ProgramSkillFrame f) => _resolve().ActualHandGainPausedEffect(f);
    internal bool ActualHandGainTurnOpen(int owner) => _resolve().ActualHandGainTurnOpen(owner);
    internal bool CanRunActualHandGainTrigger(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanRunActualHandGainTrigger(c, t, context);
    internal void CaptureForeignTurnActualHandGains(CardMovementBatchContext batch) => _resolve().CaptureForeignTurnActualHandGains(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureForeignTurnActualHandGains(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureForeignTurnActualHandGains(batch, completed);
    internal int[] CurrentForeignHandGainIds(int owner, IReadOnlyList<ActualHandGainEntity> roster) => _resolve().CurrentForeignHandGainIds(owner, roster);
    internal bool ExactActualHandGainUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context, scoped out ProgramCardTriggerWindowFrame parent, scoped out CardUseFrame use) => _resolve().ExactActualHandGainUse(c, context, out parent, out use);
    internal SkillProgramStepOutcome ExecuteActualHandGain(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteActualHandGain(supplied, effect);
    internal void FinishActualHandGain(ProgramSkillFrame f) => _resolve().FinishActualHandGain(f);
    internal ActualHandGainEntity[] ForeignHandGainRoster(int owner, string skill, string hash, int turn, int actualOwner) => _resolve().ForeignHandGainRoster(owner, skill, hash, turn, actualOwner);
    internal bool HasRecordedActualOwnHandGain(ProgramTriggerCandidate c, CardMovementBatchContext batch) => _resolve().HasRecordedActualOwnHandGain(c, batch);
    internal bool IsActualHandGainMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsActualHandGainMovement(f, e, p);
    internal bool IsActualHandGainOwnDrawBatch(CardMovementBatchContext batch, int owner, string skill, string hash) => _resolve().IsActualHandGainOwnDrawBatch(batch, owner, skill, hash);
    internal bool IsNormalizedLegacyCompletedCategoryUse(CardUseFrame use, int actor, CardKind kind, IReadOnlyList<int> completedTargets) => _resolve().IsNormalizedLegacyCompletedCategoryUse(use, actor, kind, completedTargets);
    internal int[] MatchingActualHandGainIndexes(CardMovementBatchContext batch, int owner, CardLocation location) => _resolve().MatchingActualHandGainIndexes(batch, owner, location);
    internal CardUseFrame NormalizeCompletedCategoryVirtualUse(CardUseFrame use, long? programParentFrameId) => _resolve().NormalizeCompletedCategoryVirtualUse(use, programParentFrameId);
    internal void PayActualHandGain(ProgramSkillFrame f) => _resolve().PayActualHandGain(f);
    internal void PublishActualHandGainChoice(ProgramSkillFrame f) => _resolve().PublishActualHandGainChoice(f);
    internal void ResolveActualHandGainChoice(PromptChoice choice) => _resolve().ResolveActualHandGainChoice(choice);
    internal bool ResumeActualHandGain(long id) => _resolve().ResumeActualHandGain(id);
    internal bool ReturnActualHandGainMovement(ProgramSkillFrame f) => _resolve().ReturnActualHandGainMovement(f);
    internal bool SameCategoryGiftUsed(int owner, string skill, int turn, SkillProgramCardCategory category) => _resolve().SameCategoryGiftUsed(owner, skill, turn, category);
    internal PromptChoice SelectAiActualHandGain(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiActualHandGain(decision, f);
    internal int[]? SelectWindowIndexes_MatchingActualHandGainIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _resolve().SelectWindowIndexes_MatchingActualHandGainIndexes(batch, candidate, trigger, location);
    internal bool HasActualHandGainCapability { get => _resolve().HasActualHandGainCapability; }
}
