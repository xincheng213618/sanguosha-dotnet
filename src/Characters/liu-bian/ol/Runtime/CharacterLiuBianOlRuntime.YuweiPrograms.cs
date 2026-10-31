#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBianOlRuntime
{
    internal bool IsYuWeiLimitDoubled(int ownerSeat) => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat) && ownerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat]) == "qun" && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).ProgramInstances.Any(instance => instance.Program.Triggers.Any(trigger => ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.YuWeiMarkActiveTurn)));
    internal SkillProgramStepOutcome YuWeiProgramMarkActiveTurn(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】生效：其他群势力角色的回合内，诗怨改为每回合每项限两次。", owner.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYuWeiActiveTurnEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.Continue;
    }
}
