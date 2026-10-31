#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedEquipmentObserversEntries
{
    internal bool HasRoundGainedEquipmentDrawBeforeDamageObserver(long id) => _resolve().HasRoundGainedEquipmentDrawBeforeDamageObserver(id);
    internal bool HasRoundGainedEquipmentDrawDamageObserver(long id) => _resolve().HasRoundGainedEquipmentDrawDamageObserver(id);
    internal bool HasRoundGainedEquipmentDrawObserver(long id, bool before) => _resolve().HasRoundGainedEquipmentDrawObserver(id, before);
    internal bool IsRoundGainedEquipmentDrawDying() => _resolve().IsRoundGainedEquipmentDrawDying();
    internal bool RoundGainedEquipmentDrawFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().RoundGainedEquipmentDrawFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? RoundGainedEquipmentDrawObserverRoot() => _resolve().RoundGainedEquipmentDrawObserverRoot();
    internal bool RoundGainedEquipmentDrawStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().RoundGainedEquipmentDrawStructuralEdge(parent, child);
    internal bool TryAdvanceRoundGainedEquipmentDrawSubtree() => _resolve().TryAdvanceRoundGainedEquipmentDrawSubtree();
}
