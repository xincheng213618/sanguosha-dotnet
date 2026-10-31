#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineYaodaoChitongRuntime))]
internal sealed partial class CoreGameEngineYaodaoChitongEntries
{
    private readonly Func<EngineOwners.CoreGameEngineYaodaoChitongRuntime> _resolve;
    private CoreGameEngineYaodaoChitongEntries(Func<EngineOwners.CoreGameEngineYaodaoChitongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineYaodaoChitongEntries Create(Func<EngineOwners.CoreGameEngineYaodaoChitongRuntime> resolve) => new(resolve);
}
