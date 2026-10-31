#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesEntries
{
    internal void AdjustProgramNormalDrawByPlannedCount(ProgramSkillFrame frame) => _resolve().AdjustProgramNormalDrawByPlannedCount(frame);
    internal int GetDiscardQuotaMultiplier(CharacterState owner) => _resolve().GetDiscardQuotaMultiplier(owner);
}
