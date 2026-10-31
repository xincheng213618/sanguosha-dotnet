#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairDrawsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairDrawsRuntime> _resolve;
    private CoreGameEngineEndingPairDrawsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndingPairDrawsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingPairDrawsRuntime> resolve) => new(resolve);
}
