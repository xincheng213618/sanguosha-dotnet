#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedEquipmentObserversEntries
{
    internal bool IsRoundGainedEquipmentDrawDying() => _resolve().IsRoundGainedEquipmentDrawDying();
    internal bool RoundGainedEquipmentDrawFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().RoundGainedEquipmentDrawFirstChild(frame, child);
    internal ProgramSkillFrame? RoundGainedEquipmentDrawObserverRoot() => _resolve().RoundGainedEquipmentDrawObserverRoot();
    internal bool RoundGainedEquipmentDrawStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().RoundGainedEquipmentDrawStructuralEdge(parent, child);
    internal bool TryAdvanceRoundGainedEquipmentDrawSubtree() => _resolve().TryAdvanceRoundGainedEquipmentDrawSubtree();
}
