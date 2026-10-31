#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5XuchuRuntime))]
internal sealed partial class CoreGameEngineShanheS5XuchuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5XuchuRuntime> _resolve;
    private CoreGameEngineShanheS5XuchuEntries(Func<EngineOwners.CoreGameEngineShanheS5XuchuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5XuchuEntries Create(Func<EngineOwners.CoreGameEngineShanheS5XuchuRuntime> resolve) => new(resolve);
}
