#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IWeijieHost.Resolve(ProgramSkillFrame f) => ResolveWeijie(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27462, SkillProgramEffectOp.ResolveWeijieRequiredBasicRequest)]
    internal bool IsWeijieOrdinaryActivation(ProgramActivationLegalityContext context) => false;
    internal static string WeijieDecisionHash(PendingDecision p) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guo_tu_ol_Weijie.WeijieDecisionHash(p);
    internal static bool SameWeijieNeed(WeijieNeed a, WeijieNeed b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guo_tu_ol_Weijie.SameWeijieNeed(a, b);
    internal bool IsWeijieAnswer(ChoiceId id) => _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(c => c.Id == id && (c.Parameters.ContainsKey(WeijieOpen) || c.Parameters.ContainsKey(WeijieChoice))) == true;
    internal CommandResult SubmitWeijieAnswer(int actor, PromptId prompt, ChoiceId id)
    {
        if (!_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.NotStarted, "游戏尚未开始。");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != actor || p.PromptId != prompt)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidPrompt, "诿解必须回答当前真实请求。");
        var c = p.Choices.SingleOrDefault(c => c.Id == id && (c.Parameters.ContainsKey(WeijieOpen) || c.Parameters.ContainsKey(WeijieChoice)));
        if (c is null)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidChoice, "诿解选择未公布。");
        return _kernelCoreMainOperations2.Accept(() =>
        {
            AnswerWeijie(c);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void OpenWeijie(PromptChoice c)
    {
        var p = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("Weijie has no real native request.");
        var n = CaptureWeijieNeed(p) ?? throw new InvalidOperationException("Weijie lost its exact native requester.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(c);
        if (!p.Choices.Any(x => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [c])) || !WeijieSources(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]).Any(s => s.Source == source) || !WeijieVictims(p.PlayerSeat).Any() || !WeijieOutputs(n).Any())
            throw new InvalidOperationException("Weijie cannot replace an unavailable native request or source.");
        var hash = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash;
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, p.PlayerSeat, source.SkillId, source.BindingId, hash, 0, [], [])
        {
            SkillInstanceId = source.SkillInstanceId,
            Weijie = new(source, hash, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, n, p, WeijieStage.Declare)
        };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieStartedEvent(f.Id, source, hash, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, n.Intent, n.OwnerFrameId, n.RequestFrameId, p.PromptId, p.Revision, WeijieDecisionHash(p)));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, f.OwnerSeat, f.SkillId, f.ActivationId));
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(f.Id, host, host);
    }

    internal SkillProgramStepOutcome ResolveWeijie(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.Weijie is null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Weijie must enter its registered native-only request instruction.");
        PublishWeijie(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> WeijieChoices(ProgramSkillFrame f)
    {
        var r = f.Weijie!;
        var result = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            [WeijieChoice] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        PromptChoice C(string branch, string label, IReadOnlyList<int>? targets = null, string suffix = "") => new(new($"weijie.{(f.Id)}.{(r.Stage)}.{(branch)}.{(suffix)}"), label, [], targets ?? [], Args(branch));
        if (r.Stage == WeijieStage.Declare)
            foreach (var(kind, target)in WeijieOutputs(r.Need))
            {
                var args = Args("declare");
                args["kind"] = kind.ToString();
                args["target"] = target.ToString(CultureInfo.InvariantCulture);
                result.Add(new(new($"weijie.{(f.Id)}.declare.{(kind)}.{(target)}"), $"需要【{(CardCatalog.Get(kind).DisplayName)}】" + (WeijieIsPlayed(r.Need) || r.Need.Intent == WeijieIntent.Dodge ? "" : $"，目标：{(_coreMatchParticipantsPlayersAccess._players[target].Name)}"), [], [], args));
            }
        else if (r.Stage == WeijieStage.Victim)
            foreach (var seat in WeijieVictims(f.OwnerSeat))
                result.Add(C("victim", "弃置" + _coreMatchParticipantsPlayersAccess._players[seat].Name + "的一张手牌", [seat], seat.ToString(CultureInfo.InvariantCulture)));
        else if (r.Stage == WeijieStage.Card && r.VictimSeat is { } victim)
            foreach (var old in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, victim, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard))
                result.Add(old with { Id = new($"weijie.{(f.Id)}.card.{(old.Id.Value)}"), Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>(old.Parameters) { [WeijieChoice] = "card", ["program-action"] = "weijie-card" }) });
        if (r.Stage != WeijieStage.CostChildren)
            result.Add(C("cancel", "取消，返回原请求"));
        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishWeijie(ProgramSkillFrame f)
    {
        var r = f.Weijie!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !WeijieNativeNeedCurrent(r) || r.Stage != WeijieStage.Declare && !WeijieNativeLegal(r.Need, r.OutputKind!.Value, r.TargetSeat!.Value))
        {
            FinishWeijieAttempt(f, false);
            return;
        }

        var choices = WeijieChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == WeijieStage.Declare ? "诿解：声明本次需要的基本牌。" : r.Stage == WeijieStage.Victim ? "诿解：选择距离为1的其他角色。" : "诿解：选择一张不公开牌名的手牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void AnswerWeijie(PromptChoice c)
    {
        if (c.Parameters.ContainsKey(WeijieOpen))
        {
            OpenWeijie(c);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weijie: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !WeijieChoices(f).Any(x => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [c])))
            throw new InvalidOperationException("Weijie changed its exact private declaration or opaque native slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = c.Parameters[WeijieChoice];
        if (branch == "cancel")
        {
            FinishWeijieAttempt(f, false);
            return;
        }

        if (branch == "declare")
        {
            var kind = Enum.Parse<CardKind>(c.Parameters["kind"]);
            var target = int.Parse(c.Parameters["target"], CultureInfo.InvariantCulture);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieDeclaredEvent(f.Id, kind, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weijie = r with { OutputKind = kind, TargetSeat = target, Stage = WeijieStage.Victim } });
            PublishWeijie(f);
            return;
        }

        if (branch == "victim")
        {
            var seat = c.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieVictimSelectedEvent(f.Id, seat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weijie = r with { VictimSeat = seat, Stage = WeijieStage.Card } });
            PublishWeijie(f);
            return;
        }

        if (branch != "card" || r.VictimSeat is not { } victim || !WeijieVictims(f.OwnerSeat).Contains(victim) || !WeijieNativeNeedCurrent(r) || !WeijieNativeLegal(r.Need, r.OutputKind!.Value, r.TargetSeat!.Value))
            throw new InvalidOperationException("Weijie lost its unpaid native need or distance-one Hand owner.");
        if (c.Parameters.GetValueOrDefault("source-zone") != CardZoneKind.Hand.ToString() || !int.TryParse(c.Parameters.GetValueOrDefault("slot-index"), out var slot))
            throw new InvalidOperationException("Weijie lost its genuine opaque Hand slot.");
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(victim))[slot];
        var name = global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[victim], card));
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weijie = r with { Stage = WeijieStage.CostChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(WeijieDiscardReason), (batch, moves) =>
        {
            var m = moves.Single();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = new WeijiePaidOrigin(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, r.Need, r.OutputKind!.Value, r.TargetSeat!.Value, victim, card.Id, card.Kind, name, CardLocation.Hand(victim), m.Sequence, batch);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { Weijie = current.Weijie!with { Paid = paid, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieCostPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, victim, card.Id, card.Kind, name, paid.From, m.Sequence, batch));
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weijie = f.Weijie!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueWeijie(f.Id);
    }

    internal bool DrainWeijie(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueWeijie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weijie: { } r } f || f.Id != id)
            return;
        if (r.Stage != WeijieStage.CostChildren)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishWeijie(f);
            return;
        }

        if (DrainWeijie(f))
            return;
        var matched = r.Paid is { } p && p.EffectiveName == global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(p.OutputKind);
        var legal = matched && WeijieNativeNeedCurrent(r) && WeijieNativeLegal(r.Need, r.OutputKind!.Value, r.TargetSeat!.Value);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeijieAttemptResolvedEvent(f.Id, matched, legal));
        FinishWeijieAttempt(f, legal);
    }

    internal void FinishWeijieAttempt(ProgramSkillFrame f, bool success)
    {
        var r = f.Weijie!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weijie = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        if (success)
            DispatchWeijie(r.Paid!);
        else
            RepublishWeijieNative(r);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27462)]
    internal bool ResumeWeijie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weijie: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinueWeijie(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27462)]
    internal bool ReturnWeijieMovement(ProgramSkillFrame f)
    {
        if (f.Weijie is null)
            return false;
        ContinueWeijie(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27462)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27462)]
    internal bool ContinuePaidWeijie(ProgramSkillFrame f)
    {
        if (f.Weijie?.Paid is null)
            return false;
        return true;
    }

    internal bool TryAdvanceWeijieAi()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat].IsHuman)
            return false;
        var choices = p.Choices.Where(c => c.Parameters.ContainsKey(WeijieOpen) || c.Parameters.ContainsKey(WeijieChoice)).ToArray();
        if (choices.Length == 0)
            return false;
        // No private victim card is inspected here. An actual opaque slot is selected.
        var selected = choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault(WeijieChoice) != "cancel") ?? choices[0];
        AnswerWeijie(selected);
        return true;
    }
}
