#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramWindowCandidateQueries(Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<ProgramTriggerFactProjector> getProgramTriggerFactProjector, ProgramTriggerFactQueriesFactory createProgramTriggerFactQueries, Func<ProgramTriggerRuntime> getProgramTriggerRuntime, ProgramTriggerQueriesFactory createProgramTriggerQueries, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanJiOlRuntime>> getCharacterYuanJiOlRuntime) : IProgramWindowCandidateQueries
{
    public SkillBindingShard Shard(CharacterState owner) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(owner);
    public SkillProgramTriggerFacts Capture(CharacterState owner) => getProgramTriggerFactProjector().Capture(createProgramTriggerFactQueries(), owner);
    public IReadOnlyList<ProgramTriggerCandidate> Eligible(CharacterState owner, SkillProgramTriggerWindow window, SkillProgramTriggerFacts facts) => getProgramTriggerRuntime().CollectEligibleProgramTriggerCandidates(createProgramTriggerQueries(), owner, window, facts);
    public IReadOnlyList<ProgramTriggerCandidate> IncludeIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => getCharacterYuanJiOlRuntime().Value.IncludeYuanJiIssuedReturns(owner, window, ordinary);
}
