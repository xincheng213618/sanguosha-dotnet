#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime))]
internal sealed partial class CoreGameEngineHalfHandTargetSupportEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> _resolve;
    private CoreGameEngineHalfHandTargetSupportEntries(Func<EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandTargetSupportEntries Create(Func<EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> resolve) => new(resolve);
}
