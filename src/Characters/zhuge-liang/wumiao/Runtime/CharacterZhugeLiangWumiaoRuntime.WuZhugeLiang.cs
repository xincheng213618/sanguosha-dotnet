#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangWumiaoRuntime
{
    internal const string WuZglDraw = "skill-program.wumiao:zhuge-liang.draw", WuZglCopy = "skill-program.wumiao:zhizhe.copy";
    internal bool HasWuZhugeLiangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeWuZhugeLiangJincui) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuZhugeLiangQingshi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWuZhugeLiangZhizhe);

    internal static CardConversionSource WuZglSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhuge_liang_wumiao_WuZhugeLiang.WuZglSource(f);
    internal bool WuZglTurnOpen(int turn, int owner) => turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == turn && e.OwnerSeat == owner);
    internal CardActionContext? WuZglAction(ProgramSkillWindowContext c) => c.CardUse is { } id ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Select(f => f.Action).FirstOrDefault(a => a?.ActionId == id.CardActionId) ?? _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().LastOrDefault(e => e.Action.ActionId == id.CardActionId)?.Action : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30300, SkillProgramEffectOp.InitializeWuZhugeLiangJincui, SkillProgramEffectOp.ResolveWuZhugeLiangJincui, SkillProgramEffectOp.ResolveWuZhugeLiangQingshi)]
    internal bool CanOfferWuZgl(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => t.Effects[0].Op switch
    {
        SkillProgramEffectOp.InitializeWuZhugeLiangJincui => c.Window == SkillProgramTriggerWindow.GameStarting && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuZhugeLiangStartedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Operation == SkillProgramEffectOp.InitializeWuZhugeLiangJincui),
        SkillProgramEffectOp.ResolveWuZhugeLiangJincui => c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Preparation } p && p.ActorSeat == candidate.OwnerSeat,
        _ => CanWuZglQingshi(candidate.OwnerSeat, c)};
    SkillProgramStepOutcome global::CardGame.Core.IWuZhugeLiangHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWuZgl(f, op);
    internal SkillProgramStepOutcome BeginWuZgl(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WuZhugeLiang is not null || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op)
            throw new InvalidOperationException("Wu Zhuge Liang lost his original operation.");
        var context = f.WindowContext;
        var action = context is null ? null : WuZglAction(context);
        var use = action is null ? null : _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId);
        var r = new WuZhugeLiangReceipt(WuZglSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, context?.ParentFrameId ?? 0, action?.ActionId ?? 0, use?.Id ?? 0)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.TurnOwner, r.WindowId, r.ActionId, r.UseId));
        if (op == SkillProgramEffectOp.InitializeWuZhugeLiangJincui)
        {
            StartWuZglDraw(f, f.OwnerSeat, Math.Max(0, 7 - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveWuZhugeLiangJincui)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var sevens = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Count(c => c.Rank == 7);
            var before = owner.Hp;
            var after = Math.Min(owner.MaxHp, Math.Max(1, sevens));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Paid = true, Stage = WuZhugeLiangStage.HealthChildren, HpBefore = before, HpAfter = after, SevenCount = sevens }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            owner.Hp = after;
            var change = _coreHpChangeProgramsOperations.RecordHpChange(f.Id, f.OwnerSeat, f.OwnerSeat, before, after, HpChangeKind.Setting);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = f.WuZhugeLiang!with { HpChangeId = change?.Id } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangHpSetEvent(f.Id, f.OwnerSeat, before, after, sevens, change?.Id));
            ContinueWuZgl(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveWuZhugeLiangQingshi)
        {
            if (context is null || action is null || !CanWuZglQingshi(f.OwnerSeat, context))
            {
                FinishWuZgl(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Paid = true, Name = global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(action.EffectiveKind), Stage = WuZhugeLiangStage.Choice } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangQingshiAcceptedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.ActionId, global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(action.EffectiveKind)));
            PublishWuZgl(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (f.SkillId != WuZhugeLiangComposition.Zhizhe || f.ActivationId != "copy-hand" || f.TriggerId is not null || f.SelectedCardIds is not [var originalId] || _cardZonesOperations._cardZones.GetLocation(originalId) != CardLocation.Hand(f.OwnerSeat))
            throw new InvalidOperationException("Zhizhe needs one real selected Hand card.");
        var original = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == originalId);
        var newId = checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Max(c => c.CardId) + 1);
        var copy = original with
        {
            Id = newId,
            GrantedSkillIds = Array.AsReadOnly(original.GrantedSkillIds.ToArray())
        };
        _cardZonesOperations._cardZones.AddGeneratedCard(copy);
        _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(newId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Paid = true, CreatedCardId = newId, Stage = WuZhugeLiangStage.CopyChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangCopyCreatedEvent(f.Id, r.Source, r.GameplayHash, originalId, newId, copy.Kind, copy.Suit, copy.Rank));
        _corePhysicalMovementFacadesOperations.MoveCard(copy, CardLocation.OutsideGame, CardLocation.Hand(f.OwnerSeat), new(WuZglCopy), disclosure: CardMovementDisclosure.FaceDown);
        SealWuZgl(f.Id, r.Before);
        ContinueWuZgl(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice WuZglChoice(ProgramSkillFrame f, string key, string label, int? target = null) => new(new($"wu-zhuge-liang.{(f.Id)}.{(key)}.{(target ?? -1)}"), label, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "wu-zhuge-liang", ["branch"] = key, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> WuZglChoices(ProgramSkillFrame f)
    {
        var r = f.WuZhugeLiang!;
        var choices = new List<PromptChoice>();
        if (r.Stage == WuZhugeLiangStage.DrawTargets)
        {
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => WuZglChoice(f, "toggle", (r.Targets.Contains(p.Seat) ? "取消" : "选择") + p.Name, p.Seat)));
            choices.Add(WuZglChoice(f, "finish", $"确认{(r.Targets.Count)}名角色各摸一张"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.UseId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is { Action: { } a } use && a.ActionId == r.ActionId)
            choices.AddRange(use.TargetSeats.Distinct().Where(i => _coreMatchParticipantsPlayersAccess._players[i].IsAlive).Select(i => WuZglChoice(f, "damage", $"此牌对{(_coreMatchParticipantsPlayersAccess._players[i].Name)}伤害+1", i)));
        choices.Add(WuZglChoice(f, "others", "令任意名其他角色各摸一张牌"));
        choices.Add(WuZglChoice(f, "draw-three", "摸三张牌，然后本回合情势失效"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishWuZgl(ProgramSkillFrame f)
    {
        var choices = WuZglChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wu-zhuge-liang")]
    internal void AnswerWuZgl(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.WuZhugeLiang!;
        if (!ValidWuZgl(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], WuZglChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Qingshi requires its exact published owning choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishWuZgl(f);
            return;
        }

        var branch = selected.Parameters["branch"];
        if (branch == "toggle")
        {
            var seat = selected.Targets.Single();
            var list = r.Targets.Contains(seat) ? r.Targets.Where(t => t != seat).ToArray() : r.Targets.Append(seat).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Targets = Array.AsReadOnly(list) } });
            PublishWuZgl(f);
            return;
        }

        if (branch == "others")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Stage = WuZhugeLiangStage.DrawTargets, Branch = branch } });
            PublishWuZgl(f);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangQingshiBranchEvent(f.Id, branch));
        if (branch == "damage")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangQingshiDamageEvent(f.Id, r.Source, r.GameplayHash, r.ActionId, r.UseId, selected.Targets.Single()));
            FinishWuZgl(f);
            return;
        }

        if (branch == "draw-three")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Branch = branch } });
            StartWuZglDraw(f, f.OwnerSeat, 3);
            return;
        }

        for (var i = 0; i < r.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangQingshiTargetEvent(f.Id, i, r.Targets[i]));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Branch = "others", Stage = WuZhugeLiangStage.DrawChildren, Cursor = 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueWuZgl(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuZhugeLiangQingshi)]
    internal PromptChoice AiWuZgl(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.WuZhugeLiang?.Stage != WuZhugeLiangStage.DrawTargets)
            return p.Choices.Single(c => c.Parameters["branch"] == "draw-three");
        var selected = f.WuZhugeLiang.Targets;
        return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "toggle" && !selected.Contains(c.Targets.Single()) && _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets.Single(), new(1, 0, 0, 0, 0, 0, true, false)) > 0) ?? p.Choices.Single(c => c.Parameters["branch"] == "finish");
    }

    internal void StartWuZglDraw(ProgramSkillFrame f, int recipient, int count)
    {
        if (count <= 0)
        {
            FinishWuZgl(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = f.WuZhugeLiang!with { Paid = true, Stage = WuZhugeLiangStage.Drawing, Recipient = recipient, RequestedDraw = count, Draw = null }, PendingMovementContinuation = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, count, new(WuZglDraw));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.InitializeWuZhugeLiangJincui, SkillProgramEffectOp.ResolveWuZhugeLiangQingshi)]
    internal void ReturnWuZglDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        var r = f.WuZhugeLiang!;
        if (!ValidWuZgl(f) || r.Stage != WuZhugeLiangStage.Drawing || o.RequestedCount != r.RequestedDraw || o.RecipientSeat != r.Recipient)
            throw new InvalidOperationException("Wu Zhuge Liang returned another native draw request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Draw = o, Stage = WuZhugeLiangStage.DrawChildren, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangDrawReturnedEvent(f.Id, o.RecipientSeat, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter));
        SealWuZgl(f.Id, o.SequenceBefore);
        ContinueWuZgl(f.Id);
    }

    internal void ContinueWuZgl(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuZhugeLiang: { } r } f || f.Id != id)
            return;
        if (!ValidWuZgl(f))
            throw new InvalidOperationException("Wu Zhuge Liang lost his issued native receipt.");
        if (r.Stage is WuZhugeLiangStage.Choice or WuZhugeLiangStage.DrawTargets or WuZhugeLiangStage.Drawing)
            return;
        if (DrainWuZgl(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WuZhugeLiang!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && !(r.Operation == SkillProgramEffectOp.ResolveWuZhugeLiangQingshi && r.Branch == "others"))
        {
            FinishWuZgl(f);
            return;
        }

        if (r.Stage == WuZhugeLiangStage.HealthChildren)
        {
            var count = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangOrderingIssuedEvent(f.Id, f.OwnerSeat, count));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = null, PendingMovementContinuation = null });
            if (_coreProgramTopReorderOperations.BeginProgramTopReorder(f, count, null) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Operation == SkillProgramEffectOp.ResolveWuZhugeLiangQingshi && r.Branch == "others")
        {
            var cursor = r.Cursor;
            if (r.Draw is not null)
                cursor++;
            while (cursor < r.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[r.Targets[cursor]].IsAlive)
                cursor++;
            if (cursor < r.Targets.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = r with { Cursor = cursor, Draw = null } });
                StartWuZglDraw(f, r.Targets[cursor], 1);
                return;
            }
        }

        if (r.Operation == SkillProgramEffectOp.ResolveWuZhugeLiangQingshi && r.Branch == "draw-three")
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangQingshiDisabledEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.TurnOwner));
        FinishWuZgl(f);
    }

    internal void FinishWuZgl(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuZhugeLiangCompletedEvent(f.Id, f.WuZhugeLiang!.Operation));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuZhugeLiang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal CardSnapshot[] WuZglPrivateTopCards(int viewer) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SkillId: WuZhugeLiangComposition.Jincui, TopReorder: { } order } f && f.OwnerSeat == viewer && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var seat } && seat == viewer ? order.ViewedCardIds.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray() : [];
}
