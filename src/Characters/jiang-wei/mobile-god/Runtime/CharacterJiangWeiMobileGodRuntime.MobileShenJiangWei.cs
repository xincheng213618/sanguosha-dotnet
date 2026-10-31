#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMobileGodRuntime
{
    internal const string MobileJwAction = "mobile-jiang-wei";
    internal static CardConversionSource MobileJwSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IMobileJiangWeiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMobileJw(f, op);
    internal bool MobileJwHuitianUsed(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Any(e => e.OwnerSeat == owner && e.SkillId == "mobile:huitian" && e.BindingId == "higher-hp-extra-turn");
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40600, SkillProgramEffectOp.ResolveMobileJiangWeiHuitianDeath)]
    internal bool AdmitMobileJwDeath(ProgramTriggerLegalityContext c) => MobileJwHuitianUsed(c.Owner.Seat);
    internal SkillProgramStepOutcome BeginMobileJw(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.MobileJiangWei is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Mobile Jiang Wei requires his original unpaid zero-material instruction.");
        var dying = op == SkillProgramEffectOp.ResolveMobileJiangWeiShenpei ? _coreDyingCursorRuntimeOperations.ActiveDying : null;
        if (op == SkillProgramEffectOp.ResolveMobileJiangWeiShenpei && (dying is null || dying.VictimSeat != f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering } w || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } p || p.ResumeDyingFrameId != dying.Id))
            throw new InvalidOperationException("Shenpei requires the genuine entering-Dying window and its actual victim.");
        var stage = op switch
        {
            SkillProgramEffectOp.ResolveMobileJiangWeiXinghun => MobileJiangWeiStage.Exchange,
            SkillProgramEffectOp.ResolveMobileJiangWeiTiantao => MobileJiangWeiStage.Region,
            SkillProgramEffectOp.ResolveMobileJiangWeiShenpei => MobileJiangWeiStage.RecoveryChildren,
            _ => MobileJiangWeiStage.DeathIssued
        };
        var count = dying is null ? 0 : _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Where(e => e.VictimSeat == f.OwnerSeat && e.ResolutionId <= dying.Id).Select(e => e.ResolutionId).Distinct().Count();
        if (stage == MobileJiangWeiStage.Exchange)
            _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        var r = new MobileJiangWeiReceipt(MobileJwSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0)
        {
            Stage = stage,
            DyingId = dying?.Id ?? 0,
            Amount = count,
            Top = stage == MobileJiangWeiStage.Exchange ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(5).Reverse().Select(c => c.Id).ToArray() : [],
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiangWeiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.DyingId, count));
        if (stage == MobileJiangWeiStage.RecoveryChildren)
        {
            if (count < 1)
                throw new InvalidOperationException("Shenpei lost its actual including-current dying-entry count.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, count, null, null);
            RecordMobileJwPayment(f.Id);
            ContinueMobileJw(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (stage == MobileJiangWeiStage.DeathIssued)
        {
            if (!MobileJwHuitianUsed(f.OwnerSeat))
                throw new InvalidOperationException("Huitian death requires a previously accepted real extra-turn binding.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillCauseDeathDeclaredEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.OwnerSeat));
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], null, null, null, causingProgramSkillFrameId: f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishMobileJw(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<Card> MobileJwDiscardable(ProgramSkillFrame f, int seat, CardZoneKind zone) => _cardZonesOperations._cardZones.CardsAt(new(zone, seat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, new(zone, seat), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, c, new(zone, seat), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<PromptChoice> MobileJwChoices(ProgramSkillFrame f)
    {
        var r = f.MobileJiangWei!;
        var choices = new List<PromptChoice>();
        PromptChoice C(string branch, string label, int value = 0, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"mobile-jw.{(f.Id)}.{(r.Stage)}.{(branch)}.{(value)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = MobileJwAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["value"] = value.ToString(CultureInfo.InvariantCulture) });
        switch (r.Stage)
        {
            case MobileJiangWeiStage.Exchange:
                foreach (var c in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.Exchanged.Contains(c.Id)))
                    choices.Add(C("hand", "交换手牌：" + c.DisplayName, c.Id, [c.Id]));
                choices.Add(C("exchange-done", "完成交换，排列牌堆顶"));
                break;
            case MobileJiangWeiStage.ExchangeTop:
                foreach (var id in r.Top.Where(id => !r.Exchanged.Contains(id) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile))
                    choices.Add(C("top", "换取顶牌：" + _coreCardAttackStateOperations.GetAttackCard(id).DisplayName, id, [id]));
                choices.Add(C("exchange-cancel", "重新选择手牌"));
                break;
            case MobileJiangWeiStage.Ordering:
                foreach (var id in r.Top.Where(id => !r.Ordered.Contains(id) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile))
                    choices.Add(C("order", "下一张顶牌：" + _coreCardAttackStateOperations.GetAttackCard(id).DisplayName, id, [id]));
                break;
            case MobileJiangWeiStage.Target:
            case MobileJiangWeiStage.ThunderTarget:
                foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (r.Stage == MobileJiangWeiStage.ThunderTarget || p.Seat != f.OwnerSeat)))
                    choices.Add(C("target", "选择" + p.Name, p.Seat, targets: [p.Seat]));
                break;
            case MobileJiangWeiStage.Showing:
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
                for (var i = 0; i < hand.Count; i++)
                    if (!r.Shown.Any(s => s.Id == hand[i].Id))
                        choices.Add(C("show-hand", "展示一张暗手牌", i));
                if (_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Any())
                    choices.Add(C("show-top", "展示牌堆顶一张牌"));
                break;
            case MobileJiangWeiStage.Region:
                foreach (var z in new[]
                {
                    CardZoneKind.Hand,
                    CardZoneKind.Equipment,
                    CardZoneKind.Judgment
                }

                )
                    choices.Add(C("region", z == CardZoneKind.Hand ? "弃置手牌区全部牌" : z == CardZoneKind.Equipment ? "弃置装备区全部牌" : "弃置判定区全部牌", (int)z));
                break;
            case MobileJiangWeiStage.OtherTargets:
                foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && MobileJwDiscardable(f, p.Seat, r.Region).Count > 0))
                    choices.Add(C(r.Targets.Contains(p.Seat) ? "unselect-target" : "discard-target", (r.Targets.Contains(p.Seat) ? "取消选择" : "选择弃置") + p.Name, p.Seat, targets: [p.Seat]));
                choices.Add(C("targets-done", "完成天涛弃牌"));
                break;
            case MobileJiangWeiStage.DiscardCard:
                foreach (var old in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Target, [r.Region], OwnedCardMoveIntent.Discard))
                    choices.Add(old with { Parameters = new Dictionary<string, string>(old.Parameters) { ["program-action"] = MobileJwAction, ["branch"] = "discard-card" } });
                break;
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishMobileJw(ProgramSkillFrame f)
    {
        var r = f.MobileJiangWei!;
        if (r.Stage == MobileJiangWeiStage.Ordering && r.Ordered.Count == r.Top.Count)
        {
            _cardZonesOperations._cardZones.ReorderDrawPileSubsetToTop(r.Top, r.Ordered);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.Target } });
            r = f.MobileJiangWei!;
        }

        if (r.Stage == MobileJiangWeiStage.Showing && (r.Shown.Count >= 5 || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count(c => !r.Shown.Any(s => s.Id == c.Id)) == 0 && _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) == 0))
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "xinghun-shown", r.Shown.Select(s => s.Id).ToArray(), SkillProgramCardSetVisibility.Public, r.Shown.Select(s => s.From).ToArray());
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.Slashes, Index = 0 } });
            ContinueMobileJw(f.Id);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.Stage == MobileJiangWeiStage.Showing && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
        {
            ReturnMobileJwShownTop(f);
            return;
        }

        var choices = MobileJwChoices(f);
        if (choices.Count == 0)
        {
            FinishMobileJw(f);
            return;
        }

        var chooser = r.Stage == MobileJiangWeiStage.Showing ? r.Target : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：选择下一步。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Target >= 0 ? r.Target : f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(MobileJwAction)]
    internal void AnswerMobileJw(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiangWei: { } r } f || !MobileJwChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Mobile Jiang Wei requires the current original chooser and exact private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        var value = int.TryParse(selected.Parameters.GetValueOrDefault("value"), CultureInfo.InvariantCulture, out var parsed) ? parsed : 0;
        if (branch is "hand" or "exchange-cancel" or "exchange-done" or "order" or "discard-target" or "unselect-target")
        {
            r = branch switch
            {
                "hand" => r with
                {
                    Stage = MobileJiangWeiStage.ExchangeTop,
                    ChosenHand = value
                },
                "exchange-cancel" => r with
                {
                    Stage = MobileJiangWeiStage.Exchange,
                    ChosenHand = 0
                },
                "exchange-done" => r with
                {
                    Stage = MobileJiangWeiStage.Ordering,
                    Ordered = []
                },
                "order" => r with
                {
                    Ordered = r.Ordered.Append(value).ToArray()
                },
                "discard-target" => r with
                {
                    Targets = r.Targets.Append(value).ToArray()
                },
                _ => r with
                {
                    Targets = r.Targets.Where(s => s != value).ToArray()
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r });
            PublishMobileJw(f);
            return;
        }

        if (branch == "top")
        {
            var hand = r.ChosenHand;
            var top = r.Top.Select(id => id == value ? hand : id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Top = top, Exchanged = r.Exchanged.Concat([hand, value]).ToArray(), ChosenHand = 0 } });
            MutateMobileJw(f, MobileJiangWeiStage.ExchangeChildren, MobileJiangWeiStage.Exchange, () =>
            {
                _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(value), CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new("mobile.xinghun.exchange"), disclosure: CardMovementDisclosure.FaceDown);
                _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(hand), CardLocation.Hand(f.OwnerSeat), CardLocation.DrawPile, new("mobile.xinghun.exchange"), disclosure: CardMovementDisclosure.FaceDown);
                _cardZonesOperations._cardZones.ReorderDrawPileSubsetToTop(top, top);
            });
            return;
        }

        if (branch == "target")
        {
            if (r.Stage == MobileJiangWeiStage.ThunderTarget)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.ThunderIssued, Target = value } });
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, value, r.Amount, nature: DamageNature.Thunder);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.Showing, Target = value } });
            PublishMobileJw(f);
            return;
        }

        if (branch is "show-hand" or "show-top")
        {
            var card = branch == "show-hand" ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])[value] : _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Last();
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Shown = r.Shown.Append(new(card.Id, card.Kind, from, from == CardLocation.DrawPile ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence + 1 : 0)).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
            if (from == CardLocation.DrawPile)
                MutateMobileJw(f, MobileJiangWeiStage.ReserveChildren, MobileJiangWeiStage.Showing, () => _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Processing, new("mobile.xinghun.show")));
            else
                PublishMobileJw(f);
            return;
        }

        if (branch == "region")
        {
            var zone = (CardZoneKind)value;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Region = zone, Target = f.OwnerSeat } });
            DiscardMobileJw(f, MobileJwDiscardable(f, f.OwnerSeat, zone).Select(c => c.Id).ToArray());
            return;
        }

        if (branch == "discard-card")
        {
            if (!int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), CultureInfo.InvariantCulture, out var slot))
                throw new InvalidOperationException("Tiantao lost its opaque actual source slot.");
            var card = _cardZonesOperations._cardZones.CardsAt(new(r.Region, r.Target))[slot];
            if (!MobileJwDiscardable(f, r.Target, r.Region).Contains(card))
                throw new InvalidOperationException("Tiantao cannot discard this original entity.");
            DiscardMobileJw(f, [card.Id]);
            return;
        }

        if (branch == "targets-done")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.OtherDiscard, Index = 0, Targets = r.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, s)).ToArray() } });
            ContinueMobileJw(f.Id);
            return;
        }

        throw new InvalidOperationException("Unknown mobile Jiang Wei branch.");
    }

    internal void DiscardMobileJw(ProgramSkillFrame f, IReadOnlyList<int> ids)
    {
        var r = f.MobileJiangWei!;
        MutateMobileJw(f, MobileJiangWeiStage.DiscardChildren, r.Target == f.OwnerSeat ? MobileJiangWeiStage.OtherTargets : MobileJiangWeiStage.OtherDiscard, () =>
        {
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new("mobile.tiantao.discard"));
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var current = parent.MobileJiangWei!;
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id && e.Reason.Value == "mobile.tiantao.discard" && e.SequenceBefore == current.Before);
            if (paid is { ActualCount: > 0 } && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == paid.InvocationId).All(e => !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence == e.CompletionSequence).CardKind)))
                _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MobileJiangWei = current with { HpLossSeats = current.HpLossSeats.Append(r.Target).ToArray() } });
        });
    }

    internal void MutateMobileJw(ProgramSkillFrame f, MobileJiangWeiStage stage, MobileJiangWeiStage next, Action mutation)
    {
        var r = f.MobileJiangWei!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = stage, NextStage = next, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = [], PaymentIndex = r.PaymentIndex + 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        mutation();
        RecordMobileJwPayment(f.Id);
        ContinueMobileJw(f.Id);
    }

    internal void RecordMobileJwPayment(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MobileJiangWei!;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileJiangWei = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = moves } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiangWeiPaidEvent(f.Id, r.PaymentIndex, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, moves.Length));
    }

    internal bool DrainMobileJw(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueMobileJw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiangWei: { } r } f || f.Id != id)
            return;
        if (r.Stage == MobileJiangWeiStage.DeathIssued)
        {
            FinishMobileJw(f);
            return;
        }

        if (r.Stage == MobileJiangWeiStage.SlashIssued)
            return;
        if (r.Stage == MobileJiangWeiStage.ThunderIssued)
        {
            if (f.AttackAttempt is not null)
                return;
            MutateMobileJw(f, MobileJiangWeiStage.GrantChildren, MobileJiangWeiStage.Finished, () => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().GrantSkills(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ["mobile:huitian"]));
            return;
        }

        if (f.PendingMovementContinuation is not null)
        {
            if (DrainMobileJw(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MobileJiangWei!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            var next = r.Stage switch
            {
                MobileJiangWeiStage.RecoveryChildren => MobileJiangWeiStage.ThunderTarget,
                MobileJiangWeiStage.GrantChildren or MobileJiangWeiStage.ReturnChildren => MobileJiangWeiStage.Finished,
                _ => r.NextStage
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = next } });
            r = f.MobileJiangWei!;
        }

        if (r.Stage == MobileJiangWeiStage.Finished)
        {
            FinishMobileJw(f);
            return;
        }

        if (r.Stage == MobileJiangWeiStage.OtherDiscard)
        {
            if (r.Index >= r.Targets.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.HpLoss, Index = 0 } });
                ContinueMobileJw(f.Id);
                return;
            }

            var seat = r.Targets[r.Index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Index = r.Index + 1, Target = seat } });
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || MobileJwDiscardable(f, seat, r.Region).Count == 0)
            {
                ContinueMobileJw(f.Id);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = f.MobileJiangWei with { Stage = MobileJiangWeiStage.DiscardCard } });
            PublishMobileJw(f);
            return;
        }

        if (r.Stage == MobileJiangWeiStage.HpLossIssued)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.HpLoss } });
            r = f.MobileJiangWei!;
        }

        if (r.Stage == MobileJiangWeiStage.HpLoss)
        {
            if (r.Index >= r.HpLossSeats.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMobileJw(f);
                return;
            }

            var seat = r.HpLossSeats[r.Index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.HpLossIssued, Index = r.Index + 1 } });
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, seat, 1) == SkillProgramStepOutcome.AwaitChild)
                return;
            ContinueMobileJw(f.Id);
            return;
        }

        if (r.Stage == MobileJiangWeiStage.SlashReturned)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = r with { Stage = MobileJiangWeiStage.Slashes, Use = null } });
            r = f.MobileJiangWei!;
        }

        if (r.Stage == MobileJiangWeiStage.Slashes)
        {
            NextMobileJwSlash(f);
            return;
        }

        PublishMobileJw(f);
    }

    internal void ReturnMobileJwShownTop(ProgramSkillFrame f)
    {
        var r = f.MobileJiangWei!;
        var ids = r.Shown.Where(s => s.From == CardLocation.DrawPile && _cardZonesOperations._cardZones.GetLocation(s.Id) == CardLocation.Processing).Select(s => s.Id).ToArray();
        MutateMobileJw(f, MobileJiangWeiStage.ReturnChildren, MobileJiangWeiStage.Finished, () =>
        {
            _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(_coreCardAttackStateOperations.GetAttackCard).ToArray(), CardLocation.Processing, CardLocation.DrawPile, new("mobile.xinghun.return"));
            if (ids.Length > 0)
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(ids);
        });
    }

    internal void FinishMobileJw(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileJiangWei = f.MobileJiangWei!with { Stage = MobileJiangWeiStage.Finished }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileJiangWeiCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40600)]
    internal bool ResumeMobileJw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiangWei: not null } f || f.Id != id)
            return false;
        ContinueMobileJw(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40600)]
    internal bool ReturnMobileJwMovement(ProgramSkillFrame f) => f.MobileJiangWei is not null && ResumeMobileJw(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40600)]
    internal bool KeepPaidMobileJw(ProgramSkillFrame f) => f.MobileJiangWei is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileJiangWeiXinghun, SkillProgramEffectOp.ResolveMobileJiangWeiTiantao, SkillProgramEffectOp.ResolveMobileJiangWeiShenpei)]
    internal PromptChoice ChooseMobileJw(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MobileJiangWei!;
        if (r.Stage == MobileJiangWeiStage.Exchange)
            return p.Choices.First(c => c.Parameters["branch"] == "exchange-done");
        if (r.Stage == MobileJiangWeiStage.Region)
            return p.Choices.OrderBy(c => _cardZonesOperations._cardZones.Count(new((CardZoneKind)int.Parse(c.Parameters["value"], CultureInfo.InvariantCulture), f.OwnerSeat))).First();
        if (r.Stage == MobileJiangWeiStage.OtherTargets)
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard-target" && c.Targets.Count == 1 && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? p.Choices.Last();
        if (r.Stage is MobileJiangWeiStage.Target or MobileJiangWeiStage.ThunderTarget)
            return p.Choices.FirstOrDefault(c => !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])) ?? p.Choices.First();
        return p.Choices.First();
    }
}
