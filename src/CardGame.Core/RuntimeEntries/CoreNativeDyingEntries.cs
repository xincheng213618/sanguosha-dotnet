#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed partial class CoreNativeDyingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _resolve;
    private CoreNativeDyingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDyingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> resolve) => new(resolve);
}
