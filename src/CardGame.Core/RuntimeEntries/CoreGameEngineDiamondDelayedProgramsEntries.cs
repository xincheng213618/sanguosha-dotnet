#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime))]
internal sealed partial class CoreGameEngineDiamondDelayedProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> _resolve;
    private CoreGameEngineDiamondDelayedProgramsEntries(Func<EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiamondDelayedProgramsEntries Create(Func<EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime> resolve) => new(resolve);
}
