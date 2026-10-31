#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardOriginsEntries
{
    internal CardMovementSourceCount GetProgramDiscardOriginSourceCount(CardLocation source, IReadOnlyList<CardMovementRecord> completions) => _resolve().GetProgramDiscardOriginSourceCount(source, completions);
    internal CardLocation? GetProgramDiscardSource(CardMovementRecord movement) => _resolve().GetProgramDiscardSource(movement);
}
