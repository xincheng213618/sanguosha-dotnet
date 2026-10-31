#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetReplacementCandidatesEntries
{
    internal bool CanRunSoleTargetReplacementTargetCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunSoleTargetReplacementTargetCandidate(candidate, trigger, context);
}
