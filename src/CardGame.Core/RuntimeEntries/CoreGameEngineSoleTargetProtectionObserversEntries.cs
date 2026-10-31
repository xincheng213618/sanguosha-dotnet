#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> _resolve;
    private CoreGameEngineSoleTargetProtectionObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetProtectionObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> resolve) => new(resolve);
}
