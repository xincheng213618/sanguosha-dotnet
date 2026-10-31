#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime))]
internal sealed partial class CoreGameEnginePublicPileEquipmentSequencesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> _resolve;
    private CoreGameEnginePublicPileEquipmentSequencesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileEquipmentSequencesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> resolve) => new(resolve);
}
