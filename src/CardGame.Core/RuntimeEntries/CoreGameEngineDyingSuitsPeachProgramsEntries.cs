#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingSuitsPeachProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> _resolve;
    private CoreGameEngineDyingSuitsPeachProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingSuitsPeachProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> resolve) => new(resolve);
}
