#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime))]
internal sealed partial class CoreGameEngineSoleTargetProtectionObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> _resolve;
    private CoreGameEngineSoleTargetProtectionObserversEntries(Func<EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetProtectionObserversEntries Create(Func<EngineOwners.CoreGameEngineSoleTargetProtectionObserversRuntime> resolve) => new(resolve);
}
