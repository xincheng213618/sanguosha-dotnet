#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGanNingMouMobileRuntime
{
    internal const string MouGnDiscard = "skill-program.mou-mobile:qixi.discard", MouGnStore = "skill-program.mou-mobile:fenwei.store", MouGnDraw = "skill-program.mou-mobile:fenwei.draw", MouGnObtain = "skill-program.mou-mobile:fenwei.obtain", MouGnSpend = "skill-program.mou-mobile:fenwei.discard";
    internal static CardConversionSource MouGnSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_gan_ning_mou_mobile_MouGanNingMobile.MouGnSource(f);
    internal bool HasMouGanNingMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouGanNingFenwei);

    SkillProgramStepOutcome global::CardGame.Core.IMouGanNingHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMouGn(f, op);
    internal bool IsIssuedMouGanNingPile(PublicPersistentPileSource p) => p.SkillId == MouGanNingMobileComposition.Fenwei && p.Capacity == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingWeiPlacedEvent>().Any(e => e.RecipientSeat == p.OwnerSeat && e.To == p.Location && p.SkillInstanceId == MouGnPileInstance(e.FrameId, e.RecipientSeat) && _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((p.OwnerSeat, p.SkillId, p.SkillInstanceId)) == p && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == MouGnStore));
    internal static string MouGnPileInstance(long id, int seat) => global::CardGame.Core.RuntimeSkills.RuntimeRules_gan_ning_mou_mobile_MouGanNingMobile.MouGnPileInstance(id, seat);
    internal MouGanNingWeiPlacedEvent? MouGnWei(int target, int? owner = null, string? instance = null) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingWeiPlacedEvent>().LastOrDefault(e => e.RecipientSeat == target && (owner is null || e.Source.OwnerSeat == owner) && (instance is null || e.Source.SkillInstanceId == instance) && _cardZonesOperations._cardZones.GetLocation(e.CardId) == e.To && _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((target, MouGanNingMobileComposition.Fenwei, MouGnPileInstance(e.FrameId, target)))is { } pile && IsIssuedMouGanNingPile(pile));
    internal CardUseFrame? MouGnParent(ProgramSkillWindowContext c) => c is { Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects, CardUse: { } use } && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == use.ParentCardUseFrameId)is CardUseFrame parent && parent.Action is { Type: CardActionType.Use } a && a.ActionId == use.CardActionId && a.ActorSeat == use.ActorSeat && a.EffectiveKind == use.EffectiveKind && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(a.EffectiveKind) == SkillProgramCardCategory.Trick ? parent : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31900, SkillProgramEffectOp.ResolveMouGanNingWeiTarget)]
    internal bool CanOfferMouGnWei(int owner, ProgramSkillWindowContext c) => MouGnParent(c)is { } parent && parent.TargetSeats.Any(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive && MouGnWei(t, owner)is { } e && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], MouGanNingMobileComposition.Fenwei, e.Source.SkillInstanceId));
    internal SkillProgramStepOutcome BeginMouGn(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouGanNing is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0)
            throw new InvalidOperationException("Gan Ning requires its original owning instruction.");
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        var target = -1;
        long useId = 0, actionId = 0;
        IReadOnlyList<int> targets = [];
        var stage = op == SkillProgramEffectOp.ResolveMouGanNingQixi ? MouGanNingStage.Guess : MouGanNingStage.PlaceCard;
        if (op == SkillProgramEffectOp.ResolveMouGanNingWeiTarget)
        {
            if (f.WindowContext is not { } c || MouGnParent(c)is not { } parent)
                return SkillProgramStepOutcome.Continue;
            useId = parent.Id;
            actionId = parent.Action!.ActionId;
            targets = parent.TargetSeats.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive && MouGnWei(t, f.OwnerSeat, f.SkillInstanceId)is not null).Distinct().ToArray();
            if (targets.Count == 0)
                return SkillProgramStepOutcome.Continue;
            stage = MouGanNingStage.WeiChoice;
            phase = null;
        }
        else
        {
            if (phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } p || p.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p))
                throw new InvalidOperationException("Gan Ning activation requires actual own Play.");
            if (op == SkillProgramEffectOp.ResolveMouGanNingQixi)
                target = f.SelectedTargetSeats.Single();
        }

        var r = new MouGanNingReceipt(MouGnSource(f), f.GameplayHash, op, phase, target, useId, actionId)
        {
            Stage = stage,
            Targets = targets,
            Accepted = op != SkillProgramEffectOp.ResolveMouGanNingFenwei
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingStartedEvent(f.Id, r.Source, r.GameplayHash, op, phase, target, useId, actionId));
        PublishMouGn(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MouGnChoice(ProgramSkillFrame f, string branch, string description, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, string? suit = null) => new(new($"mou-gan-ning.{(f.Id)}.{(branch)}.{(cards?.FirstOrDefault() ?? targets?.FirstOrDefault() ?? -1)}.{(suit)}"), description, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "mou-gan-ning", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["suit"] = suit ?? "" });
    internal IReadOnlyList<PromptChoice> MouGnChoices(ProgramSkillFrame f)
    {
        var r = f.MouGanNing!;
        var list = new List<PromptChoice>();
        if (r.Stage == MouGanNingStage.Guess && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
        {
            var mask = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingGuessedEvent>().Where(e => e.Phase == r.Phase && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == f.OwnerSeat)).Aggregate(0, (m, e) => m | 1 << Array.IndexOf(global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileDefinitions.MouGnSuits, e.Suit));
            foreach (var(suit, index)in global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileDefinitions.MouGnSuits.Select((s, i) => (s, i)))
                if ((mask & 1 << index) == 0)
                    list.Add(MouGnChoice(f, "guess", $"猜测{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitName(suit))}最多", suit: suit.ToString()));
        }

        if (r.Stage == MouGanNingStage.Repeat)
        {
            if (r.GuessedMask != 15 && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                list.Add(MouGnChoice(f, "continue", "令其再猜一种尚未猜过的花色"));
            list.Add(MouGnChoice(f, "stop", "停止猜测并弃置区域牌"));
        }

        if (r.Stage == MouGanNingStage.Discard && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && r.Discarded < r.Wrong)
            list.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-gan-ning", ["branch"] = "discard" } }));
        if (r.Stage == MouGanNingStage.PlaceCard)
        {
            if (r.Deposited < 3 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && MouGnWei(p.Seat)is null))
                list.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-gan-ning", ["branch"] = "card" } }));
            list.Add(MouGnChoice(f, "done", $"结束置威并摸{(r.Deposited)}张牌"));
        }

        if (r.Stage == MouGanNingStage.PlaceTarget)
            list.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && MouGnWei(p.Seat)is null).Select(p => MouGnChoice(f, "recipient", $"置于{(p.Name)}的武将牌上", targets: [p.Seat])));
        if (r.Stage == MouGanNingStage.WeiChoice && r.TargetIndex < r.Targets.Count && MouGnWei(r.Targets[r.TargetIndex], f.OwnerSeat, f.SkillInstanceId)is { } wei && _coreMatchParticipantsPlayersAccess._players[wei.RecipientSeat].IsAlive)
        {
            list.Add(MouGnChoice(f, "obtain", $"令{(_coreMatchParticipantsPlayersAccess._players[wei.RecipientSeat].Name)}获得威", [wei.CardId], [wei.RecipientSeat]));
            list.Add(MouGnChoice(f, "cancel", $"弃置威，取消{(_coreMatchParticipantsPlayersAccess._players[wei.RecipientSeat].Name)}作为当前锦囊目标", [wei.CardId], [wei.RecipientSeat]));
        }

        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishMouGn(ProgramSkillFrame f)
    {
        var r = f.MouGanNing!;
        var choices = MouGnChoices(f);
        if (choices.Count == 0)
        {
            if (r.Stage == MouGanNingStage.WeiChoice)
                NextMouGnWei(f);
            else
                FinishMouGn(f);
            return;
        }

        var chooser = r.Stage == MouGanNingStage.Guess ? r.TargetSeat : f.OwnerSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouGn(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：选择当前真实选项。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.TargetSeat >= 0 ? r.TargetSeat : null,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-gan-ning")]
    internal void AnswerMouGn(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGanNing: { } r } f || !ValidMouGn(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouGanNingStage.Guess ? r.TargetSeat : f.OwnerSeat) || !MouGnChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Gan Ning choice lost its original chooser or actual card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "guess")
        {
            var suit = Enum.Parse<Suit>(choice.Parameters["suit"]);
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
            var counts = global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileDefinitions.MouGnSuits.Select(s => hand.Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c) == s)).ToArray();
            var correct = counts[Array.IndexOf(global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileDefinitions.MouGnSuits, suit)] == counts.Max();
            r = r with
            {
                GuessedMask = r.GuessedMask | 1 << Array.IndexOf(global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileDefinitions.MouGnSuits, suit),
                Wrong = r.Wrong + (correct ? 0 : 1),
                Stage = correct ? MouGanNingStage.Discard : MouGanNingStage.Repeat
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingGuessedEvent(f.Id, r.TargetSeat, suit, correct, r.Wrong, r.Phase!));
            if (correct)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.ActivationId, f.OwnerSeat, "qixi-hand", Array.AsReadOnly(hand.Select(c => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c)).ToArray())));
            if (correct && _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "qixi-guessed-hand"))
                return;
            PublishMouGn(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (branch is "continue" or "stop")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingRepeatEvent(f.Id, branch == "continue"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { Stage = branch == "continue" ? MouGanNingStage.Guess : MouGanNingStage.Discard } });
            PublishMouGn(f);
            return;
        }

        if (branch == "card")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { SelectedCard = choice.Cards.Single(), Stage = MouGanNingStage.PlaceTarget } });
            PublishMouGn(f);
            return;
        }

        if (branch == "done")
        {
            DrawMouGn(f);
            return;
        }

        if (branch == "recipient")
        {
            var recipient = choice.Targets.Single();
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.SelectedCard);
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            if (from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                throw new InvalidOperationException("Wei requires its actual selected owned entity.");
            var instance = MouGnPileInstance(f.Id, recipient);
            var pile = new PublicPersistentPileSource(recipient, f.SkillId, instance, 1, instance);
            _cardZonesOperations._cardZones.EnsurePublicPersistentPile(pile.Location);
            _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Add((recipient, f.SkillId, instance), pile);
            PrepareMouGnMove(ref f, "place");
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, pile.Location, new(MouGnStore), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingWeiPlacedEvent(f.Id, r.Source, r.GameplayHash, recipient, m.CardId, m.CardKind, m.From, m.To, m.Sequence)));
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = current with { MouGanNing = current.MouGanNing!with { Deposited = r.Deposited + (_cardZonesOperations._cardZones.GetLocation(card.Id) == pile.Location ? 1 : 0), SelectedCard = 0 } });
            SealMouGn(f.Id);
            ContinueMouGn(f.Id);
            return;
        }

        if (branch == "discard")
        {
            var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), r.TargetSeat);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            PrepareMouGnMove(ref f, "qixi");
            var before = f.MouGanNing!.Before;
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, r.TargetSeat, card, from, new(MouGnDiscard));
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card.Id && m.From == from && m.Reason.Value == MouGnDiscard);
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = current with { MouGanNing = current.MouGanNing!with { Discarded = r.Discarded + 1 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingDiscardedEvent(f.Id, r.Discarded, card.Id, card.Kind, from, m.To, m.Sequence));
            SealMouGn(f.Id);
            ContinueMouGn(f.Id);
            return;
        }

        if (branch is "obtain" or "cancel")
        {
            var target = r.Targets[r.TargetIndex];
            var wei = MouGnWei(target, f.OwnerSeat, f.SkillInstanceId) ?? throw new InvalidOperationException("Wei lost its actual source entity.");
            PrepareMouGnMove(ref f, branch);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingWeiChosenEvent(f.Id, r.UseId, target, wei.CardId, branch));
            if (branch == "cancel")
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(wei.CardId), wei.To, CardLocation.DiscardPile, new(MouGnSpend), disclosure: CardMovementDisclosure.FaceUp);
            else
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(wei.CardId), wei.To, CardLocation.Hand(target), new(MouGnObtain));
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > f.MouGanNing!.Before && m.CardId == wei.CardId && m.From == wei.To);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingWeiSpentEvent(f.Id, r.UseId, target, wei.CardId, branch, m.From, m.To, m.Sequence));
            SealMouGn(f.Id);
            ContinueMouGn(f.Id);
            return;
        }

        throw new InvalidOperationException("Unknown Gan Ning branch.");
    }

    internal void PrepareMouGnMove(ref ProgramSkillFrame f, string purpose)
    {
        var r = f.MouGanNing!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { Stage = MouGanNingStage.MovementChildren, MovePurpose = purpose, Accepted = true, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
    }

    internal void DrawMouGn(ProgramSkillFrame f)
    {
        var r = f.MouGanNing!;
        if (r.Deposited == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishMouGn(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { Stage = MouGanNingStage.Drawing, Accepted = true }, PendingMovementContinuation = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Deposited, new(MouGnDraw));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouGanNingFenwei)]
    internal void ReturnMouGnDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouGanNing is not { Stage: MouGanNingStage.Drawing, Draw: null } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Deposited)
            throw new InvalidOperationException("Fenwei lost its issued actual Draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { Stage = MouGanNingStage.DrawChildren, Draw = o, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingDrawReturnedEvent(f.Id, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.AttemptFrameId));
        SealMouGn(f.Id);
        ContinueMouGn(f.Id);
    }

    internal void ContinueMouGn(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGanNing: { } r } f || f.Id != id)
            return;
        if (!ValidMouGn(f))
            throw new InvalidOperationException("Gan Ning lost its original issue and actual movement invoice.");
        if (r.Stage is not (MouGanNingStage.MovementChildren or MouGanNingStage.DrawChildren))
            return;
        if (DrainMouGn(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouGanNing!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouGn(f);
            return;
        }

        if (r.Stage == MouGanNingStage.DrawChildren)
        {
            FinishMouGn(f);
            return;
        }

        if (r.MovePurpose == "qixi")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { Stage = MouGanNingStage.Discard } });
            PublishMouGn(f);
            return;
        }

        if (r.MovePurpose == "place")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || r.Deposited >= 3)
            {
                DrawMouGn(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { Stage = MouGanNingStage.PlaceCard } });
            PublishMouGn(f);
            return;
        }

        if (r.MovePurpose == "cancel")
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingWeiSpentEvent>().Single(e => e.FrameId == f.Id && e.TargetSeat == r.Targets[r.TargetIndex]);
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.UseId)is CardUseFrame parent && parent.Action?.ActionId == r.ActionId && parent.TargetSeats.Contains(paid.TargetSeat))
            {
                _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(r.UseId, paid.TargetSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingTargetCancelledEvent(f.Id, r.UseId, r.ActionId, paid.TargetSeat, paid.CardId));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(r.UseId, parent.Action.ActorSeat, paid.TargetSeat, parent.Action.EffectiveKind, CardEffectSkipReason.SkillNullified));
            }
        }

        NextMouGnWei(f);
    }

    internal void NextMouGnWei(ProgramSkillFrame f)
    {
        var r = f.MouGanNing!;
        if (r.TargetIndex + 1 >= r.Targets.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishMouGn(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = r with { TargetIndex = r.TargetIndex + 1, Stage = MouGanNingStage.WeiChoice, MovePurpose = null }, PendingMovementContinuation = null });
        PublishMouGn(f);
    }

    internal void FinishMouGn(ProgramSkillFrame f)
    {
        var r = f.MouGanNing!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGanNingCompletedEvent(f.Id, r.Wrong, r.Discarded, r.Deposited));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGanNing = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouGanNingQixi, SkillProgramEffectOp.ResolveMouGanNingFenwei, SkillProgramEffectOp.ResolveMouGanNingWeiTarget)]
    internal PromptChoice ChooseMouGnAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouGanNing!;
        // Guessing reads only the published remaining suits, never the opposing private Hand.
        if (r.Stage == MouGanNingStage.Guess)
            return p.Choices[_randomOperations._random.Next(p.Choices.Count)];
        if (r.Stage == MouGanNingStage.Repeat)
            return p.Choices.First();
        if (r.Stage == MouGanNingStage.WeiChoice)
        {
            var target = r.Targets[r.TargetIndex];
            var parent = _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId);
            var friendly = global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target]);
            return p.Choices.Single(c => c.Parameters["branch"] == (friendly && parent?.Action is { } a && (global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(a.EffectiveKind) || a.EffectiveKind == CardKind.Dismantlement || a.EffectiveKind == CardKind.Snatch) ? "cancel" : "obtain"));
        }

        if (r.Stage == MouGanNingStage.PlaceTarget)
            return p.Choices.OrderByDescending(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).ThenBy(c => c.Targets.Single()).First();
        return p.Choices.First();
    }
}
