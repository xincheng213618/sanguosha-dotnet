namespace CardGame.Core;

// Window entry and progression share the same trigger/fact services. Their
// frames own frozen candidates; this query service retains no candidate cache.
internal interface IProgramWindowCandidateQueries : IProgramSelectionQuery
{
    SkillBindingShard Shard(CharacterState owner);
    SkillProgramTriggerFacts Capture(CharacterState owner);
    IReadOnlyList<ProgramTriggerCandidate> Eligible(CharacterState owner, SkillProgramTriggerWindow window,
        SkillProgramTriggerFacts facts);
    IReadOnlyList<ProgramTriggerCandidate> IncludeIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window,
        IReadOnlyList<ProgramTriggerCandidate> ordinary);
}
