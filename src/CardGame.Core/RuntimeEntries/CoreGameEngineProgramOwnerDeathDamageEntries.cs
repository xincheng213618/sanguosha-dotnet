#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime))]
internal sealed partial class CoreGameEngineProgramOwnerDeathDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> _resolve;
    private CoreGameEngineProgramOwnerDeathDamageEntries(Func<EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnerDeathDamageEntries Create(Func<EngineOwners.CoreGameEngineProgramOwnerDeathDamageRuntime> resolve) => new(resolve);
}
