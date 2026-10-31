#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime))]
internal sealed partial class CoreGameEngineShanheBossCancelledMaterialsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> _resolve;
    private CoreGameEngineShanheBossCancelledMaterialsEntries(Func<EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheBossCancelledMaterialsEntries Create(Func<EngineOwners.CoreGameEngineShanheBossCancelledMaterialsRuntime> resolve) => new(resolve);
}
