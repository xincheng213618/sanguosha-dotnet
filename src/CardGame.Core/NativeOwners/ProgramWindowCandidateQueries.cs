#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramWindowCandidateQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.ProgramTriggerFactProjector> getProgramTriggerFactProjector, global::CardGame.Core.NativeOwners.ProgramTriggerFactQueriesFactory createProgramTriggerFactQueries, global::System.Func<global::CardGame.Core.ProgramTriggerRuntime> getProgramTriggerRuntime, global::CardGame.Core.NativeOwners.ProgramTriggerQueriesFactory createProgramTriggerQueries, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries> getCharacterYuanJiOlRuntime) : IProgramWindowCandidateQueries
{
    public SkillBindingShard Shard(CharacterState owner) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(owner);
    public SkillProgramTriggerFacts Capture(CharacterState owner) => getProgramTriggerFactProjector().Capture(createProgramTriggerFactQueries(), owner);
    public IReadOnlyList<ProgramTriggerCandidate> Eligible(CharacterState owner, SkillProgramTriggerWindow window, SkillProgramTriggerFacts facts) => getProgramTriggerRuntime().CollectEligibleProgramTriggerCandidates(createProgramTriggerQueries(), owner, window, facts);
    public IReadOnlyList<ProgramTriggerCandidate> IncludeIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => getCharacterYuanJiOlRuntime().IncludeYuanJiIssuedReturns(owner, window, ordinary);
}
