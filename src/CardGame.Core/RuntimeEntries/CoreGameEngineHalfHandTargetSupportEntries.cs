#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime))]
internal sealed partial class CoreGameEngineHalfHandTargetSupportEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> _resolve;
    private CoreGameEngineHalfHandTargetSupportEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandTargetSupportEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> resolve) => new(resolve);
}
