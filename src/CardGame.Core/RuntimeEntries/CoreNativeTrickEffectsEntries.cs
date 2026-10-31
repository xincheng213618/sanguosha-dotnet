#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeTrickEffectsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> _resolve;
    private CoreNativeTrickEffectsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeTrickEffectsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> resolve) => new(resolve);
}
