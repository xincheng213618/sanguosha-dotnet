#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiKangMobileRuntime
{
    internal Dictionary<string, string> MobileJiKangArgs(ProgramSkillFrame f, string branch) => new()
    {
        ["program-action"] = "mobile-ji-kang",
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["branch"] = branch
    };
    internal IReadOnlyList<PromptChoice> MobileJiKangChoices(ProgramSkillFrame f)
    {
        var r = f.MobileJiKang!;
        var choices = new List<PromptChoice>();
        void Add(string key, string label, string branch, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => choices.Add(new(new($"mobile-ji-kang.{(f.Id)}.{(key)}"), label, cards, targets, MobileJiKangArgs(f, branch)));
        switch (r.Stage)
        {
            case MobileJiKangStage.Targets:
                var cap = Math.Min(r.InitialHp, MobileJiKangCosts(f.Id, f.OwnerSeat).SelectMany(c => c.Cards).Distinct().Count());
                foreach (var seat in r.Targets)
                    Add("unpick." + seat, "取消目标：" + _coreMatchParticipantsPlayersAccess._players[seat].Name, "unpick-target", [], [seat]);
                if (r.Targets.Count < cap)
                    foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => !r.Targets.Contains(p.Seat) && MobileJiKangTargetAllowed(r.Source, p.Seat)))
                        Add("target." + player.Seat, "选择目标：" + player.Name, "pick-target", [], [player.Seat]);
                if (r.Targets.Count > 0)
                    Add("targets-done", "完成目标选择", "targets-done", [], []);
                break;
            case MobileJiKangStage.Cards:
                foreach (var card in r.Cards)
                    Add("unpay." + card, "取消弃牌：" + _coreAdvancedLifecycleOperations.GetAdvancedCard(card).DisplayName, "unpick-card", [card], []);
                if (r.Cards.Count < r.Targets.Count)
                    foreach (var choice in MobileJiKangCosts(f.Id, f.OwnerSeat).Where(c => !r.Cards.Contains(c.Cards.Single())))
                        Add("card." + choice.Cards.Single(), choice.Label, "pick-card", choice.Cards, []);
                if (r.Cards.Count == r.Targets.Count)
                    Add("cards-done", "弃置所选牌", "cards-done", [], []);
                break;
            case MobileJiKangStage.Recipient:
                foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                    Add("recipient." + player.Seat, "令" + player.Name + "获得残韵", "recipient", [], [player.Seat]);
                Add("decline", "不授予技能", "decline", [], []);
                break;
            case MobileJiKangStage.Club:
                if (_coreMatchParticipantsPlayersAccess._players[r.Subject].IsAlive)
                    foreach (var card in MobileJiKangClubCards(r.Subject))
                        Add("club." + card.Id, "弃置" + _coreMatchParticipantsPlayersAccess._players[_cardZonesOperations._cardZones.GetLocation(card.Id).OwnerSeat!.Value].Name + "场上的" + card.DisplayName, "club", [card.Id], [_cardZonesOperations._cardZones.GetLocation(card.Id).OwnerSeat!.Value]);
                Add("decline", "不弃置场上梅花牌", "decline", [], []);
                break;
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal IEnumerable<Card> MobileJiKangClubCards(int actor) => _coreMatchParticipantsPlayersAccess._players.SelectMany(player => _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(player.Seat)))).Where(card => _cardZonesOperations._cardZones.GetLocation(card.Id).OwnerSeat is { } owner && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card) == Suit.Club && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    internal void PublishMobileJiKang(ProgramSkillFrame f)
    {
        var r = f.MobileJiKang!;
        var choices = MobileJiKangChoices(f);
        if (choices.Count == 0)
        {
            FinishMobileJiKang(f);
            return;
        }

        var chooser = r.Stage == MobileJiKangStage.Club ? r.Subject : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage switch
        {
            MobileJiKangStage.Targets => "选择至多体力值名其他角色。",
            MobileJiKangStage.Cards => "弃置与目标数相同数量的牌。",
            MobileJiKangStage.Recipient => "绝响：可以令一名其他角色获得残韵。",
            _ => "可以弃置场上一张梅花牌并获得绝响。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), chooser)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == MobileJiKangStage.Cards,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-ji-kang")]
    internal void AnswerMobileJiKang(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Ji Kang answer lost its original program frame.");
        var r = f.MobileJiKang ?? throw new InvalidOperationException("Ji Kang answer lost its issued receipt.");
        if (selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MobileJiKangChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Ji Kang answer lost its current actual card, participant or exact selection stage.");
        var branch = selected.Parameters["branch"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (branch)
        {
            case "pick-target":
                StepMobileJiKang(f, r with { Targets = r.Targets.Append(selected.Targets.Single()).ToArray() });
                break;
            case "unpick-target":
                StepMobileJiKang(f, r with { Targets = r.Targets.Where(seat => seat != selected.Targets.Single()).ToArray() });
                break;
            case "targets-done":
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.Cards, Targets = r.Targets.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, seat)).ToArray(), Cards = [] });
                break;
            case "pick-card":
                StepMobileJiKang(f, r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() });
                break;
            case "unpick-card":
                StepMobileJiKang(f, r with { Cards = r.Cards.Where(card => card != selected.Cards.Single()).ToArray() });
                break;
            case "cards-done":
                if (r.Targets.Count < 1 || r.Targets.Count > r.InitialHp || r.Cards.Count != r.Targets.Count || r.Targets.Any(seat => !MobileJiKangTargetAllowed(r.Source, seat)))
                    throw new InvalidOperationException("Qingxian lost its equal-card current living target payment.");
                PayMobileJiKang(f, r.Cards, f.OwnerSeat, f.OwnerSeat, MobileJiKangStage.CostChildren);
                break;
            case "recipient":
                var recipient = selected.Targets.Single();
                StepMobileJiKang(f, r with { Stage = MobileJiKangStage.GrantChildren, Subject = recipient, Paid = true });
                _coreProgramSkillChangesOperations.GrantProgramSkills2(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), recipient, ["mobile:canyun"]);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiKangProgressEvent(f.Id, "grant-canyun", r.Cursor, recipient, 1, 0));
                break;
            case "club":
                var card = selected.Cards.Single();
                var owner = _cardZonesOperations._cardZones.GetLocation(card).OwnerSeat!.Value;
                PayMobileJiKang(f, [card], r.Subject, owner, MobileJiKangStage.ClubChildren);
                break;
            case "decline":
                FinishMobileJiKang(f);
                return;
            default:
                throw new InvalidOperationException("Unknown Ji Kang selection branch.");
        }

        ContinueMobileJiKang(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileJiKangQingxian, SkillProgramEffectOp.ResolveMobileJiKangJuexiang)]
    internal PromptChoice SelectMobileJiKangAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var choices = decision.Choices;
        return f.MobileJiKang!.Stage switch
        {
            MobileJiKangStage.Targets => choices.FirstOrDefault(c => c.Parameters["branch"] == "pick-target") ?? choices.First(c => c.Parameters["branch"] == "targets-done"),
            MobileJiKangStage.Cards => choices.FirstOrDefault(c => c.Parameters["branch"] == "cards-done") ?? choices.First(c => c.Parameters["branch"] == "pick-card"),
            MobileJiKangStage.Recipient => choices.FirstOrDefault(c => c.Parameters["branch"] == "recipient") ?? choices[^1],
            _ => choices.FirstOrDefault(c => c.Parameters["branch"] == "club") ?? choices[^1]
        };
    }
}
