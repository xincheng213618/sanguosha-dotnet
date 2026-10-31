#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnLegacyCompletionEntries
{
    internal int? CapturedNormalizedLegacyActualTurnOrdinal(global::CardGame.Core.CardActionContext action) => _resolve().CapturedNormalizedLegacyActualTurnOrdinal(action);
    internal bool FinishActualTurnLegacyDyingAlcohol(long useId, long programParentId) => _resolve().FinishActualTurnLegacyDyingAlcohol(useId, programParentId);
    internal bool IsExactLegacyActualUseCompletion(global::CardGame.Core.CardUseFrame use) => _resolve().IsExactLegacyActualUseCompletion(use);
    internal global::CardGame.Core.CardUseFrame NormalizeActualTurnLegacyCompletedUse(global::CardGame.Core.CardUseFrame use, long? programParentId) => _resolve().NormalizeActualTurnLegacyCompletedUse(use, programParentId);
    internal void ReturnLegacyDyingAlcoholCompletion(global::CardGame.Core.CardUseFrame completed) => _resolve().ReturnLegacyDyingAlcoholCompletion(completed);
}
