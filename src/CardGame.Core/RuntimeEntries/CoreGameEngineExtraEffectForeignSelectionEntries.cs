#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime))]
internal sealed partial class CoreGameEngineExtraEffectForeignSelectionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> _resolve;
    private CoreGameEngineExtraEffectForeignSelectionEntries(Func<EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectForeignSelectionEntries Create(Func<EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> resolve) => new(resolve);
}
