#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime))]
internal sealed partial class CoreGameEngineShanheS5XuchuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime> _resolve;
    private CoreGameEngineShanheS5XuchuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5XuchuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuRuntime> resolve) => new(resolve);
}
