#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime))]
internal sealed partial class CoreGameEngineRicePileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> _resolve;
    private CoreGameEngineRicePileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> resolve) => new(resolve);
}
