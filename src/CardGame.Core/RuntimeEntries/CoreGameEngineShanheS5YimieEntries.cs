#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime))]
internal sealed partial class CoreGameEngineShanheS5YimieEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> _resolve;
    private CoreGameEngineShanheS5YimieEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5YimieEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5YimieRuntime> resolve) => new(resolve);
}
