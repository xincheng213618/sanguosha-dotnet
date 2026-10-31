#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardUseRolesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AddCurrentProgramCardUseTarget(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().AddCurrentProgramCardUseTarget(frame, targetSeat);
    internal bool CanBeProgramCardUseRoleTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeProgramCardUseRoleTarget(use, target);
    internal global::CardGame.Core.CardActionContext CloneRoleAction(global::CardGame.Core.CardActionContext action, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().CloneRoleAction(action, actorSeat, targets);
    internal void CompleteProgramCardUseTargetAddition(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets, int targetSeat) => _resolve().CompleteProgramCardUseTargetAddition(frame, use, targets, targetSeat);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetProgramCardUseRoleTargets(global::CardGame.Core.ProgramSkillFrame frame, bool ownerMayBeTarget = false) => _resolve().GetProgramCardUseRoleTargets(frame, ownerMayBeTarget);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetProgramCardUseRoleTargets2(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().GetProgramCardUseRoleTargets(ownerSeat, context);
    internal global::CardGame.Core.CardUseFrame GetProgramRoleCardUse(global::CardGame.Core.ProgramSkillFrame frame, bool ownerMayBeTarget = false) => _resolve().GetProgramRoleCardUse(frame, ownerMayBeTarget);
    internal void ReplaceCurrentProgramCardUseActor(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat) => _resolve().ReplaceCurrentProgramCardUseActor(frame, actorSeat);
    internal void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().UpdateProgramRoleCardUse(use, action);
}
