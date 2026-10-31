#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeNullificationFramesEntries
{
    internal Card GetNullificationEffectCard(NullificationWindowFrame frame) => _resolve().GetNullificationEffectCard(frame);
    internal NullificationWindowFrame ReplaceNullificationWindowFrame(NullificationWindowFrame next) => _resolve().ReplaceNullificationWindowFrame(next);
    internal void SetCardUseTargetIndex(long frameId, int targetIndex) => _resolve().SetCardUseTargetIndex(frameId, targetIndex);
}
