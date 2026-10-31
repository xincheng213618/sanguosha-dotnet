#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed partial class CoreGameEngineTreatedWoundedEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTreatedWoundedRuntime> _resolve;
    private CoreGameEngineTreatedWoundedEntries(Func<EngineOwners.CoreGameEngineTreatedWoundedRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTreatedWoundedEntries Create(Func<EngineOwners.CoreGameEngineTreatedWoundedRuntime> resolve) => new(resolve);
}
