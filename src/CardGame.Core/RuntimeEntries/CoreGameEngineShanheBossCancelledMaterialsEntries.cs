#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime))]
internal sealed partial class CoreGameEngineShanheBossCancelledMaterialsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> _resolve;
    private CoreGameEngineShanheBossCancelledMaterialsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheBossCancelledMaterialsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> resolve) => new(resolve);
}
