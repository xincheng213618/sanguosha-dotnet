#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheWoxinRuntime))]
internal sealed partial class CoreGameEngineShanheWoxinEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheWoxinRuntime> _resolve;
    private CoreGameEngineShanheWoxinEntries(Func<EngineOwners.CoreGameEngineShanheWoxinRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheWoxinEntries Create(Func<EngineOwners.CoreGameEngineShanheWoxinRuntime> resolve) => new(resolve);
}
