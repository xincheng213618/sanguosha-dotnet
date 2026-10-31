#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardEffectDrawsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _resolve;
    private CoreNativeCardEffectDrawsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardEffectDrawsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> resolve) => new(resolve);
}
