#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCoreInvariantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> _resolve;
    private CoreNativeCoreInvariantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCoreInvariantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> resolve) => new(resolve);
}
