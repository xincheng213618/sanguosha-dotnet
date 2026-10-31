#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardOriginsEntries
{
    internal global::CardGame.Core.CardMovementSourceCount GetProgramDiscardOriginSourceCount(global::CardGame.Core.CardLocation source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> completions) => _resolve().GetProgramDiscardOriginSourceCount(source, completions);
    internal global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement) => _resolve().GetProgramDiscardSource(movement);
}
