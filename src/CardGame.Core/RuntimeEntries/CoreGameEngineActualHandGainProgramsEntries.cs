#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime))]
internal sealed partial class CoreGameEngineActualHandGainProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> _resolve;
    private CoreGameEngineActualHandGainProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualHandGainProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> resolve) => new(resolve);
}
