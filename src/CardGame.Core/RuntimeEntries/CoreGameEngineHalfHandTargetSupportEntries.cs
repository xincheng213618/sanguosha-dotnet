#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandTargetSupportEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> _resolve;
    private CoreGameEngineHalfHandTargetSupportEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandTargetSupportEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandTargetSupportRuntime> resolve) => new(resolve);
}
