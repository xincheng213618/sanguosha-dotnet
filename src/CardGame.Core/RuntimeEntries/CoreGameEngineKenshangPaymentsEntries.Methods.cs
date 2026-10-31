#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKenshangPaymentsEntries
{
    internal bool IsKenshangRemovedMaterial(long useId, int cardId) => _resolve().IsKenshangRemovedMaterial(useId, cardId);
}
