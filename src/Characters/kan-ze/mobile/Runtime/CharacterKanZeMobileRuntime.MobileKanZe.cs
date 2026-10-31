#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKanZeMobileRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 75800)]
    internal void ConsumeMobileKuanshiFirstPrevention(KuanshiDamagePreventedEvent fact)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players)
        {
            var grant = owner.KuanshiGrants.SingleOrDefault(g => g.FrameId == fact.GrantFrameId && g.Source.SkillId == "mobile:kuanshi");
            if (grant is null)
                continue;
            owner.KuanshiGrants = owner.KuanshiGrants.Where(g => g.FrameId != grant.FrameId).ToArray();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuanshiExpiredEvent(grant.FrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber, "mobile-first-heavy-damage"));
        }
    }
}
