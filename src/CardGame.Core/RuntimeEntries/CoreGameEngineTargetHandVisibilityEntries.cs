#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime))]
internal sealed partial class CoreGameEngineTargetHandVisibilityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> _resolve;
    private CoreGameEngineTargetHandVisibilityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetHandVisibilityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> resolve) => new(resolve);
}
