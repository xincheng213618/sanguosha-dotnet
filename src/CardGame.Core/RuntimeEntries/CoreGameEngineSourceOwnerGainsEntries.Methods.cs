#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceOwnerGainsEntries
{
    internal IEnumerable<ProgramTriggerCandidate> CollectSourceOwnerGainCandidates(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardMovementSourceCount count) => _resolve().CollectSourceOwnerGainCandidates(batch, candidate, trigger, count);
}
