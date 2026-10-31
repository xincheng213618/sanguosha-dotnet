#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime))]
internal sealed partial class CoreGameEngineShanheS5XuchuAiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> _resolve;
    private CoreGameEngineShanheS5XuchuAiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5XuchuAiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> resolve) => new(resolve);
}
