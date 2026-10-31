#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowPipeline(global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGameDomainsEntries> getCoreGameEngineGameDomainsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries> getCoreGameEngineWenhePoisonRuntime) : ICardsMovedWindowPipeline
{
    public bool ExcludeOrdinaryMovementCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => getProgramPipelineHooks().ExcludeOrdinaryMovementCandidate(candidate, trigger);
    public void CollectMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => getProgramPipelineHooks().CollectMovementCandidates(batch, candidates);
    public ProgramSkillWindowContext? CreateMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => getProgramPipelineHooks().CreateMovementContext(frame, candidate);
    public bool IsProgramMovementSettlement(ProgramSkillFrame frame, CardMovementBatchContext batch) => getProgramPipelineHooks().IsProgramMovementSettlement(frame, batch);
    public int[]? SelectLegacyMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => getProgramPipelineHooks().SelectLegacyMovementIndexes(batch, candidate, trigger, location);
    public int[]? SelectDiscardBatchIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => getProgramPipelineHooks().SelectDiscardBatchIndexes(batch, candidate, features);
    public int[]? SelectLegacyDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => getProgramPipelineHooks().SelectLegacyDiscardIndexes(batch, candidate, trigger);
    public void PrepareLingkongGainBatch(CardMovementBatchContext batch) => getCharacterZhouFeiLeRuntime().PrepareLingkongGainBatch(batch);
    public IEnumerable<ProgramTriggerCandidate> CollectFirstDomainCandidates(CardMovementBatchContext batch) => getCoreGameEngineGameDomainsRuntime().CollectFirstDomainCandidates(batch);
    public bool HasWenhePoisonDepartures(CardMovementBatchContext batch) => getCoreGameEngineWenhePoisonRuntime().HasWenhePoisonDepartures(batch);
}
