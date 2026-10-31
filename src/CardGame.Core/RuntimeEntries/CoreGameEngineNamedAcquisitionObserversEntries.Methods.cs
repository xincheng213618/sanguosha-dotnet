#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAcquisitionObserversEntries
{
    internal bool CanStartLostHpTargetSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanStartLostHpTargetSelection(owner, activation);
    internal bool HasLostHpTargetSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasLostHpTargetSelection(frame);
    internal bool IsNamedAcquisitionProgramDying() => _resolve().IsNamedAcquisitionProgramDying();
    internal global::CardGame.Core.ProgramSkillFrame? NamedAcquisitionObserverRoot(long? damageWindowId = default) => _resolve().NamedAcquisitionObserverRoot(damageWindowId);
    internal bool NamedAcquisitionParentMatches(global::CardGame.Core.ProgramSkillFrame root) => _resolve().NamedAcquisitionParentMatches(root);
}
