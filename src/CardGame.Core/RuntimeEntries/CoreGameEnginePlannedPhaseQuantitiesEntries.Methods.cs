#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesEntries
{
    internal void AdjustProgramNormalDrawByPlannedCount(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AdjustProgramNormalDrawByPlannedCount(frame);
    internal int GetDiscardQuotaMultiplier(global::CardGame.Core.CharacterState owner) => _resolve().GetDiscardQuotaMultiplier(owner);
}
