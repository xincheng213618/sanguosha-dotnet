#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime))]
internal sealed partial class CoreGameEngineColdDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> _resolve;
    private CoreGameEngineColdDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineColdDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColdDamageRuntime> resolve) => new(resolve);
}
