using System.Globalization;

namespace CardGame.Core;

internal sealed partial class ProgramPaidCardRuntime
{
    private SkillProgramStepOutcome BeginOwnedCardRequest(ProgramSkillFrame frame, SkillProgramEffect effect,
        int chooserSeat, int cardOwnerSeat, IReadOnlyList<PromptChoice> choices, bool skipIfNoCards)
    {
        var requested = effect.CountExpression is { } count ? _facts.EvaluateCountExpression(frame, count) : effect.Amount;
        if (requested is null or < 0)
        {
            _operations.Cancel(frame, "选牌数量未知或为负数，未移动任何牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }
        choices = ExcludeOwnedRequestCards(frame, effect, choices);
        if (requested == 0)
        {
            SetOwnedRequestResult(frame, effect, [], []);
            return SkillProgramStepOutcome.Continue;
        }
        var exact = ProgramOwnedCardRequests.IsExact(effect);
        var destination = OwnedRequestDestination(frame, effect);
        if (exact && (!_facts.IsAlive(chooserSeat) || !_facts.IsAlive(cardOwnerSeat) ||
            destination.OwnerSeat is not { } recipient || !_facts.IsAlive(recipient) || recipient == cardOwnerSeat) ||
            choices.Count < requested)
        {
            if (!exact && skipIfNoCards)
            {
                if (effect.FreezeMovedCardSuit && effect.ResultBind is { } bind)
                    _operations.SetCardSet(frame.Id, bind, [], SkillProgramCardSetVisibility.Public, []);
                return SkillProgramStepOutcome.Continue;
            }
            return InsufficientOwnedRequest(frame, effect);
        }
        var entities = choices.Select(choice => OwnedRequestEntity(frame, effect, choice)).ToArray();
        frame = frame with
        {
            OwnedCardSelection = new(cardOwnerSeat, effect.ResultBind ?? string.Empty, requested.Value,
                Array.AsReadOnly(entities.Select(entity => entity.Card.Id).ToArray()),
                Array.AsReadOnly(entities.Select(entity => entity.From).ToArray()), [])
        };
        _frames.ReplaceTop(frame);
        PublishOwnedCardRequest(frame, effect, chooserSeat, choices);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<PromptChoice> ExcludeOwnedRequestCards(ProgramSkillFrame frame,
        SkillProgramEffect effect, IReadOnlyList<PromptChoice> choices)
    {
        if (effect.ExceptBind is not { } except) return choices;
        var excluded = CardSet(frame, except).CardIds.ToHashSet();
        return Array.AsReadOnly(choices.Where(choice =>
            !excluded.Contains(OwnedRequestEntity(frame, effect, choice).Card.Id)).ToArray());
    }

    private IReadOnlyList<PromptChoice> CurrentOwnedRequestChoices(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var chooser = ResolveProgramParticipant(frame, effect.ChooserRef!);
        var owner = ResolveProgramParticipant(frame, effect.CardOwnerRef!);
        var destinationSeat = effect.TargetReference is { } target ? ResolveProgramParticipant(frame, target) : (int?)null;
        var suits = effect.Suits.Count == 0 && effect.SuitFrom == SkillProgramSuitSource.DamageCard
            ? new[] { frame.WindowContext!.Facts!.DamageCardSuit!.Value } : effect.Suits;
        return ExcludeOwnedRequestCards(frame, effect, BuildOwnedCardPaymentChoices(frame.Id, chooser, owner,
            effect.Zones, effect.Destination == SkillProgramCardDestination.DiscardPile ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer,
            effect.CardCategories, effect.CardKinds, suits,
            effect.Destination == SkillProgramCardDestination.SelectedTargetCorrespondingZone
                ? (zone, card) => destinationSeat is { } seat && CanMoveProgramCardToCorrespondingZone(card, zone, seat) : null,
            effect.Destination!.Value, destinationSeat, effect.ProhibitReplacingEquipment,
            correspondingZoneSeat: effect.ProhibitReplacingEquipment && effect.Zones.Contains(CardZoneKind.Equipment)
                ? destinationSeat : null));
    }

    private (Card Card, CardLocation From) OwnedRequestEntity(ProgramSkillFrame frame,
        SkillProgramEffect effect, PromptChoice choice)
    {
        var owner = ResolveProgramParticipant(frame, effect.CardOwnerRef!);
        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var cards = zone switch
        {
            CardZoneKind.Hand => _facts.Hand(owner),
            CardZoneKind.Equipment => _facts.Equipment(owner),
            CardZoneKind.Judgment => _facts.Judgment(owner),
            _ => throw new InvalidOperationException("The card request has an unsupported owned zone.")
        };
        return (cards[slot], new(zone, owner));
    }

    private void PublishOwnedCardRequest(ProgramSkillFrame frame, SkillProgramEffect effect, int chooser,
        IReadOnlyList<PromptChoice> legal)
    {
        var draft = frame.OwnedCardSelection!;
        var choices = legal.Where(choice =>
            !draft.SelectedCardIds.Contains(OwnedRequestEntity(frame, effect, choice).Card.Id)).ToList();
        if (effect.AllowDecline)
            choices.Add(new(new ChoiceId($"program-field-decline.frame-{frame.Id}"), "不移动场上的牌。", [], [],
                new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>
                { ["program-action"] = "decline-owned-field-move", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) })));
        var skill = _facts.Skill(frame.SkillId);
        var handGift = effect.Destination == SkillProgramCardDestination.SelectedTargetHand;
        _prompts.Set(new(DecisionKind.ProgramTrigger, chooser,
            draft.RequiredCount == 1 ? handGift ? "请选择一张区域牌交给目标。" : "请选择一张区域牌支付。" :
                $"请选择{draft.RequiredCount}张区域牌（已选{draft.SelectedCardIds.Count}张，选齐前不移动牌）。",
            choices.SelectMany(choice => choice.Cards).Distinct().Order().ToArray(), [], chooser)
        {
            PromptId = _prompts.NextId(), IsPrivate = true,
            TargetSeat = frame.WindowContext?.TargetSeat ?? draft.CardOwnerSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, handGift ? $"{skill.Name} · 选择转交牌" : $"{skill.Name} · 选择支付牌", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        }, _facts.IsHuman(chooser));
    }

    // All requests use this original draft. Only the existing single-card
    // equipment/Processing commit protocols differ after selection is complete.
    private bool ResolveOwnedCardRequestSelection(ref ProgramSkillFrame frame, SkillProgramEffect effect,
        PromptChoice selected)
    {
        if (frame.OwnedCardSelection is not { } draft) return false;
        if (!ProgramOwnedCardRequests.IsExact(effect))
        {
            // A one-click legacy request retains its own opaque-slot and
            // dynamic cancellation contract at the original commit boundary.
            _frames.ReplaceTop(frame = frame with { OwnedCardSelection = null });
            return false;
        }
        var chooser = ResolveProgramParticipant(frame, effect.ChooserRef!);
        var owner = ResolveProgramParticipant(frame, effect.CardOwnerRef!);
        if (!_facts.IsAlive(frame.OwnerSeat) || !_facts.CanInvokeProgramSkill(frame))
        {
            _prompts.Clear();
            _operations.Cancel(frame, "支付持有人或技能实例已失效，未移动任何所选牌。");
            return true;
        }
        var destination = OwnedRequestDestination(frame, effect);
        var legal = CurrentOwnedRequestChoices(frame, effect);
        if (!_facts.IsAlive(chooser) || !_facts.IsAlive(owner) ||
            destination.OwnerSeat is not { } recipient || !_facts.IsAlive(recipient) || recipient == owner ||
            legal.Count < draft.RequiredCount)
        {
            _prompts.Clear();
            if (InsufficientOwnedRequest(frame, effect) == SkillProgramStepOutcome.Continue)
                _frames.AdvanceProgram(frame.Id);
            return true;
        }
        if (!legal.Any(choice => choice.Id == selected.Id && choice.Cards.SequenceEqual(selected.Cards) &&
            choice.Targets.SequenceEqual(selected.Targets) && choice.Parameters.Count == selected.Parameters.Count &&
            choice.Parameters.All(pair => selected.Parameters.GetValueOrDefault(pair.Key) == pair.Value)))
            throw new InvalidOperationException("The card request accepts only its current canonical material choice.");
        var selectionFrame = frame;
        var entities = legal.Select(choice => OwnedRequestEntity(selectionFrame, effect, choice)).ToArray();
        var previous = draft.SelectedCardIds.Where(id => entities.Any(entity => entity.Card.Id == id &&
            entity.From == draft.CandidateLocations[draft.CandidateCardIds.ToList().IndexOf(id)])).ToArray();
        var picked = OwnedRequestEntity(frame, effect, selected).Card.Id;
        if (previous.Contains(picked)) throw new InvalidOperationException("The request already selected this physical card.");
        var ids = previous.Append(picked).ToArray();
        _prompts.Clear();
        if (ids.Length < draft.RequiredCount)
        {
            frame = frame with { OwnedCardSelection = draft with
            {
                CandidateCardIds = Array.AsReadOnly(entities.Select(entity => entity.Card.Id).ToArray()),
                CandidateLocations = Array.AsReadOnly(entities.Select(entity => entity.From).ToArray()),
                SelectedCardIds = Array.AsReadOnly(ids)
            } };
            _frames.ReplaceTop(frame);
            PublishOwnedCardRequest(frame, effect, chooser, legal);
            return true;
        }
        _frames.ReplaceTop(frame = frame with { OwnedCardSelection = null });
        CommitOwnedCardRequest(frame, effect, ids, entities.Where(entity => ids.Contains(entity.Card.Id)).ToArray(), destination);
        return true;
    }

    private CardLocation OwnedRequestDestination(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Destination switch
    {
        SkillProgramCardDestination.OwnerHand => CardLocation.Hand(frame.OwnerSeat),
        SkillProgramCardDestination.SelectedTargetHand => CardLocation.Hand(ResolveProgramParticipant(frame, effect.TargetReference!)),
        _ => CardLocation.Processing
    };

    private SkillProgramStepOutcome InsufficientOwnedRequest(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        _frames.ReplaceTop(frame = frame with { OwnedCardSelection = null });
        if (effect.OnInsufficient == ProgramInsufficientCardBehavior.BindEmpty)
        {
            SetOwnedRequestResult(frame, effect, [], []);
            return SkillProgramStepOutcome.Continue;
        }
        _operations.Cancel(frame, "没有足额的合法区域牌，未移动任何所选牌。");
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void SetOwnedRequestResult(ProgramSkillFrame frame, SkillProgramEffect effect,
        IReadOnlyList<int> ids, IReadOnlyList<CardLocation> sources)
    {
        if (effect.ResultBind is { } bind)
            _operations.SetCardSet(frame.Id, bind, ids, SkillProgramCardSetVisibility.Private, sources);
    }

    private void CommitOwnedCardRequest(ProgramSkillFrame frame, SkillProgramEffect effect, int[] ids,
        (Card Card, CardLocation From)[] entities, CardLocation destination)
    {
        var sources = ids.Select(id => entities.Single(entity => entity.Card.Id == id).From).ToArray();
        _frames.ReplaceTop(frame = frame with
        {
            SelectedCardPayment = new(frame.InstructionIndex, effect.Op, Array.AsReadOnly(ids), destination.OwnerSeat!.Value),
            SelectedCardPaymentResult = null
        });
        var giver = ProgramCardGiving.Giver(frame, effect.GiveBy, sources);
        _operations.MoveCards(frame, ids, destination, new($"skill-program.{frame.SkillId}.{effect.Op}"), giver, settlement =>
        {
            var current = _frames.ActiveProgram(frame.Id);
            _frames.Replace(current.Id, current with
            {
                SelectedCardPayment = current.SelectedCardPayment! with { MovementCommitted = true, GainRequestSettlement = settlement },
                InstructionDelivery = new(current.InstructionIndex, effect.Op)
            });
            SetOwnedRequestResult(current, effect, ids, sources);
        });
        if (!_operations.BeginMovementWindow()) _operations.ReturnMovement(frame.Id);
    }
}
