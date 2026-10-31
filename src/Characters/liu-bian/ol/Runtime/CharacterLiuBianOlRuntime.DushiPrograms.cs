#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBianOlRuntime
{
    internal SkillProgramStepOutcome DuShiProgramGrantSkill(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || recipient == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "毒逝的继承角色已失效，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var recipientState = _coreMatchParticipantsPlayersAccess._players[recipient];
        _coreCharacterHostOperations.AcquireRuntimeSkills(recipientState, active.SkillId, [active.SkillId]);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】由 {(recipientState.Name)} 获得。", active.OwnerSeat, recipient);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDuShiGrantEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, recipient));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.Continue;
    }
}
