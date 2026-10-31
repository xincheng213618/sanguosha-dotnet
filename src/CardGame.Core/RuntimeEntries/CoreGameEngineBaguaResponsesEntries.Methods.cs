#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBaguaResponsesEntries
{
    internal void PublishFailedBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().PublishFailedBaguaResponse(attack);
    internal void SkipBaguaResponseCore() => _resolve().SkipBaguaResponseCore();
}
