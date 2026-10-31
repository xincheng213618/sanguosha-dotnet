#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeNullificationFramesEntries
{
    internal global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame) => _resolve().GetNullificationEffectCard(frame);
    internal global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next) => _resolve().ReplaceNullificationWindowFrame(next);
    internal void SetCardUseTargetIndex(long frameId, int targetIndex) => _resolve().SetCardUseTargetIndex(frameId, targetIndex);
}
