#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoZhaoAnniversaryRuntime
{
    internal const string GuoZhaoObtainReason = "skill-program.anniversary:pianchong.obtain", GuoZhaoColorDrawReason = "skill-program.anniversary:pianchong.draw", GuoZhaoZunweiDrawReason = "skill-program.anniversary:zunwei.draw";
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryGuoZhaoHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginGuoZhao(f, op);
    internal CardConversionSource GuoZhaoSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int GuoZhaoOwnerStart(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().LastOrDefault(e => e.ActorSeat == owner)?.TurnNumber ?? 0;
    internal IEnumerable<AnniversaryGuoZhaoLeaseEvent> GuoZhaoLeases(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoLeaseEvent>().Where(e => e.Source.OwnerSeat == owner && e.OwnerStartTurn == GuoZhaoOwnerStart(owner) && e.Turn <= _coreActualTurnProgressTurnNumberAccess._turnNumber);
    internal AnniversaryGuoZhaoLoss? CaptureAnniversaryGuoZhaoLoss(Card card, CardLocation from, CardLocation to, int sequence)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor) || from.OwnerSeat is not { } owner || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || from == to)
            return null;
        var leases = GuoZhaoLeases(owner).ToArray();
        if (leases.Length == 0 || global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card))is not { } red)
            return null;
        return new(owner, red, GuoZhaoOwnerStart(owner), leases.Max(l => l.FrameId), sequence);
    }

    internal IReadOnlyList<AnniversaryGuoZhaoBenefit> GuoZhaoLostBenefits(int owner, string instance, string hash, CardMovementBatchContext? batch)
    {
        if (batch is null)
            return Array.AsReadOnly(Array.Empty<AnniversaryGuoZhaoBenefit>());
        var result = new List<AnniversaryGuoZhaoBenefit>();
        foreach (var lease in _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoLeaseEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillInstanceId == instance && e.Hash == hash))
        {
            var count = batch.Movements.Where(m => m.AnniversaryGuoZhaoLoss is { } loss && loss.OwnerSeat == owner && loss.Sequence == m.Sequence && loss.OwnerStartTurn == lease.OwnerStartTurn && loss.LastLeaseFrameId >= lease.FrameId && loss.IsRed == lease.LoseRed && lease.Turn <= m.TurnNumber && m.From.OwnerSeat == owner && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)).Select(m => m.CardId).Distinct().Count();
            if (count > 0)
                result.Add(new(lease.FrameId, count, !lease.LoseRed));
        }

        return Array.AsReadOnly(result.OrderBy(b => b.LeaseId).ToArray());
    }

    internal bool GuoZhaoOptionUsed(int owner, string option) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoOptionPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Option == option);
    internal bool GuoZhaoHasAvailableOption(int owner) => new[]
    {
        "hand",
        "equipment",
        "hp"
    }.Any(o => !GuoZhaoOptionUsed(owner, o));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 44300, SkillProgramEffectOp.ResolveAnniversaryGuoZhaoZunwei)]
    internal bool CanStartGuoZhaoZunwei(ProgramActivationLegalityContext q) => GuoZhaoHasAvailableOption(q.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != q.OwnerSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 44300)]
    internal bool QualifyGuoZhao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is not [{ Op: SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor }] || GuoZhaoLostBenefits(c.OwnerSeat, c.SkillInstanceId, c.GameplayHash, context.MovementBatch).Count > 0;
    internal SkillProgramStepOutcome BeginGuoZhao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryGuoZhao is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !GuoZhaoOriginal(f))
            throw new InvalidOperationException($"Guo Zhao needs her exact original unstarted program ({(f.Id)}/{(f.SkillId)}/{(f.TriggerId)}/{(f.ActivationId)}/{(f.InstructionIndex)}).");
        var r = new AnniversaryGuoZhaoReceipt(GuoZhaoSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressTurnNumberAccess._turnNumber, op == SkillProgramEffectOp.ResolveAnniversaryGuoZhaoZunwei ? AnniversaryGuoZhaoStage.OptionChoice : AnniversaryGuoZhaoStage.GainChildren);
        if (op == SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor)
            r = r with
            {
                Benefits = GuoZhaoLostBenefits(f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, f.WindowContext!.MovementBatch)
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryGuoZhao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoStartedEvent(f.Id, r.Source, r.Hash, op, r.ParentId, r.Turn));
        if (op == SkillProgramEffectOp.ResolveAnniversaryGuoZhaoPianchong)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var selected = new List<AnniversaryGuoZhaoGain>();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var red in new[]
            {
                true,
                false
            }

            )
            {
                var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(c => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c)) == red);
                if (card is null)
                    continue;
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(GuoZhaoObtainReason));
                var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.CardId == card.Id && m.Sequence > before && m.Reason.Value == GuoZhaoObtainReason);
                selected.Add(new(card.Id, red, move.Sequence));
            }

            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryGuoZhao = r with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Gains = selected } });
            ContinueGuoZhao(f.Id);
        }
        else if (op == SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor)
            BeginGuoZhaoColorBenefit(f);
        else
            PublishGuoZhao(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> GuoZhaoChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryGuoZhao!;
        var result = new List<PromptChoice>();
        PromptChoice Choice(string option, string label, int? target = null) => new(new($"guo-zhao.{(f.Id)}.{(r.Stage)}.{(option)}.{(target)}"), label, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "anniversary-guo-zhao", ["option"] = option });
        if (r.Stage == AnniversaryGuoZhaoStage.ColorChoice)
        {
            result.Add(Choice("lose-red", "失去红色牌时摸黑色牌"));
            result.Add(Choice("lose-black", "失去黑色牌时摸红色牌"));
        }

        if (r.Stage == AnniversaryGuoZhaoStage.OptionChoice)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
            {
                if (!GuoZhaoOptionUsed(f.OwnerSeat, "hand"))
                    result.Add(Choice("hand", $"向{(target.Name)}看齐手牌数（最多摸五张）", target.Seat));
                if (!GuoZhaoOptionUsed(f.OwnerSeat, "equipment"))
                    result.Add(Choice("equipment", $"随机使用装备至与{(target.Name)}相同", target.Seat));
                if (!GuoZhaoOptionUsed(f.OwnerSeat, "hp") && (target.HasHpValue || target.HappyQiaoHealth is not null))
                    result.Add(Choice("hp", $"回复体力至与{(target.Name)}相同", target.Seat));
            }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishGuoZhao(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishGuoZhao(f);
            return;
        }

        var choices = GuoZhaoChoices(f);
        if (choices.Count == 0)
        {
            FinishGuoZhao(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, rText(f), f.OwnerSeat);
        static string rText(ProgramSkillFrame frame) => frame.AnniversaryGuoZhao!.Stage == AnniversaryGuoZhaoStage.ColorChoice ? "偏宠：选择失牌与摸牌颜色，直到你的下个实际回合开始。" : "尊位：选择一名其他角色及一项本局尚未移除的效果。";
    }

    [ProgramAnswer("anniversary-guo-zhao")]
    internal void AnswerGuoZhao(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryGuoZhao: { } r } f || !ValidGuoZhao(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], GuoZhaoChoices(f).Where(x => x.Id == c.Id).ToArray()))
            throw new InvalidOperationException("Guo Zhao lost her exact original color or permanent-option menu.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var option = c.Parameters["option"];
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishGuoZhao(f);
            return;
        }

        if (r.Stage == AnniversaryGuoZhaoStage.ColorChoice)
        {
            if (option is not ("lose-red" or "lose-black") || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryGuoZhaoLeaseEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Pianchong cannot issue the same color lease twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoLeaseEvent(f.Id, r.Source, r.Hash, r.Turn, GuoZhaoOwnerStart(f.OwnerSeat), option == "lose-red"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, AnniversaryGuoZhao = r with { Option = option, Paid = true } });
            FinishGuoZhao(f);
            return;
        }

        var target = c.Targets.Single();
        if (r.Stage != AnniversaryGuoZhaoStage.OptionChoice || option is not ("hand" or "equipment" or "hp") || GuoZhaoOptionUsed(f.OwnerSeat, option) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == f.OwnerSeat)
            throw new InvalidOperationException("Zunwei cannot repeat an option or replace its living other participant.");
        var amount = option == "hand" ? Math.Min(5, Math.Max(0, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count)) : option == "equipment" ? _corePhysicalOwnedCardQueriesOperations.GetEquipment2(target).Count : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, AnniversaryGuoZhao = r with { Paid = true, Target = target, Option = option, Amount = amount, Stage = option == "hand" ? AnniversaryGuoZhaoStage.Drawing : option == "equipment" ? AnniversaryGuoZhaoStage.EquipmentNext : AnniversaryGuoZhaoStage.RecoveryChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoOptionPaidEvent(f.Id, r.Source, r.Hash, target, option, amount));
        if (option == "hand")
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, amount, new(GuoZhaoZunweiDrawReason));
        else
            ContinueGuoZhao(f.Id);
    }

    internal bool DrainGuoZhao(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueGuoZhao(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryGuoZhao: { } r } f && f.Id == id)
        {
            if (!ValidGuoZhao(f))
                throw new InvalidOperationException($"Guo Zhao lost original candidate, paid option or native child return ({(id)}/{(r.Stage)}).");
            if (r.Stage is AnniversaryGuoZhaoStage.Drawing or AnniversaryGuoZhaoStage.NativeEquipment or AnniversaryGuoZhaoStage.Completed)
                return;
            if (r.Stage is AnniversaryGuoZhaoStage.ColorChoice or AnniversaryGuoZhaoStage.OptionChoice)
            {
                PublishGuoZhao(f);
                return;
            }

            if (DrainGuoZhao(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.AnniversaryGuoZhao!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == AnniversaryGuoZhaoStage.GainChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryGuoZhao = r with { Stage = AnniversaryGuoZhaoStage.ColorChoice } });
                PublishGuoZhao(f);
                return;
            }

            if (r.Stage == AnniversaryGuoZhaoStage.DrawChildren)
            {
                if (r.Op == SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryGuoZhao = r with { Cursor = r.Cursor + 1, Draw = null } });
                    BeginGuoZhaoColorBenefit(f);
                    return;
                }

                FinishGuoZhao(f);
                return;
            }

            if (r.Stage == AnniversaryGuoZhaoStage.RecoveryChildren)
            {
                if (r.Cursor == 0)
                {
                    if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoQuery(f, r.Target, "guo-zhao-zunwei-target"))
                        return;
                    f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    var targetHp = _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(r.Target) ? _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoQueryValue(f, r.Target, "guo-zhao-zunwei-target") : _coreHealthQueriesOperations.UnambiguousHp(_coreMatchParticipantsPlayersAccess._players[r.Target]) ?? 0;
                    if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoQuery(f, f.OwnerSeat, "guo-zhao-zunwei-owner"))
                        return;
                    f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    var ownHp = _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(f.OwnerSeat) ? _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoQueryValue(f, f.OwnerSeat, "guo-zhao-zunwei-owner") : _coreHealthQueriesOperations.UnambiguousHp(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ?? 0;
                    var amount = Math.Max(0, targetHp - ownHp);
                    var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryGuoZhao = r with { Amount = amount, Cursor = 1, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoRecoveryIssuedEvent(f.Id, amount, ownHp, targetHp));
                    if (amount > 0)
                        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
                    continue;
                }

                FinishGuoZhao(f);
                return;
            }

            if (r.Stage is AnniversaryGuoZhaoStage.EquipmentNext or AnniversaryGuoZhaoStage.EquipmentChildren)
            {
                BeginGuoZhaoEquipment(f);
                return;
            }
        }
    }

    internal void BeginGuoZhaoColorBenefit(ProgramSkillFrame f)
    {
        var r = f.AnniversaryGuoZhao!;
        if (r.Cursor >= r.Benefits.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishGuoZhao(f);
            return;
        }

        var benefit = r.Benefits[r.Cursor];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryGuoZhao = r with { Stage = AnniversaryGuoZhaoStage.Drawing, DrawRed = benefit.DrawRed, Amount = benefit.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoBenefitIssuedEvent(f.Id, r.Cursor, benefit.LeaseId, benefit.Count, benefit.DrawRed));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, benefit.Count, new(GuoZhaoColorDrawReason));
    }

    internal bool TryChooseAnniversaryGuoZhaoDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> drawPile, out Card? material)
    {
        material = null;
        if (reason.Value != GuoZhaoColorDrawReason || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryGuoZhao: { Op: SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor, Stage: AnniversaryGuoZhaoStage.Drawing, DrawRed: { } red } r, NativeDraw: { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, Operation: SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor } paid } f)
            return false;
        if (!ValidGuoZhao(f) || f.OwnerSeat != player.Seat || paid.Recipients is not [var recipient] || recipient != player.Seat || paid.RequestedCount != r.Amount || paid.Reason != reason)
            throw new InvalidOperationException("Pianchong colored draw lost its exact paid native request.");
        var candidates = drawPile.Where(c => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(player, c)) == red);
        material = _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.DrawFromBottom) ? candidates.FirstOrDefault() : candidates.LastOrDefault();
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor)]
    internal void ReturnGuoZhaoColoredDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnGuoZhaoDraw(f, outcome);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryGuoZhaoZunwei)]
    internal void ReturnGuoZhaoZunweiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnGuoZhaoDraw(f, outcome);
    internal void ReturnGuoZhaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryGuoZhao!;
        if (!ValidGuoZhao(f) || r.Stage != AnniversaryGuoZhaoStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Amount || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Guo Zhao lost her exact once-only native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryGuoZhao = r with { Stage = AnniversaryGuoZhaoStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoDrawReturnedEvent(f.Id, r.Cursor, r.Amount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueGuoZhao(f.Id);
    }

    internal void FinishGuoZhao(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, PendingMovementContinuation = null, AnniversaryGuoZhao = f.AnniversaryGuoZhao!with { Stage = AnniversaryGuoZhaoStage.Completed } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 44300)]
    internal bool ResumeGuoZhao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryGuoZhao: { Stage: not AnniversaryGuoZhaoStage.Completed } } f || f.Id != id)
            return false;
        ContinueGuoZhao(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 44300)]
    internal bool ReturnGuoZhaoMovement(ProgramSkillFrame f) => f.AnniversaryGuoZhao is not null && ResumeGuoZhao(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryGuoZhaoPianchong, SkillProgramEffectOp.ResolveAnniversaryGuoZhaoZunwei)]
    internal PromptChoice ChooseGuoZhao(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") == "hand") ?? p.Choices[0];
}
