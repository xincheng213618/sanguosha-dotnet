#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnLegacyCompletionEntries
{
    internal int? CapturedNormalizedLegacyActualTurnOrdinal(CardActionContext action) => _resolve().CapturedNormalizedLegacyActualTurnOrdinal(action);
    internal bool FinishActualTurnLegacyDyingAlcohol(long useId, long programParentId) => _resolve().FinishActualTurnLegacyDyingAlcohol(useId, programParentId);
    internal bool IsExactLegacyActualUseCompletion(CardUseFrame use) => _resolve().IsExactLegacyActualUseCompletion(use);
    internal CardUseFrame NormalizeActualTurnLegacyCompletedUse(CardUseFrame use, long? programParentId) => _resolve().NormalizeActualTurnLegacyCompletedUse(use, programParentId);
    internal void ReturnLegacyDyingAlcoholCompletion(CardUseFrame completed) => _resolve().ReturnLegacyDyingAlcoholCompletion(completed);
}
