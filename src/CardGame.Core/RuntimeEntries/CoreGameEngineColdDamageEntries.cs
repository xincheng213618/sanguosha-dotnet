#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineColdDamageRuntime))]
internal sealed partial class CoreGameEngineColdDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineColdDamageRuntime> _resolve;
    private CoreGameEngineColdDamageEntries(Func<EngineOwners.CoreGameEngineColdDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineColdDamageEntries Create(Func<EngineOwners.CoreGameEngineColdDamageRuntime> resolve) => new(resolve);
}
