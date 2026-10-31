#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiamondDelayedProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> _resolve;
    private CoreGameEngineDiamondDelayedProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiamondDelayedProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> resolve) => new(resolve);
}
