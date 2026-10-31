#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangShiPingOlRuntime
{
    internal const string XingGuStoreReason = "skill-program.zhang-shi-ping.xinggu.initial";
    internal const string XingGuDiamondReason = "skill-program.zhang-shi-ping.xinggu.diamond";
    internal static bool IsXingGuMount(Card card) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhang_shi_ping_ol_ZhangShiPingXingGu.IsXingGuMount(card);
    internal IReadOnlyList<PromptChoice> XingGuPlacementChoices(int owner, string instance)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((owner, "ol:xinggu", instance))is not { } pile)
            return[];
        var result = new List<PromptChoice>();
        foreach (var card in _corePublicPileSourcesOperations.PublicPileCards(pile).Where(IsXingGuMount).OrderBy(c => c.Id))
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner))
                if (CanPlaceZhangShiPingMount(owner, target.Seat, card, pile.Location))
                    result.Add(new(new($"xinggu.{(owner)}.{(instance)}.{(card.Id)}.{(target.Seat)}"), $"将【{(card.DisplayName)}】置于{(target.Name)}的装备区", [card.Id], [target.Seat], new Dictionary<string, string> { ["program-action"] = "zhang-shi-ping-xinggu", ["card"] = card.Id.ToString(CultureInfo.InvariantCulture) }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal SkillProgramStepOutcome BeginXingGu(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "ol:xinggu" || f.InstructionIndex != 1 || f.ZhangShiPingXingGu is not null || f.WindowContext is not { } context || operation is not (SkillProgramEffectOp.InitializeXingGuMountPile or SkillProgramEffectOp.PlaceXingGuMountAndObtainDiamond) || context.Window != (operation == SkillProgramEffectOp.InitializeXingGuMountPile ? SkillProgramTriggerWindow.GameStarting : SkillProgramTriggerWindow.TurnEnding))
            throw new InvalidOperationException("Xinggu requires its exact zero-input game-start or own ending binding.");
        var initial = operation == SkillProgramEffectOp.InitializeXingGuMountPile;
        var pile = initial ? _corePublicPileSourcesOperations.EnsurePublicPileSource(f, 3) : _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((f.OwnerSeat, f.SkillId, f.SkillInstanceId)) ?? throw new InvalidOperationException("Xinggu lost its exact public mount pile source.");
        if (initial && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangShiPingXingGuStartedEvent>().Any(e => e.Source == ZhangShiPingSource(f) && e.Initial))
            throw new InvalidOperationException("Xinggu cannot initialize its public mount pile twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangShiPingXingGu = new(ZhangShiPingSource(f), f.GameplayHash, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, initial ? XingGuStage.InitialChildren : XingGuStage.Choose, pile.Location, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingXingGuStartedEvent(f.Id, ZhangShiPingSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, context.ParentFrameId, initial, pile.Location));
        if (initial)
        {
            for (var count = 0; count < 3; count++)
            {
                var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(IsXingGuMount).OrderBy(c => c.Id).ToArray();
                if (candidates.Length == 0)
                    break;
                var card = candidates[_randomOperations._random.Next(candidates.Length)];
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, pile.Location, new(XingGuStoreReason), movement =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { ZhangShiPingXingGu = current.ZhangShiPingXingGu!with { SequenceAfter = movement.Sequence } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingXingGuCardStoredEvent(f.Id, card.Id, movement.Sequence));
                });
            }
        }

        ContinueXingGu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PublishXingGu(ProgramSkillFrame f)
    {
        var choices = XingGuPlacementChoices(f.OwnerSeat, f.SkillInstanceId);
        if (choices.Count == 0)
        {
            FinishXingGu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "行贾：将一张坐骑牌置于其他角色的装备区。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhang-shi-ping-xinggu")]
    internal void AnswerXingGu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangShiPingXingGu: { Stage: XingGuStage.Choose } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], XingGuPlacementChoices(f.OwnerSeat, f.SkillInstanceId).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Xinggu lost its exact physical public mount or legal other recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
        var target = choice.Targets.Single();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ZhangShiPingXingGu = r with { Stage = XingGuStage.PlacementChildren, CardId = card.Id, RecipientSeat = target, SequenceBefore = before, SequenceAfter = before } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingXingGuPlacementIssuedEvent(f.Id, card.Id, target, before));
        PlaceZhangShiPingMount(f, target, card, r.PileLocation);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhangShiPingXingGu = f.ZhangShiPingXingGu!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueXingGu(f.Id);
    }

    internal bool DrainXingGu(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueXingGu(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhangShiPingXingGu: { } r } f && f.Id == id)
        {
            if (DrainXingGu(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ZhangShiPingXingGu!;
            if (r.Stage is XingGuStage.InitialChildren or XingGuStage.DiamondChildren || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishXingGu(f);
                return;
            }

            if (r.Stage == XingGuStage.Choose)
            {
                PublishXingGu(f);
                return;
            }

            if (r.Stage != XingGuStage.PlacementChildren)
                throw new InvalidOperationException("Xinggu encountered an unknown paid continuation.");
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => c.Suit == Suit.Diamond).OrderBy(c => c.Id).ToArray();
            var card = candidates.Length == 0 ? null : candidates[_randomOperations._random.Next(candidates.Length)];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangShiPingXingGu = r with { Stage = XingGuStage.DiamondChildren, DiamondCardId = card?.Id, SequenceBefore = before, SequenceAfter = before } });
            if (card is not null)
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(XingGuDiamondReason), movement =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { ZhangShiPingXingGu = current.ZhangShiPingXingGu!with { SequenceAfter = movement.Sequence } });
                });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ZhangShiPingXingGu!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangShiPingXingGuDiamondIssuedEvent(id, f.OwnerSeat, card?.Id, before, r.SequenceAfter));
        }
    }

    internal void FinishXingGu(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangShiPingXingGu = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26610)]
    internal bool ResumeXingGu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangShiPingXingGu: not null } f || f.Id != id)
            return false;
        ContinueXingGu(id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PlaceXingGuMountAndObtainDiamond)]
    internal PromptChoice AiXingGu(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => (global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]) ? 10 : 0) + (_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Any(e => _coreEquipmentContentOperations.EquipmentRules.Get(e.Kind).Slot == _coreEquipmentContentOperations.EquipmentRules.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).Slot) ? 0 : 2)).First();
}
