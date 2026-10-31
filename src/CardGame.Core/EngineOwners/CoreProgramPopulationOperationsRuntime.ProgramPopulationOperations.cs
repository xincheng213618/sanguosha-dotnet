#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPopulationOperationsRuntime
{
    internal void GrowProgramMaximumHpAndHp(ProgramSkillFrame frame, SkillProgramNumberExpression expression)
    {
        if (expression != SkillProgramNumberExpression.LivingFactionCount || frame.WindowContext?.Window != SkillProgramTriggerWindow.GameStarting)
            throw new InvalidOperationException("Population growth requires the game-start window.");
        var amount = _coreRuleQueriesOperations.GetLivingFactionCount();
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(frame, amount);
        if (amount == 0)
            return;
        var target = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        target.Hp += amount;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PopulationHpIncreasedEvent(frame.OwnerSeat, amount, target.Hp));
    }
}
