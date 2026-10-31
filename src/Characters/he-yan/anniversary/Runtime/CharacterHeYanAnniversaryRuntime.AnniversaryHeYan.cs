#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeYanAnniversaryRuntime
{
    internal const string HeYanRevealReturn = "anniversary-he-yan-reveal";
    internal static CardConversionSource HeYanSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    internal static CardMoveReason HeYanMoveReason(string kind) => new("skill.anniversary-he-yan." + kind);
    internal HeYanHandPlan HeYanFreezeHand(int seat) => new(seat, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(c => new HeYanHandCard(c.Id, c.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], c))).ToArray());
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 44200, SkillProgramEffectOp.ResolveAnniversaryHeYanYachai)]
    internal bool CanOfferHeYan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, DamageFrameId: not null, Amount: > 0 } && context.TargetSeat == c.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive;
    SkillProgramStepOutcome IAnniversaryHeYanHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHeYan(f, op);
    internal SkillProgramStepOutcome BeginHeYan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryHeYan is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("He Yan cannot restart an issued operation.");
        var yachai = op == SkillProgramEffectOp.ResolveAnniversaryHeYanYachai;
        var source = f.OwnerSeat;
        if (yachai)
        {
            if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } actor, DamageFrameId: { } damage, Amount: > 0 } context || context.TargetSeat != f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId && w.ParentFrameId == damage)is not { } window || !_coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window).DamageWasApplied)
                throw new InvalidOperationException("Yachai requires the actual sourced damage and its living source, including self damage.");
            source = actor;
        }
        else if (op != SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan || f.SkillId != AnniversaryHeYanComposition.Qingtan || f.TriggerId is not null || f.ActivationId != "simultaneous-suit-hands" || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Qingtan requires its own accepted zero-input Play activation.");
        var plans = yachai ? new[]
        {
            HeYanFreezeHand(source)
        }

        : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => HeYanFreezeHand(p.Seat)).ToArray();
        var r = new AnniversaryHeYanReceipt(HeYanSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, f.WindowContext?.ParentFrameId, f.WindowContext?.DamageFrameId, source, yachai ? AnniversaryHeYanStage.SourceChoice : AnniversaryHeYanStage.SealedChoice)
        {
            Plans = plans
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanStartedEvent(f.Id, r.Source, r.Hash, op, r.ActualTurn, r.TurnOwner, r.ParentId, r.DamageId, source, plans.Length));
        if (plans.Length == 0)
            FinishHeYan(f);
        else
            PublishHeYan(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int HeYanChooser(AnniversaryHeYanReceipt r, int owner) => r.Stage is AnniversaryHeYanStage.SourceChoice or AnniversaryHeYanStage.DiscardChoice ? r.SourceSeat : r.Stage == AnniversaryHeYanStage.SealedChoice ? r.Plans[r.ChooserIndex].Seat : r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanYachai ? r.SourceSeat : owner;
    internal PromptChoice HeYanChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"anniversary-he-yan.{(f.Id)}.{(f.AnniversaryHeYan!.Stage)}.{(branch)}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-he-yan", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> HeYanDiscardChoices(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.AnniversaryHeYan!.SourceSeat, f.AnniversaryHeYan.SourceSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !f.AnniversaryHeYan.Selected.Contains(c.Id));
    internal IReadOnlyList<PromptChoice> HeYanChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHeYan!;
        if (r.Stage == AnniversaryHeYanStage.SourceChoice)
        {
            var choices = new List<PromptChoice>
            {
                HeYanChoice(f, "ban", "本回合不能再使用手牌，然后令何晏摸两张牌")
            };
            if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.SourceSeat]).Count > 0)
            {
                choices.Add(HeYanChoice(f, "discard", "弃置一半手牌（向上取整）"));
                choices.Add(HeYanChoice(f, "reveal", "展示所有手牌并交给何晏其中一种花色的所有牌"));
            }

            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == AnniversaryHeYanStage.DiscardChoice)
            return Array.AsReadOnly(HeYanDiscardChoices(f).Select(c => HeYanChoice(f, "card-" + c.Cards.Single(), c.Label, c.Cards)).ToArray());
        if (r.Stage == AnniversaryHeYanStage.SealedChoice)
            return Array.AsReadOnly(r.Plans[r.ChooserIndex].Cards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Hand(r.Plans[r.ChooserIndex].Seat)).Select(c => HeYanChoice(f, "card-" + c.Id, "选择【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Id).DisplayName + "】", [c.Id])).ToArray());
        if (r.Stage != AnniversaryHeYanStage.SuitChoice)
            return[];
        var shown = r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanYachai ? r.Plans.SelectMany(p => p.Cards) : r.Plans.Select(p => p.Cards.Single(c => c.Id == p.SelectedId));
        var result = shown.Select(c => c.Suit).Distinct().Order().Select(s => HeYanChoice(f, "suit-" + (int)s, "选择" + s + "花色的所有展示牌")).ToList();
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan)
            result.Add(HeYanChoice(f, "decline", "不获得牌（不弃置展示牌）"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishHeYan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryHeYan!;
        var chooser = HeYanChooser(r, f.OwnerSeat);
        var choices = HeYanChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || choices.Count == 0)
        {
            FinishHeYan(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse, PendingMovementContinuation = null });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, chooser, choices, r.Stage == AnniversaryHeYanStage.SealedChoice ? "清谈：私下选择一张手牌，所有角色选定后同时展示。" : "请选择【" + _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name + "】的结算。", chooser);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-he-yan")]
    internal void AnswerHeYan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHeYan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != HeYanChooser(r, f.OwnerSeat) || !HeYanChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("He Yan rejects a stale or foreign private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == AnniversaryHeYanStage.SourceChoice)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanBranchEvent(f.Id, r.SourceSeat, branch));
            r = r with
            {
                Branch = branch
            };
            if (branch == "ban")
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanHandUseBannedEvent(f.Id, r.Source, r.ActualTurn, r.TurnOwner, r.SourceSeat));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.DrawReady, Recipients = [f.OwnerSeat], DrawAmount = 2 } });
                ContinueHeYan(f.Id);
                return;
            }

            if (branch == "discard")
            {
                var count = Math.Min((_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.SourceSeat]).Count + 1) / 2, HeYanDiscardChoices(f).Count);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.DiscardChoice, RequiredDiscard = count } });
                if (count == 0)
                    FinishHeYan(f);
                else
                    PublishHeYan(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Plans = [HeYanFreezeHand(r.SourceSeat)] } });
            RevealHeYan(f);
            return;
        }

        if (r.Stage == AnniversaryHeYanStage.DiscardChoice)
        {
            r = r with
            {
                Selected = r.Selected.Append(choice.Cards.Single()).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r });
            if (r.Selected.Count < r.RequiredDiscard)
                PublishHeYan(f);
            else
                PayHeYan(f, "yachai-discard", r.SourceSeat, r.Selected, CardLocation.DiscardPile, AnniversaryHeYanStage.Complete, discard: true);
            return;
        }

        if (r.Stage == AnniversaryHeYanStage.SealedChoice)
        {
            var plans = r.Plans.ToArray();
            var p = plans[r.ChooserIndex];
            plans[r.ChooserIndex] = p with
            {
                SelectedId = choice.Cards.Single()
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanChoiceCommittedEvent(f.Id, p.Seat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Plans = plans, ChooserIndex = r.ChooserIndex + 1 } });
            if (r.ChooserIndex + 1 == plans.Length)
                RevealHeYan(f);
            else
                PublishHeYan(f);
            return;
        }

        if (r.Stage != AnniversaryHeYanStage.SuitChoice)
            throw new InvalidOperationException("He Yan has no unissued suit decision.");
        if (branch == "decline")
        {
            FinishHeYan(f);
            return;
        }

        var suit = (Suit)int.Parse(branch.AsSpan(5), CultureInfo.InvariantCulture);
        var y = r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanYachai;
        var matches = y ? r.Plans.SelectMany(p => p.Cards.Where(c => c.Suit == suit).Select(c => (p.Seat, c.Id))) : r.Plans.Where(p => p.Cards.Single(c => c.Id == p.SelectedId).Suit == suit).Select(p => (p.Seat, Id: p.SelectedId!.Value));
        var matching = matches.ToArray();
        r = r with
        {
            ChosenSuit = suit,
            Recipients = y ? [] : matching.Select(m => m.Seat).ToArray(),
            DrawAmount = 1
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r });
        var ids = matching.Where(m => m.Seat != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[m.Seat].IsAlive && _cardZonesOperations._cardZones.GetLocation(m.Id) == CardLocation.Hand(m.Seat)).Select(m => m.Id).ToArray();
        PayHeYan(f, y ? "yachai-give" : "qingtan-obtain", y ? r.SourceSeat : f.OwnerSeat, ids, CardLocation.Hand(f.OwnerSeat), y ? AnniversaryHeYanStage.Complete : AnniversaryHeYanStage.DrawReady, discard: false);
    }

    internal void RevealHeYan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryHeYan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.Revealing } });
        var y = r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanYachai;
        var ids = r.Plans.SelectMany(p => (y ? p.Cards : p.Cards.Where(c => c.Id == p.SelectedId)).Select(c => c.Id)).ToArray();
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "he-yan-shown", ids, SkillProgramCardSetVisibility.Public);
        foreach (var p in r.Plans)
        {
            var shown = (y ? p.Cards : p.Cards.Where(c => c.Id == p.SelectedId)).Select(c => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[p.Seat], _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Id))).ToArray();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), p.Seat, "he-yan-shown", Array.AsReadOnly(shown)));
        }

        if (!_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, HeYanRevealReturn))
            ReturnHeYanReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [PublicHandRevealReturn(HeYanRevealReturn)]
    internal void ReturnHeYanReveal(ProgramSkillFrame f)
    {
        if (f.AnniversaryHeYan is not { Stage: AnniversaryHeYanStage.Revealing } r)
            throw new InvalidOperationException("He Yan lost his complete public reveal return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.SuitChoice } });
        PublishHeYan(f);
    }

    internal void PayHeYan(ProgramSkillFrame supplied, string kind, int executor, IReadOnlyList<int> ids, CardLocation destination, AnniversaryHeYanStage following, bool discard)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryHeYan!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id).Zone != CardZoneKind.Hand))
            throw new InvalidOperationException("He Yan cannot substitute a card that has left its original Hand.");
        var payment = new HeYanPayment(kind, executor, before, before, ids, []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.MovementChildren, Following = following, Payment = payment }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        void Paid(long _, IReadOnlyList<CardMovementRecord> records)
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = payment with
            {
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                Movements = records
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { AnniversaryHeYan = active.AnniversaryHeYan!with { Payment = paid } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanPaidEvent(f.Id, kind, executor, before, paid.After, ids.Count));
        }

        if (ids.Count == 0)
            Paid(0, []);
        else if (discard)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, executor, ids, HeYanMoveReason(kind), Paid);
        else if (kind == "yachai-give")
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, HeYanMoveReason(kind), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, executor, f.OwnerSeat, ids, before, Paid), disclosure: CardMovementDisclosure.FaceUp);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, HeYanMoveReason(kind), Paid, disclosure: CardMovementDisclosure.FaceUp);
        ContinueHeYan(f.Id);
    }

    internal bool DrainHeYan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 44200)]
    internal bool ContinueHeYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHeYan: { } r } f || f.Id != id)
            return false;
        if (r.Stage is AnniversaryHeYanStage.SourceChoice or AnniversaryHeYanStage.DiscardChoice or AnniversaryHeYanStage.SealedChoice or AnniversaryHeYanStage.SuitChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishHeYan(f);
            return true;
        }

        if (r.Stage is AnniversaryHeYanStage.Revealing or AnniversaryHeYanStage.Drawing)
            return true;
        if (DrainHeYan(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryHeYan!;
        if (r.Stage == AnniversaryHeYanStage.MovementChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r = r with { Stage = r.Following }, PendingMovementContinuation = null });
        }

        if (r.Stage == AnniversaryHeYanStage.DrawChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r = r with { Stage = AnniversaryHeYanStage.DrawReady }, PendingMovementContinuation = null });
        }

        if (r.Stage == AnniversaryHeYanStage.DrawReady)
        {
            while (r.DrawIndex < r.Recipients.Count)
            {
                var target = r.Recipients[r.DrawIndex];
                r = r with
                {
                    DrawIndex = r.DrawIndex + 1,
                    DrawTarget = target
                };
                if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.Drawing } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, r.DrawAmount, HeYanMoveReason("draw"));
                return true;
            }

            if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan)
            {
                var rest = r.Plans.Where(p => p.Cards.Single(c => c.Id == p.SelectedId).Suit != r.ChosenSuit && p.SelectedId is { } card && _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.Hand(p.Seat)).Select(p => p.SelectedId!.Value).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(card), _cardZonesOperations._cardZones.GetLocation(card), OwnedCardMoveIntent.Discard)).ToArray();
                PayHeYan(f, "qingtan-rest", f.OwnerSeat, rest, CardLocation.DiscardPile, AnniversaryHeYanStage.Complete, true);
                return true;
            }

            FinishHeYan(f);
            return true;
        }

        if (r.Stage == AnniversaryHeYanStage.Complete)
            FinishHeYan(f);
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryHeYanYachai, SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan)]
    internal void ReturnHeYanDraw(ProgramSkillFrame f, NativeDrawOutcome draw)
    {
        if (f.AnniversaryHeYan is not { Stage: AnniversaryHeYanStage.Drawing } r || draw.RecipientSeat != r.DrawTarget || draw.RequestedCount != r.DrawAmount || draw.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || draw.AttemptFrameId is { } attempt && r.Draws.Any(d => d.AttemptFrameId == attempt))
            throw new InvalidOperationException("He Yan lost his once-issued native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryHeYan = r with { Stage = AnniversaryHeYanStage.DrawChildren, Draws = r.Draws.Append(draw).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanDrawReturnedEvent(f.Id, r.DrawIndex, draw.RecipientSeat, draw.RequestedCount, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter, draw.AttemptFrameId));
        ContinueHeYan(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 44200)]
    internal bool ReturnHeYanMovement(ProgramSkillFrame f) => f.AnniversaryHeYan is not null && ContinueHeYan(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryHeYanYachai, SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan)]
    internal PromptChoice ChooseHeYan(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.AnniversaryHeYan!;
        if (r.Stage == AnniversaryHeYanStage.SourceChoice)
            return p.Choices.First(c => c.Parameters["branch"] == "ban");
        if (r.Stage == AnniversaryHeYanStage.SuitChoice && r.Operation == SkillProgramEffectOp.ResolveAnniversaryHeYanQingtan)
            return p.Choices.First(c => c.Parameters["branch"] != "decline");
        return p.Choices.OrderBy(c => c.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]) : 0).First();
    }

    internal void FinishHeYan(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHeYanCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHeYan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 44200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 44200)]
    internal bool RetainIssuedHeYan(ProgramSkillFrame f) => f.AnniversaryHeYan is not null;
}
