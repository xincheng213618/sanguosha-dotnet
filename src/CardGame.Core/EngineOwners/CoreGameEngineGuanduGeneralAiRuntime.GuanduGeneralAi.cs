#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduGeneralAiRuntime
{
    [ProgramAiChoice(SkillProgramEffectOp.ResolveGuanduShicai, SkillProgramEffectOp.ResolveGuanduZezhu, SkillProgramEffectOp.ResolveGuanduJieliang, SkillProgramEffectOp.ResolveGuanduJieliangClaim, SkillProgramEffectOp.ResolveGuanduCangchu, SkillProgramEffectOp.ResolveGuanduSushou, SkillProgramEffectOp.ResolveGuanduPolu, SkillProgramEffectOp.ResolveGuanduThunderChariot, SkillProgramEffectOp.ResolveGuanduChoulue, SkillProgramEffectOp.ResolveGuanduHengjiang, SkillProgramEffectOp.ResolveGuanduHengjiangReward, SkillProgramEffectOp.ResolveGuanduXiying, SkillProgramEffectOp.ResolveGuanduBeizhan, SkillProgramEffectOp.ResolveGuanduYuanlve, SkillProgramEffectOp.ResolveGuanduMoushi, SkillProgramEffectOp.ResolveGuanduZhongjie, SkillProgramEffectOp.ResolveGuanduSuishiDying, SkillProgramEffectOp.ResolveGuanduSuishiDeath, SkillProgramEffectOp.ResolveGuanduFuyuan)]
    internal PromptChoice SelectGuanduGeneralAi(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.SkillId == "guandu:yuanlve" && f.HuangQuan is { Stage: "offer" } pending)
        {
            var actions = _characterHuangQuanOlOperations.JianjiActions(f);
            double NativeValue(PromptChoice c)
            {
                if (c.Parameters.GetValueOrDefault("option") == "decline")
                    return 0;
                var a = actions[int.Parse(c.Parameters["option"], CultureInfo.InvariantCulture)];
                var harmful = a.Kind is LegalActionKind.Slash or LegalActionKind.Duel or LegalActionKind.FireAttack or LegalActionKind.Snatch or LegalActionKind.Dismantlement or LegalActionKind.UnexpectedAssault;
                if (harmful)
                    return a.TargetSeats.Any(s => _coreGuanduActivitiesOperations.GuanduAllies(pending.RecipientSeat, s)) ? -100 : 20;
                return a.Kind == LegalActionKind.Intoxication && a.TargetSeats.Any(s => _coreGuanduActivitiesOperations.GuanduAllies(pending.RecipientSeat, s)) ? -50 : 10;
            }

            return decision.Choices.OrderByDescending(NativeValue).First();
        }

        var r = f.GuanduGeneral;
        if (r is null)
            return decision.Choices[0];
        double Value(PromptChoice c)
        {
            var option = c.Parameters.GetValueOrDefault("option");
            var keep = c.Cards.Count == 1 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]) : 0;
            var target = c.Targets.FirstOrDefault(-1);
            var allied = target >= 0 && _coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, target);
            if (option == "skip")
                return -10;
            return r.Stage switch
            {
                "shicai-cost" => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()is { } top ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(top, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) - keep : -20,
                "jieliang-cost" => _coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, r.TargetSeat) ? -30 : 30 - keep,
                "jieliang-claim" => keep,
                "zezhu-take" => option == "take-hand" ? 7 : keep + 3,
                "zezhu-give" => -keep,
                "sushou-give" => (allied && target != f.OwnerSeat ? 50 : -50) - VaryGuanduRecipientHand(target) - keep,
                "xiying-cost" => 50 - keep,
                "xiying-answer" => option == "ban" ? _coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, decision.PlayerSeat) ? 10 : -20 : -keep,
                "beizhan-target" => allied ? 50 + _coreMatchParticipantsPlayersAccess._players[target].MaxHp - VaryGuanduRecipientHand(target) : -50,
                "zhongjie-target" => allied ? 60 - _coreMatchParticipantsPlayersAccess._players[target].Hp : -50,
                "yuanlve-give" or "moushi-give" => (allied && target != f.OwnerSeat ? 60 : target == f.OwnerSeat ? 10 : -50) - VaryGuanduRecipientHand(target) - keep,
                "choulue-target" => allied ? 30 + VaryGuanduRecipientHand(target) : 0,
                "choulue-gift" => _coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, decision.PlayerSeat) ? 30 - keep : -30,
                _ => -keep
            };
        }

        return decision.Choices.OrderByDescending(Value).First();
    }

    internal int VaryGuanduRecipientHand(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count : 0;
}
