#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuRuntime
{
    internal const string SourceHuanfuDiscard = "skill-program.source-huanfu-3156.discard";
    internal const string SourceHuanfuDraw = "skill-program.source-huanfu-3156.draw";
    internal static CardConversionSource SourceHuanfuSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal CardUseFrame? SourceHuanfuUse(ProgramSkillWindowContext context) => context.CardUse is { } card ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == card.ParentCardUseFrameId && u.Action is { Type: CardActionType.Use } a && a.ActionId == card.CardActionId && a.ActorSeat == card.ActorSeat && u.SourceSeat == card.ActorSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) && u.CardKind == card.EffectiveKind) : null;
    internal IReadOnlyList<PromptChoice> SourceHuanfuCosts(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1).ToArray();
    internal bool SourceHuanfuArmQualifies(int owner, string binding, ProgramSkillWindowContext context) => _coreSourceExecutionerYishiOperations.SourceExecutionerMode && context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].MaxHp > 0 && SourceHuanfuUse(context)is { Action: { } a } use && use.SourceExecutionerHuanfuBets?.Any(b => b.Source.OwnerSeat == owner) != true && (binding == "actor-wager" ? a.ActorSeat == owner : binding == "target-wager" && a.ActorSeat != owner && a.TargetSeats.Contains(owner)) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Any(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    internal CardUseFrame ExactSourceHuanfuParent(ProgramSkillFrame f)
    {
        if (f.SkillId != SourceExecutionerContract.Huanfu || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || SourceHuanfuUse(context)is not { Action: { } action } use || !_coreSourceExecutionerYishiOperations.SourceExecutionerMode)
            throw new InvalidOperationException("Source3156 Huanfu lost its original real Slash Use.");
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 2 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId || window.ParentFrameId != use.Id || _resolutionStackOperations._resolutionStack[i - 2].Id != use.Id || window.Action.ActionId != action.ActionId || window.Action.Type != CardActionType.Use || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || _coreCardActionsOperations.CreateCardActionProgramContext2(window, window.Candidates[window.CandidateIndex]) != context)
            throw new InvalidOperationException("Source3156 Huanfu lost its original card window and exact instance candidate.");
        return use;
    }

    internal SkillProgramStepOutcome BeginSourceExecutionerHuanfu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = ExactSourceHuanfuParent(f);
        if (f.SourceExecutionerHuanfu is not null)
            throw new InvalidOperationException("Huanfu cannot begin its native payment twice.");
        if (f.TriggerId == "settle-wager")
            return SettleSourceHuanfu(f, use);
        if (!SourceHuanfuArmQualifies(f.OwnerSeat, f.TriggerId!, f.WindowContext!) || SourceHuanfuCosts(f).Count == 0)
            throw new InvalidOperationException("Huanfu requires the actor or original target's actual payable HE wager.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = new(use.Id, use.Action!.ActionId, SourceExecutionerHuanfuStage.Cards, f.TriggerId == "actor-wager", _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp) });
        ContinueSourceHuanfu(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SourceHuanfuChoices(ProgramSkillFrame f)
    {
        var r = f.SourceExecutionerHuanfu!;
        var choices = new List<PromptChoice>();
        void Add(string key, string label, string branch, IReadOnlyList<int> cards) => choices.Add(new(new($"source-huanfu.{(f.Id)}.{(key)}"), label, cards, [], new Dictionary<string, string> { ["program-action"] = "source-huanfu-wager", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        foreach (var id in r.Cards)
            Add("unpick." + id, "取消已选牌", "unpick", [id]);
        var limit = Math.Min(r.Limit, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp);
        if (r.Cards.Count < limit)
            foreach (var c in SourceHuanfuCosts(f).Where(c => !r.Cards.Contains(c.Cards.Single())))
                Add("card." + c.Cards.Single(), c.Label, "card", c.Cards);
        if (r.Cards.Count > 0 && r.Cards.Count <= limit)
            Add("pay", $"弃置{(r.Cards.Count)}张牌下注", "pay", []);
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    [ProgramAnswer("source-huanfu-wager")]
    internal void AnswerSourceHuanfu(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Huanfu lost its own selection frame.");
        var r = f.SourceExecutionerHuanfu ?? throw new InvalidOperationException("Huanfu lost its wager receipt.");
        if (r.Stage != SourceExecutionerHuanfuStage.Cards || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], SourceHuanfuChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Huanfu's real HE wager or maximum HP changed.");
        ExactSourceHuanfuParent(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "card":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } });
                break;
            case "unpick":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = r with { Cards = r.Cards.Where(c => c != selected.Cards.Single()).ToArray() } });
                break;
            case "pay":
                var available = SourceHuanfuCosts(f).SelectMany(c => c.Cards).ToHashSet();
                var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
                if (r.Cards.Count < 1 || r.Cards.Count > Math.Min(r.Limit, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp) || r.Cards.Any(c => !available.Contains(c)))
                    throw new InvalidOperationException("Huanfu must discard one to current maximum HP actual HE entities.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceExecutionerHuanfu = r with { Stage = SourceExecutionerHuanfuStage.CostChildren, Limit = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, SequenceBefore = before, SequenceAfter = before } });
                var invocation = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, r.Cards, new(SourceHuanfuDiscard), f.Id);
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.DiscardPile, new(SourceHuanfuDiscard), (_, moves) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var paid = current.SourceExecutionerHuanfu!with
                    {
                        SequenceAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
                        ActualCount = moves.Count(m => m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame)
                    };
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { SourceExecutionerHuanfu = paid });
                    var use = ExactSourceHuanfuParent(current);
                    var bet = new SourceExecutionerHuanfuBet(use.Id, use.Action!.ActionId, SourceHuanfuSource(current), current.GameplayHash, current.Id, use.SourceSeat, paid.ActorWager, paid.Limit, paid.ActualCount, before, paid.SequenceAfter)
                    {
                        Cards = paid.Cards,
                        Targets = use.Action.TargetSeats
                    };
                    _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { SourceExecutionerHuanfuBets = (use.SourceExecutionerHuanfuBets ?? []).Append(bet).ToArray() });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceExecutionerHuanfuPaidEvent(current.Id, use.Id, bet.ActionId, bet.Source, bet.GameplayHash, bet.Amount, bet.MaxHpAtPayment, before, bet.SequenceAfter));
                });
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(invocation);
                break;
            default:
                throw new InvalidOperationException("Unknown Huanfu wager selection.");
        }

        ContinueSourceHuanfu(f.Id);
    }

    internal void FinishSourceHuanfu(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceExecutionerHuanfu = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinueSourceHuanfu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SourceExecutionerHuanfu: { } r } f || f.Id != id || f.NativeDraw is not null)
            return;
        ExactSourceHuanfuParent(f);
        if (r.Stage == SourceExecutionerHuanfuStage.Cards)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishSourceHuanfu(f);
                return;
            }

            var choices = SourceHuanfuChoices(f);
            if (choices.Count == 0)
            {
                FinishSourceHuanfu(f);
                return;
            }

            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"宦浮：弃置至多{(Math.Min(r.Limit, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp))}张手牌或装备牌下注。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (r.Stage == SourceExecutionerHuanfuStage.CostChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                return;
            FinishSourceHuanfu(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        }
    }

    internal SkillProgramStepOutcome SettleSourceHuanfu(ProgramSkillFrame f, CardUseFrame use)
    {
        var bet = _coreSourceExecutionerHuanfuObserversOperations.SourceHuanfuSettlement(f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, use) ?? throw new InvalidOperationException("Huanfu cannot settle without its original paid wager.");
        if (bet.Settled)
            throw new InvalidOperationException("Huanfu cannot settle the same wager twice.");
        var requested = bet.Amount > 0 && bet.DamageTotal == bet.Amount && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? checked(bet.Amount * 2) : 0;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { SourceExecutionerHuanfuBets = use.SourceExecutionerHuanfuBets!.Select(b => b == bet ? b with { Settled = true } : b).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceExecutionerHuanfuSettledEvent(f.Id, bet.ProgramFrameId, use.Id, f.OwnerSeat, bet.Amount, bet.DamageTotal, requested));
        if (requested == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceExecutionerHuanfu = new(use.Id, use.Action!.ActionId, SourceExecutionerHuanfuStage.Drawing, bet.ActorWager, bet.MaxHpAtPayment, bet.SequenceBefore, bet.SequenceAfter, bet.Amount, bet.ProgramFrameId) });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, requested, new(SourceHuanfuDraw));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 62060)]
    internal bool ResumeSourceHuanfu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SourceExecutionerHuanfu: not null } f || f.Id != id)
            return false;
        ContinueSourceHuanfu(id);
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveSourceExecutionerHuanfu)]
    internal void ReturnSourceHuanfuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.SourceExecutionerHuanfu ?? throw new InvalidOperationException("Huanfu's native draw lost its paid wager.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceExecutionerHuanfuDrawReturnedEvent(f.Id, r.UseId, outcome.RequestedCount, outcome.ActualDrawIds.Count));
        FinishSourceHuanfu(f);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveSourceExecutionerHuanfu)]
    internal PromptChoice ChooseSourceHuanfuAi(PendingDecision d, ProgramSkillFrame f) => d.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? d.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "card");
}
