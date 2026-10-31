#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed partial class CoreGameEngineSoleTargetQuarantineEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _resolve;
    private CoreGameEngineSoleTargetQuarantineEntries(Func<EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetQuarantineEntries Create(Func<EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> resolve) => new(resolve);
}
