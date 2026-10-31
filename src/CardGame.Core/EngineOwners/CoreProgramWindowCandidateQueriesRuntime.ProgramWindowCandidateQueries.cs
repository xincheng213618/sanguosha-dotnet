#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramWindowCandidateQueriesRuntime
{
    SkillBindingShard global::CardGame.Core.IProgramWindowCandidateQueries.Shard(CharacterState owner) => _coreNationalSkillsOperations.GetSkillBindingShard(owner);
    SkillProgramTriggerFacts global::CardGame.Core.IProgramWindowCandidateQueries.Capture(CharacterState owner) => _programTriggerFactProjectorOperations.Get().Capture(_kernelCoreProgramTriggerFactProjectorQueriesOperations.CreateProgramTriggerFactQueries(), owner);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramWindowCandidateQueries.Eligible(CharacterState owner, SkillProgramTriggerWindow window, SkillProgramTriggerFacts facts) => _programTriggerOperations.Get().CollectEligibleProgramTriggerCandidates(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), owner, window, facts);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramWindowCandidateQueries.IncludeIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _characterYuanJiOlOperations.IncludeYuanJiIssuedReturns(owner, window, ordinary);
}
