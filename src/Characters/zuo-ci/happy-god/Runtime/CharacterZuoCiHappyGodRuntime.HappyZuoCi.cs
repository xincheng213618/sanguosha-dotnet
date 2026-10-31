#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZuoCiHappyGodRuntime
{
    internal const string HappyZcAction = "happy-zuo-ci";
    SkillProgramStepOutcome global::CardGame.Core.IHappyZuoCiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyZuoCi(f, op);
    internal CardConversionSource HappyZcSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int HappyZcBonus(CharacterState owner) => owner.HappyZuoCi is { BonusTurn: var turn, BonusUses: var count } && turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber ? count : 0;
    internal int HappyZcTransformCount(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZuoCiTransformedEvent>().Count(e => e.OwnerSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZuoCiStartedEvent>().Any(s => s.FrameId == e.FrameId && s.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
    internal IReadOnlyList<Card> HappyZcPhantomChoices(CharacterState owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => HappyZcEntity(c.Id)is { } p && p.Owner == owner.Seat && !p.Entity.ManuallyTransformed && HappyZcTemplates(owner, c.Id).Count > 0).ToArray());
    internal IReadOnlyList<Card> HappyZcTemplates(CharacterState owner, int phantomId) => owner.HappyZuoCi is not { } state ? [] : Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => c.Id != phantomId && !state.UsedTemplates.Contains(c.Id) && (HappyZcEntity(c.Id)is null || HappyZcBonus(owner) > 0)).ToArray());
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 42600, SkillProgramEffectOp.TransformHappyZuoCiPhantom)]
    internal bool CanTransformHappyZc(CharacterState owner) => owner.HappyZuoCi is { } state && HappyZcSourceProof(owner.Seat, state) && HappyZcTransformCount(owner.Seat) < 2 + HappyZcBonus(owner) && HappyZcPhantomChoices(owner).Count > 0;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 42601, SkillProgramEffectOp.ResolveHappyZuoCiHuanjing)]
    internal bool CanHuanjingHappyZc(CharacterState owner) => owner.HappyZuoCi is { } state && HappyZcSourceProof(owner.Seat, state) && owner.HasHpValue;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42600)]
    internal bool CanOfferHappyZc(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != HappyZuoCiComposition.Huanshu)
            return true;
        if (gate.WindowContext is not { } context)
            return false;
        if (gate.Trigger.Id == "initial-phantoms")
            return gate.Owner.HappyZuoCi is null;
        if (gate.Owner.HappyZuoCi is not { } state || !HappyZcSourceProof(gate.OwnerSeat, state))
            return false;
        if (gate.Trigger.Id == "round-phantoms")
            return IsHappyZcActualRoundStart(gate.Candidate, context);
        if (gate.Trigger.Id == "foreign-phantom")
            return IsHappyZuoCiForeignRewardCandidate(gate.Candidate, context);
        return true;
    }

    internal bool IsHappyZcActualRoundStart(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.RoundStarting || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(facts, e => e is RoundStartedEvent r && r.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        if (start < 0 || facts[start] is not RoundStartedEvent round || round.ActorSeat != context.SourceSeat)
            return false;
        var since = facts.Skip(start + 1).ToArray();
        if (since.OfType<ProgramBindingResolvedEvent>().Any(e => e.Window == SkillProgramTriggerWindow.RoundStarting && e.OwnerSeat == candidate.OwnerSeat && e.SkillId == candidate.SkillId && e.BindingId == candidate.BindingId && e.SkillInstanceId == candidate.SkillInstanceId))
            return false;
        var turns = since.OfType<TurnStartedEvent>().ToArray();
        return window.BeforeTurnPreludeProof is not null ? turns.Length == 0 : turns is [var first] && first.ActorSeat == round.ActorSeat && first.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;
    }

    internal SkillProgramStepOutcome BeginHappyZuoCi(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyZuoCi is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !HappyZcOriginalFrame(f))
            throw new InvalidOperationException($"Happy Zuo Ci needs its exact original unstarted zero-input program: frame={(f.Id)}, skill={(f.SkillId)}, binding={(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f))}, instruction={(f.InstructionIndex)}, window={(f.WindowContext?.Window)}, parent={(_resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == f.WindowContext?.ParentFrameId)?.GetType().Name)}, selectedCards={(f.SelectedCardIds.Count)}, selectedTargets={(f.SelectedTargetSeats.Count)}.");
        var stage = operation == SkillProgramEffectOp.TransformHappyZuoCiPhantom ? HappyZuoCiStage.PhantomChoice : HappyZuoCiStage.InitialChildren;
        var r = new HappyZuoCiReceipt(HappyZcSource(f), f.GameplayHash, operation, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, stage)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiStartedEvent(f.Id, r.Source, r.GameplayHash, operation, r.ParentId, r.ActualTurn));
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (operation == SkillProgramEffectOp.InitializeHappyZuoCiPhantoms)
        {
            if (owner.HappyZuoCi is not null)
                throw new InvalidOperationException("A phantom deck may initialize once.");
            var prototypes = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => new HappyZuoCiPrototype(c.Id, HappyZcFace(c))).ToArray();
            var deck = new CardLocation(CardZoneKind.PrivatePersistentPile, f.OwnerSeat, $"happy-huanshu:{(f.SkillInstanceId)}");
            _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(deck);
            owner.HappyZuoCi = new(f.Id, r.Source, r.GameplayHash, deck);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiPoolInitializedEvent(f.Id, r.Source, r.GameplayHash, prototypes.Length));
            f = HappyZcChildren(f, HappyZuoCiStage.InitialChildren);
            CreateHappyZcPoolCards(f, prototypes);
            CompleteHappyZcMoves(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (owner.HappyZuoCi is not { } state || !HappyZcSourceProof(owner.Seat, state))
            throw new InvalidOperationException("Happy Zuo Ci lost his issued physical deck.");
        if (operation == SkillProgramEffectOp.TransformHappyZuoCiPhantom)
        {
            PublishHappyZc(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (operation == SkillProgramEffectOp.RefreshHappyZuoCiPhantoms)
        {
            f = HappyZcChildren(f, HappyZuoCiStage.RefreshChildren);
            var changed = 0;
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(owner).ToArray())
                if (HappyZcEntity(card.Id)is { } found && found.Owner == owner.Seat && !found.Entity.ManuallyTransformed)
                {
                    // The source says another same-suit card, not another name.
                    // Keep the copied physical prototype weights, including
                    // repeated faces, and permit the same name at another rank.
                    var currentFace = HappyZcFace(card);
                    var faces = state.Entities.Select(e => e.Faces[0]).Where(p => p.Suit == card.Suit && p != currentFace).ToArray();
                    if (faces.Length > 0)
                    {
                        ChangeHappyZcFace(owner, card, faces[_randomOperations._random.Next(faces.Length)], false);
                        changed++;
                    }
                }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiRefreshedEvent(f.Id, f.OwnerSeat, changed));
            ContinueHappyZc(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (operation == SkillProgramEffectOp.ResolveHappyZuoCiHuanjing)
        {
            var hp = _coreHealthQueriesOperations.UnambiguousHp(owner) ?? throw new InvalidOperationException("Huanjing requires a chosen real health pool before computing lost HP.");
            var amount = Math.Max(1, checked(2 * (owner.MaxHp - hp)));
            owner.HappyZuoCi = state with
            {
                BonusTurn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
                BonusUses = amount
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiHuanjingIssuedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, amount));
            GainHappyZc(f, amount);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (operation != SkillProgramEffectOp.GainHappyZuoCiPhantoms)
            throw new InvalidOperationException("Unknown phantom operation.");
        if (f.TriggerId == "foreign-phantom")
        {
            BeginHappyZcDraw(f, 1);
            return SkillProgramStepOutcome.AwaitChild;
        }

        GainHappyZc(f, 2);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal ProgramSkillFrame HappyZcChildren(ProgramSkillFrame supplied, HappyZuoCiStage stage)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = f.HappyZuoCi!with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        return f;
    }

    internal void CompleteHappyZcMoves(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZuoCi = f.HappyZuoCi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueHappyZc(id);
    }

    internal void GainHappyZc(ProgramSkillFrame supplied, int requested)
    {
        var f = HappyZcChildren(supplied, HappyZuoCiStage.GainChildren);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var state = owner.HappyZuoCi!;
        var holding = state.Entities.Count(e => _cardZonesOperations._cardZones.GetLocation(e.CardId)is var location && location != state.Deck && location != CardLocation.OutsideGame);
        var amount = Math.Min(requested, Math.Max(0, checked(2 * owner.MaxHp) - holding));
        var gained = new List<int>();
        for (var i = 0; i < amount; i++)
        {
            if (_cardZonesOperations._cardZones.Count(state.Deck) == 0)
            {
                var recycle = state.Entities.Where(e => _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.OutsideGame).Select(e => _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId)).ToArray();
                if (recycle.Length > 0)
                {
                    _corePhysicalMovementFacadesOperations.MoveCards(recycle, CardLocation.OutsideGame, state.Deck, new("skill-program.happy:huanshu.pool-refill"));
                    _cardZonesOperations._cardZones.Shuffle(state.Deck, _randomOperations._random);
                }
            }

            if (_cardZonesOperations._cardZones.Count(state.Deck) == 0)
                break;
            var card = _cardZonesOperations._cardZones.CardsAt(state.Deck)[0];
            _corePhysicalMovementFacadesOperations.MoveCard(card, state.Deck, CardLocation.Hand(f.OwnerSeat), new("skill-program.happy:huanshu.gain"));
            gained.Add(card.Id);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.HappyZuoCi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = r with { Requested = requested, Overflow = requested - amount, GainedIds = gained, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiGainIssuedEvent(f.Id, requested, gained.Count, requested - amount, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueHappyZc(f.Id);
    }

    internal PromptChoice HappyZcChoice(ProgramSkillFrame f, string mode, Card? card = null) => new(new($"happy-zc.{(f.Id)}.{(f.HappyZuoCi!.Stage)}.{(mode)}.{(card?.Id ?? 0)}"), card is null ? "取消幻化" : $"{((mode == "phantom" ? "幻化" : "参照"))}【{(card.DisplayName)}】{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))}{(card.RankText)}", card is null ? [] : [card.Id], [], new Dictionary<string, string> { { "program-action", HappyZcAction }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) }, { "mode", mode } });
    internal IReadOnlyList<PromptChoice> HappyZcChoices(ProgramSkillFrame f)
    {
        var r = f.HappyZuoCi!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var cards = r.Stage == HappyZuoCiStage.PhantomChoice ? HappyZcPhantomChoices(owner) : r.Stage == HappyZuoCiStage.TemplateChoice ? HappyZcTemplates(owner, r.PhantomId) : [];
        return Array.AsReadOnly(cards.Select(c => HappyZcChoice(f, r.Stage == HappyZuoCiStage.PhantomChoice ? "phantom" : "template", c)).Append(HappyZcChoice(f, "cancel")).ToArray());
    }

    internal void PublishHappyZc(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, HappyZcChoices(f), "【幻化】请选择真实手牌。", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(HappyZcAction)]
    internal void AnswerHappyZc(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZuoCi: { } r } f || !ValidHappyZc(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HappyZcChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Phantom choice lost its exact original private hand and cursor.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["mode"] == "cancel")
        {
            FinishHappyZc(f);
            return;
        }

        if (r.Stage == HappyZuoCiStage.PhantomChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = r with { Stage = HappyZuoCiStage.TemplateChoice, PhantomId = choice.Cards.Single() } });
            PublishHappyZc(f);
            return;
        }

        if (r.Stage != HappyZuoCiStage.TemplateChoice || !CanTransformHappyZc(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
            throw new InvalidOperationException("A phantom transform cannot reuse its material or turn allowance.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var phantom = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Single(c => c.Id == r.PhantomId);
        var template = HappyZcTemplates(owner, r.PhantomId).Single(c => c.Id == choice.Cards.Single());
        var same = phantom.Suit == template.Suit;
        ChangeHappyZcFace(owner, phantom, HappyZcFace(template), true);
        owner.HappyZuoCi = owner.HappyZuoCi!with
        {
            UsedTemplates = owner.HappyZuoCi.UsedTemplates.Append(template.Id).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = r with { TemplateId = template.Id } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiTransformedEvent(f.Id, f.OwnerSeat, same));
        if (same)
            GainHappyZc(f, 1);
        else
            FinishHappyZc(f);
    }

    internal void BeginHappyZcDraw(ProgramSkillFrame supplied, int amount)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = f.HappyZuoCi!with { Stage = HappyZuoCiStage.Drawing, Overflow = amount }, PendingMovementContinuation = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, amount, new("skill-program.happy:huanshu.native-draw"));
    }

    internal bool DrainHappyZc(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueHappyZc(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZuoCi: { } r } f || f.Id != id)
            return;
        if (!ValidHappyZc(f))
            throw new InvalidOperationException("Happy Zuo Ci lost its original frame or paid native result.");
        if (r.Stage is HappyZuoCiStage.PhantomChoice or HappyZuoCiStage.TemplateChoice)
        {
            PublishHappyZc(f);
            return;
        }

        if (r.Stage == HappyZuoCiStage.Drawing)
            return;
        if (DrainHappyZc(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.HappyZuoCi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == HappyZuoCiStage.GainChildren && r.Overflow > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            BeginHappyZcDraw(f, r.Overflow);
            return;
        }

        FinishHappyZc(f);
    }

    internal void FinishHappyZc(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 42600)]
    internal bool ResumeHappyZc(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZuoCi: not null } f || f.Id != id)
            return false;
        ContinueHappyZc(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42600)]
    internal bool ReturnHappyZcMovement(ProgramSkillFrame f) => f.HappyZuoCi is not null && ResumeHappyZc(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.GainHappyZuoCiPhantoms)]
    internal void ReturnHappyZcGainDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnHappyZcDraw(f, outcome);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.TransformHappyZuoCiPhantom)]
    internal void ReturnHappyZcTransformDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnHappyZcDraw(f, outcome);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyZuoCiHuanjing)]
    internal void ReturnHappyZcLimitedDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnHappyZcDraw(f, outcome);
    internal void ReturnHappyZcDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.HappyZuoCi!;
        if (!ValidHappyZc(f) || r.Stage != HappyZuoCiStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Overflow || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("A phantom overflow/destruction draw lost its original once-issued native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZuoCi = r with { Stage = HappyZuoCiStage.DrawChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZuoCiDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueHappyZc(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.TransformHappyZuoCiPhantom)]
    internal PromptChoice HappyZcAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mode") != "cancel") ?? p.Choices[0];
}
