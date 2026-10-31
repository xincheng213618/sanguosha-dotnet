#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramContinuationPermissionsEntries
{
    internal bool CanContinueProgramAfterOwnerDeath(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueProgramAfterOwnerDeath(frame);
    internal bool CanContinueProgramAfterSkillLoss(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueProgramAfterSkillLoss(frame);
}
