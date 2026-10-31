#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTargetCardPreselectionRuntime))]
internal sealed partial class CoreGameEngineTargetCardPreselectionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> _resolve;
    private CoreGameEngineTargetCardPreselectionEntries(Func<EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetCardPreselectionEntries Create(Func<EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> resolve) => new(resolve);
}
