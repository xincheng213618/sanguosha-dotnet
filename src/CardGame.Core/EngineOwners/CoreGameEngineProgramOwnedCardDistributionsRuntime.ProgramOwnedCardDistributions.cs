#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramOwnedCardDistributionsRuntime
{
    internal SkillProgramStepOutcome DistributeProgramOwnedCards(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, string sourceBind, SkillProgramTargetKind targetKind, bool allowDeclineBeforeFirst, CardMoveReason reason)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.OwnedCardDistribution is not null)
            throw new InvalidOperationException("An owned-card distribution is already active.");
        var requiredCount = _coreProgramLifecycleOperations.GetProgramCardSet(active, sourceBind).CardIds.Count;
        if (requiredCount <= 0)
            return SkillProgramStepOutcome.Continue;
        var availableCount = zones.Sum(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, active.OwnerSeat)).Count);
        var targetSeats = _coreProgramLifecycleOperations.GetProgramTargetSeats(active.OwnerSeat, targetKind);
        if (availableCount < requiredCount || targetSeats.Count == 0)
            return SkillProgramStepOutcome.Continue;
        active = active with
        {
            OwnedCardDistribution = new ProgramOwnedCardDistribution(active.OwnerSeat, sourceBind, requiredCount, Array.AsReadOnly(zones.ToArray()), targetKind, allowDeclineBeforeFirst, [], [])
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramOwnedCardDistribution(active, reason);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramOwnedCardDistribution(ProgramSkillFrame frame, CardMoveReason reason)
    {
        var distribution = frame.OwnedCardDistribution ?? throw new InvalidOperationException("Missing owned-card distribution state.");
        var owner = _coreMatchParticipantsPlayersAccess._players[distribution.CardOwnerSeat];
        var cards = distribution.Zones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, owner.Seat)).Select(card => (Card: card, Zone: zone))).OrderBy(item => item.Zone).ThenBy(item => item.Card.Id).ToArray();
        var targets = _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, distribution.TargetKind).Select(seat => _coreMatchParticipantsPlayersAccess._players[seat]).ToArray();
        var remaining = distribution.RequiredCount - distribution.GivenCardIds.Count;
        if (cards.Length < remaining || targets.Length == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OwnedCardDistribution = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var choices = cards.SelectMany(item => targets.Select(target => new PromptChoice(new ChoiceId($"program-owned-distribution.frame-{(frame.Id)}.index-{(distribution.GivenCardIds.Count)}.card-{(item.Card.Id)}.target-{(target.Seat)}"), $"将{(ProgramOwnedDistributionZoneName(item.Zone))}【{(item.Card.DisplayName)}】交给 {(target.Name)}。", [item.Card.Id], [target.Seat], new Dictionary<string, string> { ["program-action"] = "distribute-owned-card", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["source-bind"] = distribution.SourceBind, ["source-zone"] = item.Zone.ToString(), ["target-kind"] = distribution.TargetKind.ToString(), ["distribution-index"] = distribution.GivenCardIds.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), ["move-reason"] = reason.Value }))).ToList();
        if (distribution.AllowDeclineBeforeFirst && distribution.GivenCardIds.Count == 0)
        {
            choices.Add(new PromptChoice(new ChoiceId($"program-owned-distribution.frame-{(frame.Id)}.decline"), "不分配卡牌，结束此项结算。", [], [], new Dictionary<string, string> { ["program-action"] = "decline-owned-card-distribution", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["source-bind"] = distribution.SourceBind, ["target-kind"] = distribution.TargetKind.ToString(), ["distribution-index"] = "0", ["move-reason"] = reason.Value }));
        }

        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, owner.Seat, distribution.GivenCardIds.Count == 0 ? $"你可以分组分配 {(remaining)} 张自己的牌；一旦交出第一张，必须交足冻结数量。" : $"还须分配 {(remaining)} 张自己的牌。", cards.Select(item => item.Card.Id).ToArray(), targets.Select(target => target.Seat).ToArray(), SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = owner.Seat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 分配卡牌", skill.Description),
            Choices = _coreCardDistributionPresentationOperations.PresentCardDistribution(frame, choices, "owned", remaining)
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    internal void ResolveProgramOwnedCardDistribution(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected)
    {
        var distribution = frame.OwnedCardDistribution ?? throw new InvalidOperationException("Missing owned-card distribution state.");
        var action = selected.Parameters.GetValueOrDefault("program-action");
        var expectedReason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        if (effect.Op != SkillProgramEffectOp.DistributeOwnedCards || effect.SourceBind != distribution.SourceBind || effect.TargetKind != distribution.TargetKind || effect.AllowDeclineBeforeFirst != distribution.AllowDeclineBeforeFirst || !effect.Zones.SequenceEqual(distribution.Zones) || selected.Parameters.GetValueOrDefault("source-bind") != distribution.SourceBind || selected.Parameters.GetValueOrDefault("target-kind") != distribution.TargetKind.ToString() || selected.Parameters.GetValueOrDefault("move-reason") != expectedReason.Value || selected.Parameters.GetValueOrDefault("distribution-index") != distribution.GivenCardIds.Count.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The owned-card distribution choice does not match its suspended instruction.");
        if (action == "decline-owned-card-distribution")
        {
            if (!distribution.AllowDeclineBeforeFirst || distribution.GivenCardIds.Count != 0 || selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The owned-card distribution can no longer be declined.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OwnedCardDistribution = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (action != "distribute-owned-card" || selected.Cards.Count != 1 || selected.Targets.Count != 1 || !Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone) || !distribution.Zones.Contains(zone))
            throw new InvalidOperationException("The owned-card distribution requires one legal card and target.");
        var cardId = selected.Cards.Single();
        var targetSeat = selected.Targets.Single();
        if (!_coreProgramLifecycleOperations.GetProgramTargetSeats(distribution.CardOwnerSeat, distribution.TargetKind).Contains(targetSeat))
            throw new InvalidOperationException("The owned-card distribution target is no longer legal.");
        var from = new CardLocation(zone, distribution.CardOwnerSeat);
        if (_cardZonesOperations._cardZones.GetLocation(cardId) != from)
            throw new InvalidOperationException("The distributed card is no longer in the selected owned zone.");
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(item => item.Id == cardId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Processing, expectedReason);
        _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(card, CardLocation.Hand(targetSeat), expectedReason, _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, distribution.CardOwnerSeat, targetSeat, [cardId], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        var givenIds = distribution.GivenCardIds.Append(cardId).ToArray();
        var targetSeats = distribution.TargetSeats.Append(targetSeat).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramOwnedCardDistributedEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), distribution.CardOwnerSeat, targetSeat, cardId, givenIds.Length, distribution.RequiredCount));
        _coreMatchLogOperations.AddLog("SkillCardGiven", $"{(_coreMatchParticipantsPlayersAccess._players[distribution.CardOwnerSeat].Name)} 通过【{(_contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Name)}】交给 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 一张牌。", distribution.CardOwnerSeat, targetSeat);
        if (givenIds.Length >= distribution.RequiredCount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { OwnedCardDistribution = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        frame = frame with
        {
            OwnedCardDistribution = distribution with
            {
                GivenCardIds = Array.AsReadOnly(givenIds),
                TargetSeats = Array.AsReadOnly(targetSeats)
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        PublishProgramOwnedCardDistribution(frame, expectedReason);
    }

    [ProgramAiChoice(SkillProgramEffectOp.DistributeOwnedCards)]
    internal PromptChoice SelectAiProgramOwnedCardDistribution(PendingDecision decision, ProgramSkillFrame frame)
    {
        var distribution = frame.OwnedCardDistribution ?? throw new InvalidOperationException("Missing AI owned-card distribution state.");
        var owner = _coreMatchParticipantsPlayersAccess._players[distribution.CardOwnerSeat];
        var giveChoices = decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "distribute-owned-card").ToArray();
        var decline = decision.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "decline-owned-card-distribution");
        var allies = giveChoices.Where(choice => choice.Targets.Count == 1 && AreProgramDistributionAllies(owner, _coreMatchParticipantsPlayersAccess._players[choice.Targets[0]])).ToArray();
        if (allies.Length == 0 && decline is not null)
            return decline;
        var candidates = allies.Length > 0 ? allies : giveChoices;
        return candidates.OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(distribution.Zones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, distribution.CardOwnerSeat))).Single(card => card.Id == choice.Cards.Single()), owner)).ThenBy(choice => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()]).Count).ThenBy(choice => choice.Targets.Single()).ThenBy(choice => choice.Cards.Single()).First();
    }

    internal static bool AreProgramDistributionAllies(CharacterState owner, CharacterState target) => owner.TeamId is not null || target.TeamId is not null ? owner.TeamId is not null && owner.TeamId == target.TeamId : owner.Role switch
    {
        Role.Lord => target.Role is Role.Lord or Role.Loyalist,
        Role.Loyalist => target.Role is Role.Lord or Role.Loyalist,
        Role.Rebel => target.Role == Role.Rebel,
        _ => false
    };

    internal static string ProgramOwnedDistributionZoneName(CardZoneKind zone) => RuntimeSkills.RuntimeRules_Core_ProgramOwnedCardDistributions.ProgramOwnedDistributionZoneName(zone);
}
