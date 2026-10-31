#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime))]
internal sealed partial class CoreGameEnginePersistentPrivateWhirlEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> _resolve;
    private CoreGameEnginePersistentPrivateWhirlEntries(Func<EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePersistentPrivateWhirlEntries Create(Func<EngineOwners.CoreGameEnginePersistentPrivateWhirlRuntime> resolve) => new(resolve);
}
