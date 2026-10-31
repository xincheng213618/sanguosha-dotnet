#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime))]
internal sealed partial class CoreGameEngineTreatedWoundedEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> _resolve;
    private CoreGameEngineTreatedWoundedEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTreatedWoundedEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTreatedWoundedRuntime> resolve) => new(resolve);
}
