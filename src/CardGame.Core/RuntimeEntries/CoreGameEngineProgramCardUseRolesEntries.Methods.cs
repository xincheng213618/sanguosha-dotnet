#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardUseRolesEntries
{
    internal SkillProgramStepOutcome AddCurrentProgramCardUseTarget(ProgramSkillFrame frame, int targetSeat) => _resolve().AddCurrentProgramCardUseTarget(frame, targetSeat);
    internal bool CanBeProgramCardUseRoleTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeProgramCardUseRoleTarget(use, target);
    internal CardActionContext CloneRoleAction(CardActionContext action, int actorSeat, IReadOnlyList<int> targets) => _resolve().CloneRoleAction(action, actorSeat, targets);
    internal void CompleteProgramCardUseTargetAddition(ProgramSkillFrame frame, CardUseFrame use, IReadOnlyList<int> targets, int targetSeat) => _resolve().CompleteProgramCardUseTargetAddition(frame, use, targets, targetSeat);
    internal IReadOnlyList<int> GetProgramCardUseRoleTargets(ProgramSkillFrame frame, bool ownerMayBeTarget = false) => _resolve().GetProgramCardUseRoleTargets(frame, ownerMayBeTarget);
    internal IReadOnlyList<int> GetProgramCardUseRoleTargets2(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().GetProgramCardUseRoleTargets(ownerSeat, context);
    internal CardUseFrame GetProgramRoleCardUse(ProgramSkillFrame frame, bool ownerMayBeTarget = false) => _resolve().GetProgramRoleCardUse(frame, ownerMayBeTarget);
    internal void ReplaceCurrentProgramCardUseActor(ProgramSkillFrame frame, int actorSeat) => _resolve().ReplaceCurrentProgramCardUseActor(frame, actorSeat);
    internal void UpdateProgramRoleCardUse(CardUseFrame use, CardActionContext action) => _resolve().UpdateProgramRoleCardUse(use, action);
}
