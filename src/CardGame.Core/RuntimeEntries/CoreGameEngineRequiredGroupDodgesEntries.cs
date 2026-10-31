#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime))]
internal sealed partial class CoreGameEngineRequiredGroupDodgesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime> _resolve;
    private CoreGameEngineRequiredGroupDodgesEntries(Func<EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRequiredGroupDodgesEntries Create(Func<EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime> resolve) => new(resolve);
}
