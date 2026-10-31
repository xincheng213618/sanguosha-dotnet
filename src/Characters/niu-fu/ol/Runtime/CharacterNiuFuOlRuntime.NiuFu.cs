#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterNiuFuOlRuntime
{
    internal const string ShisuanGiftReason = "skill-program.niu-fu.shisuan.gift", ZonglueTakeReason = "skill-program.niu-fu.zonglue.obtain";
    internal static CardConversionSource NiuFuSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_niu_fu_ol_NiuFu.NiuFuSource(f);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? NiuFuOriginalAttack(long damageId) => _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is { } d ? _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == d.ParentFrameId) switch
    {
        ProgramSkillFrame { AttackAttempt: not null } => _kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(d.ParentFrameId),
        CardUseFrame { CardAttack: not null } or ProgramSkillFrame { CardAttack: not null } or JudgmentFrame { CardAttack: not null } => _kernelCoreMainOperations.CreateCardAttackHandle(d.ParentFrameId),
        _ => null
    } : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27050)]
    internal bool QualifyNiuFu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.ObtainZonglueDamageTargetZones }])
            return IsNiuFuZonglueDamage(candidate.OwnerSeat, c);
        if (trigger.Effects is not [{ Op: SkillProgramEffectOp.ResolveShisuanRecastSourceChoice }])
            return true;
        return c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.TargetSeat == candidate.OwnerSeat && c.Amount > 0 && c.DamageFrameId is { } id && NiuFuOriginalAttack(id)is { DamageWasApplied: true } attack && attack.TargetSeat == c.TargetSeat && attack.DamageAmount == c.Amount;
    }

    SkillProgramStepOutcome global::CardGame.Core.INiuFuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginNiuFu(f, op);
    internal SkillProgramStepOutcome BeginNiuFu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.NiuFu is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { DamageFrameId: { } id, TargetSeat: { } target } c || NiuFuOriginalAttack(id)is not { } attack || !QualifyNiuFu(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("NiuFu requires its exact positive whole-damage candidate and original attack.");
        var shisuan = op == SkillProgramEffectOp.ResolveShisuanRecastSourceChoice;
        var use = shisuan ? null : _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        var physical = shisuan ? Array.Empty<NiuFuEntity>() : attack.PhysicalCards.Select(card => new NiuFuEntity(card.Id, card.Kind, use?.Action?.PhysicalCards.Single(x => x.CardId == card.Id).From ?? CardLocation.Processing)).ToArray();
        var r = new NiuFuReceipt(NiuFuSource(f), f.GameplayHash, op, 1, _coreActualTurnProgressTurnNumberAccess._turnNumber, c.ParentFrameId, id, c.Amount, target, attack.IsSourceLess ? null : attack.SourceSeat, attack.ResolutionId, use?.Action?.ActionId, shisuan ? NiuFuStage.RecastChoice : NiuFuStage.ZoneChoice, physical, []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuFuStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.WindowId, id, r.DamageAmount, target, r.DamageSourceSeat, r.AttackId, r.ActionId, physical.Length));
        for (var index = 0; index < physical.Length; index++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuFuSlashEntityEvent(f.Id, index, physical[index].CardId, physical[index].Kind));
        PublishNiuFu(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> NiuFuChoices(ProgramSkillFrame f)
    {
        var r = f.NiuFu!;
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"niu-fu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('.', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "niu-fu", ["branch"] = branch });
        if (r.Stage == NiuFuStage.RecastChoice)
            return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Select(card => Choice("recast", "重铸【" + card.DisplayName + "】", [card.Id])).ToArray());
        if (r.Stage == NiuFuStage.SourceChoice && r.DamageSourceSeat is { } source && _coreMatchParticipantsPlayersAccess._players[source].IsAlive)
        {
            var choices = new List<PromptChoice>
            {
                Choice("lose-hp", "失去1点体力"),
                Choice("turn-over", "将武将牌翻面")
            };
            if (NiuFuSourceEquipment(f).Count > 0)
                choices.Add(Choice("give-equipment", "交给牛辅一张装备区里的牌"));
            return Array.AsReadOnly(choices.ToArray());
        }

        if (r.Stage == NiuFuStage.SourceGiftChoice)
            return Array.AsReadOnly(NiuFuSourceEquipment(f).Select(c => Choice("gift", "交给牛辅【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).DisplayName + "】", c.Cards)).ToArray());
        if (r.Stage == NiuFuStage.ZoneChoice && r.NextZone < 3 && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.TargetSeat, [global::CardGame.Core.EngineOwners.CharacterNiuFuOlDefinitions.NiuFuZones[r.NextZone]], OwnedCardMoveIntent.Obtain).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "niu-fu", ["branch"] = "obtain" } })).ToArray());
        return[];
    }

    internal IReadOnlyList<PromptChoice> NiuFuSourceEquipment(ProgramSkillFrame f) => f.NiuFu!.DamageSourceSeat is { } source && _coreMatchParticipantsPlayersAccess._players[source].IsAlive ? _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, source, source, [CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: static (_, card) => !card.IsGeneralWeapon) : [];
    internal void PublishNiuFu(ProgramSkillFrame f)
    {
        while (true)
        {
            var r = f.NiuFu!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishNiuFu(f);
                return;
            }

            var choices = NiuFuChoices(f);
            if (choices.Count == 0)
            {
                if (r.Stage == NiuFuStage.RecastChoice)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = r with { Stage = NiuFuStage.SourceChoice } });
                    continue;
                }

                if (r.Stage == NiuFuStage.ZoneChoice && r.NextZone < 3)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = r with { NextZone = r.NextZone + 1 } });
                    continue;
                }

                FinishNiuFu(f);
                return;
            }

            var chooser = r.Stage is NiuFuStage.SourceChoice or NiuFuStage.SourceGiftChoice ? r.DamageSourceSeat!.Value : f.OwnerSeat;
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == NiuFuStage.RecastChoice ? "蓍算：重铸一张牌。" : r.Stage == NiuFuStage.SourceChoice ? "蓍算：选择一项执行。" : r.Stage == NiuFuStage.SourceGiftChoice ? "蓍算：交给牛辅一张自己的装备。" : "纵掠：获得此区域一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                TargetSeat = chooser,
                Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }
    }

    [ProgramAnswer("niu-fu")]
    internal void AnswerNiuFu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { NiuFu: { } r } f || !NiuFuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage is NiuFuStage.SourceChoice or NiuFuStage.SourceGiftChoice ? r.DamageSourceSeat : f.OwnerSeat))
            throw new InvalidOperationException("NiuFu requires its exact chooser's published physical card, opaque slot or source branch.");
        AssertNiuFu(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "give-equipment")
        {
            var source = r.DamageSourceSeat!.Value;
            var player = _coreMatchParticipantsPlayersAccess._players[source];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = r with { Stage = NiuFuStage.SourceGiftChoice, SourceBranch = "gift", SourceHpBefore = player.Hp, SourceWasFaceDown = player.IsFaceDown } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuFuSourceChoiceEvent(f.Id, source, "gift", player.Hp, player.IsFaceDown));
            PublishNiuFu(f);
            return;
        }

        if (branch is "lose-hp" or "turn-over")
        {
            var source = r.DamageSourceSeat!.Value;
            var target = _coreMatchParticipantsPlayersAccess._players[source];
            var stage = branch == "lose-hp" ? NiuFuStage.SourceHpChildren : NiuFuStage.SourceTurnChildren;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = r with { Stage = stage, SourceBranch = branch, SourceHpBefore = target.Hp, SourceWasFaceDown = target.IsFaceDown } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuFuSourceChoiceEvent(f.Id, source, branch, target.Hp, target.IsFaceDown));
            if (branch == "lose-hp")
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, source, 1);
            else
                _coreProgramLifecycleOperations.TurnOverProgramTarget(f.Id, f.OwnerSeat, source);
            ContinueNiuFu(f.Id);
            return;
        }

        var from = branch == "obtain" ? new CardLocation(global::CardGame.Core.EngineOwners.CharacterNiuFuOlDefinitions.NiuFuZones[r.NextZone], r.TargetSeat) : _cardZonesOperations._cardZones.GetLocation(choice.Cards.Single());
        var id = branch == "obtain" ? _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id : choice.Cards.Single();
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        if (branch == "obtain" && (from.Zone == CardZoneKind.Hand ? choice.Cards.Count != 0 : !choice.Cards.SequenceEqual([id])))
            throw new InvalidOperationException("NiuFu cannot replace a public card or expose a foreign hand entity.");
        var stagePaid = branch == "recast" ? NiuFuStage.RecastCostChildren : branch == "gift" ? NiuFuStage.SourceGiftChildren : NiuFuStage.ZoneChildren;
        var reason = branch == "recast" ? CardMoveReasons.RecastDiscard : new CardMoveReason(branch == "gift" ? ShisuanGiftReason : ZonglueTakeReason);
        var destination = branch == "recast" ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (branch == "recast")
            f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, f.OwnerSeat, [id]);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = r with { Stage = stagePaid, Recast = branch == "recast" ? new(id, card.Kind, from, card.IsGeneralWeapon) : r.Recast }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, destination, reason, (branch == "gift" ? _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, from.OwnerSeat!.Value, f.OwnerSeat, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null) : null));
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == id && m.Reason == reason);
        AddNiuFuStep(f.Id, stagePaid, reason.Value, before, 1, card, from, move.To);
        ContinueNiuFu(f.Id);
    }

    internal void AddNiuFuStep(long id, NiuFuStage stage, string reason, long before, int count, Card? card = null, CardLocation? from = null, CardLocation? to = null)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.NiuFu!;
        var step = new NiuFuStep(r.Steps.Count, stage, reason, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count, card?.Id, card?.Kind, from, to);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { NiuFu = r with { Steps = r.Steps.Append(step).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuFuStepIssuedEvent(id, step));
    }

    internal bool DrainNiuFu(long id, bool movement) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    internal void ContinueNiuFu(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { NiuFu: { } r } f && f.Id == id)
        {
            AssertNiuFu(f);
            if (r.Stage is NiuFuStage.RecastChoice or NiuFuStage.SourceChoice or NiuFuStage.SourceGiftChoice or NiuFuStage.ZoneChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishNiuFu(f);
                return;
            }

            if (DrainNiuFu(id, f.PendingMovementContinuation is not null))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                if (r.Stage == NiuFuStage.RecastCostChildren)
                    f = _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, 0);
                FinishNiuFu(f);
                return;
            }

            if (r.Stage == NiuFuStage.RecastCostChildren)
            {
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = f.NiuFu!with { Stage = NiuFuStage.RecastDrawChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, CardMoveReasons.RecastDraw).Count;
                AddNiuFuStep(f.Id, NiuFuStage.RecastDrawChildren, CardMoveReasons.RecastDraw.Value, before, drawn);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(f.OwnerSeat, r.Recast!.CardId, r.Recast.Kind, drawn));
                _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), drawn);
                continue;
            }

            if (r.Stage == NiuFuStage.RecastDrawChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = f.NiuFu!with { Stage = NiuFuStage.SourceChoice } });
                PublishNiuFu(f);
                return;
            }

            if (r.Stage == NiuFuStage.ZoneChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = f.NiuFu!with { Stage = NiuFuStage.ZoneChoice, NextZone = r.NextZone + 1 } });
                PublishNiuFu(f);
                return;
            }

            FinishNiuFu(f);
            return;
        }
    }

    internal void FinishNiuFu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuFuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NiuFu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27050)]
    internal bool ResumeNiuFu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { NiuFu: not null } f || f.Id != id)
            return false;
        ContinueNiuFu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27050)]
    internal bool ReturnNiuFuMovement(ProgramSkillFrame f)
    {
        if (f.NiuFu is null)
            return false;
        ContinueNiuFu(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShisuanRecastSourceChoice, SkillProgramEffectOp.ObtainZonglueDamageTargetZones)]
    internal PromptChoice AiNiuFu(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderBy(c => c.Parameters.GetValueOrDefault("branch") == "lose-hp" ? 1 : 0).ThenBy(c => c.Cards is [var id] ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
