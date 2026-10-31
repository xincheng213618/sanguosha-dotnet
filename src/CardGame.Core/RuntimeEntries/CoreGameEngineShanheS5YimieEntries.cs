#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5YimieRuntime))]
internal sealed partial class CoreGameEngineShanheS5YimieEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5YimieRuntime> _resolve;
    private CoreGameEngineShanheS5YimieEntries(Func<EngineOwners.CoreGameEngineShanheS5YimieRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5YimieEntries Create(Func<EngineOwners.CoreGameEngineShanheS5YimieRuntime> resolve) => new(resolve);
}
