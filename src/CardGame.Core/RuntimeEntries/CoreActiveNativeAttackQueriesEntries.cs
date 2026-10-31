#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed partial class CoreActiveNativeAttackQueriesEntries
{
    private readonly Func<EngineOwners.CoreActiveNativeAttackQueriesRuntime> _resolve;
    private CoreActiveNativeAttackQueriesEntries(Func<EngineOwners.CoreActiveNativeAttackQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActiveNativeAttackQueriesEntries Create(Func<EngineOwners.CoreActiveNativeAttackQueriesRuntime> resolve) => new(resolve);
}
