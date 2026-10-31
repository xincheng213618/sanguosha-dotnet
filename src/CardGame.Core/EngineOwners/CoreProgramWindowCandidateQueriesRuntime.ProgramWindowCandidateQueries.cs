#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramWindowCandidateQueriesRuntime
{
    SkillBindingShard IProgramWindowCandidateQueries.Shard(CharacterState owner) => _coreNationalSkillsOperations.GetSkillBindingShard(owner);
    SkillProgramTriggerFacts IProgramWindowCandidateQueries.Capture(CharacterState owner) => _programTriggerFactProjectorOperations.Get().Capture(_kernelCoreProgramTriggerFactProjectorQueriesOperations.CreateProgramTriggerFactQueries(), owner);
    IReadOnlyList<ProgramTriggerCandidate> IProgramWindowCandidateQueries.Eligible(CharacterState owner, SkillProgramTriggerWindow window, SkillProgramTriggerFacts facts) => _programTriggerOperations.Get().CollectEligibleProgramTriggerCandidates(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), owner, window, facts);
    IReadOnlyList<ProgramTriggerCandidate> IProgramWindowCandidateQueries.IncludeIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _characterYuanJiOlOperations.Value.IncludeYuanJiIssuedReturns(owner, window, ordinary);
}
