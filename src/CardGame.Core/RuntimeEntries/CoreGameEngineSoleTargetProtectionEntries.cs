#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed partial class CoreGameEngineSoleTargetProtectionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _resolve;
    private CoreGameEngineSoleTargetProtectionEntries(Func<EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetProtectionEntries Create(Func<EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> resolve) => new(resolve);
}
