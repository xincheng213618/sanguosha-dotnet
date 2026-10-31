#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime))]
internal sealed partial class CoreGameEngineShanheS5XuchuAiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> _resolve;
    private CoreGameEngineShanheS5XuchuAiEntries(Func<EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5XuchuAiEntries Create(Func<EngineOwners.CoreGameEngineShanheS5XuchuAiRuntime> resolve) => new(resolve);
}
