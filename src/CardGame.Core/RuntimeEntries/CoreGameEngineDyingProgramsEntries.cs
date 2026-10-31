#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingProgramsRuntime> _resolve;
    private CoreGameEngineDyingProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingProgramsRuntime> resolve) => new(resolve);
}
