#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime))]
internal sealed partial class CoreGameEnginePublicPileEquipmentSequencesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> _resolve;
    private CoreGameEnginePublicPileEquipmentSequencesEntries(Func<EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileEquipmentSequencesEntries Create(Func<EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> resolve) => new(resolve);
}
