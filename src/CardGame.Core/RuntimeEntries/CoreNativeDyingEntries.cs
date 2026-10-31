#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDyingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _resolve;
    private CoreNativeDyingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDyingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> resolve) => new(resolve);
}
