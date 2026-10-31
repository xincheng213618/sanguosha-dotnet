#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAcquisitionObserversEntries
{
    internal bool CanStartLostHpTargetSelection(CharacterState owner, SkillProgramActivation activation) => _resolve().CanStartLostHpTargetSelection(owner, activation);
    internal bool HasLostHpTargetSelection(ProgramSkillFrame frame) => _resolve().HasLostHpTargetSelection(frame);
    internal bool IsNamedAcquisitionProgramDying() => _resolve().IsNamedAcquisitionProgramDying();
    internal ProgramSkillFrame? NamedAcquisitionObserverRoot(long? damageWindowId = default) => _resolve().NamedAcquisitionObserverRoot(damageWindowId);
    internal bool NamedAcquisitionParentMatches(ProgramSkillFrame root) => _resolve().NamedAcquisitionParentMatches(root);
}
