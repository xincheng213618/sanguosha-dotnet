#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineStrategicProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> _resolve;
    private CoreGameEngineStrategicProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineStrategicProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineStrategicProgramsRuntime> resolve) => new(resolve);
}
