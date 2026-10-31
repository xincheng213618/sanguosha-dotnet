#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    internal const string LiehouReason = "skill-program.liehou.hand-relay";
    internal static CardConversionSource LvKuangLvXiangSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lv_kuang_lv_xiang_ol_LvKuangLvXiangPrograms.LvKuangLvXiangSource(f);
    internal bool LvKuangLvXiangCurrent(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    internal bool LvKuangLvXiangActivationTargetAllowed(CharacterState target, SkillProgramActivation a, int ownerSeat) => !ProgramInstructionResolver.Default.Features(a).HasOperation(SkillProgramEffectOp.ResolveLiehouRangeHandRelay) || target.IsAlive && target.Seat != ownerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0 && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(ownerSeat, target.Seat);
    internal bool QigongOriginal(ProgramTriggerCandidate c, ProgramSkillWindowContext w, out CardUseFrame original, bool requireLivingTarget = true)
    {
        original = null !;
        if (w.Window != SkillProgramTriggerWindow.SlashFullyDodged || w.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame parent || parent.Continuation != ProgramCardContinuation.SlashFullyDodged || parent.ParentFrameId != identity.ParentCardUseFrameId || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { Action: { Type: CardActionType.Use } a, CardAttack: { } attack } use || a.ActionId != identity.CardActionId || a.ActorSeat != c.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || a.EffectiveDesignatedTargetSeats.Count != 1 || use.TargetSeats is not [var target] || target != attack.TargetSeat || attack.RequiredDodgeResponses < 1 || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || parent.AttackOwnerFrameId != use.Id || parent.Action.ActionId != a.ActionId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])is not { } actual || actual.OwnerSeat != c.OwnerSeat || actual.SkillId != c.SkillId || actual.SkillInstanceId != c.SkillInstanceId || actual.BindingId != c.BindingId || actual.GameplayHash != c.GameplayHash || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, a))
            return false;
        original = use;
        return !requireLivingTarget || _coreMatchParticipantsPlayersAccess._players[target].IsAlive;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 540)]
    internal bool CanOfferLvKuangLvXiang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => !ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.ResolveQigongActualDodgedSlashRequest) || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && QigongOriginal(c, w, out _);
    internal SkillProgramStepOutcome ExecuteLvKuangLvXiang(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LvKuangLvXiang is not null || f.InstructionIndex != 1 || !LvKuangLvXiangCurrent(f))
            throw new InvalidOperationException("The relay requires its original live unpaid instruction.");
        int target;
        long? useId = null, actionId = null;
        if (op == SkillProgramEffectOp.ResolveQigongActualDodgedSlashRequest)
        {
            if (f.TriggerId is not { } binding || f.WindowContext is not { } w || !QigongOriginal(new(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, 0), w, out var original))
                throw new InvalidOperationException("Qigong requires an actual originally sole Slash completed by real Dodge responses.");
            target = original.TargetSeats.Single();
            useId = original.Id;
            actionId = original.Action!.ActionId;
        }
        else
        {
            if (f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var giver] || !LvKuangLvXiangActivationTargetAllowed(_coreMatchParticipantsPlayersAccess._players[giver], ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation!, f.OwnerSeat))
                throw new InvalidOperationException("Liehou requires an actual other range participant with hand cards.");
            target = giver;
        }

        var r = new LvKuangLvXiangReceipt(LvKuangLvXiangSource(f), f.GameplayHash, 1, _coreActualTurnProgressTurnNumberAccess._turnNumber, useId is null ? "receive" : "actor", target, OriginalUseId: useId, OriginalActionId: actionId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LvKuangLvXiangStartedEvent(f.Id, r.Source, r.GameplayHash, 1, r.Turn, target, useId, actionId));
        PublishLvKuangLvXiang(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice LvChoice(ProgramSkillFrame f, string branch, string key, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, IReadOnlyDictionary<string, string>? extra = null)
    {
        var args = extra is null ? new Dictionary<string, string>() : extra.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
        args["program-action"] = "lv-kuang-lv-xiang";
        args["branch"] = branch;
        return new(new($"lv-kuang-lv-xiang.{(f.Id)}.{(key)}"), label, cards ?? [], targets ?? [], args);
    }

    internal IReadOnlyList<PromptChoice> LvKuangLvXiangChoices(ProgramSkillFrame f)
    {
        var r = f.LvKuangLvXiang!;
        var result = new List<PromptChoice>();
        if (r.Stage == "actor")
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.Target))
                result.Add(LvChoice(f, "actor", $"actor-{(p.Seat)}", "令" + p.Name + "使用杀", targets: [p.Seat]));
        if (r.Stage == "slash")
            return QigongSlashChoices(f);
        if (r.Stage == "receive" && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, r.Target))
            result.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Target, r.Target, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer).Select(c => LvChoice(f, "receive", "receive-" + c.Cards.Single(), "交给其一张手牌", c.Cards, extra: c.Parameters)));
        if (r.Stage == "give" && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Seat != r.Target && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, p.Seat)))
                foreach (var choice in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer))
                    result.Add(LvChoice(f, "give", $"give-{(choice.Cards.Single())}-{(p.Seat)}", "交给" + p.Name + "一张手牌", choice.Cards, [p.Seat], choice.Parameters));
        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal static int[] LvKuangLvXiangValidTargets(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lv_kuang_lv_xiang_ol_LvKuangLvXiangPrograms.LvKuangLvXiangValidTargets(f, choices);
    internal int LvChooser(ProgramSkillFrame f) => f.LvKuangLvXiang!.Stage switch
    {
        "receive" => f.LvKuangLvXiang.Target,
        "slash" => f.LvKuangLvXiang.Actor,
        _ => f.OwnerSeat
    };
    internal void PublishLvKuangLvXiang(ProgramSkillFrame f)
    {
        var r = f.LvKuangLvXiang!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Payments.Count == 0 && !LvKuangLvXiangCurrent(f) || r.OriginalUseId is not null && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
        {
            FinishLvKuangLvXiang(f);
            return;
        }

        var choices = LvKuangLvXiangChoices(f);
        if (choices.Count == 0)
        {
            FinishLvKuangLvXiang(f);
            return;
        }

        var chooser = LvChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：请选择本次牌与角色。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), LvKuangLvXiangValidTargets(f, choices), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("lv-kuang-lv-xiang")]
    internal void AnswerLvKuangLvXiang(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvKuangLvXiang: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != LvChooser(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], LvKuangLvXiangChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("The relay lost its actual private chooser or legal native payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Payments.Count == 0 && !LvKuangLvXiangCurrent(f))
        {
            FinishLvKuangLvXiang(f);
            return;
        }

        var branch = choice.Parameters["branch"];
        if (branch == "actor")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r with { Stage = "slash", Actor = choice.Targets.Single() } });
            PublishLvKuangLvXiang(f);
            return;
        }

        if (branch == "decline")
        {
            FinishLvKuangLvXiang(f);
            return;
        }

        if (r.Stage == "slash")
        {
            IssueQigongSlash(f, choice);
            return;
        }

        var giver = r.Stage == "receive" ? r.Target : f.OwnerSeat;
        var recipient = r.Stage == "receive" ? f.OwnerSeat : choice.Targets.Single();
        var id = choice.Cards.Single();
        if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(giver))
            throw new InvalidOperationException("Liehou may transfer only the selected actual giver's hand card.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r with { Stage = r.Stage == "receive" ? "receive-children" : "give-children" }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), new(LiehouReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, giver, recipient, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, moves) =>
        {
            if (moves is not [var m] || m.CardId != id || m.From != CardLocation.Hand(giver) || m.To != CardLocation.Hand(recipient))
                throw new InvalidOperationException("Liehou failed its exact actual Hand-to-Hand payment.");
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = new LiehouPayment(id, m.From, recipient, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, batch);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { LvKuangLvXiang = active.LvKuangLvXiang!with { Payments = active.LvKuangLvXiang.Payments.Append(paid).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiehouHandPaidEvent(f.Id, r.Source, r.GameplayHash, active.LvKuangLvXiang.Payments.Count + 1, giver, recipient, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, batch));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainLvKuangLvXiang(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 25000)]
    internal bool ResumeLvKuangLvXiang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvKuangLvXiang: { } r } f || f.Id != id)
            return false;
        if (r.Stage == "native" && !r.Returned)
            return false;
        if (r.Stage is "receive-children" or "give-children" or "native")
        {
            if (DrainLvKuangLvXiang(f))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.LvKuangLvXiang!;
            if (r.Stage == "receive-children" && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r with { Stage = "give" }, PendingMovementContinuation = null });
                PublishLvKuangLvXiang(f);
            }
            else
                FinishLvKuangLvXiang(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishLvKuangLvXiang(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 25000)]
    internal bool ReturnLvKuangLvXiangMovement(ProgramSkillFrame f) => f.LvKuangLvXiang is not null && ResumeLvKuangLvXiang(f.Id);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 320)]
    internal bool IsLvKuangLvXiangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e is not null && LvKuangLvXiangComposition.Own(e.Op) && f.LvKuangLvXiang is { Stage: "receive-children" or "give-children" } && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal void FinishLvKuangLvXiang(ProgramSkillFrame f)
    {
        RestoreQigongParentAttack(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveQigongActualDodgedSlashRequest, SkillProgramEffectOp.ResolveLiehouRangeHandRelay)]
    internal PromptChoice LvKuangLvXiangAi(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    SkillProgramStepOutcome global::CardGame.Core.ILvKuangLvXiangProgramHost.ExecuteLvKuangLvXiang(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteLvKuangLvXiang(f, op);
}
