#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime))]
internal sealed partial class CoreNativeCoreInvariantsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> _resolve;
    private CoreNativeCoreInvariantsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCoreInvariantsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> resolve) => new(resolve);
}
