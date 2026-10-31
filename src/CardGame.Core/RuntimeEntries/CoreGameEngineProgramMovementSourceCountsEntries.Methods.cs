#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMovementSourceCountsEntries
{
    internal IEnumerable<(CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(CardMovementBatchContext batch, SkillProgramTriggerWindow window) => _resolve().ProgramMovementSourceCounts(batch, window);
}
