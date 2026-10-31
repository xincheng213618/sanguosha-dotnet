#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUnequalHandHpEndingRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ITargetedTrickAndHandHpHost.ResolveUnequalHandHpEnding(ProgramSkillFrame frame) => BeginUnequalHandHpEnding(frame);
    SkillProgramStepOutcome global::CardGame.Core.ITargetedTrickAndHandHpHost.AdjustTargetedOrdinaryTrick(ProgramSkillFrame frame) => _coreTargetedOrdinaryTrickOperations.BeginTargetedOrdinaryTrick(frame);
    internal CardConversionSource TargetedTrickHandHpSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int[] UnequalHandHpTargets(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count != p.Hp).Select(p => p.Seat).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 100)]
    internal bool CanOfferUnequalHandHp(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int owner) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveUnequalHandHpEnding) || context.Window == SkillProgramTriggerWindow.PlayEnding && owner == _coreActualTurnProgressCurrentSeatAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && UnequalHandHpTargets(owner).Length > 0;
    internal static string UnequalHandHpReason(ProgramSkillFrame f, string step) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_UnequalHandHpEnding.UnequalHandHpReason(f, step);
    internal SkillProgramStepOutcome BeginUnequalHandHpEnding(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreVirtualOrdinaryTrickUsesOperations.IsExactOwnPlayEndingProgram(f) || f.InstructionIndex != 1 || f.UnequalHandHp is not null)
            throw new InvalidOperationException("An unequal hand/HP choice requires its exact own Play ending.");
        var r = new UnequalHandHpReceipt
        {
            WindowFrameId = f.WindowContext!.ParentFrameId,
            Source = TargetedTrickHandHpSource(f),
            GameplayHash = f.GameplayHash,
            InstructionIndex = f.InstructionIndex,
            Stage = UnequalHandHpStage.Target,
            Targets = UnequalHandHpTargets(f.OwnerSeat)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnequalHandHpIssuedEvent(f.Id, r.WindowFrameId, r.Source, r.GameplayHash, r.InstructionIndex));
        PublishUnequalHandHpChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> UnequalHandHpChoices(ProgramSkillFrame f)
    {
        var r = f.UnequalHandHp!;
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = "unequal-hand-hp-ending",
            ["branch"] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        PromptChoice Choice(string branch, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"unequal-hand-hp.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}"), text, cards ?? [], targets ?? [], Parameters(branch));
        var choices = new List<PromptChoice>();
        switch (r.Stage)
        {
            case UnequalHandHpStage.Target:
                choices.AddRange(r.Targets.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count != _coreMatchParticipantsPlayersAccess._players[s].Hp).Select(s => Choice("target", _coreMatchParticipantsPlayersAccess._players[s].Name, targets: [s])));
                break;
            case UnequalHandHpStage.Action:
                if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Count > 0)
                    choices.Add(Choice("discard", "令其弃置一张手牌"));
                choices.Add(Choice("draw", "令其摸一张牌"));
                break;
            case UnequalHandHpStage.Discard:
                choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).Select(c => Choice("discard-card", $"弃置【{(c.DisplayName)}】", [c.Id])));
                break;
            case UnequalHandHpStage.Gift:
                if (r.TargetSeat != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                    choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(f.OwnerSeat)).Where(c => !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c)).Select(c => Choice("give-card", $"交给 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)}【{(c.DisplayName)}】", [c.Id], [r.TargetSeat])));
                break;
        }

        if (r.Stage is UnequalHandHpStage.Target or UnequalHandHpStage.Gift)
            choices.Add(Choice("decline", "不执行"));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishUnequalHandHpChoice(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.UnequalHandHp!;
        var choices = UnequalHandHpChoices(f);
        var chooser = r.Stage == UnequalHandHpStage.Discard ? r.TargetSeat : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { UnequalHandHp = r with { Choices = choices } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage is UnequalHandHpStage.Discard or UnequalHandHpStage.Gift,
            Choices = choices,
            TargetSeat = r.TargetSeat >= 0 ? r.TargetSeat : null,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("unequal-hand-hp-ending")]
    internal void ResolveUnequalHandHpChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("No owning unequal hand/HP frame.");
        RequireUnequalHandHpReceipt(f);
        var r = f.UnequalHandHp!;
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], UnequalHandHpChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("The exact hand/HP participant or payment changed.");
        var branch = choice.Parameters["branch"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (branch == "decline")
        {
            CompleteUnequalHandHp(f);
            return;
        }

        if (r.Stage == UnequalHandHpStage.Target)
        {
            var target = choice.Targets.Single();
            r = r with
            {
                TargetSeat = target,
                FrozenHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count,
                FrozenHp = _coreMatchParticipantsPlayersAccess._players[target].Hp,
                Stage = UnequalHandHpStage.Action,
                Choices = []
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnequalHandHpTargetSelectedEvent(f.Id, target, r.FrozenHandCount, r.FrozenHp));
            PublishUnequalHandHpChoice(f);
            return;
        }

        if (r.Stage == UnequalHandHpStage.Action)
        {
            var draw = branch == "draw";
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = r with { Draw = draw, Choices = [] } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnequalHandHpActionSelectedEvent(f.Id, draw));
            if (draw)
                IssueUnequalHandHpMovement(f, "adjust-draw", r.TargetSeat, null, CardLocation.Hand(r.TargetSeat), UnequalHandHpStage.AdjustmentChildren);
            else
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = f.UnequalHandHp!with { Stage = UnequalHandHpStage.Discard } });
                PublishUnequalHandHpChoice(f);
            }

            return;
        }

        if (r.Stage == UnequalHandHpStage.Discard)
        {
            IssueUnequalHandHpMovement(f, "adjust-discard", r.TargetSeat, choice.Cards.Single(), CardLocation.DiscardPile, UnequalHandHpStage.AdjustmentChildren);
            return;
        }

        if (r.Stage == UnequalHandHpStage.Gift)
        {
            IssueUnequalHandHpMovement(f, "gift", r.TargetSeat, choice.Cards.Single(), CardLocation.Hand(r.TargetSeat), UnequalHandHpStage.GiftChildren);
            return;
        }

        throw new InvalidOperationException("A hand/HP choice appeared after its movement was already issued.");
    }

    internal void IssueUnequalHandHpMovement(ProgramSkillFrame supplied, string step, int subject, int? cardId, CardLocation to, UnequalHandHpStage stage)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var from = cardId is { } id ? _cardZonesOperations._cardZones.GetLocation(id) : (CardLocation? )null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = f.UnequalHandHp!with { Stage = stage, Choices = [] }, PendingMovementContinuation = new(subject, 0, null) });
        var reason = new CardMoveReason(UnequalHandHpReason(f, step));
        var logicalDiscard = cardId is { } cost && to == CardLocation.DiscardPile ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [cost], reason, f.Id) : null;
        var count = cardId is null ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[subject], 1, true, reason).Count : 1;
        if (cardId is { } paid)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([paid], to, reason, (step == "gift" ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, subject, [paid], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null) : null));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        var payment = new UnequalHandHpPayment(step, subject, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count, cardId, from, to);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = f.UnequalHandHp!with { Payments = f.UnequalHandHp.Payments.Append(payment).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnequalHandHpMovementIssuedEvent(f.Id, payment));
        if (!DrainUnequalHandHpChildren(f))
            ReturnUnequalHandHpMovement(f);
    }

    internal bool DrainUnequalHandHpChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void CompleteUnequalHandHp(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnequalHandHpCompletedEvent(f.Id, f.UnequalHandHp!.TargetSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 20400)]
    internal bool ReturnUnequalHandHpMovement(ProgramSkillFrame f)
    {
        if (f.UnequalHandHp is null)
            return false;
        RequireUnequalHandHpReceipt(f);
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("An issued hand/HP move lost its typed return.");
        if (DrainUnequalHandHpChildren(f))
            return true;
        var r = f.UnequalHandHp!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            CompleteUnequalHandHp(f);
            return true;
        }

        if (r.Stage == UnequalHandHpStage.AdjustmentChildren)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count;
            var qualified = target.IsAlive && hand == target.Hp;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = r with { RewardQualified = qualified } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UnequalHandHpRewardQualifiedEvent(f.Id, target.Seat, hand, target.Hp, qualified));
            if (!qualified)
            {
                CompleteUnequalHandHp(f);
                return true;
            }

            IssueUnequalHandHpMovement(f, "reward", f.OwnerSeat, null, CardLocation.Hand(f.OwnerSeat), UnequalHandHpStage.RewardChildren);
            return true;
        }

        if (r.Stage == UnequalHandHpStage.RewardChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || r.TargetSeat == f.OwnerSeat)
            {
                CompleteUnequalHandHp(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UnequalHandHp = r with { Stage = UnequalHandHpStage.Gift } });
            PublishUnequalHandHpChoice(f);
            return true;
        }

        if (r.Stage != UnequalHandHpStage.GiftChildren)
            throw new InvalidOperationException("The hand/HP movement returned to an unissued stage.");
        CompleteUnequalHandHp(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 20400)]
    internal bool ResumeUnequalHandHp(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { UnequalHandHp: not null } f || f.Id != id)
            return false;
        RequireUnequalHandHpReceipt(f);
        if (f.PendingMovementContinuation is not null && !DrainUnequalHandHpChildren(f))
            ReturnUnequalHandHpMovement(f);
        return true;
    }

    internal void RequireUnequalHandHpReceipt(ProgramSkillFrame f)
    {
        if (f.UnequalHandHp is not { } r || f.TriggerId is null || r.Source != TargetedTrickHandHpSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != 1 || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayEnding } context || r.WindowFrameId != context.ParentFrameId || !_coreVirtualOrdinaryTrickUsesOperations.IsExactOwnPlayEndingProgram(f) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } program || program.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveUnequalHandHpEnding || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UnequalHandHpIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.WindowFrameId == r.WindowFrameId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.InstructionIndex == 1) != 1 || r.Payments.Count > 3 || r.Stage != UnequalHandHpStage.Target && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.FrozenHandCount == r.FrozenHp))
            throw new InvalidOperationException("The hand/HP receipt lost its exact source, preparation parent or instruction.");
        var expectedCount = r.Stage switch
        {
            UnequalHandHpStage.Target or UnequalHandHpStage.Action or UnequalHandHpStage.Discard => 0,
            UnequalHandHpStage.AdjustmentChildren => 1,
            UnequalHandHpStage.RewardChildren or UnequalHandHpStage.Gift => 2,
            UnequalHandHpStage.GiftChildren => 3,
            _ => -1
        };
        var targets = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UnequalHandHpTargetSelectedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        var actions = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UnequalHandHpActionSelectedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        var rewards = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UnequalHandHpRewardQualifiedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.Payments.Count != expectedCount || r.Targets.Count != r.Targets.Distinct().Count() || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || (r.Stage == UnequalHandHpStage.Target ? r.TargetSeat != -1 || targets.Length != 0 : targets is not [var selected] || selected.TargetSeat != r.TargetSeat || selected.HandCount != r.FrozenHandCount || selected.Hp != r.FrozenHp || !r.Targets.Contains(r.TargetSeat)) || (r.Stage is UnequalHandHpStage.Target or UnequalHandHpStage.Action ? r.Draw is not null || actions.Length != 0 : r.Draw is not { } draw || actions is not [var action] || action.Draw != draw || r.Stage == UnequalHandHpStage.Discard && draw) || (expectedCount < 2 ? r.RewardQualified is not null || rewards.Length != 0 : r.RewardQualified != true || rewards is not [var reward] || !reward.Qualified || reward.TargetSeat != r.TargetSeat || reward.HandCount != reward.Hp))
            throw new InvalidOperationException("The hand/HP receipt lost its exact participant, choice or paid stage prefix.");
        for (var index = 0; index < r.Payments.Count; index++)
        {
            var p = r.Payments[index];
            var expectedStep = index == 0 ? r.Draw == true ? "adjust-draw" : "adjust-discard" : index == 1 ? "reward" : "gift";
            var expectedSubject = index == 1 ? f.OwnerSeat : r.TargetSeat;
            var expectedTo = expectedStep == "adjust-discard" ? CardLocation.DiscardPile : CardLocation.Hand(expectedSubject);
            var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == UnequalHandHpReason(f, p.Step)).ToArray();
            var fact = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UnequalHandHpMovementIssuedEvent>().Where(e => e.ProgramFrameId == f.Id && e.Payment.Step == p.Step).ToArray();
            if (p.Step != expectedStep || p.SubjectSeat != expectedSubject || p.To != expectedTo || p.Before < 0 || p.After < p.Before || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || index > 0 && p.Before < r.Payments[index - 1].After || (expectedStep is "reward" or "adjust-draw" ? p.CardId is not null || p.From is not null : p.CardId is null || expectedStep == "adjust-discard" && p.From != CardLocation.Hand(r.TargetSeat) || expectedStep == "gift" && p.From != CardLocation.Hand(f.OwnerSeat) && p.From != CardLocation.Equipment(f.OwnerSeat)) || fact is not [var issued] || issued.Payment != p || p.Count is < 0 or > 1 || actual.Length != p.Count || (p.CardId is { } paid ? p.Count != 1 || p.From is null || actual[0].CardId != paid || actual[0].From != p.From || actual[0].To != p.To && !(actual[0].To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(paid).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(actual[0]))) : actual.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(p.SubjectSeat))))
                throw new InvalidOperationException("The hand/HP receipt lost its exact once-only actual movement producer.");
        }
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 50)]
    internal void AssertRegisteredUnequalHandHpReceipt(ProgramSkillFrame frame)
    {
        if (frame.UnequalHandHp is not null)
            RequireUnequalHandHpReceipt(frame);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 10)]
    internal bool IsUnequalHandHpMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.UnequalHandHp is { Payments.Count: > 0 } r && effect?.Op == SkillProgramEffectOp.ResolveUnequalHandHpEnding && pending.SubjectSeat == r.Payments[^1].SubjectSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 470)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 480)]
    internal bool CanContinueUnequalHandHp(ProgramSkillFrame f)
    {
        if (f.UnequalHandHp is not { Payments.Count: > 0 })
            return false;
        RequireUnequalHandHpReceipt(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveUnequalHandHpEnding)]
    internal PromptChoice SelectAiUnequalHandHp(PendingDecision prompt, ProgramSkillFrame f)
    {
        var r = f.UnequalHandHp!;
        if (r.Stage == UnequalHandHpStage.Target)
            return prompt.Choices.OrderByDescending(c => c.Targets.Count == 0 ? 0d : Math.Max(_coreTargetedTrickHandHpAiOperations.UnequalHandHpPublicChoiceScore(f.OwnerSeat, c.Targets[0], true), _coreTargetedTrickHandHpAiOperations.UnequalHandHpPublicChoiceScore(f.OwnerSeat, c.Targets[0], false))).First();
        if (r.Stage == UnequalHandHpStage.Action)
        {
            var desired = _coreTargetedTrickHandHpAiOperations.UnequalHandHpPublicChoiceScore(f.OwnerSeat, r.TargetSeat, true) >= _coreTargetedTrickHandHpAiOperations.UnequalHandHpPublicChoiceScore(f.OwnerSeat, r.TargetSeat, false) ? "draw" : "discard";
            return prompt.Choices.FirstOrDefault(c => c.Parameters["branch"] == desired) ?? prompt.Choices.First();
        }

        if (r.Stage == UnequalHandHpStage.Gift && _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreCardEffectIntervention(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), CardKind.DrawTwo, r.TargetSeat) >= 0d)
            return prompt.Choices.Single(c => c.Parameters["branch"] == "decline");
        return prompt.Choices.First();
    }
}
