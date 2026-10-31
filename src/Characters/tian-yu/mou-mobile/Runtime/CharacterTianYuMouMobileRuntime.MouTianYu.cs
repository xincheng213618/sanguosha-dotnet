#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuMouMobileRuntime
{
    internal bool HasMouTianYu => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouTianYuYangshi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouTianYuZhenxi);
    internal int MouTianYuActualTurn => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Count();

    internal static CardConversionSource MouTianYuSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool IsMouTianYuTargetTrigger(SkillProgramTrigger t) => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouTianYuZhenxi);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30600, SkillProgramEffectOp.ResolveMouTianYuZhenxi, SkillProgramEffectOp.ResolveMouTianYuYangshi)]
    internal bool CanOfferMouTianYu(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouTianYuYangshi))
            return c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.TargetSeat == candidate.OwnerSeat;
        return c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && c.CardUse is { } a && a.ActorSeat == candidate.OwnerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && c.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouTianYuStartedEvent>().Any(e => e.Source.SkillId == MouTianYuComposition.Zhenxi && e.Source.OwnerSeat == candidate.OwnerSeat && e.TurnOrdinal == MouTianYuActualTurn);
    }

    internal SkillProgramStepOutcome BeginMouTianYu(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.InstructionIndex != 1 || f.MouTianYu is not null || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || !CanOfferMouTianYu(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Tian Yu requires its current unpaid native trigger.");
        var target = c.TargetSeat!.Value;
        var zhenxi = op == SkillProgramEffectOp.ResolveMouTianYuZhenxi;
        var distance = zhenxi ? _coreNativeRuleQueriesOperations.GetCombatDistance(f.OwnerSeat, target) : 0;
        var backwater = zhenxi && _coreMatchParticipantsPlayersAccess._players[target].HasHpValue && (_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue).All(p => p.Hp <= _coreMatchParticipantsPlayersAccess._players[target].Hp) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[target].Hp > _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
        var r = new MouTianYuReceipt(MouTianYuSource(f), f.GameplayHash, op, c.ParentFrameId, c.CardUse?.ParentCardUseFrameId, c.CardUse?.CardActionId, target, distance, backwater, MouTianYuActualTurn, zhenxi ? MouTianYuStage.Branch : MouTianYuStage.Complete);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.UseId, r.ActionId, target, distance, backwater, r.TurnOrdinal));
        if (zhenxi)
        {
            PublishMouTianYu(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).All(p => _coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, p.Seat)))
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuSlashObtainedEvent(f.Id, f.OwnerSeat, card?.Id));
            if (card is not null)
            {
                MutateMouTianYu(f, MouTianYuStage.Complete, () => _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.mou-mobile:yangshi.obtain"), disclosure: CardMovementDisclosure.FaceDown));
                return SkillProgramStepOutcome.AwaitChild;
            }
        }
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuRangeIncreasedEvent(f.Id, f.OwnerSeat, r.Source, r.GameplayHash, _coreNativeRuleQueriesOperations.GetAttackRange(f.OwnerSeat)));
        CompleteMouTianYu(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> MouTianYuChoices(ProgramSkillFrame f)
    {
        var r = f.MouTianYu!;
        var choices = new List<PromptChoice>();
        PromptChoice C(string verb, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, IReadOnlyDictionary<string, string>? native = null) => new(new($"mou-tian-yu.{(f.Id)}.{(r.Stage)}.{(verb)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}.{(native?.GetValueOrDefault("slot-index"))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string>(native ?? new Dictionary<string, string>()) { ["program-action"] = "mou-tian-yu", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["verb"] = verb });
        if (r.Stage == MouTianYuStage.Branch)
        {
            choices.Add(C("discard", $"弃置其至多{(r.Distance)}张手牌"));
            choices.Add(C("move", "移动其场上的一张牌"));
            if (r.Backwater)
                choices.Add(C("backwater", "背水：依次执行两项"));
        }

        if (r.Stage == MouTianYuStage.Discard)
        {
            foreach (var choice in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.TargetSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (_, card) => r.Hand.Contains(card.Id) && (r.Selected.Contains(card.Id) || r.Selected.Count < r.Distance)))
                choices.Add(C("toggle", choice.Description, choice.Cards, native: choice.Parameters));
            choices.Add(C("confirm", $"确认弃置{(r.Selected.Count)}张手牌"));
        }

        if (r.Stage == MouTianYuStage.Move && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
        {
            foreach (var zone in new[]
            {
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            )
                foreach (var card in _cardZonesOperations._cardZones.CardsAt(new(zone, r.TargetSeat)))
                    foreach (var recipient in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.TargetSeat))
                        if (zone == CardZoneKind.Equipment ? !card.IsGeneralWeapon && _coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(recipient.Seat, card) : _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(card, zone, recipient.Seat))
                            choices.Add(C("field", $"将【{(card.DisplayName)}】移至{(recipient.Name)}", [card.Id], [recipient.Seat], new Dictionary<string, string> { ["source-zone"] = zone.ToString() }));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouTianYu(ProgramSkillFrame f)
    {
        var choices = MouTianYuChoices(f);
        if (choices.Count == 0)
        {
            CompleteMouTianYu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：选择本次原生效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mou-tian-yu")]
    internal void AnswerMouTianYu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouTianYu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MouTianYuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Tian Yu lost its exact published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var verb = choice.Parameters["verb"];
        if (r.Stage == MouTianYuStage.Branch)
        {
            var stage = verb == "move" ? MouTianYuStage.Move : MouTianYuStage.Discard;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = r with { Branch = verb, Stage = stage, Hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Select(c => c.Id).ToArray(), Selected = [] } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuBranchChosenEvent(f.Id, verb));
            PublishMouTianYu(f);
            return;
        }

        if (r.Stage == MouTianYuStage.Discard)
        {
            if (verb == "toggle")
            {
                var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
                var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat])[slot];
                var ids = r.Selected.Contains(card.Id) ? r.Selected.Where(id => id != card.Id).ToArray() : r.Selected.Append(card.Id).ToArray();
                if (ids.Length > r.Distance)
                    throw new InvalidOperationException("Zhenxi cannot exceed its frozen native distance.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = r with { Selected = ids } });
                PublishMouTianYu(f);
                return;
            }

            if (verb != "confirm" || r.Selected.Count > r.Distance || r.Selected.Any(id => !r.Hand.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.TargetSeat)))
                throw new InvalidOperationException("Zhenxi requires its frozen current Hand materials.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuDiscardPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.Selected.Count));
            var next = r.Branch == "backwater" ? MouTianYuStage.Move : MouTianYuStage.Complete;
            if (r.Selected.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = r with { Stage = next } });
                ContinueMouTianYu(f.Id);
            }
            else
                MutateMouTianYu(f, next, () => _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new("skill-program.mou-mobile:zhenxi.discard")));
            return;
        }

        if (r.Stage != MouTianYuStage.Move || verb != "field")
            throw new InvalidOperationException("Unknown Tian Yu branch.");
        var id = choice.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var cardToMove = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
        var destination = new CardLocation(from.Zone, choice.Targets.Single());
        MutateMouTianYu(f, MouTianYuStage.Complete, () => _corePhysicalMovementFacadesOperations.MoveCard(cardToMove, from, destination, new("skill-program.mou-mobile:zhenxi.field"), move => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuFieldMovedEvent(f.Id, id, move.From, move.To)), disclosure: CardMovementDisclosure.FaceUp));
    }

    internal void MutateMouTianYu(ProgramSkillFrame f, MouTianYuStage next, Action mutation)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = f.MouTianYu!with { Stage = MouTianYuStage.Children, AfterChildren = next, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        mutation();
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = f.MouTianYu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
        ContinueMouTianYu(f.Id);
    }

    internal bool DrainMouTianYu(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouTianYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouTianYu: { } r } f || f.Id != id)
            return;
        if (r.Stage == MouTianYuStage.Children)
        {
            if (DrainMouTianYu(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, MouTianYu = r with { Stage = r.AfterChildren } });
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || f.MouTianYu!.Stage == MouTianYuStage.Complete)
        {
            CompleteMouTianYu(f);
            return;
        }

        PublishMouTianYu(f);
    }

    internal void CompleteMouTianYu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouTianYuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouTianYu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 30600)]
    internal bool ResumeMouTianYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouTianYu: not null } f || f.Id != id)
            return false;
        ContinueMouTianYu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30600)]
    internal bool ReturnMouTianYuMovement(ProgramSkillFrame f) => f.MouTianYu is not null && ResumeMouTianYu(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30600)]
    internal bool ContinuePaidMouTianYu(ProgramSkillFrame f) => f.MouTianYu is { } r && (r.Before < r.After || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouTianYuDiscardPaidEvent>().Any(e => e.FrameId == f.Id));
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouTianYuZhenxi)]
    internal PromptChoice SelectAiMouTianYu(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouTianYu!;
        if (r.Stage == MouTianYuStage.Branch)
            return p.Choices.FirstOrDefault(c => c.Parameters["verb"] == "backwater") ?? p.Choices.First();
        if (r.Stage == MouTianYuStage.Discard)
            return r.Selected.Count >= Math.Min(r.Distance, r.Hand.Count) ? p.Choices.Single(c => c.Parameters["verb"] == "confirm") : p.Choices.FirstOrDefault(c => c.Parameters["verb"] == "toggle" && !r.Selected.Contains(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat])[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id)) ?? p.Choices.Single(c => c.Parameters["verb"] == "confirm");
        return p.Choices.First();
    }

    SkillProgramStepOutcome IMouTianYuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMouTianYu(f, op);
}
