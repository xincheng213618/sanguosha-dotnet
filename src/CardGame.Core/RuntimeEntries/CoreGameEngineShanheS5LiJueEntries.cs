#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5LiJueRuntime))]
internal sealed partial class CoreGameEngineShanheS5LiJueEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5LiJueRuntime> _resolve;
    private CoreGameEngineShanheS5LiJueEntries(Func<EngineOwners.CoreGameEngineShanheS5LiJueRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5LiJueEntries Create(Func<EngineOwners.CoreGameEngineShanheS5LiJueRuntime> resolve) => new(resolve);
}
