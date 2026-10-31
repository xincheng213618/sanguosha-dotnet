#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsEntries
{
    internal bool CanContinueProgramAfterOwnerDeath(ProgramSkillFrame frame) => _resolve().CanContinueProgramAfterOwnerDeath(frame);
    internal bool CanContinueProgramAfterSkillLoss(ProgramSkillFrame frame) => _resolve().CanContinueProgramAfterSkillLoss(frame);
}
