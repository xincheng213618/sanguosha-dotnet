#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoMouOlRuntime
{
    internal IEnumerable<PromptChoice> MouYishiFieldChoices(ProgramSkillFrame? f, int owner)
    {
        foreach (var source in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var zone in new[]
            {
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            )
                foreach (var card in _cardZonesOperations._cardZones.CardsAt(new(zone, source.Seat)))
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != source.Seat))
                    {
                        if (!(zone == CardZoneKind.Equipment ? !card.IsGeneralWeapon && _coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(target.Seat, card) : _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(card, zone, target.Seat)))
                            continue;
                        if (f is null)
                        {
                            yield return new(new("mou-dong-zhao.field-probe"), "移动场上牌", [card.Id], [target.Seat], new Dictionary<string, string>());
                            yield break;
                        }

                        yield return MouDongZhaoChoice(f, "field", $"将{(source.Name)}的【{(card.DisplayName)}】移给{(target.Name)}", [card.Id], [target.Seat], new Dictionary<string, string> { ["source-zone"] = zone.ToString(), ["card-owner-seat"] = source.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture) });
                    }
    }

    internal int MouYishiPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhaoYishiStartedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Phase == phase);
    [ProgramPhaseUsedProjection(33500)]
    internal int MouYishiPublicPhaseUses(int owner, string skill, int previous) => skill == MouDongZhaoComposition.Yishi && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner ? MouYishiPhaseUses(owner, phase) : previous;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 33500, SkillProgramEffectOp.ResolveMouDongZhaoYishi)]
    internal bool CanStartMouYishi(ProgramActivationLegalityContext c) => c.SkillId == MouDongZhaoComposition.Yishi && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == c.OwnerSeat && MouYishiPhaseUses(c.OwnerSeat, phase) < 1 && MouYishiFieldChoices(null, c.OwnerSeat).Any();
    internal SkillProgramStepOutcome BeginMouYishi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouDongZhao is not null)
        {
            ContinueMouDongZhao(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouDongZhaoComposition.Yishi || f.ActivationId != "keep-suits-and-move" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || MouYishiPhaseUses(f.OwnerSeat, phase) != 0 || !MouYishiFieldChoices(null, f.OwnerSeat).Any())
            throw new InvalidOperationException("Yishi needs one actual Play and a currently legal native field move.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => new MouDongZhaoHandEntity(c.Id, c.Kind, _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c))).ToArray();
        var receipt = new MouDongZhaoReceipt(MouDongZhaoSource(f), f.GameplayHash, SkillProgramEffectOp.ResolveMouDongZhaoYishi, MouDongZhaoStage.ChoosingKept, f.Id, _coreActualRoundProgressRoundNumberAccess._roundNumber, f.OwnerSeat, phase)
        {
            Hand = hand
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoYishiStartedEvent(f.Id, receipt.Source, receipt.GameplayHash, phase, hand.Length));
        PublishMouDongZhao(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouYishiChoices(ProgramSkillFrame f)
    {
        var r = f.MouDongZhao!;
        if (r.Stage == MouDongZhaoStage.ChoosingMove)
            return Array.AsReadOnly(MouYishiFieldChoices(f, f.OwnerSeat).ToArray());
        if (r.Stage != MouDongZhaoStage.ChoosingKept)
            return[];
        var choices = new List<PromptChoice>();
        foreach (var card in r.Hand.Where(c => c.Suit != Suit.None && _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Hand(f.OwnerSeat)))
        {
            if (!r.Selected.Contains(card.CardId) && r.Selected.Any(id => r.Hand.Single(c => c.CardId == id).Suit == card.Suit))
                continue;
            choices.Add(MouDongZhaoChoice(f, "keep-toggle", (r.Selected.Contains(card.CardId) ? "取消保留" : "保留") + _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId).DisplayName, [card.CardId]));
        }

        if (r.Selected.Count == r.Hand.Where(c => c.Suit != Suit.None).Select(c => c.Suit).Distinct().Count() && r.Hand.Where(c => c.Suit != Suit.None).Select(c => c.Suit).Distinct().All(suit => r.Selected.Count(id => r.Hand.Single(c => c.CardId == id).Suit == suit) == 1))
            choices.Add(MouDongZhaoChoice(f, "keep-confirm", "展示保留牌，将其余手牌置入弃牌堆"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void AnswerMouYishi(ProgramSkillFrame f, PromptChoice selected)
    {
        var r = f.MouDongZhao!;
        var verb = selected.Parameters["verb"];
        if (r.Stage == MouDongZhaoStage.ChoosingKept)
        {
            if (verb == "keep-toggle")
            {
                var id = selected.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { Selected = r.Selected.Contains(id) ? r.Selected.Where(c => c != id).ToArray() : r.Selected.Append(id).ToArray() } });
                PublishMouDongZhao(f);
                return;
            }

            if (verb != "keep-confirm" || r.Hand.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.Hand(f.OwnerSeat)))
                throw new InvalidOperationException("Yishi retains exactly one current owned Hand entity of every frozen suit.");
            if (r.Selected.Count > 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(r.Selected.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "yishi-kept-hand"))
            {
                return;
            }

            ContinueYishiAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (r.Stage != MouDongZhaoStage.ChoosingMove || verb != "field")
            throw new InvalidOperationException("Yishi lost its field-move choice.");
        var moving = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(moving.Id);
        var destination = new CardLocation(from.Zone, selected.Targets.Single());
        var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[from.OwnerSeat!.Value], moving);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { MovedCardId = moving.Id, MovedSuit = suit } });
        MutateMouDongZhao(f, MouDongZhaoStage.MoveChildren, () => _corePhysicalMovementFacadesOperations.MoveCard(moving, from, destination, new("skill-program.mou-ol:yishi.field"), movement =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MouDongZhao = parent.MouDongZhao!with { FieldMoved = movement.To == destination && movement.From != movement.To } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoFieldMovedEvent(f.Id, moving.Id, suit, movement.From, movement.To, movement.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp));
    }

    internal PromptChoice AiMouYishi(PendingDecision prompt, ProgramSkillFrame f)
    {
        if (f.MouDongZhao!.Stage == MouDongZhaoStage.ChoosingMove)
            return prompt.Choices.First();
        return prompt.Choices.FirstOrDefault(c => c.Parameters["verb"] == "keep-confirm") ?? prompt.Choices.First(c => c.Parameters["verb"] == "keep-toggle" && !f.MouDongZhao.Selected.Contains(c.Cards.Single()));
    }

    internal void GrantMouYishi(ProgramSkillFrame f)
    {
        var r = f.MouDongZhao!;
        if (!r.LostHand && r.FieldMoved && r.MovedSuit is { } suit && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoYishiGrantedEvent(f.Id, r.Source, r.GameplayHash, suit, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Count(e => e.ActorSeat == f.OwnerSeat), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    [PublicHandRevealReturn("yishi-kept-hand")]
    internal void ContinueYishiAfterPublicReveal(ProgramSkillFrame f)
    {
        var r = f.MouDongZhao ?? throw new InvalidOperationException("Yishi lost its original Hand selection.");
        var remainder = r.Hand.Where(c => !r.Selected.Contains(c.CardId) && _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Hand(f.OwnerSeat)).Select(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { LostHand = remainder.Length > 0 } });
        MutateMouDongZhao(f, MouDongZhaoStage.DiscardChildren, () =>
        {
            if (remainder.Length > 0)
                _corePhysicalMovementFacadesOperations.MoveCards(remainder, CardLocation.Hand(f.OwnerSeat), CardLocation.DiscardPile, new("skill-program.mou-ol:yishi.place-rest"), disclosure: CardMovementDisclosure.FaceUp);
        });
        return;
    }
}
