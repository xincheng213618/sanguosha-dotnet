#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed partial class CoreActiveNativeAttackQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _resolve;
    private CoreActiveNativeAttackQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActiveNativeAttackQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> resolve) => new(resolve);
}
