#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuHuangMouMobileRuntime
{
    internal const string MouXuHuangObtainReason = "skill-program.mou-mobile:duanliang.obtain";
    internal bool HasMouXuHuangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouXuHuangDuanliang) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouXuHuangShipo);

    internal static CardConversionSource MouXuHuangSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_xu_huang_mou_mobile_MouXuHuangDuanliang.MouXuHuangSource(f);
    internal bool MouXuHuangPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangMouyiStartedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MouXuHuangComposition.Duanliang && e.Phase == phase);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 29500, SkillProgramEffectOp.ResolveMouXuHuangDuanliang)]
    internal bool CanStartMouXuHuangDuanliang(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && !MouXuHuangPhaseSpent(owner.Seat, phase) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat);
    internal bool MouXuHuangDuanliangRoot(ProgramSkillFrame f) => f.MouXuHuangDuanliang is { } r && f.SkillId == MouXuHuangComposition.Duanliang && f.TriggerId is null && f.ActivationId == "private-mouyi" && f.InstructionIndex == 1 && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.SequenceEqual([r.TargetSeat]) && r.Source == MouXuHuangSource(f) && r.GameplayHash == f.GameplayHash && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) && r.TargetSeat != f.OwnerSeat && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && r.Phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is { } program && program.GameplayHash == f.GameplayHash && MouXuHuangComposition.IsDuanliang(program) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuHuangMouyiStartedEvent>().Count(e => e == new MouXuHuangMouyiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.TargetSeat)) == 1;
    internal SkillProgramStepOutcome BeginMouXuHuangDuanliang(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouXuHuangDuanliang is not null || f.SkillId != MouXuHuangComposition.Duanliang || f.ActivationId != "private-mouyi" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || MouXuHuangPhaseSpent(f.OwnerSeat, phase))
            throw new InvalidOperationException("Duanliang requires one other living strategy participant and one unspent actual own Play.");
        var r = new MouXuHuangDuanliangReceipt(MouXuHuangSource(f), f.GameplayHash, phase, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangMouyiStartedEvent(f.Id, r.Source, r.GameplayHash, phase, target));
        PublishMouXuHuang(f, f.OwnerSeat, MouXuHuangStrategyChoices(f), "断粮：私密选择进攻策略，双方承诺后共同揭晓。");
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouXuHuangStrategyChoices(ProgramSkillFrame f)
    {
        var attack = f.MouXuHuangDuanliang!.Stage == MouXuHuangDuanliangStage.AttackChoice;
        return Array.AsReadOnly((attack ? new[] { ("siege", "围城断粮"), ("advance", "擂鼓进军") } : new[] { ("block-siege", "防备断粮"), ("block-advance", "防备进军") }).Select(p => new PromptChoice(new($"mou-xu-huang.{(f.Id)}.{((attack ? "attack" : "defense"))}.{(p.Item1)}"), p.Item2, [], [], new Dictionary<string, string> { ["program-action"] = "mou-xu-huang-mouyi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["side"] = attack ? "attack" : "defense", ["option-id"] = p.Item1 })).ToArray());
    }

    internal void PublishMouXuHuang(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices, string text)
    {
        if (choices.Count == 0)
            throw new InvalidOperationException("Xu Huang cannot publish an empty private selection.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-xu-huang-mouyi")]
    internal void AnswerMouXuHuangStrategy(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouXuHuangDuanliang;
        if (!MouXuHuangDuanliangRoot(f) || r is null || r.Stage is not (MouXuHuangDuanliangStage.AttackChoice or MouXuHuangDuanliangStage.DefenseChoice) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouXuHuangDuanliangStage.AttackChoice ? f.OwnerSeat : r.TargetSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouXuHuangStrategyChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The strategy commitment must be its current participant's exact private published answer.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MouXuHuangDuanliangStage.AttackChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Attack = selected.Parameters["option-id"] == "siege" ? MouXuHuangAttack.Siege : MouXuHuangAttack.Advance, Stage = MouXuHuangDuanliangStage.DefenseChoice } });
            // No attack bit enters a fact, log or the defender's choices before reveal.
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangStrategyCommittedEvent(f.Id, f.OwnerSeat, true));
            PublishMouXuHuang(f, r.TargetSeat, MouXuHuangStrategyChoices(f), "断粮：私密选择防守策略，无法查看对方尚未揭晓的选择。");
            return;
        }

        var defense = selected.Parameters["option-id"] == "block-siege" ? MouXuHuangDefense.BlockSiege : MouXuHuangDefense.BlockAdvance;
        var success = r.Attack == MouXuHuangAttack.Siege ? defense == MouXuHuangDefense.BlockAdvance : defense == MouXuHuangDefense.BlockSiege;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Defense = defense, Revealed = true, Success = success, Stage = MouXuHuangDuanliangStage.Revealed } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangStrategyCommittedEvent(f.Id, r.TargetSeat, false));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangMouyiRevealedEvent(f.Id, r.Attack!.Value, defense, success));
        ContinueMouXuHuangDuanliang(f.Id);
    }

    internal void ContinueMouXuHuangDuanliang(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXuHuangDuanliang: { } r } f || f.Id != id)
            return;
        if (r.Stage == MouXuHuangDuanliangStage.NativeChildren && !r.AttackReturned)
            return;
        if (r.Stage == MouXuHuangDuanliangStage.NativeUse || r.Stage == MouXuHuangDuanliangStage.NativeIssuing)
            return;
        if (DrainMouXuHuangChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouXuHuangDuanliang!;
        if (r.Stage is MouXuHuangDuanliangStage.NativeChildren or MouXuHuangDuanliangStage.ObtainChildren || !r.Success || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouXuHuangDuanliang(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == MouXuHuangDuanliangStage.RefillChildren)
        {
            IssueMouXuHuangTop(f);
            return;
        }

        if (r.Stage != MouXuHuangDuanliangStage.Revealed)
            throw new InvalidOperationException("Duanliang cannot repeat a private commitment or native consequence.");
        if (r.Attack == MouXuHuangAttack.Advance)
        {
            if (CanMouXuHuangDuel(f.OwnerSeat, r.TargetSeat, r.Source))
                BeginMouXuHuangDuelTargets(f);
            else
                FinishMouXuHuangDuanliang(f);
            return;
        }

        if (_coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], CardKind.SupplyShortage))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Stage = MouXuHuangDuanliangStage.ObtainChoice } });
            var choices = MouXuHuangObtainChoices(f);
            if (choices.Count == 0)
                FinishMouXuHuangDuanliang(f);
            else
                PublishMouXuHuang(f, f.OwnerSeat, choices, "围城断粮：获得该角色区域内的一张牌。");
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Stage = MouXuHuangDuanliangStage.RefillChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        SealMouXuHuangMovements(f.Id, before);
        ContinueMouXuHuangDuanliang(f.Id);
    }

    internal IReadOnlyList<PromptChoice> MouXuHuangObtainChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.MouXuHuangDuanliang!.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-xu-huang-obtain" } }).ToArray());
    [ProgramAnswer("mou-xu-huang-obtain")]
    internal void AnswerMouXuHuangObtain(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouXuHuangDuanliang!;
        if (!MouXuHuangDuanliangRoot(f) || r.Stage != MouXuHuangDuanliangStage.ObtainChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouXuHuangObtainChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Duanliang obtain lost its canonical private HEJ slot.");
        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), r.TargetSeat);
        var slot = int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = r with { Stage = MouXuHuangDuanliangStage.ObtainChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new(MouXuHuangObtainReason), m =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouXuHuangDuanliang = current.MouXuHuangDuanliang!with { Obtained = new(0, r.TargetSeat, m.CardId, m.From, m.To, m.Sequence) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangObtainPaidEvent(f.Id, r.TargetSeat, m.CardId, m.From, m.To, m.Sequence));
        }, disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        SealMouXuHuangMovements(f.Id, before);
        ContinueMouXuHuangDuanliang(f.Id);
    }

    internal void FinishMouXuHuangDuanliang(ProgramSkillFrame f)
    {
        var r = f.MouXuHuangDuanliang!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuHuangDuanliangCompletedEvent(f.Id, r.Success, r.Native is not null, r.Obtained?.To == CardLocation.Hand(f.OwnerSeat)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXuHuangDuanliang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouXuHuangDuanliang)]
    internal PromptChoice AiMouXuHuangDuanliang(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouXuHuangDuanliang!;
        if (r.Stage == MouXuHuangDuanliangStage.ObtainChoice)
            return p.Choices.OrderByDescending(c => c.Parameters["source-zone"] != CardZoneKind.Hand.ToString()).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        // The risk function accepts a viewer snapshot, never the other commitment.
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
        var target = view.Players[r.TargetSeat];
        if (r.Stage == MouXuHuangDuanliangStage.DuelTargets)
            return p.Choices.OrderByDescending(c => c.Targets.Sum(t => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, t, new(0, 0, 0, 0, 0, 0, false, false) { TargetDamage = 1, TargetHealthEffects = [new(PublicHealthEffectKind.Damage, 1)] }))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        var duelRisk = target.Armor > 0 ? 1 : target.Hp <= 1 ? 20 : target.Hp == 2 ? 12 : 4;
        var siegeRisk = target.Judgment.Any(c => c.Kind == CardKind.SupplyShortage) ? target.HandCount + target.Equipment.Count + target.Judgment.Count > 0 ? 5 : 0 : 8;
        var block = duelRisk >= siegeRisk ? "block-advance" : "block-siege";
        if (p.PlayerSeat != f.OwnerSeat)
            return p.Choices.Single(c => c.Parameters["option-id"] == block);
        var ownView = view.Players[f.OwnerSeat];
        var damageScore = _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, r.TargetSeat, new(0, 0, 0, 0, 0, 0, false, false) { TargetDamage = 1, TargetHealthEffects = [new(PublicHealthEffectKind.Damage, 1)] });
        var attack = damageScore > 0 && (ownView.HandCount > 0 || target.Hp <= 2) ? "advance" : "siege";
        return p.Choices.Single(c => c.Parameters["option-id"] == attack);
    }

    SkillProgramStepOutcome global::CardGame.Core.IMouXuHuangHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouXuHuangDuanliang ? BeginMouXuHuangDuanliang(f) : BeginMouXuHuangShipo(f);
}
