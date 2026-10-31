#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeckProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> _resolve;
    private CoreGameEngineDeckProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeckProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckProgramsRuntime> resolve) => new(resolve);
}
