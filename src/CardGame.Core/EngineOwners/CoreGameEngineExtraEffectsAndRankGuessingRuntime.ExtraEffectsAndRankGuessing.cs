#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessingRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 130)]
    internal bool ExtraEffectsFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardSameCategoryForExtraEffect) && ExtraEffectCardKind(query.Kind);
    internal const string RankGuessRevealReason = "skill-program.rank-guess.reveal";
    internal const string RankGuessGainReason = "skill-program.rank-guess.gain";
    internal const string ExtraEffectCostReason = "skill-program.extra-effect.cost";
    SkillProgramStepOutcome global::CardGame.Core.IExtraEffectsAndRankGuessHost.BeginExtraEffectsAndRankGuess(SkillProgramEffect effect, ProgramSkillFrame frame) => BeginExtraEffectsAndRankGuess(effect, frame);
    internal CardConversionSource ExtraEffectsSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool ExtraEffectCardKind(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ExtraEffectsAndRankGuessing.ExtraEffectCardKind(kind);
    internal bool TryGetExtraEffectUse(int owner, ProgramSkillWindowContext? context, out CardUseFrame use)
    {
        use = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCommitted } || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != owner || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.Action is not { Type: CardActionType.Use } action || action.ActorSeat != owner || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { } current || current.Action?.ActionId != action.ActionId || current.SourceSeat != owner || !ExtraEffectCardKind(current.CardKind) || current.CardKind is CardKind.Dodge or CardKind.Nullification || current.ExtraCardEffect is not null)
            return false;
        use = current;
        return true;
    }

    internal Card[] ExtraEffectCosts(int owner, CardKind kind) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => CardCatalog.Get(c.Kind).CategoryName == CardCatalog.Get(kind).CategoryName && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard)).OrderBy(c => c.Id).ToArray();
    internal int[] DeathSkillRecipients(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.MaxHp > _coreMatchParticipantsPlayersAccess._players[owner].MaxHp).Select(p => p.Seat).Order().ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 290)]
    internal bool CanOfferExtraEffectsAndRankGuess(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.DiscardSameCategoryForExtraEffect }])
            return TryGetExtraEffectUse(candidate.OwnerSeat, context, out var use) && ExtraEffectCosts(candidate.OwnerSeat, use.CardKind).Length > 0;
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.GuessRanksForDrawReplacement }])
            return _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw;
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.DeathRecipientChooseSkill }])
            return !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && context.Window == SkillProgramTriggerWindow.OwnerDied && DeathSkillRecipients(candidate.OwnerSeat).Length > 0;
        return true;
    }

    internal SkillProgramStepOutcome BeginExtraEffectsAndRankGuess(SkillProgramEffect effect, ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ExtraEffectsAndRankGuess is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || !CanOfferExtraEffectsAndRankGuess(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Extra effects require one exact unpaid actual trigger.");
        CardUseFrame? use = null;
        if (effect.Op == SkillProgramEffectOp.DiscardSameCategoryForExtraEffect && !TryGetExtraEffectUse(f.OwnerSeat, context, out use!))
            throw new InvalidOperationException("Extra effect lost its exact actual use.");
        var r = new ExtraEffectsAndRankGuessReceipt
        {
            Operation = effect.Op,
            Source = ExtraEffectsSource(f),
            GameplayHash = f.GameplayHash,
            WindowFrameId = context.ParentFrameId,
            CardUseFrameId = use?.Id,
            ActionId = use?.Action?.ActionId,
            InstructionIndex = f.InstructionIndex,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat,
            FrozenOwnerMaxHp = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp,
            Stage = effect.Op == SkillProgramEffectOp.DiscardSameCategoryForExtraEffect ? ExtraEffectsAndRankGuessStage.Cost : effect.Op == SkillProgramEffectOp.DeathRecipientChooseSkill ? ExtraEffectsAndRankGuessStage.DeathTarget : ExtraEffectsAndRankGuessStage.Guess
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraEffectsAndRankGuessOfferedEvent(f.Id, r));
        if (effect.Op == SkillProgramEffectOp.GuessRanksForDrawReplacement)
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId);
            var frozen = parent.FrozenBaseDrawCount ?? _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RankGuessNormalDrawResetEvent(f.Id, parent.Id, frozen, Math.Max(0, frozen + parent.NormalDrawAdjustment)));
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { FrozenBaseDrawCount = frozen, NormalDrawAdjustment = -frozen });
            RevealNextRankGuessCard(f, guessedGreater: null);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishExtraEffectsAndRankGuessChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ExtraEffectsAndRankGuessChoices(ProgramSkillFrame f)
    {
        var r = f.ExtraEffectsAndRankGuess!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, string? skill = null) => new(new($"extra-rank.{(f.Id)}.{(r.Stage)}.{(r.GuessIndex)}.{(branch)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}.{(skill)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "extra-effects-rank-guess", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["grant-skill"] = skill ?? "" });
        if (r.Stage == ExtraEffectsAndRankGuessStage.Cost && r.CardUseFrameId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use)
            choices.AddRange(ExtraEffectCosts(f.OwnerSeat, use.CardKind).Select(c => Choice("cost", $"弃置【{(c.DisplayName)}】，令此牌效果额外结算一次", [c.Id])));
        else if (r.Stage == ExtraEffectsAndRankGuessStage.Guess)
        {
            choices.Add(Choice("greater", $"猜下一张点数大于 {(r.LastRank)}"));
            choices.Add(Choice("less", $"猜下一张点数小于 {(r.LastRank)}"));
            if (r.GuessIndex > 0)
                choices.Add(Choice("stop", "停止猜测，获得全部展示牌"));
        }
        else if (r.Stage == ExtraEffectsAndRankGuessStage.DeathTarget)
            choices.AddRange(DeathSkillRecipients(f.OwnerSeat).Select(s => Choice("target", $"令 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 选择获得一个技能", targets: [s])));
        else if (r.Stage == ExtraEffectsAndRankGuessStage.DeathSkill && r.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            choices.AddRange(_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)!.SkillIds.Select(s => Choice("skill", $"获得【{(_contentRegistryOperations._contentRegistry.GetSkill(s).Name)}】", skill: s)));
        if (r.Stage is ExtraEffectsAndRankGuessStage.Cost or ExtraEffectsAndRankGuessStage.DeathTarget)
            choices.Add(Choice("decline", "取消"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishExtraEffectsAndRankGuessChoice(ProgramSkillFrame f)
    {
        var r = f.ExtraEffectsAndRankGuess!;
        var choices = ExtraEffectsAndRankGuessChoices(f);
        if (choices.Count == 0)
        {
            CompleteExtraEffectsAndRankGuess(f, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r with { Choices = choices }, Step = ResolutionFrameStep.AwaitingResponse });
        var chooser = r.Stage == ExtraEffectsAndRankGuessStage.DeathSkill ? r.TargetSeat!.Value : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择。", r.Stage == ExtraEffectsAndRankGuessStage.Guess ? r.RevealedCardIds : choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == ExtraEffectsAndRankGuessStage.Cost,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("extra-effects-rank-guess")]
    internal void ResolveExtraEffectsAndRankGuessChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Rank/effect choice lost its owner.");
        RequireExtraEffectsAndRankGuess(f);
        var r = f.ExtraEffectsAndRankGuess!;
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ExtraEffectsAndRankGuessChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Rank/effect choice is stale or already paid.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            CompleteExtraEffectsAndRankGuess(f, false);
            return;
        }

        if (r.Stage == ExtraEffectsAndRankGuessStage.DeathTarget)
        {
            var recipient = choice.Targets.Single();
            var recipientMax = _coreMatchParticipantsPlayersAccess._players[recipient].MaxHp;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeathRecipientSelectedEvent(f.Id, r.Source, recipient, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, recipientMax));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r with { TargetSeat = recipient, FrozenOwnerMaxHp = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, FrozenRecipientMaxHp = recipientMax, Stage = ExtraEffectsAndRankGuessStage.DeathSkill, Choices = [] } });
            PublishExtraEffectsAndRankGuessChoice(f);
            return;
        }

        if (r.Stage == ExtraEffectsAndRankGuessStage.DeathSkill)
        {
            var selected = choice.Parameters["grant-skill"];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r with { Stage = ExtraEffectsAndRankGuessStage.SkillChildren, GrantedSkillId = selected, Choices = [] } });
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat!.Value], f.SkillId, [selected]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeathRecipientSkillChosenEvent(f.Id, r.Source, r.TargetSeat.Value, selected));
            if (!DrainExtraEffectsAndRankGuessChildren(f))
                CompleteExtraEffectsAndRankGuess(f, true);
            return;
        }

        if (r.Stage == ExtraEffectsAndRankGuessStage.Guess)
        {
            if (branch == "stop")
                FinishRankGuessGain(f);
            else
                RevealNextRankGuessCard(f, branch == "greater");
            return;
        }

        if (r.Stage != ExtraEffectsAndRankGuessStage.Cost || !TryGetExtraEffectUse(f.OwnerSeat, f.WindowContext, out var use))
            throw new InvalidOperationException("Extra-effect payment lost its current real use.");
        var card = ExtraEffectCosts(f.OwnerSeat, use.CardKind).Single(c => c.Id == choice.Cards.Single());
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r with { Stage = ExtraEffectsAndRankGuessStage.MovementChildren, PaidCardId = card.Id, MovementBefore = before, MovementAfter = before, Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, CardLocation.Hand(f.OwnerSeat), new(ExtraEffectCostReason));
        _coreProgramLifecycleOperations.ConsumeProgramTriggerUsage(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ExtraEffectsAndRankGuess = f.ExtraEffectsAndRankGuess!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraEffectsAndRankGuessPaidEvent(f.Id, f.ExtraEffectsAndRankGuess!));
        if (!DrainExtraEffectsAndRankGuessChildren(f))
            ReturnExtraEffectsAndRankGuessMovement(f);
    }

    internal void RevealNextRankGuessCard(ProgramSkillFrame f, bool? guessedGreater)
    {
        var r = f.ExtraEffectsAndRankGuess!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ExtraEffectsAndRankGuess = r with { Stage = ExtraEffectsAndRankGuessStage.MovementChildren, Choices = [], MovementBefore = before, MovementAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var card = _corePhysicalDrawFacadesOperations.DrawOneToProcessing(new(RankGuessRevealReason));
        var correct = card is not null && (guessedGreater is null || (guessedGreater.Value ? card.Rank > r.LastRank : card.Rank < r.LastRank));
        var next = r with
        {
            Stage = ExtraEffectsAndRankGuessStage.MovementChildren,
            Choices = [],
            MovementBefore = before,
            MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            RevealedCardIds = card is null ? r.RevealedCardIds : r.RevealedCardIds.Append(card.Id).ToArray(),
            LastRank = card?.Rank ?? r.LastRank,
            GuessIndex = r.GuessIndex + (guessedGreater is null ? 0 : 1),
            GuessFinished = !correct
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ExtraEffectsAndRankGuess = next });
        if (card is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
        if (guessedGreater is not null && card is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RankGuessComparedEvent(f.Id, r.LastRank, card.Rank, guessedGreater.Value, correct, next.GuessIndex));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraEffectsAndRankGuessPaidEvent(f.Id, next));
        if (!DrainExtraEffectsAndRankGuessChildren(f))
            ReturnExtraEffectsAndRankGuessMovement(f);
    }

    internal bool DrainExtraEffectsAndRankGuessChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 20900)]
    internal bool ReturnExtraEffectsAndRankGuessMovement(ProgramSkillFrame f)
    {
        if (f.ExtraEffectsAndRankGuess is null)
            return false;
        RequireExtraEffectsAndRankGuess(f);
        var r = f.ExtraEffectsAndRankGuess;
        if (r.Stage is not (ExtraEffectsAndRankGuessStage.MovementChildren or ExtraEffectsAndRankGuessStage.GainChildren) || f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Rank/effect movement returned without its issued receipt.");
        if (DrainExtraEffectsAndRankGuessChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == ExtraEffectsAndRankGuessStage.GainChildren)
        {
            CompleteExtraEffectsAndRankGuess(f, true);
            return true;
        }

        if (r.Operation == SkillProgramEffectOp.DiscardSameCategoryForExtraEffect)
        {
            if (r.CardUseFrameId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { } action } use && action.ActionId == r.ActionId)
            {
                var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == r.WindowFrameId);
                _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ExtraCardEffect = new() { Source = r.Source, ProgramFrameId = f.Id, ActionId = action.ActionId, PaidCardId = r.PaidCardId!.Value, Simple = window.SimpleContinuation, Trick = window.TrickContinuation } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectIssuedEvent(f.Id, r.Source, use.Id, action.ActionId, r.PaidCardId.Value));
            }

            CompleteExtraEffectsAndRankGuess(f, true);
            return true;
        }

        if (r.Operation != SkillProgramEffectOp.GuessRanksForDrawReplacement)
            throw new InvalidOperationException("Unknown rank movement return.");
        if (r.GuessFinished || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishRankGuessGain(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r with { Stage = ExtraEffectsAndRankGuessStage.Guess, Choices = [] } });
        PublishExtraEffectsAndRankGuessChoice(f);
        return true;
    }

    internal void FinishRankGuessGain(ProgramSkillFrame f)
    {
        var r = f.ExtraEffectsAndRankGuess!;
        var remaining = r.RevealedCardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Select(_coreCardAttackStateOperations.GetAttackCard).ToArray();
        if (remaining.Length == 0)
        {
            CompleteExtraEffectsAndRankGuess(f, true);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = r with { Stage = ExtraEffectsAndRankGuessStage.GainChildren, GuessFinished = true, GainToHand = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive, MovementBefore = before, MovementAfter = before, Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var card in remaining)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile, new(RankGuessGainReason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ExtraEffectsAndRankGuess = f.ExtraEffectsAndRankGuess!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraEffectsAndRankGuessPaidEvent(f.Id, f.ExtraEffectsAndRankGuess!));
        if (!DrainExtraEffectsAndRankGuessChildren(f))
            CompleteExtraEffectsAndRankGuess(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 20900)]
    internal bool ResumeExtraEffectsAndRankGuess(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ExtraEffectsAndRankGuess: { } r } f || f.Id != id)
            return false;
        RequireExtraEffectsAndRankGuess(f);
        if (r.Stage == ExtraEffectsAndRankGuessStage.SkillChildren)
        {
            if (!DrainExtraEffectsAndRankGuessChildren(f))
                CompleteExtraEffectsAndRankGuess(f, true);
            return true;
        }

        if (f.PendingMovementContinuation is not null)
            ReturnExtraEffectsAndRankGuessMovement(f);
        return true;
    }

    internal void CompleteExtraEffectsAndRankGuess(ProgramSkillFrame f, bool completed)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ExtraEffectsAndRankGuess = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    internal void RequireExtraEffectsAndRankGuess(ProgramSkillFrame f)
    {
        var r = f.ExtraEffectsAndRankGuess ?? throw new InvalidOperationException("Rank/effect receipt is missing.");
        if (f.TriggerId is null || r.Source != ExtraEffectsSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.WindowContext?.ParentFrameId != r.WindowFrameId || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Operation || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ExtraEffectsAndRankGuessOfferedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.Source == r.Source && e.Receipt.GameplayHash == r.GameplayHash && e.Receipt.Operation == r.Operation && e.Receipt.WindowFrameId == r.WindowFrameId && e.Receipt.CardUseFrameId == r.CardUseFrameId && e.Receipt.ActionId == r.ActionId) != 1)
            throw new InvalidOperationException("Rank/effect receipt lost exact source, trigger, instruction or actual turn.");
        if (r.Operation == SkillProgramEffectOp.GuessRanksForDrawReplacement && (f.WindowContext!.Window != SkillProgramTriggerWindow.DrawPhaseStarting || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Draw || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowFrameId)is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } draw || draw.OwnerSeat != f.OwnerSeat || r.RevealedCardIds.Distinct().Count() != r.RevealedCardIds.Count || r.RevealedCardIds.Any(id => id <= 0)))
            throw new InvalidOperationException("Rank guessing lost its exact actual own draw parent or distinct public cards.");
        if (r.Operation == SkillProgramEffectOp.DeathRecipientChooseSkill)
        {
            if (f.WindowContext!.Window != SkillProgramTriggerWindow.OwnerDied || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowFrameId)is not { } deathWindow || deathWindow.OwnerSeat != f.OwnerSeat || !_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Any(d => d.Id == deathWindow.DeathFrameId && d.VictimSeat == f.OwnerSeat))
                throw new InvalidOperationException("A death skill choice lost its actual dead-owner parent.");
            if (r.TargetSeat is { } recipient && (r.FrozenRecipientMaxHp <= r.FrozenOwnerMaxHp || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DeathRecipientSelectedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.RecipientSeat == recipient && e.OwnerMaxHp == r.FrozenOwnerMaxHp && e.RecipientMaxHp == r.FrozenRecipientMaxHp) != 1 || r.Stage == ExtraEffectsAndRankGuessStage.DeathSkill && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } recipientPrompt && recipientPrompt.PlayerSeat != recipient))
                throw new InvalidOperationException("Death grant recipient lost its actual greater maximum HP choice or chooser.");
        }

        if (r.Stage is not (ExtraEffectsAndRankGuessStage.MovementChildren or ExtraEffectsAndRankGuessStage.GainChildren))
            return;
        if (r.MovementBefore < 0 || r.MovementAfter < r.MovementBefore || r.MovementAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } movement || movement.SubjectSeat != f.OwnerSeat || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ExtraEffectsAndRankGuessPaidEvent>().Any(e => e.ProgramFrameId == f.Id && e.Receipt.MovementBefore == r.MovementBefore && e.Receipt.MovementAfter == r.MovementAfter && e.Receipt.PaidCardId == r.PaidCardId && e.Receipt.RevealedCardIds.SequenceEqual(r.RevealedCardIds)))
            throw new InvalidOperationException("Rank/effect movement lost its frozen payment fact.");
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value is ExtraEffectCostReason or RankGuessRevealReason or RankGuessGainReason).ToArray();
        if (r.Operation == SkillProgramEffectOp.DiscardSameCategoryForExtraEffect && (moves is not [var paid] || paid.CardId != r.PaidCardId || paid.From != CardLocation.Hand(f.OwnerSeat) || paid.To != CardLocation.DiscardPile || paid.Reason.Value != ExtraEffectCostReason))
            throw new InvalidOperationException("Extra effect payment is not one actual matching hand discard.");
        if (r.Operation == SkillProgramEffectOp.GuessRanksForDrawReplacement)
        {
            if (r.Stage == ExtraEffectsAndRankGuessStage.GainChildren)
            {
                if (moves.Length == 0 || moves.Any(m => !r.RevealedCardIds.Contains(m.CardId) || m.From != CardLocation.Processing || m.To != (r.GainToHand ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile) || m.Reason.Value != RankGuessGainReason) || moves.Select(m => m.CardId).Distinct().Count() != moves.Length)
                    throw new InvalidOperationException("Rank gain lost its exact one-time public-card movement.");
                return;
            }

            if (moves.Length > 1 || moves.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Processing || m.CardId != r.RevealedCardIds.LastOrDefault() || m.Reason.Value != RankGuessRevealReason))
                throw new InvalidOperationException("A guess reveal was not its exact top entity moving into Processing.");
            var revealed = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardsRevealedEvent>().Where(e => e.ResolutionId == f.Id).SelectMany(e => e.Cards).ToArray();
            if (!revealed.Select(c => c.Id).SequenceEqual(r.RevealedCardIds) || revealed.Length > 0 && revealed[^1].Rank != r.LastRank)
                throw new InvalidOperationException("Rank receipt disagrees with its actual ordered public reveals.");
            var compared = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RankGuessComparedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
            if (compared.Length != revealed.Length - 1 && revealed.Length > 0 || compared.Select((e, index) => (e, index)).Any(p => p.e.GuessIndex != p.index + 1 || p.e.PreviousRank != revealed[p.index].Rank || p.e.NewRank != revealed[p.index + 1].Rank || p.e.Correct != (p.e.GuessedGreater ? p.e.NewRank > p.e.PreviousRank : p.e.NewRank < p.e.PreviousRank)) || r.GuessIndex != compared.Length && moves.Length == 1 || !r.GuessFinished && compared.LastOrDefault()is { Correct: false })
                throw new InvalidOperationException("Rank receipt lost its strict sequential comparison facts.");
        }
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 410)]
    internal bool IsExtraEffectsAndRankGuessMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation movement) => f.ExtraEffectsAndRankGuess is { Stage: ExtraEffectsAndRankGuessStage.MovementChildren or ExtraEffectsAndRankGuessStage.GainChildren } r && e?.Op == r.Operation && movement.SubjectSeat == f.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 400)]
    internal bool CanContinueExtraEffectsAndRankGuess(ProgramSkillFrame f)
    {
        if (f.ExtraEffectsAndRankGuess is not { } r || r.Stage is not (ExtraEffectsAndRankGuessStage.MovementChildren or ExtraEffectsAndRankGuessStage.GainChildren or ExtraEffectsAndRankGuessStage.SkillChildren or ExtraEffectsAndRankGuessStage.Guess) || r.Stage == ExtraEffectsAndRankGuessStage.Guess && r.RevealedCardIds.Count == 0)
            return false;
        RequireExtraEffectsAndRankGuess(f);
        return true;
    }
}
