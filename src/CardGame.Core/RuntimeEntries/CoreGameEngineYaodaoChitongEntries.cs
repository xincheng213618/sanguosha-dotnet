#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime))]
internal sealed partial class CoreGameEngineYaodaoChitongEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> _resolve;
    private CoreGameEngineYaodaoChitongEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineYaodaoChitongEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> resolve) => new(resolve);
}
