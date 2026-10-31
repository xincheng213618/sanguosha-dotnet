#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangTangOlRuntime
{
    internal const string ZexingTakeReason = "skill-program.tang-tang.zexing.take", ZexingGiftReason = "skill-program.tang-tang.zexing.gift", ZhiyiDrawReason = "skill-program.tang-tang.zhiyi.draw";
    internal static CardConversionSource TangTangSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_tang_tang_ol_TangTang.TangTangSource(f);
    internal IReadOnlyList<Card> TangTangTransferCards(int chooser, int holder) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(holder)).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer)).ToArray();
    internal TangTangEntity TangTangEntityAt(int holder, int id)
    {
        var c = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        return new(id, c.Kind, _cardZonesOperations._cardZones.GetLocation(id), _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[holder], c));
    }

    SkillProgramStepOutcome global::CardGame.Core.ITangTangHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginTangTang(f, op);
    internal SkillProgramStepOutcome BeginTangTang(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TangTang is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !TangTangComposition.Handles(op))
            throw new InvalidOperationException("Tang Tang requires her exact new owning instruction.");
        var r = new TangTangReceipt(TangTangSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, f.WindowContext?.ParentFrameId ?? 0, op == SkillProgramEffectOp.ResolveZexingShownCardChain ? TangTangStage.ShownCard : TangTangStage.BenefitTarget);
        if (op == SkillProgramEffectOp.ResolveZexingShownCardChain)
        {
            if (f.TriggerId is not null || f.ActivationId != "shown-card-chain" || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
                throw new InvalidOperationException("Zexing lost the real current Play phase.");
            r = r with
            {
                PlayPhase = phase
            };
        }
        else
        {
            if (f.TriggerId is null || f.WindowContext is not { } context || CurrentTangTangGains(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context)is not { Count: > 0 } gains || !ZhiyiAvailable(f.OwnerSeat))
                throw new InvalidOperationException("Zhiyi lost its exact frozen gain batch or shared actual-turn opportunity.");
            r = r with
            {
                Paid = true,
                Gains = gains
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.TurnOwner, r.WindowId, r.PlayPhase?.Token ?? 0));
        if (op == SkillProgramEffectOp.ResolveZhiyiGainColorBenefit)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhiyiOpportunityPaidEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.TurnOwner, r.WindowId));
        PublishTangTang(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int TangTangChooser(ProgramSkillFrame f) => f.TangTang is { Stage: TangTangStage.PeerOption, Peer: >= 0 } r ? r.Peer : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> TangTangChoices(ProgramSkillFrame f)
    {
        var r = f.TangTang ?? throw new InvalidOperationException("No owning Tang Tang instruction.");
        PromptChoice Choice(string branch, string label, int? card = null, int? target = null) => new(new($"tang-tang.{(f.Id)}.{(r.Step)}.{(r.Stage)}.{(branch)}.{(card ?? target ?? -1)}"), label, card is { } id ? [id] : [], target is { } peer ? [peer] : [], new Dictionary<string, string> { ["program-action"] = "tang-tang", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        if (r.Stage == TangTangStage.ShownCard)
            return Array.AsReadOnly(TangTangTransferCards(f.OwnerSeat, f.OwnerSeat).Select(c => Choice("show", $"展示【{(c.DisplayName)}】", c.Id)).ToArray());
        if (r.Stage is TangTangStage.FirstPeer or TangTangStage.RepeatPeer)
        {
            var choices = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !ZexingVisited(f.OwnerSeat, p.Seat)).Select(p => Choice("peer", $"对{(p.Name)}发动择行", target: p.Seat)).ToList();
            if (r.Stage == TangTangStage.RepeatPeer)
                choices.Add(Choice("decline", "结束择行"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == TangTangStage.PeerOption)
        {
            var choices = new List<PromptChoice>();
            if (ZexingTakeAllowed(f))
                choices.Add(Choice("take", "令其观看你的手牌并获得两张异花色牌，然后你获得展示牌"));
            if (_coreMatchParticipantsPlayersAccess._players[r.Peer].IsAlive)
                choices.Add(Choice("lose-hp", "失去1点体力，令其可继续对其他角色发动择行"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == TangTangStage.TakeCards)
        {
            var legal = ZexingTakeCandidates(f);
            var choices = r.Candidates.Where(c => legal.Contains(c) && !r.Selected.Contains(c.CardId) && r.Selected.Count < 2).Select(c => Choice("pick", $"获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId).DisplayName)}】（{(c.Suit)}）", c.CardId)).ToList();
            choices.AddRange(r.Selected.Select(id => Choice("unpick", $"撤回【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", id)));
            if (r.Selected.Count == 2)
                choices.Add(Choice("take-pay", "获得已选两张牌"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == TangTangStage.BenefitTarget)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => Choice("benefit", $"令{(p.Name)}摸一张牌", target: p.Seat)).ToArray());
        return[];
    }

    internal void PublishTangTang(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishTangTang(f);
            return;
        }

        var choices = TangTangChoices(f);
        if (choices.Count == 0)
        {
            FinishTangTang(f);
            return;
        }

        var r = f.TangTang!;
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, TangTangChooser(f), choices, r.Stage switch
        {
            TangTangStage.ShownCard => "择行：选择一张真实手牌或装备牌展示。",
            TangTangStage.PeerOption => "择行：请选择一项。",
            TangTangStage.TakeCards => "择行：观看其手牌并选择两张异花色牌。",
            TangTangStage.BenefitTarget => "知意：令一名角色摸一张牌。",
            _ => "择行：选择一名本回合未指定过的其他角色。"
        }, r.Peer >= 0 ? r.Peer : f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("tang-tang")]
    internal void AnswerTangTang(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TangTang: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != TangTangChooser(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], TangTangChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Tang Tang lost her exact private chooser, original card or live option.");
        AssertTangTang(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            FinishTangTang(f);
            return;
        }

        if (r.Stage == TangTangStage.ShownCard)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Shown = TangTangEntityAt(f.OwnerSeat, choice.Cards.Single()), Stage = TangTangStage.FirstPeer } });
            PublishTangTang(f);
            return;
        }

        if (r.Stage is TangTangStage.FirstPeer or TangTangStage.RepeatPeer)
        {
            var peer = choice.Targets.Single();
            if (!r.Paid)
            {
                if (!ZexingShownStillOwned(f) || r.Shown is not { } shown)
                {
                    FinishTangTang(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Shown = TangTangEntityAt(f.OwnerSeat, shown.CardId), Paid = true, Peer = peer, Stage = TangTangStage.ShowChildren } });
                var frozen = f.TangTang!.Shown!;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZexingShownEvent(f.Id, f.OwnerSeat, frozen.CardId, frozen.Kind, frozen.From, frozen.Suit));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, "zexing-shown", [_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(frozen.CardId))]));
            }
            else
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Peer = peer, Stage = TangTangStage.ShowChildren, Selected = [], Candidates = [], Viewed = [] } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZexingPeerChosenEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.TurnOwner, peer, _coreProgramEventHistoryOperations.ProgramEventHistory<ZexingPeerChosenEvent>().Count(e => e.FrameId == f.Id)));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "zexing-shown-card"))
                return;
            ContinueTangTang(f.Id);
            return;
        }

        if (r.Stage == TangTangStage.PeerOption)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZexingOptionChosenEvent(f.Id, r.Peer, branch == "take" ? 1 : 2));
            if (branch == "take")
            {
                var viewed = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Peer]).Select(c => c.Id).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Stage = TangTangStage.TakeCards, Viewed = viewed, Candidates = ZexingTakeCandidates(f), Selected = [] } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZexingHandViewedEvent(f.Id, f.OwnerSeat, r.Peer, viewed.Length));
                PublishTangTang(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Stage = TangTangStage.HpIssuing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.Peer, 0, null) });
            ContinueTangTang(f.Id);
            return;
        }

        if (r.Stage == TangTangStage.TakeCards)
        {
            if (branch != "take-pay")
            {
                var id = choice.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Selected = branch == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(c => c != id).ToArray() } });
                PublishTangTang(f);
                return;
            }

            if (r.Selected.Count != 2 || r.Selected.Any(id => !ZexingTakeCandidates(f).Any(c => c.CardId == id)))
                throw new InvalidOperationException("Zexing cannot obtain a partial or replaced two-card set.");
            IssueTangTangMovement(f, r.Selected, CardLocation.Hand(f.OwnerSeat), TangTangStage.TakeChildren, ZexingTakeReason);
            ContinueTangTang(f.Id);
            return;
        }

        if (r.Stage == TangTangStage.BenefitTarget)
        {
            var recipient = choice.Targets.Single();
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Recipient = recipient, Stage = TangTangStage.DrawChildren, Step = r.Step + 1, Before = before, After = before }, PendingMovementContinuation = new(recipient, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhiyiRecipientChosenEvent(f.Id, recipient));
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, recipient, 1, null, null, SkillProgramCardSetVisibility.Private, new(ZhiyiDrawReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = f.TangTang!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.To == CardLocation.Hand(recipient) && m.From == CardLocation.DrawPile && m.Reason.Value == ZhiyiDrawReason) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangMovementIssuedEvent(f.Id, f.TangTang!.Step, TangTangStage.DrawChildren, f.TangTang.Before, f.TangTang.After, f.TangTang.ActualCount));
            ContinueTangTang(f.Id);
            return;
        }

        throw new InvalidOperationException("Tang Tang cannot answer outside her actual choice.");
    }

    internal void IssueTangTangMovement(ProgramSkillFrame supplied, IReadOnlyList<int> ids, CardLocation destination, TangTangStage stage, string reason)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.TangTang!;
        var materials = ids.Select(id => TangTangEntityAt(_cardZonesOperations._cardZones.GetLocation(id).OwnerSeat!.Value, id)).ToArray();
        var grain = materials.Where(c => c.Kind == CardKind.WoodenOx && c.From.Zone == CardZoneKind.Equipment).SelectMany(c => _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[c.From.OwnerSeat!.Value])).Select(c => c.Id).ToArray();
        r = r with
        {
            Stage = stage,
            Step = r.Step + 1,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Materials = materials,
            Grain = grain,
            ActualCount = ids.Count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r, PendingMovementContinuation = new(stage == TangTangStage.TakeChildren ? r.Peer : f.OwnerSeat, 0, null) });
        for (var i = 0; i < materials.Length; i++)
        {
            var c = materials[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangMovementMaterialEvent(f.Id, r.Step, i, c.CardId, c.Kind, c.From, c.Suit));
        }

        for (var i = 0; i < grain.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangGrainEvent(f.Id, r.Step, i, grain[i]));
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, new(reason), (batch, records) =>
        {
            foreach (var c in materials)
            {
                var m = records.Single(m => m.CardId == c.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangEntityMovedEvent(f.Id, r.Step, stage, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
            }
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = f.TangTang!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangMovementIssuedEvent(f.Id, r.Step, stage, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ids.Count));
    }

    internal bool DrainTangTang(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.TangTang?.Stage == TangTangStage.HpChildren ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueTangTang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TangTang: { } r } f || f.Id != id)
            return;
        AssertTangTang(f);
        if (r.Stage == TangTangStage.HpIssuing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.Peer].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                FinishTangTang(f);
                return;
            }

            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(id, f.SkillId, r.Peer, 1);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.TangTang!;
            if (r.Stage == TangTangStage.HpIssuing)
                return;
        }

        if (r.Stage is TangTangStage.ShownCard or TangTangStage.FirstPeer or TangTangStage.RepeatPeer or TangTangStage.PeerOption or TangTangStage.TakeCards or TangTangStage.BenefitTarget)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishTangTang(f);
            return;
        }

        if (DrainTangTang(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.TangTang!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishTangTang(f);
            return;
        }

        if (r.Stage == TangTangStage.ShowChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.Peer].IsAlive)
            {
                FinishTangTang(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Stage = TangTangStage.PeerOption } });
            PublishTangTang(f);
            return;
        }

        if (r.Stage == TangTangStage.TakeChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.Peer].IsAlive && ZexingShownStillOwned(f))
            {
                IssueTangTangMovement(f, [r.Shown!.CardId], CardLocation.Hand(r.Peer), TangTangStage.GiftChildren, ZexingGiftReason);
                ContinueTangTang(id);
            }
            else
                FinishTangTang(f);
            return;
        }

        if (r.Stage == TangTangStage.HpChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = r with { Stage = TangTangStage.RepeatPeer, Selected = [], Viewed = [], Candidates = [] } });
            PublishTangTang(f);
            return;
        }

        if (r.Stage is TangTangStage.GiftChildren or TangTangStage.DrawChildren)
        {
            FinishTangTang(f);
            return;
        }

        throw new InvalidOperationException("Tang Tang returned to an unissued native stage.");
    }

    internal void FinishTangTang(ProgramSkillFrame f)
    {
        var applied = f.TangTang?.Paid == true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TangTangCompletedEvent(f.Id, applied));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TangTang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, applied);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27530)]
    internal bool ResumeTangTang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TangTang: not null } f || f.Id != id)
            return false;
        ContinueTangTang(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27530)]
    internal bool ReturnTangTangMovement(ProgramSkillFrame f)
    {
        if (f.TangTang is null)
            return false;
        ContinueTangTang(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27530)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27530)]
    internal bool ContinuePaidTangTang(ProgramSkillFrame f)
    {
        if (f.TangTang is not { Paid: true })
            return false;
        AssertTangTang(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveZexingShownCardChain, SkillProgramEffectOp.ResolveZhiyiGainColorBenefit)]
    internal PromptChoice TangTangAi(PendingDecision p, ProgramSkillFrame f) => f.TangTang?.Stage == TangTangStage.PeerOption ? p.Choices.First() : p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch")is not ("decline" or "unpick")) ?? p.Choices.Last();
    [PublicHandRevealReturn("zexing-shown-card")]
    internal void ReturnZexingPublicHand(ProgramSkillFrame f)
    {
        if (f.TangTang is not { Paid: true, Stage: TangTangStage.ShowChildren })
            throw new InvalidOperationException("Zexing lost its accepted displayed material and peer.");
        ContinueTangTang(f.Id);
    }
}
