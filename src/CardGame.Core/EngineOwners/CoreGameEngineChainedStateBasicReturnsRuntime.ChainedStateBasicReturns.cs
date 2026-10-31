#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineChainedStateBasicReturnsRuntime
{
    internal void ExecuteChainedStateBasicPaid(ChainedStateBasicFrame frame)
    {
        var p = frame.Payment;
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        if (p.Intent == ChainedStateBasicIntent.OwnSlashDodge)
        {
            ResolveChainedStateBasicDodge(p);
            return;
        }

        if (p.Intent == ChainedStateBasicIntent.Play)
        {
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, true, null);
            return;
        }

        if (p.Intent == ChainedStateBasicIntent.BorrowedSword)
        {
            var borrowed = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(borrowed.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(borrowed.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            borrowed.AwaitingSlashChoice = false;
            borrowed.SlashCardId = 0;
            borrowed.EffectiveSlashKind = p.EffectiveKind;
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, borrowed, true, null);
            return;
        }

        if (p.Intent == ChainedStateBasicIntent.Qinglong)
        {
            var original = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(original.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(original.ResolutionId, actor.Seat, p.TargetSeat!.Value, Used: true, SlashCardIds: [], EffectiveSlashKind: p.EffectiveKind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(original.ResolutionId);
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, null);
            return;
        }

        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A paid chained-state Slash lost its request parent.");
        if (!ChainedStateBasicProgramActor(parent, p))
            throw new InvalidOperationException("A paid chained-state Slash changed its original actor or instruction.");
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(parent))
        {
            _characterLvKuangLvXiangOlOperations.PrepareQigongChainedSlash(parent, p);
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (_characterWuJingOlOperations.IsWuJingHejiRequest(parent))
        {
            _characterWuJingOlOperations.PrepareWuJingChainedSlash(parent, p);
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(parent))
        {
            _characterCaoChunOlOperations.PrepareCaoChunChainedSlash(parent, p);
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        var effect = ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect;
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(parent))
        {
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(parent, actor.Seat, actor.Seat, p.TargetSeat!.Value, p.EffectiveKind, [], p.Source);
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(parent))
        {
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(parent, actor.Seat, actor.Seat, p.TargetSeat!.Value, p.EffectiveKind, [], p.Source);
            BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
            return;
        }

        if (p.Intent == ChainedStateBasicIntent.ProgramSlash)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, effect.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, actor.Seat, "解除连环后视为使用【杀】。");
        else if (p.Intent == ChainedStateBasicIntent.AssistedSlash)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(parent.Id, effect.ResultBind!, "used-slash", actor.Seat, "解除连环后视为使用【杀】。");
        }
        else if (p.Intent == ChainedStateBasicIntent.NearestLegalSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { NearestLegalSlashRequest = null });
        else if (p.Intent != ChainedStateBasicIntent.ProgramNearestSlash)
            throw new InvalidOperationException("A paid chained-state Slash has an unknown return.");
        BeginChainedStateBasicSlash(actor, frame.TargetSeats, p, null, false, parent.Id);
    }

    internal void BeginChainedStateBasicSlash(CharacterState actor, IReadOnlyList<int> targets, ChainedStateBasicPayment payment, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit, long? programParent)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(payment.EffectiveKind) || targets.Count == 0)
            throw new InvalidOperationException("A chained-state Slash requires its accepted actual targets.");
        var kind = payment.EffectiveKind;
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, kind, Suit.None, 0), actor.Seat, targets, kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], kind), physicalCardIds: [], conversionSource: payment.Source, chainedStatePayment: payment);
        var actualTargets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, actualTargets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[actualTargets[0]], kind), programSkillCardUseFrameId: programParent);
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (countsTowardLimit && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, payment.Source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, actualTargets[0], IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[actualTargets[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, actualTargets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal CardActionContext PrepareChainedStateBasicAction(CardActionContext action, ChainedStateBasicPayment? payment) => payment is null ? action : _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: false));
    internal void IssueChainedStateBasicUse(CardUseFrame use, ChainedStateBasicPayment? payment)
    {
        if (payment is not { } p)
            return;
        if (!_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p) || p.Intent == ChainedStateBasicIntent.OwnSlashDodge || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: false } action || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || action.ActorSeat != p.Source.OwnerSeat || action.ProviderSeat != action.ActorSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.EffectiveKind != p.EffectiveKind || use.CardKind != p.EffectiveKind || action.ConversionChain is not [var source] || source != p.Source || action.TargetSeats.Count == 0 || action.TargetSeats.Distinct().Count() != action.TargetSeats.Count || !action.TargetSeats.SequenceEqual(use.TargetSeats))
            throw new InvalidOperationException("A chained-state Use lost its exact paid source, neutral zero material or native targets.");
        var receipt = new ChainedStateBasicUseReceipt(p, use.Id, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ChainedStateBasicUse = receipt });
        EmitChainedStateBasicIssued(receipt);
    }

    internal void EmitChainedStateBasicIssued(ChainedStateBasicUseReceipt receipt)
    {
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChainedStateBasicIssuedEvent>().Any(e => e.Receipt.Payment.PaymentFrameId == receipt.Payment.PaymentFrameId))
            throw new InvalidOperationException("One chained-state payment cannot issue two basic Uses.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedStateBasicIssuedEvent(receipt));
    }

    internal bool IsChainedStateBasicIssued(ChainedStateBasicUseReceipt receipt) => _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(receipt.Payment) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChainedStateBasicIssuedEvent>().Where(e => e.Receipt.Payment.PaymentFrameId == receipt.Payment.PaymentFrameId).ToArray()is [var fact] && fact.Receipt == receipt;
    internal bool IsIssuedChainedStateBasicUse(CardUseFrame use)
    {
        if (use.ChainedStateBasicUse is not { } r || r.OwnerFrameId != use.Id || !IsChainedStateBasicIssued(r) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use } current)
            return false;
        var action = use.CurrentSlashFirePolicy is { } fire ? fire.OriginalAction : current;
        if (use.CurrentSlashFirePolicy is not null)
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        var p = r.Payment;
        return action.ActionId == r.CardActionId && action.EffectiveKind == p.EffectiveKind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectiveKind) && (use.CardKind == p.EffectiveKind || use.CurrentSlashFirePolicy is not null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind)) && action.ProviderSeat == p.Source.OwnerSeat && action.RequesterSeat is null && action.ResponderSeat is null && action.OpponentSeat is null && action.PhysicalCards.Count == 0 && action.ConversionChain is [var source] && source == p.Source && action.EffectiveSuit == Suit.None && action.EffectiveRank == 0 && action.EffectiveIsRed == false && current.TargetSeats.SequenceEqual(use.TargetSeats) && (action.ActorSeat == p.Source.OwnerSeat && use.SourceSeat == action.ActorSeat || _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, p.Source.OwnerSeat));
    }

    internal bool IsChainedStateBasicUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedChainedStateBasicUse(use);
    internal Card ChainedStateBasicRepresentation(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedChainedStateBasicUse(use) ? new(0, use.CardKind, Suit.None, 0) : throw new InvalidOperationException("A chained-state logical card lost its unique paid receipt.");
    internal Card ReadChainedStateBasicUseAppearance(long id, CardAppearanceReference appearance) => appearance.Id == 0 && IsChainedStateBasicUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind == appearance.Kind && appearance.Suit == Suit.None && appearance.Rank == 0 ? ChainedStateBasicRepresentation(id) : _coreDrawFundedDistinctBasicReturnsOperations.ReadDrawFundedDistinctBasicUseAppearance(id, appearance);
    internal bool SkipChainedStateBasicFinishedMovement(long id, Card card)
    {
        if (card.Id != 0 || !IsChainedStateBasicUse(id))
            return false;
        if (card.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind)
            throw new InvalidOperationException("A chained-state logical completion changed its card name.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 10)]
    internal bool FinishChainedStateBasicAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsChainedStateBasicUse(attack.ResolutionId) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    internal void ResolveChainedStateBasicDodge(ChainedStateBasicPayment p)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat];
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("A chained-state Dodge lost its original Slash.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        if (!_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p) || p.Intent != ChainedStateBasicIntent.OwnSlashDodge || p.ParentFrameId != use.Id || p.ParentActionId != use.Action?.ActionId || p.Cursor != attack.SuccessfulDodgeResponses || actor.Seat != attack.TargetSeat || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, CardKind.Dodge))
            throw new InvalidOperationException("A paid chained-state Dodge changed its original native Slash-defense direction.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, use.Action?.ActionId, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, attack.SourceSeat, CardKind.Dodge, [], [], [p.Source], effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: false));
        var receipt = new ChainedStateBasicUseReceipt(p, use.Id, action.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!with { ChainedStateBasicResponse = receipt });
        EmitChainedStateBasicIssued(receipt);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Dodge);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(0, actor.Seat, attack.SourceSeat, CardKind.Dodge));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(attack, action, ProgramCardContinuation.Dodge))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [attack.SourceSeat], ProgramCardContinuation.Dodge))
            return;
        if (_coreResponseUseCompletionOperations.HasResponseUseCompletionObserver(action, ProgramCardContinuation.Dodge) || _coreResponseCompletionOperations.HasCardResponseCompletedObserver(action, ProgramCardContinuation.Dodge))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, ProgramCardContinuation.Dodge);
        else
            _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }

    internal CardActionContext RequireChainedStateBasicResponseAction(CardUseFrame use)
    {
        var receipt = use.ChainedStateBasicResponse ?? throw new InvalidOperationException("A paid chained-state Dodge lost its owning receipt.");
        var p = receipt.Payment;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == receipt.CardActionId).ToArray()is not [var accepted] || !IsChainedStateBasicIssued(receipt) || receipt.OwnerFrameId != use.Id || p.ParentFrameId != use.Id || p.Intent != ChainedStateBasicIntent.OwnSlashDodge || use.CardAttack is not { } attack || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.TargetSeat != p.Source.OwnerSeat || attack.SuccessfulDodgeResponses != p.Cursor || p.TargetSeat != attack.SourceSeat || accepted.Action is not { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge, PhysicalCards.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: false } action || action.ParentActionId != use.Action?.ActionId || action.ParentActionId != p.ParentActionId || action.ActorSeat != p.Source.OwnerSeat || action.ProviderSeat != action.ActorSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null || action.OpponentSeat != attack.SourceSeat || action.ConversionChain is not [var source] || source != p.Source || action.TargetSeats.Count != 0)
            throw new InvalidOperationException("A chained-state Dodge changed its unique accepted action, native owner or paid response cursor.");
        return action;
    }

    internal void ClearChainedStateBasicDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ChainedStateBasicResponse: not null } use)
            return;
        var action = RequireChainedStateBasicResponseAction(use);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Action.ActionId == action.ActionId) || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null)
            throw new InvalidOperationException("A chained-state Dodge cannot return before its exact native children.");
        EmitChainedStateBasicReturned(use.ChainedStateBasicResponse!);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ChainedStateBasicResponse = null });
    }

    internal bool ChainedStateBasicProgramActor(ProgramSkillFrame parent, ChainedStateBasicPayment p)
    {
        if (parent.Id != p.ParentFrameId || parent.Id != p.RequestFrameId || parent.InstructionIndex != p.Cursor || parent.InstructionIndex < 1 || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(parent.SkillId)?.Program is not { } definition || definition.GameplayHash != parent.GameplayHash)
            return false;
        if (_characterSunYuanOlOperations.TryGetSayingSlashRequest(parent, out var sayingActor, out var sayingTargets))
            return p.Intent == ChainedStateBasicIntent.AssistedSlash && sayingActor == p.Source.OwnerSeat && p.TargetSeat is { } sayingTarget && sayingTargets.Contains(sayingTarget);
        if (parent.Chanshuang is not null)
            return p.Intent == ChainedStateBasicIntent.AssistedSlash && _characterLiYiOlOperations.TryGetChanshuangSlashRequest(parent, out var actor, out var targets) && actor == p.Source.OwnerSeat && p.TargetSeat is { } target && targets.Contains(target);
        if (parent.LvKuangLvXiang is not null)
            return p.Intent == ChainedStateBasicIntent.AssistedSlash && _characterLvKuangLvXiangOlOperations.QigongAdapterActor(parent, p.Source.OwnerSeat, p.TargetSeat, p.Cursor, p.ParentFrameId, p.RequestFrameId);
        if (parent.WuJing is not null)
            return p.Intent == ChainedStateBasicIntent.AssistedSlash && _characterWuJingOlOperations.WuJingHejiAdapterActor(parent, p.Source.OwnerSeat, p.TargetSeat, p.Cursor, p.ParentFrameId, p.RequestFrameId);
        if (parent.CaoChun is not null)
            return p.Intent == ChainedStateBasicIntent.AssistedSlash && _characterCaoChunOlOperations.CaoChunInitialAdapterActor(parent, p.Source.OwnerSeat, p.TargetSeat, p.Cursor, p.ParentFrameId, p.RequestFrameId);
        var plan = ProgramInstructionResolver.Default.Resolve(parent, definition);
        if (parent.InstructionIndex > plan.Instructions.Count)
            return false;
        var op = plan.GetPausedInstruction(parent.InstructionIndex).Effect.Op;
        if (p.Intent == ChainedStateBasicIntent.ProgramNearestSlash)
        {
            var cursors = parent.NumberBindings.Where(b => b.Name == "participant-cursor-" + (parent.InstructionIndex - 1)).ToArray();
            return op == SkillProgramEffectOp.RequestSlashByNearest && parent.ReexecuteParticipantInstruction && cursors is [var cursor] && cursor.Value >= 1 && cursor.Value < _coreMatchDefinitionPlayerCountAccess._playerCount && p.Source.OwnerSeat == _coreTableOrderOperations.TableStepSeat(parent.OwnerSeat, cursor.Value);
        }

        return parent.SelectedTargetSeats.SequenceEqual([p.Source.OwnerSeat]) && (p.Intent switch
        {
            ChainedStateBasicIntent.ProgramSlash => op == SkillProgramEffectOp.RequestSlashByTarget,
            ChainedStateBasicIntent.AssistedSlash => op == SkillProgramEffectOp.RequestSlashAgainstChosenTarget,
            ChainedStateBasicIntent.NearestLegalSlash => op == SkillProgramEffectOp.RequestLegalSlashByNearest,
            _ => false
        });
    }

    internal bool IsChainedStateBasicProgramSelection(ProgramSkillFrame parent, SkillProgramEffect effect, ChainedStateBasicFrame paid)
    {
        var p = paid.Payment;
        if (!_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p) || !ChainedStateBasicProgramActor(parent, p) || paid.OriginalDecision.PlayerSeat != p.Source.OwnerSeat || paid.OriginalDecision.TargetSeat != p.Source.OwnerSeat || paid.OriginalDecision.PromptId != p.OriginalPromptId || paid.OriginalDecision.Revision != p.OriginalRevision || ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect != effect)
            return false;
        if (p.Intent != ChainedStateBasicIntent.ProgramNearestSlash)
            return true;
        var native = global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(paid.OriginalDecision)));
        return native.Kind == DecisionKind.ProgramTrigger && native.Choices.Count > 0 && native.Choices.All(c => c.Parameters.GetValueOrDefault("frame-id") == parent.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) && c.Parameters.GetValueOrDefault("seat") == p.Source.OwnerSeat.ToString(System.Globalization.CultureInfo.InvariantCulture)) && native.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "request-slash-nearest-decline");
    }

    [NativeUseValidation(30)]
    internal void AssertChainedStateBasicUseCompletion(CardUseFrame use)
    {
        if (use.ChainedStateBasicUse is null && use.ChainedStateBasicResponse is null)
            return;
        if (use.ChainedStateBasicUse is not { } r || use.ChainedStateBasicResponse is not null || !IsIssuedChainedStateBasicUse(use) || use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.ParentFrameId == use.Id))
            throw new InvalidOperationException("A chained-state Use cannot finish before its original native children return.");
        if (r.Payment.Intent is ChainedStateBasicIntent.ProgramSlash or ChainedStateBasicIntent.ProgramNearestSlash or ChainedStateBasicIntent.AssistedSlash or ChainedStateBasicIntent.NearestLegalSlash && (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.Payment.ParentFrameId)is not ProgramSkillFrame parent || !ChainedStateBasicProgramActor(parent, r.Payment) || use.CardAttack?.ProgramSkillCardUseFrameId != parent.Id))
            throw new InvalidOperationException("A chained-state Slash lost its exact original program return.");
    }

    [NativeUseReturn(100)]
    internal void ReturnChainedStateBasicUse(CardUseFrame completed)
    {
        if (completed.ChainedStateBasicUse is not { } receipt)
            return;
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == completed.Id) || !IsChainedStateBasicIssued(receipt))
            throw new InvalidOperationException("A chained-state return must follow native owner completion.");
        EmitChainedStateBasicReturned(receipt);
    }

    internal void EmitChainedStateBasicReturned(ChainedStateBasicUseReceipt r)
    {
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChainedStateBasicReturnedEvent>().Any(e => e.Receipt.Payment.PaymentFrameId == r.Payment.PaymentFrameId))
            throw new InvalidOperationException("A chained-state Use cannot return twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChainedStateBasicReturnedEvent(r));
    }

    internal bool IsChainedStateBasicAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || !IsIssuedChainedStateBasicUse(use) || attack.Card is not null || attack.PhysicalCards.Count != 0 || attack.EffectiveCardKind != use.CardKind || attack.SourceSeat != use.SourceSeat)
            return false;
        var p = use.ChainedStateBasicUse!.Payment;
        int[] parentIds = [];
        if (p.Intent == ChainedStateBasicIntent.BorrowedSword)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not { ActiveAttack: { } child } b || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(child, attack) || b.ResolutionId != p.ParentFrameId || b.WeaponOwnerSeat != p.Source.OwnerSeat || b.SlashTargetSeat != p.TargetSeat || use.Action?.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(b.ResolutionId)?.Action?.ActionId)
                return false;
            parentIds = _coreNativeTrickUseOperations.GetCardUsePhysicalCards(b.ResolutionId).Select(c => c.Id).ToArray();
        }
        else if (p.Intent == ChainedStateBasicIntent.Qinglong)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongFollowup is not { } q || q.OuterResolutionId != p.ParentFrameId || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QinglongCrescentBladeResolvedEvent>().LastOrDefault(e => e.ResolutionId == p.ParentFrameId)is not { Used: true, SlashCardIds.Count: 0 } fact || fact.SourceSeat != p.Source.OwnerSeat || fact.TargetSeat != p.TargetSeat || fact.EffectiveSlashKind != p.EffectiveKind)
                return false;
        // Suspended Qinglong ancestor materials are checked and excluded by the native attack boundary.
        }
        else if (p.Intent is ChainedStateBasicIntent.ProgramSlash or ChainedStateBasicIntent.ProgramNearestSlash or ChainedStateBasicIntent.AssistedSlash or ChainedStateBasicIntent.NearestLegalSlash)
        {
            if (attack.ProgramSkillCardUseFrameId != p.ParentFrameId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.ParentFrameId)is not ProgramSkillFrame parent || !ChainedStateBasicProgramActor(parent, p))
                return false;
            parentIds = _resolutionStackOperations._resolutionStack.Take(_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id)).OfType<CardUseFrame>().SelectMany(f => f.PhysicalCardIds ?? []).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Distinct().ToArray();
        }
        else if (p.Intent != ChainedStateBasicIntent.Play || attack.ProgramSkillCardUseFrameId is not null)
            return false;
        return parentIds.All(id => processing.Any(c => c.Id == id)) && processing.Select(c => c.Id).Order().SequenceEqual(parentIds.Order());
    }

    internal bool ChainedStateBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => child is ChainedStateBasicFrame paid && paid.Payment.RequestFrameId == parent.Id && paid.Payment.ParentFrameId is { } owner && _resolutionStackOperations._resolutionStack.Any(f => f.Id == owner) && paid.OriginalDecision.PlayerSeat == paid.Payment.Source.OwnerSeat && paid.OriginalDecision.PromptId == paid.Payment.OriginalPromptId && paid.OriginalDecision.Revision == paid.Payment.OriginalRevision && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(paid.Payment);
    internal bool IsChainedStateQinglongCompletedWindowRide(ProgramCardTriggerWindowFrame window)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        if (index < 1 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index - 1] is not CardUseFrame original || window.Continuation != ProgramCardContinuation.CompletedSlash || window.ParentFrameId != original.Id || window.AttackOwnerFrameId != original.Id || original.Step != ResolutionFrameStep.Completed || original.Action is not { Type: CardActionType.Use } action || window.Action.ActionId != action.ActionId || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(original.CardKind) || original.CardAttack is not { } attack || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || original.Continuations.QinglongFollowup is not { Active: true, Decision: { } saved } suspension || suspension.NextAttackOwnerId != original.Id || suspension.FangtianOwnerId is not null || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var bound = _resolutionStackOperations._resolutionStack[index + 1] as ProgramSkillFrame;
        var next = index + (bound is null ? 1 : 2);
        if (next >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[next] is not CardUseFrame followup || window.Activated != (bound is not null) || !IsIssuedChainedStateBasicUse(followup) || followup.ChainedStateBasicUse!.Payment is not { Intent: ChainedStateBasicIntent.Qinglong, Cursor: 0 } p || p.ParentFrameId != original.Id || p.RequestFrameId != original.Id || p.ParentActionId != action.ActionId || p.Source.OwnerSeat != original.SourceSeat || p.TargetSeat != attack.TargetSeat || followup.SourceSeat != original.SourceSeat || followup.Action!.ParentActionId != action.ActionId || !followup.TargetSeats.SequenceEqual([attack.TargetSeat]) || !followup.Action.TargetSeats.SequenceEqual(followup.TargetSeats) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(followup.CardKind))
            return false;
        var candidate = window.Candidates[window.CandidateIndex];
        var context = candidate.FrozenContext;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } cardUse } || context.ParentFrameId != window.Id || cardUse.ParentCardUseFrameId != original.Id || cardUse.CardActionId != action.ActionId || _coreCardActionsOperations.CreateCardActionProgramContext2(window, candidate) != context || saved.Kind != DecisionKind.ProgramTrigger || !saved.IsPrivate || saved.SkillPrompt?.SkillId != candidate.SkillId || saved.Choices.Count == 0)
            return false;
        if (bound is null)
        {
            if (!_coreProgramLifecycleOperations.GetProgramTrigger2(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)).Optional || saved.PlayerSeat != (context.OptionalChooserSeat ?? candidate.OwnerSeat) || saved.SourceSeat != (context.OptionalChooserSeat is not null ? candidate.OwnerSeat : context.SourceSeat) || saved.TargetSeat != (context.OptionalChooserSeat ?? context.TargetSeat ?? candidate.OwnerSeat) || saved.Choices.Count != 2 || !saved.Choices.Select(c => c.Parameters.GetValueOrDefault("program-action")).Order().SequenceEqual(["activate", "skip"]) || saved.Choices.Any(c => c.Cards.Count != 0 || c.Targets.Count != 0 || c.Parameters.GetValueOrDefault("skill-id") != candidate.SkillId || c.Parameters.GetValueOrDefault("binding-id") != candidate.TriggerId || c.Parameters.GetValueOrDefault("skill-instance-id") != candidate.SkillInstanceId))
                return false;
        }
        else
        {
            if (bound.WindowContext != context || bound.OwnerSeat != candidate.OwnerSeat || bound.SkillId != candidate.SkillId || bound.SkillInstanceId != candidate.SkillInstanceId || bound.TriggerId != candidate.TriggerId || bound.GameplayHash != candidate.GameplayHash || bound.InstructionIndex < 1)
                return false;
            var effect = ProgramInstructionResolver.Default.Resolve(bound, _contentRegistryOperations._contentRegistry.GetSkill(bound.SkillId).Program!).GetPausedInstruction(bound.InstructionIndex).Effect;
            if (effect.Op != SkillProgramEffectOp.ChooseOption || bound.ChoiceBindings.Any(b => b.Name == effect.ResultBind) || saved.PlayerSeat != (effect.ChooserRef is { } chooser ? _coreProgramCardPaymentsOperations.ResolveProgramParticipant(bound, chooser) : global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(bound, effect.Target)) || saved.SourceSeat != bound.OwnerSeat || saved.TargetSeat != saved.PlayerSeat || saved.Choices.Select(c => c.Id).Distinct().Count() != saved.Choices.Count || saved.Choices.Any(c => c.Cards.Count != 0 || c.Targets.Count != 0 || c.Parameters.GetValueOrDefault("program-action") != "choose-option" || c.Parameters.GetValueOrDefault("frame-id") != bound.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || c.Parameters.GetValueOrDefault("result-bind") != effect.ResultBind || !effect.Options.Any(o => o.Id == c.Parameters.GetValueOrDefault("option-id") && c.Id.Value == $"program-option.frame-{(bound.Id)}.{(effect.ResultBind)}.{(o.Id)}")))
                return false;
        }

        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QinglongCrescentBladeResolvedEvent>().LastOrDefault(e => e.ResolutionId == original.Id)is { Used: true, SlashCardIds.Count: 0 } issued && issued.SourceSeat == p.Source.OwnerSeat && issued.TargetSeat == p.TargetSeat && issued.EffectiveSlashKind == p.EffectiveKind && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == followup.Id).ToArray()is [var declared] && declared.SourceSeat == p.Source.OwnerSeat && declared.CardId == 0 && declared.CardKind == p.EffectiveKind;
    }

    internal bool ChainedStateBasicLegacyAlcoholRide(ChainedStateBasicFrame paid, int dyingIndex, DyingFrame dying)
    {
        var paidIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == paid.Id);
        if (paidIndex < 0 || paidIndex + 1 >= dyingIndex || dyingIndex + 3 >= _resolutionStackOperations._resolutionStack.Count || !_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(paid.Payment) || !ChainedStateBasicStateChild(paid, _resolutionStackOperations._resolutionStack[paidIndex + 1]) || _resolutionStackOperations._resolutionStack[dyingIndex].Id != dying.Id || _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId != dying.Id || dying.Continuation != DyingContinuationKind.ProgramSkill || dying.KillerSeat is not null || dying.ResponderIndex < 0 || dying.ResponderIndex >= dying.ResponderSeats.Count || _resolutionStackOperations._resolutionStack[dyingIndex - 1] is not ProgramSkillFrame losing || losing.Id != dying.ParentFrameId || _resolutionStackOperations._resolutionStack[dyingIndex + 1] is not ProgramSkillFrame program || program.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dying.Id || context.TargetSeat != dying.VictimSeat || context.SourceSeat is not null || context.DamageFrameId is not null || context.OwnerSeat != program.OwnerSeat || context.OccurrenceIndex != 0 || program.OwnerSeat != dying.VictimSeat || program.OwnerSeat != dying.ResponderSeat || string.IsNullOrWhiteSpace(program.TriggerId) || string.IsNullOrWhiteSpace(program.SkillInstanceId) || program.ActivationId != program.TriggerId || program.InstructionIndex < 1 || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(program.SkillId)?.Program is not { } definition || definition.GameplayHash != program.GameplayHash || definition.Triggers.SingleOrDefault(trigger => trigger.Id == program.TriggerId)?.Window != context.Window)
            return false;
        for (var index = paidIndex + 2; index <= dyingIndex; index++)
        {
            var child = _resolutionStackOperations._resolutionStack[index];
            var parent = _resolutionStackOperations._resolutionStack[index - 1];
            if (!(ChainedStateBasicStructuralEdge(parent, child) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(child, parent) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingRide(child, parent) || parent is DyingFrame entry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, entry) || index >= 2 && _resolutionStackOperations._resolutionStack[index - 2] is DyingFrame originalEntry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, originalEntry)))
                return false;
        }

        var plan = ProgramInstructionResolver.Default.Resolve(program, definition);
        if (program.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(program.InstructionIndex).Effect is not { Op: SkillProgramEffectOp.UseVirtualDyingAlcohol, Target: SkillProgramEffectTarget.Owner } || _resolutionStackOperations._resolutionStack[dyingIndex + 2] is not CardUseFrame { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, Action: not null, LegacyDyingAlcoholReturn: { } returned, Step: ResolutionFrameStep.ResolvingEffect } use || use.SourceSeat != dying.VictimSeat || !use.TargetSeats.SequenceEqual([dying.VictimSeat]) || use.DyingResponse is not null || use.VirtualBasicReturn is not null || returned.ProgramFrameId != program.Id || returned.InstructionIndex != program.InstructionIndex || returned.ProducerSource != new CardConversionSource(program.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || _resolutionStackOperations._resolutionStack[dyingIndex + 3] is not HpChangedTriggerWindowFrame hp || hp.Id != hp.Change.Id || hp.ResumeFrameId != use.Id || hp.Change.ParentFrameId != use.Id || hp.Continuation != PostEventContinuation.CardUse || hp.CardId != 0 || hp.CardKind != CardKind.Alcohol || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.SourceSeat != dying.VictimSeat || hp.Change.TargetSeat != dying.VictimSeat || hp.Change.Amount != 1 || hp.Change.HpBefore > 0 || hp.Change.HpAfter != hp.Change.HpBefore + 1 || hp.Candidates.Count == 0 || hp.Candidates.Count != hp.Contexts.Count || hp.CandidateIndex < 0 || hp.CandidateIndex > hp.Candidates.Count)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        // A paid rescue keeps its accepted source identity even if a child later
        // disables that skill. Re-enumerating live eligibility would revoke it.
        if (history.OfType<ProgramBindingStartedEvent>().Count(fact => fact.FrameId == program.Id && fact.SkillId == program.SkillId && fact.BindingId == program.TriggerId && fact.SkillInstanceId == program.SkillInstanceId && fact.OwnerSeat == program.OwnerSeat && fact.Window == context.Window) != 1 || history.OfType<PlayerDyingEvent>().Count(fact => fact.ResolutionId == dying.Id && fact.VictimSeat == dying.VictimSeat && fact.KillerSeat is null) != 1 || history.OfType<CardUseDeclaredEvent>().Count(fact => fact.ResolutionId == use.Id && fact.CardId == 0 && fact.CardKind == CardKind.Alcohol && fact.SourceSeat == dying.VictimSeat) != 1 || history.OfType<TargetsConfirmedEvent>().Count(fact => fact.ResolutionId == use.Id && fact.TargetSeats.SequenceEqual([dying.VictimSeat])) != 1 || history.OfType<ProgramDyingRescueEvent>().Count(fact => fact.DyingFrameId == dying.Id && fact.SkillId == program.SkillId && fact.OwnerSeat == program.OwnerSeat && fact.VictimSeat == dying.VictimSeat && fact.CardId == 0 && fact.RecoveredHp == 1 && fact.VictimHp == hp.Change.HpAfter) != 1)
            return false;
        for (var index = 0; index < hp.Candidates.Count; index++)
        {
            var candidate = hp.Candidates[index];
            var hpContext = hp.Contexts[index];
            if (candidate.OwnerSeat != dying.VictimSeat || hpContext.OwnerSeat != candidate.OwnerSeat || hpContext.ParentFrameId != hp.Id || hpContext.HpChange != hp.Change || hpContext.SourceSeat != hp.Change.SourceSeat || hpContext.TargetSeat != hp.Change.TargetSeat || hpContext.Amount != hp.Change.Amount || hpContext.OccurrenceIndex != candidate.OccurrenceIndex || hpContext.Window is not (SkillProgramTriggerWindow.AfterHpRecovered or SkillProgramTriggerWindow.AfterHealthChanged) || _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Window != hpContext.Window)
                return false;
        }

        if (dyingIndex + 4 == _resolutionStackOperations._resolutionStack.Count)
            return true;
        return dyingIndex + 5 == _resolutionStackOperations._resolutionStack.Count && hp.CandidateIndex < hp.Candidates.Count && _resolutionStackOperations._resolutionStack[dyingIndex + 4] is ProgramSkillFrame observer && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(observer, hp.Candidates[hp.CandidateIndex]) && observer.WindowContext == hp.Contexts[hp.CandidateIndex];
    }

    internal bool ChainedStateBasicStateChild(ChainedStateBasicFrame paid, ResolutionFrame child) => child is ProgramLifecycleTriggerWindowFrame state && paid.ActiveChildFrameId == state.Id && paid.Payment.CharacterStateChangeId == state.Id && state.ResumeProgramFrameId == paid.Id && state.CharacterStateContinuation == CharacterStateContinuation.ChainedStateBasic && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.Window == SkillProgramTriggerWindow.CharacterEnteredChain && state.OwnerSeat == paid.Payment.Source.OwnerSeat && state.ResumeCardId is null && state.ResumeCardKind is null;
    internal bool ChainedStateBasicStructuralEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (parent is ChainedStateBasicFrame paid)
            return _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(paid.Payment) && ChainedStateBasicStateChild(paid, child);
        if (parent is ProgramLifecycleTriggerWindowFrame { CharacterStateContinuation: CharacterStateContinuation.ChainedStateBasic } state && child is ProgramSkillFrame observer)
            return state.CandidateIndex >= 0 && state.CandidateIndex < state.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(observer, state.Candidates[state.CandidateIndex]) && observer.WindowContext is { Window: SkillProgramTriggerWindow.CharacterEnteredChain } context && context.ParentFrameId == state.Id && context.OwnerSeat == observer.OwnerSeat && context.TargetSeat == state.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<ChainedStateBasicFrame>().Any(root => ChainedStateBasicStateChild(root, state) && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(root.Payment));
        if (parent is ProgramSkillFrame program && child is CardUseFrame { ChainedStateBasicUse: { } receipt } use)
            return IsIssuedChainedStateBasicUse(use) && ChainedStateBasicProgramActor(program, receipt.Payment) && use.CardAttack?.ProgramSkillCardUseFrameId == program.Id;
        return ChainedStateBasicFrameRidesOn(child, parent);
    }

    internal bool ChainedStateBasicPaidSuffix(int rootIndex)
    {
        if (_resolutionStackOperations._resolutionStack[rootIndex] is not ChainedStateBasicFrame paid || !_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(paid.Payment))
            return false;
        if (rootIndex == _resolutionStackOperations._resolutionStack.Count - 1)
            return paid.ActiveChildFrameId is null;
        if (!ChainedStateBasicStateChild(paid, _resolutionStackOperations._resolutionStack[rootIndex + 1]))
            return false;
        for (var dyingIndex = rootIndex + 2; dyingIndex < _resolutionStackOperations._resolutionStack.Count; dyingIndex++)
            if (_resolutionStackOperations._resolutionStack[dyingIndex] is DyingFrame dying && ChainedStateBasicLegacyAlcoholRide(paid, dyingIndex, dying))
                return true;
        for (var i = rootIndex + 2; i < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            var child = _resolutionStackOperations._resolutionStack[i];
            var parent = _resolutionStackOperations._resolutionStack[i - 1];
            if (parent is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(i - 1, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i - 1, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(i - 1, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(i - 1, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i - 1, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i - 1, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i - 1, dying))))
                return true;
            if (ChainedStateBasicStructuralEdge(parent, child) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(i) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(child, parent) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingRide(child, parent) || parent is DyingFrame entry && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, entry) || i >= 2 && _resolutionStackOperations._resolutionStack[i - 2] is DyingFrame prior && global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, prior))
                continue;
            return false;
        }

        return true;
    }

    internal ChainedStateBasicFrame? ChainedStateBasicObserverRoot(long? requestedId = null)
    {
        for (var i = 0; i < _resolutionStackOperations._resolutionStack.Count; i++)
            if (_resolutionStackOperations._resolutionStack[i] is ChainedStateBasicFrame paid && paid.Stage == ChainedStateBasicStage.StateChildren && (requestedId is null || paid.Payment.ParentFrameId == requestedId || _resolutionStackOperations._resolutionStack.Skip(i + 1).Any(f => f.Id == requestedId)) && ChainedStateBasicPaidSuffix(i))
                return paid;
        return null;
    }

    internal bool HasChainedStateBasicObserver(long id) => ChainedStateBasicObserverRoot(id)is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1630)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 20)]
    internal bool HasChainedStateBasicDamageObserver(long id) => ChainedStateBasicObserverRoot(id)is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 20, group: 80)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 130)]
    internal bool IsChainedStateBasicProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ChainedStateBasicObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    internal PendingDecision? ChainedStateBasicInvariantDecision(PendingDecision? native) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ChainedStateBasicFrame paid ? global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(paid.OriginalDecision))) : native is { } prompt ? global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(prompt) : null;
    internal ResolutionFrame? ChainedStateBasicInvariantTop(ResolutionFrame? native) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ChainedStateBasicFrame paid ? _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.Payment.RequestFrameId) : native;
    internal void AssertChainedStateBasicFramesAndUses()
    {
        foreach (var paid in _resolutionStackOperations._resolutionStack.OfType<ChainedStateBasicFrame>())
        {
            var p = paid.Payment;
            var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == paid.Id);
            if (paid.Stage != ChainedStateBasicStage.StateChildren || p.PaymentFrameId != paid.Id || !_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p) || paid.OriginalDecision.PlayerSeat != p.Source.OwnerSeat || paid.OriginalDecision.PromptId != p.OriginalPromptId || paid.OriginalDecision.Revision != p.OriginalRevision || paid.TargetSeats.Count == 0 || paid.TargetSeats.Distinct().Count() != paid.TargetSeats.Count || paid.TargetSeats.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t)) || (p.Intent == ChainedStateBasicIntent.Play ? i != 0 && !_characterKeBiNengOlOperations.IsKeBiNengPingduanPaymentFrame(paid) && !(_characterKongRongMouOlOperations.IsMouKrLiwenPaymentFrame(paid) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTiePaymentFrame(paid)) && !_characterLuSuMouOperations.IsMouLuSuPaymentFrame(paid) : i <= 0 || !ChainedStateBasicFrameRidesOn(paid, _resolutionStackOperations._resolutionStack[i - 1])) || !ChainedStateBasicPaidSuffix(i) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChainedStateBasicIssuedEvent>().Any(e => e.Receipt.Payment.PaymentFrameId == p.PaymentFrameId))
                throw new InvalidOperationException("A chained-state payment lost its frozen original need, real cost or exact child subtree.");
        }

        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            if (use.ChainedStateBasicUse is not null && !IsIssuedChainedStateBasicUse(use))
                throw new InvalidOperationException("A chained-state Slash lost its unique paid native issuance.");
            if (use.ChainedStateBasicResponse is not null)
                RequireChainedStateBasicResponseAction(use);
            if (use.Action?.ConversionChain.Any(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.ChainedStateCost.HasValue == true) == true && use.ChainedStateBasicUse is null)
                throw new InvalidOperationException("A chained-state Use cannot disguise a deleted receipt as a legacy zero card.");
        }

        foreach (var window in _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Where(w => w.Action.Type == CardActionType.Response && w.Action.ConversionChain.Any(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.ChainedStateCost.HasValue == true)))
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { ChainedStateBasicResponse: not null } owner || !global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime.TieredRoundActionsStructurallyMatch(RequireChainedStateBasicResponseAction(owner), window.Action))
                throw new InvalidOperationException("A chained-state Dodge child lost its exact accepted response owner.");
    }
}
