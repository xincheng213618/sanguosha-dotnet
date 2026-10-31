#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime))]
internal sealed partial class CoreGameEngineGuanduExtraEffectsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> _resolve;
    private CoreGameEngineGuanduExtraEffectsEntries(Func<EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduExtraEffectsEntries Create(Func<EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> resolve) => new(resolve);
}
