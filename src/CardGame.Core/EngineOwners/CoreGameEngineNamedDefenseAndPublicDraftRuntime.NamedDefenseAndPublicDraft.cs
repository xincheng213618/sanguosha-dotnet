#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedDefenseAndPublicDraftRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.INamedDefenseAndPublicDraftHost.BeginNamedTargetDefense(ProgramSkillFrame frame, string ledgerId, IReadOnlyList<CardKind> names) => BeginNamedTargetDefense(frame, ledgerId, names);
    SkillProgramStepOutcome global::CardGame.Core.INamedDefenseAndPublicDraftHost.BeginLowHandPublicDraft(ProgramSkillFrame frame) => BeginLowHandPublicDraft(frame);
    internal static string DefenseNameUsage(string ledger, CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_NamedDefenseAndPublicDraft.DefenseNameUsage(ledger, kind);
    internal CardUseFrame NamedDefenseUse(ProgramSkillFrame frame)
    {
        var context = frame.WindowContext;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } use } || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.EffectiveKind) || use.EventTargetSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(parent => parent.Id == use.ParentCardUseFrameId)is not { Action: { } action } parent || action.ActionId != use.CardActionId || action.ActorSeat != use.ActorSeat || action.EffectiveKind != use.EffectiveKind || !parent.TargetSeats.Contains(frame.OwnerSeat))
            throw new InvalidOperationException("A named defense requires its target's frozen actual Slash.");
        return parent;
    }

    internal SkillProgramStepOutcome BeginNamedTargetDefense(ProgramSkillFrame frame, string ledgerId, IReadOnlyList<CardKind> names)
    {
        _ = NamedDefenseUse(frame);
        var available = names.Where(kind => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, DefenseNameUsage(ledgerId, kind), SkillUsageScope.Game) == 0).ToArray();
        if (available.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NamedTargetDefense = new(ledgerId) });
        PublishNamedDraftPrompt(frame, frame.OwnerSeat, "声明一种尚未声明的非装备牌名。", available.Select(kind => NamedDraftChoice(frame, "named-defense-name", CardCatalog.Get(kind).DisplayName, name: kind)).ToArray(), true);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishNamedDefensePayment(ProgramSkillFrame frame)
    {
        var defense = frame.NamedTargetDefense!;
        var parent = NamedDefenseUse(frame);
        var actor = parent.Action!.ActorSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[actor].IsAlive)
        {
            FinishNamedDefense(frame, true);
            return;
        }

        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[actor])).Where(card => global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(card.Kind) == defense.Name).OrderBy(card => card.Id).ToArray();
        var choices = cards.Select(card => NamedDraftChoice(frame, "named-defense-pay", $"弃置【{(card.DisplayName)}】并获得对方一张牌", card.Id)).ToList();
        choices.Add(NamedDraftChoice(frame, "named-defense-decline", "令此杀对目标无效"));
        PublishNamedDraftPrompt(frame, actor, $"弃置一张【{(CardCatalog.Get(defense.Name!.Value).DisplayName)}】，或令此杀对目标无效。", choices, true);
    }

    internal void PublishNamedDefenseTake(ProgramSkillFrame frame)
    {
        var parent = NamedDefenseUse(frame);
        var actor = parent.Action!.ActorSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            FinishNamedDefense(frame, false);
            return;
        }

        var choices = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select((card, slot) => NamedDraftChoice(frame, "named-defense-take-hand", $"获得第 {(slot + 1)} 张手牌", slot: slot)).ToList();
        choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(card => NamedDraftChoice(frame, "named-defense-take-equipment", $"获得【{(card.DisplayName)}】", card.Id)));
        if (choices.Count == 0)
        {
            FinishNamedDefense(frame, false);
            return;
        }

        PublishNamedDraftPrompt(frame, actor, "获得目标一张手牌或装备牌。", choices, true);
    }

    internal void FinishNamedDefense(ProgramSkillFrame frame, bool nullify)
    {
        if (nullify)
        {
            var parent = NamedDefenseUse(frame);
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(parent.Id, frame.OwnerSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(parent.Id, parent.Action!.ActorSeat, frame.OwnerSeat, parent.CardKind, CardEffectSkipReason.SkillNullified));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NamedTargetDefense = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal SkillProgramStepOutcome BeginLowHandPublicDraft(ProgramSkillFrame frame)
    {
        var seats = frame.WindowContext?.Facts?.LowHandPopulationSeats ?? throw new InvalidOperationException("A population draft requires its frozen end-phase population.");
        if (seats.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicHandDraft = new(seats, []) });
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], seats.Count, log: true, new CardMoveReason($"skill-program.{(frame.SkillId)}.population-draw"));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1020)]
    internal bool TryResumeNamedDefenseAndPublicDraft(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.PendingMovementContinuation is not null)
            return false;
        if (frame.NamedTargetDefense is { Stage: 2 })
        {
            PublishNamedDefenseTake(frame);
            return true;
        }

        if (frame.NamedTargetDefense is { Stage: 3 })
        {
            FinishNamedDefense(frame, false);
            return true;
        }

        if (frame.PublicHandDraft is { } draft)
        {
            if (draft.Stage == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicHandDraft = draft with { Stage = 1 } });
                PublishPublicDraftSelection(frame);
                return true;
            }

            if (draft.Stage == 3)
            {
                ContinuePublicHandDraft(frame);
                return true;
            }
        }

        return false;
    }

    internal void PublishPublicDraftSelection(ProgramSkillFrame frame)
    {
        var draft = frame.PublicHandDraft!;
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Where(card => !draft.CardIds.Contains(card.Id)).ToArray();
        if (cards.Length == 0 && draft.CardIds.Count < draft.RecipientSeats.Count)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicHandDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        PublishNamedDraftPrompt(frame, frame.OwnerSeat, $"选择展示的手牌（{(draft.CardIds.Count)}/{(draft.RecipientSeats.Count)}）。", cards.Select(card => NamedDraftChoice(frame, "public-draft-show", $"展示【{(card.DisplayName)}】（{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))} {(card.RankText)}）", card.Id)).ToArray(), true);
    }

    internal void ContinuePublicHandDraft(ProgramSkillFrame frame)
    {
        var draft = frame.PublicHandDraft!;
        var cards = draft.CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat)).ToArray();
        var cursor = draft.Cursor;
        while (cursor < draft.RecipientSeats.Count && !_coreMatchParticipantsPlayersAccess._players[draft.RecipientSeats[cursor]].IsAlive)
            cursor++;
        if (cursor >= draft.RecipientSeats.Count || cards.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicHandDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicHandDraft = draft with { CardIds = cards, Cursor = cursor } });
        var recipient = draft.RecipientSeats[cursor];
        PublishNamedDraftPrompt(frame, recipient, "从展示的牌中选择获得一张。", cards.Select(id =>
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(card => card.Id == id);
            return NamedDraftChoice(frame, "public-draft-gain", $"获得【{(card.DisplayName)}】（{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))} {(card.RankText)}）", id);
        }).ToArray(), false);
    }

    internal static PromptChoice NamedDraftChoice(ProgramSkillFrame frame, string action, string label, int? card = null, int? slot = null, CardKind? name = null, int? seat = null) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_NamedDefenseAndPublicDraft.NamedDraftChoice(frame, action, label, card, slot, name, seat);
    internal void PublishNamedDraftPrompt(ProgramSkillFrame frame, int chooser, string text, IReadOnlyList<PromptChoice> choices, bool privacy)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(choice => choice.Cards).ToArray(), choices.SelectMany(choice => choice.Targets).ToArray(), SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = privacy,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswerPrefix("named-defense-", "public-draft-")]
    internal void ResolveNamedDefenseAndDraftChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The named/draft choice lost its program.");
        if (choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString())
            throw new InvalidOperationException("The named/draft choice belongs to another frame.");
        var action = choice.Parameters["program-action"];
        if (frame.NamedTargetDefense is { } defense)
        {
            var parent = NamedDefenseUse(frame);
            var actor = parent.Action!.ActorSeat;
            if (defense.Stage == 0 && action == "named-defense-name")
            {
                var name = Enum.Parse<CardKind>(choice.Parameters["card-name"]);
                if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, DefenseNameUsage(defense.LedgerId, name), SkillUsageScope.Game, 1))
                    throw new InvalidOperationException("The declared card name was already used.");
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDefenseNameDeclaredEvent(frame.Id, frame.SkillId, frame.OwnerSeat, name));
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { NamedTargetDefense = defense with { Name = name, Stage = 1 } });
                PublishNamedDefensePayment(frame);
                return;
            }

            if (defense.Stage == 1 && action == "named-defense-decline")
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                FinishNamedDefense(frame, true);
                return;
            }

            if (defense.Stage == 1 && action == "named-defense-pay")
            {
                var id = choice.Cards.Single();
                var from = _cardZonesOperations._cardZones.GetLocation(id);
                if (from.OwnerSeat != actor || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                    throw new InvalidOperationException("The named cost is not the actor's owned card.");
                var card = _cardZonesOperations._cardZones.CardsAt(from).Single(card => card.Id == id);
                if (global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(card.Kind) != defense.Name)
                    throw new InvalidOperationException("The real cost has another name.");
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { NamedTargetDefense = defense with { Stage = 2 } });
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, actor, card, from, new CardMoveReason($"skill-program.{(frame.SkillId)}.named-cost"));
                if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                return;
            }

            if (defense.Stage == 2 && action is "named-defense-take-hand" or "named-defense-take-equipment")
            {
                Card card;
                CardLocation from;
                if (action == "named-defense-take-hand")
                {
                    from = CardLocation.Hand(frame.OwnerSeat);
                    card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"])];
                }
                else
                {
                    var id = choice.Cards.Single();
                    from = _cardZonesOperations._cardZones.GetLocation(id);
                    if (from != CardLocation.Equipment(frame.OwnerSeat))
                        throw new InvalidOperationException("The selected equipment moved.");
                    card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
                }

                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { NamedTargetDefense = defense with { Stage = 3 } });
                _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(actor), new CardMoveReason($"skill-program.{(frame.SkillId)}.named-take"));
                if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
                return;
            }

            throw new InvalidOperationException("The named defense has an invalid stage/choice.");
        }

        if (frame.PublicHandDraft is not { } draft)
            throw new InvalidOperationException("The public hand draft lost its state.");
        if (draft.Stage == 1 && action == "public-draft-show")
        {
            var id = choice.Cards.Single();
            if (draft.CardIds.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat))
                throw new InvalidOperationException("The reveal must select another real owner hand card.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            var selected = draft.CardIds.Append(id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicHandDraft = draft with { CardIds = selected } });
            if (selected.Length < draft.RecipientSeats.Count)
            {
                PublishPublicDraftSelection(frame);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicHandDraft = frame.PublicHandDraft!with { Stage = 2 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandDraftRevealedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, draft.RecipientSeats, selected));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(frame.Id, "public-hand-draft-roster"))
                return;
            ReturnPublicHandDraftReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        if (draft.Stage == 2 && action == "public-draft-start")
        {
            var start = choice.Targets.Single();
            if (!PublicHandDraftStartingSeats(frame).Contains(start))
                throw new InvalidOperationException("The chosen starting character no longer meets this frozen draft's starting policy.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicHandDraft = draft with { Stage = 3, RecipientSeats = draft.RecipientSeats.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(start, seat)).ToArray() } });
            ContinuePublicHandDraft(frame);
            return;
        }

        if (draft.Stage == 3 && action == "public-draft-gain")
        {
            var id = choice.Cards.Single();
            var recipient = draft.RecipientSeats[draft.Cursor];
            if (!draft.CardIds.Contains(id) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat))
                throw new InvalidOperationException("The draft card or current recipient is unavailable.");
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(card => card.Id == id);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicHandDraft = draft with { CardIds = draft.CardIds.Where(cardId => cardId != id).ToArray(), Cursor = draft.Cursor + 1 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandDraftCardObtainedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, recipient, id));
            if (recipient != frame.OwnerSeat)
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(frame.OwnerSeat), CardLocation.Hand(recipient), new CardMoveReason($"skill-program.{(frame.SkillId)}.public-draft"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        throw new InvalidOperationException("The public hand draft has an invalid stage/choice.");
    }

    [PublicHandRevealReturn("public-hand-draft-roster")]
    internal void ReturnPublicHandDraftReveal(ProgramSkillFrame frame)
    {
        if (frame.PublicHandDraft is not { Stage: 2 })
            throw new InvalidOperationException("A public Hand draft lost its complete shown roster.");
        var starts = PublicHandDraftStartingSeats(frame);
        if (starts.Length == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicHandDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        PublishNamedDraftPrompt(frame, frame.OwnerSeat, "选择顺时针分配的起始角色。", starts.Select(seat => NamedDraftChoice(frame, "public-draft-start", _coreMatchParticipantsPlayersAccess._players[seat].Name, seat: seat)).ToArray(), false);
    }

    internal int[] PublicHandDraftStartingSeats(ProgramSkillFrame frame) => _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.StateId == "eligible-start" ? frame.PublicHandDraft!.RecipientSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray() : _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => player.Seat).ToArray();
    internal void AssertNamedDefenseAndPublicDraft(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.NamedTargetDefense is { } defense)
        {
            if (paused.Op != SkillProgramEffectOp.DeclareNameForTargetDefense || defense.LedgerId != paused.StateId || defense.Stage is < 0 or > 3 || (defense.Stage == 0) != (defense.Name is null) || defense.Name is { } name && (!paused.CardKinds.Contains(name) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, DefenseNameUsage(defense.LedgerId, name), SkillUsageScope.Game) != 1))
                throw new InvalidOperationException("A named defense lost its declared canonical name or paid instruction.");
            _ = NamedDefenseUse(frame);
        }

        if (frame.PublicHandDraft is { } draft && (paused.Op != SkillProgramEffectOp.DrawAndDraftLowHandPopulation || draft.Stage is < 0 or > 3 || draft.RecipientSeats.Distinct().Count() != draft.RecipientSeats.Count || draft.CardIds.Distinct().Count() != draft.CardIds.Count || draft.CardIds.Count > draft.RecipientSeats.Count || draft.Cursor < 0 || draft.Cursor > draft.RecipientSeats.Count || frame.WindowContext?.Facts?.LowHandPopulationSeats is not { } frozen || !draft.RecipientSeats.Order().SequenceEqual(frozen.Order())))
            throw new InvalidOperationException("A public draft lost its frozen participants, real card set or cursor.");
        if (frame.NamedTargetDefense is null && frame.PublicHandDraft is null || !ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) || frame.PendingMovementContinuation is not null)
            return;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.Choices.Count == 0 || prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString()))
            throw new InvalidOperationException("The named/draft operation lost its published decision.");
        var expectedChooser = frame.NamedTargetDefense is { } named ? named.Stage == 0 ? frame.OwnerSeat : NamedDefenseUse(frame).Action!.ActorSeat : frame.PublicHandDraft!.Stage == 3 ? frame.PublicHandDraft.RecipientSeats[frame.PublicHandDraft.Cursor] : frame.OwnerSeat;
        if (prompt.PlayerSeat != expectedChooser || prompt.IsPrivate != (frame.NamedTargetDefense is not null || frame.PublicHandDraft!.Stage < 2))
            throw new InvalidOperationException("The named/draft decision changed its actor or privacy boundary.");
    }
}
