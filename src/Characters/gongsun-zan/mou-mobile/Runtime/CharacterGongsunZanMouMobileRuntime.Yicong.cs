#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGongsunZanMouMobileRuntime
{
    internal bool HasMouGszMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouGongsunZanYicong);

    internal static CardConversionSource MouGszMobileSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int MouGszMobileCharge(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(PlayerMarkerKind.MouGongsunZanCharge);
    internal void ChangeMouGszMobileCharge(long frame, int owner, int delta)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[owner];
        var before = MouGszMobileCharge(owner);
        var after = Math.Clamp(checked(before + delta), 0, 4);
        if (before == after)
            return;
        var key = (PlayerMarkerKind.MouGongsunZanCharge, owner);
        if (after == 0)
        {
            p.Markers.Remove(key.Item1);
            p.MarkerSourceCounts.Remove(key);
        }
        else
        {
            p.Markers[key.Item1] = after;
            p.MarkerSourceCounts[key] = after;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame, owner, key.Item1, after - before, after, owner, MouGongsunZanMobileComposition.Yicong));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileChargeEvent(frame, owner, before, after));
    }

    internal IEnumerable<PublicPersistentPileSource> MouGszHuPiles(int owner) => _corePublicPileSourcesOperations.PublicPileSources(owner).Where(IsIssuedMouGszHuPile);
    internal bool IsIssuedMouGszHuPile(PublicPersistentPileSource p) => p.SkillId == MouGongsunZanMobileComposition.Yicong && p.Capacity == 4 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileStoredEvent>().Any(e => e.To == p.Location && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == p.OwnerSeat && s.Source.SkillId == p.SkillId && s.Source.SkillInstanceId == p.SkillInstanceId));
    internal bool IsLiveMouGszHuPile(PublicPersistentPileSource p) => IsIssuedMouGszHuPile(p) && _coreMatchParticipantsPlayersAccess._players[p.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[p.OwnerSeat], p.SkillId, p.SkillInstanceId);
    internal IReadOnlyList<Card> MouGszHuCards(CharacterState owner) => HasMouGszMobile ? MouGszHuPiles(owner.Seat).Where(IsLiveMouGszHuPile).SelectMany(_corePublicPileSourcesOperations.PublicPileCards).ToArray() : [];
    internal bool IsMouGszHuCard(CharacterState owner, int id) => id > 0 && MouGszHuCards(owner).Any(c => c.Id == id);
    SkillProgramStepOutcome IMouGongsunZanMobileHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMouGszMobile(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33600, SkillProgramEffectOp.ResolveMouGongsunZanYicong, SkillProgramEffectOp.ResolveMouGongsunZanQiaomeng)]
    internal bool OfferMouGszMobile(ProgramTriggerLegalityContext c)
    {
        if (!c.Owner.IsAlive)
            return false;
        if (c.SkillId == MouGongsunZanMobileComposition.Yicong)
            return c.Context.Window == SkillProgramTriggerWindow.RoundStarting && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0;
        return c.SkillId == MouGongsunZanMobileComposition.Qiaomeng && _coreNationalSkillsOperations.HasRuntimeSkill(c.Owner, MouGongsunZanMobileComposition.Yicong) && MouGszMobileDamage(c.Context, c.OwnerSeat)is not null;
    }

    internal SkillProgramStepOutcome BeginMouGszMobile(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext ?? throw new InvalidOperationException("Mobile Gongsun Zan needs a native trigger.");
        if (op == SkillProgramEffectOp.ExpireMouGongsunZanRound)
            return c.Window == SkillProgramTriggerWindow.RoundEnding ? SkillProgramStepOutcome.Continue : throw new InvalidOperationException("Yicong round fence lost its actual boundary.");
        if (op == SkillProgramEffectOp.InitializeMouGongsunZanMobile)
        {
            if (c.Window != SkillProgramTriggerWindow.GameStarting || f.SkillId != MouGongsunZanMobileComposition.Yicong)
                throw new InvalidOperationException("Yicong initial charge needs real setup.");
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMobileInitializedEvent>().Any(e => e.OwnerSeat == f.OwnerSeat))
            {
                ChangeMouGszMobileCharge(f.Id, f.OwnerSeat, 2);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileInitializedEvent(f.Id, f.OwnerSeat, MouGszMobileSource(f), 2));
            }

            return SkillProgramStepOutcome.Continue;
        }

        var damage = op == SkillProgramEffectOp.ResolveMouGongsunZanQiaomeng ? MouGszMobileDamage(c, f.OwnerSeat) : null;
        if (f.MouGongsunZanMobile is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || (op == SkillProgramEffectOp.ResolveMouGongsunZanYicong ? f.SkillId != MouGongsunZanMobileComposition.Yicong || c.Window != SkillProgramTriggerWindow.RoundStarting || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 : damage is null || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], MouGongsunZanMobileComposition.Yicong)))
            throw new InvalidOperationException("Mobile Gongsun Zan lost its exact native issue.");
        var r = new MouGongsunZanMobileReceipt(MouGszMobileSource(f), f.GameplayHash, op, c.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, damage?.Id ?? 0, damage?.TargetSeat ?? -1)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGongsunZanMobile = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.Round, r.DamageFrameId, r.TargetSeat));
        PublishMouGszMobile(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MouGszMobileChoice(ProgramSkillFrame f, string branch, string text, int amount = 0) => new(new($"mou-gsz-mobile.{(f.Id)}.{(branch)}.{(amount)}"), text, [], [], new Dictionary<string, string> { ["program-action"] = "mou-gongsun-zan-mobile", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["amount"] = amount.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> MouGszMobileChoices(ProgramSkillFrame f)
    {
        var r = f.MouGongsunZanMobile!;
        if (r.Stage != MouGongsunZanMobileStage.Choice)
            return[];
        var list = new List<PromptChoice>();
        if (r.Op == SkillProgramEffectOp.ResolveMouGongsunZanYicong)
        {
            for (var i = 0; i <= MouGszMobileCharge(f.OwnerSeat); i++)
            {
                list.Add(MouGszMobileChoice(f, "offense", $"消耗{(i)}蓄力：本轮进攻距离-1，至多置{(i)}张杀为扈", i));
                list.Add(MouGszMobileChoice(f, "defense", $"消耗{(i)}蓄力：本轮防御距离+1，至多置{(i)}张闪为扈", i));
            }
        }
        else
        {
            list.Add(MouGszMobileChoice(f, "charge", "获得3点蓄力（至多4）"));
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                list.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-gongsun-zan-mobile", ["branch"] = "discard" } }));
        }

        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishMouGszMobile(ProgramSkillFrame f)
    {
        var choices = MouGszMobileChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouGszMobile(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：选择一项真实效果。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-gongsun-zan-mobile")]
    internal void AnswerMouGszMobile(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGongsunZanMobile: { } r } f || !ValidMouGszMobile(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MouGszMobileChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mobile Gongsun Zan choice changed its native owner or actual material.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch is "offense" or "defense")
        {
            var n = int.Parse(choice.Parameters["amount"], CultureInfo.InvariantCulture);
            if (n < 0 || n > MouGszMobileCharge(f.OwnerSeat))
                throw new InvalidOperationException("Yicong cannot spend missing charge.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGongsunZanMobile = r with { Accepted = true, Charge = n, Offensive = branch == "offense", Stage = MouGongsunZanMobileStage.MovementChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            ChangeMouGszMobileCharge(f.Id, f.OwnerSeat, -n);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileDistanceEvent(f.Id, r.Source, r.GameplayHash, r.Round, branch == "offense", n));
            var room = Math.Max(0, 4 - MouGszHuPiles(f.OwnerSeat).Sum(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count));
            var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Where(card => branch == "offense" ? CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) : card.Kind == CardKind.Dodge).Take(Math.Min(n, room)).ToArray();
            if (cards.Length > 0)
            {
                var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, 4);
                foreach (var card in cards)
                    _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, pile.Location, new("skill-program.mou-mobile:yicong.store"), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileStoredEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Sequence)), disclosure: CardMovementDisclosure.FaceUp);
            }

            SealMouGszMobile(f.Id);
            ContinueMouGszMobile(f.Id);
            return;
        }

        if (branch == "charge")
        {
            ChangeMouGszMobileCharge(f.Id, f.OwnerSeat, 3);
            FinishMouGszMobile(f);
            return;
        }

        var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), r.TargetSeat);
        var cardToDiscard = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGongsunZanMobile = r with { Accepted = true, Stage = MouGongsunZanMobileStage.MovementChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, cardToDiscard, from, new("skill-program.mou-mobile:qiaomeng.discard"), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileDiscardedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Sequence)), f.Id);
        SealMouGszMobile(f.Id);
        ContinueMouGszMobile(f.Id);
    }

    internal void SealMouGszMobile(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGongsunZanMobile = f.MouGongsunZanMobile!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainMouGszMobile(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouGszMobile(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGongsunZanMobile: { } r } f || f.Id != id)
            return;
        if (!ValidMouGszMobile(f))
            throw new InvalidOperationException("Mobile Gongsun Zan lost its paid trigger.");
        if (r.Stage is not (MouGongsunZanMobileStage.MovementChildren or MouGongsunZanMobileStage.DrawChildren))
            return;
        if (DrainMouGszMobile(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouGongsunZanMobile!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == MouGongsunZanMobileStage.MovementChildren && r.Op == SkillProgramEffectOp.ResolveMouGongsunZanQiaomeng && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGongsunZanMobile = r with { Stage = MouGongsunZanMobileStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill-program.mou-mobile:qiaomeng.Draw"));
            return;
        }

        FinishMouGszMobile(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouGongsunZanQiaomeng)]
    internal void ReturnMouGszMobileDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouGongsunZanMobile is not { Stage: MouGongsunZanMobileStage.Drawing, Draw: null } r || o.RequestedCount != 1 || o.RecipientSeat != f.OwnerSeat)
            throw new InvalidOperationException("Qiaomeng must return its one native requested draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGongsunZanMobile = r with { Stage = MouGongsunZanMobileStage.DrawChildren, Draw = o, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileDrawReturnedEvent(f.Id, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.AttemptFrameId));
        ContinueMouGszMobile(f.Id);
    }

    internal void FinishMouGszMobile(ProgramSkillFrame f)
    {
        var r = f.MouGongsunZanMobile!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMobileCompletedEvent(f.Id, r.Op));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGongsunZanMobile = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouGongsunZanYicong, SkillProgramEffectOp.ResolveMouGongsunZanQiaomeng)]
    internal PromptChoice AiMouGszMobile(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouGongsunZanMobile!.Op == SkillProgramEffectOp.ResolveMouGongsunZanYicong)
            return p.Choices.Where(c => c.Parameters["branch"] == "offense").Last();
        if (MouGszMobileCharge(f.OwnerSeat) <= 1)
            return p.Choices.Single(c => c.Parameters["branch"] == "charge");
        return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "discard") ?? p.Choices.Single();
    }
}
