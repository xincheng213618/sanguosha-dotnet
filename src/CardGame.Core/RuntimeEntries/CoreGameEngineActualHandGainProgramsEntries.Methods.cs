#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualHandGainProgramsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ActualHandGainChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ActualHandGainChoices(f);
    internal global::CardGame.Core.ActualHandGainEntity[] ActualHandGainEntities(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().ActualHandGainEntities(batch, owner);
    internal global::CardGame.Core.CardLocation ActualHandGainOriginalSource(global::CardGame.Core.CardMovementRecord first, global::CardGame.Core.CardMovementBatchContext? batch = default) => _resolve().ActualHandGainOriginalSource(first, batch);
    internal global::CardGame.Core.SkillProgramEffect? ActualHandGainPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ActualHandGainPausedEffect(f);
    internal bool ActualHandGainTurnOpen(int owner) => _resolve().ActualHandGainTurnOpen(owner);
    internal bool CanRunActualHandGainTrigger(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunActualHandGainTrigger(c, t, context);
    internal void CaptureForeignTurnActualHandGains(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureForeignTurnActualHandGains(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureForeignTurnActualHandGains(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureForeignTurnActualHandGains(batch, completed);
    internal int[] CurrentForeignHandGainIds(int owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualHandGainEntity> roster) => _resolve().CurrentForeignHandGainIds(owner, roster);
    internal bool ExactActualHandGainUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame parent, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().ExactActualHandGainUse(c, context, out parent, out use);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteActualHandGain(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteActualHandGain(supplied, effect);
    internal void FinishActualHandGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishActualHandGain(f);
    internal global::CardGame.Core.ActualHandGainEntity[] ForeignHandGainRoster(int owner, string skill, string hash, int turn, int actualOwner) => _resolve().ForeignHandGainRoster(owner, skill, hash, turn, actualOwner);
    internal bool HasRecordedActualOwnHandGain(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HasRecordedActualOwnHandGain(c, batch);
    internal bool IsActualHandGainMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsActualHandGainMovement(f, e, p);
    internal bool IsActualHandGainOwnDrawBatch(global::CardGame.Core.CardMovementBatchContext batch, int owner, string skill, string hash) => _resolve().IsActualHandGainOwnDrawBatch(batch, owner, skill, hash);
    internal bool IsNormalizedLegacyCompletedCategoryUse(global::CardGame.Core.CardUseFrame use, int actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> completedTargets) => _resolve().IsNormalizedLegacyCompletedCategoryUse(use, actor, kind, completedTargets);
    internal int[] MatchingActualHandGainIndexes(global::CardGame.Core.CardMovementBatchContext batch, int owner, global::CardGame.Core.CardLocation location) => _resolve().MatchingActualHandGainIndexes(batch, owner, location);
    internal global::CardGame.Core.CardUseFrame NormalizeCompletedCategoryVirtualUse(global::CardGame.Core.CardUseFrame use, long? programParentFrameId) => _resolve().NormalizeCompletedCategoryVirtualUse(use, programParentFrameId);
    internal void PayActualHandGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayActualHandGain(f);
    internal void PublishActualHandGainChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishActualHandGainChoice(f);
    internal void ResolveActualHandGainChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveActualHandGainChoice(choice);
    internal bool ResumeActualHandGain(long id) => _resolve().ResumeActualHandGain(id);
    internal bool ReturnActualHandGainMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnActualHandGainMovement(f);
    internal bool SameCategoryGiftUsed(int owner, string skill, int turn, global::CardGame.Core.SkillProgramCardCategory category) => _resolve().SameCategoryGiftUsed(owner, skill, turn, category);
    internal global::CardGame.Core.PromptChoice SelectAiActualHandGain(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiActualHandGain(decision, f);
    internal int[]? SelectWindowIndexes_MatchingActualHandGainIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardLocation location) => _resolve().SelectWindowIndexes_MatchingActualHandGainIndexes(batch, candidate, trigger, location);
    internal bool HasActualHandGainCapability { get => _resolve().HasActualHandGainCapability; }
}
