#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedHandRefreshProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> _resolve;
    private CoreGameEngineCappedHandRefreshProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedHandRefreshProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> resolve) => new(resolve);
}
