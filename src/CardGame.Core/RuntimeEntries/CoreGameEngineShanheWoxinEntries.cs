#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime))]
internal sealed partial class CoreGameEngineShanheWoxinEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> _resolve;
    private CoreGameEngineShanheWoxinEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheWoxinEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> resolve) => new(resolve);
}
