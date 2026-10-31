#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuJiMobileShiRuntime
{
    internal const string MobileShiDaozhuanSkill = "mobile:shi-daozhuan";
    // 道转 uses are recorded per accepted conversion. The same facts drive the
    // per-round-per-name ledger, the shared one-per-turn ceiling, the round
    // disable after an own-turn use and audits.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 67211)]
    internal void RecordMobileShiDaozhuanUse(CardActionAcceptedEvent accepted)
    {
        var action = accepted.Action;
        if (action.ConversionChain.Count == 0)
            return;
        var source = action.ConversionChain.FirstOrDefault(item => item.SkillId == MobileShiDaozhuanSkill);
        if (source is null)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        var program = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program;
        if (!owner.IsAlive || program is null || program.ViewAs.All(rule => rule.Id != source.BindingId))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiDaozhuanUseRecordedEvent>().Any(item => item.ActionId == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileShiDaozhuanUseRecordedEvent(source.OwnerSeat, MobileShiYuJiComposition.Name(action.EffectiveKind), action.ActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, action.ActorSeat == source.OwnerSeat && source.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 67211)]
    internal bool MobileShiDaozhuanConversionUnavailable(ProgramConfiguredViewAsQuery query)
    {
        if (query.Instance.SkillId != MobileShiDaozhuanSkill)
            return false;
        var owner = query.Owner;
        var uses = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<MobileShiDaozhuanUseRecordedEvent>().Where(item => item.OwnerSeat == owner.Seat).ToArray();
        // 每回合限一次（跨牌名）。
        if (uses.Any(item => item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber))
            return true;
        // 每轮每基本牌名限一次（普通杀与属性杀同名）。
        var name = MobileShiYuJiComposition.Name(query.Rule.OutputKind);
        if (uses.Any(item => item.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && item.Name == name))
            return true;
        // 本轮失效：本回合自己发动过道转后，直至下轮开始不可再用。
        return uses.Any(item => item.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && item.OwnerIsTurnPlayer);
    }
}
