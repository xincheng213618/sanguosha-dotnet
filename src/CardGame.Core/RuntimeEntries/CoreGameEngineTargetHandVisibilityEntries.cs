#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTargetHandVisibilityRuntime))]
internal sealed partial class CoreGameEngineTargetHandVisibilityEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> _resolve;
    private CoreGameEngineTargetHandVisibilityEntries(Func<EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetHandVisibilityEntries Create(Func<EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> resolve) => new(resolve);
}
