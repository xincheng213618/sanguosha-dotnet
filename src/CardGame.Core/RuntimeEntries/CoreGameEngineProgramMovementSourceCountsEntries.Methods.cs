#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMovementSourceCountsEntries
{
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().ProgramMovementSourceCounts(batch, window);
}
