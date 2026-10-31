#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed partial class CoreNativeDamageHandlesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _resolve;
    private CoreNativeDamageHandlesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageHandlesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> resolve) => new(resolve);
}
