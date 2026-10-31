#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanHappyGodRuntime
{
    internal const string HappyBoneEnter = "skill-program.happy:zigu.bone-enter", HappyBoneReplace = "skill-program.happy:zigu.bone-replace", HappyZiguGain = "skill-program.happy:zigu.damage-card";
    internal static CardConversionSource HappyWeiYanSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IHappyWeiYanHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginHappyWeiYan(frame, operation);
    bool global::CardGame.Core.IHappyWeiYanTargetQuery.Allows(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && owner != target && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && HappyBoneAvailableSlots(target).Any();
    internal HappyWeiYanBone? HappyBoneRecord(int id) => _coreMatchParticipantsPlayersAccess._players.SelectMany(p => p.HappyWeiYanBones).Where(b => b.CardId == id).OrderByDescending(b => b.Sequence).FirstOrDefault();
    internal HappyWeiYanBone? CurrentHappyBone(int cardId)
    {
        if (cardId <= 0 || HappyBoneRecord(cardId)is null)
            return null;
        foreach (var holder in _coreMatchParticipantsPlayersAccess._players)
        {
            var location = CardLocation.Equipment(holder.Seat);
            if (_cardZonesOperations._cardZones.Contains(location, cardId))
                return HappyBoneAt(cardId, location, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        }

        return null;
    }

    internal EquipmentSlot? HappyBoneSlot(Card card) => CurrentHappyBone(card.Id)is { } bone && bone.PrintedKind == card.Kind ? bone.Slot : null;
    internal bool IsHappyWeiYanBone(int id) => CurrentHappyBone(id)is not null;
    internal int? HappyBoneSourceOwner(int cardId) => IsHappyWeiYanBone(cardId) ? HappyBoneRecord(cardId)!.Source.OwnerSeat : null;
    internal HappyWeiYanBone? HappyBoneAt(int cardId, CardLocation location, int sequence)
    {
        var bone = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => p.HappyWeiYanBones).Where(b => b.CardId == cardId && b.Sequence <= sequence).OrderByDescending(b => b.Sequence).FirstOrDefault();
        if (bone is null || location.Zone != CardZoneKind.Equipment || bone.Sequence <= 0 || bone.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(bone.Source.SkillId).Program!.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanBonePlacedEvent>().Count(e => e.FrameId == bone.FrameId && e.Source == bone.Source && e.Holder == bone.Holder && e.Slot == bone.Slot && e.After == bone.Sequence) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == bone.Sequence)is not { } entry || entry.CardId != cardId || entry.CardKind != bone.PrintedKind || entry.From != CardLocation.Hand(bone.Source.OwnerSeat) || entry.To != CardLocation.Equipment(bone.Holder) || entry.Reason.Value != HappyBoneEnter)
            return null;
        var current = entry.To;
        foreach (var move in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardId == cardId && m.Sequence > entry.Sequence && m.Sequence <= sequence))
        {
            if (move.From != current || move.From.Zone != CardZoneKind.Equipment || move.To.Zone != CardZoneKind.Equipment)
                return null;
            current = move.To;
        }

        return current == location ? bone : null;
    }

    internal bool HasExactHappyBonePlacement(Card card, int holder)
    {
        return card.Id > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) && _cardZonesOperations._cardZones.Contains(CardLocation.Equipment(holder), card.Id) && HappyBoneAt(card.Id, CardLocation.Equipment(holder), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)is { } bone && bone.PrintedKind == card.Kind;
    }

    internal IEnumerable<EquipmentSlot> HappyBoneAvailableSlots(int holder) => Enum.GetValues<EquipmentSlot>().Where(s => !_coreMatchParticipantsPlayersAccess._players[holder].EquipmentAreaAbolished && _coreMatchParticipantsPlayersAccess._players[holder].EquipmentSlotCapacity(s) > 0);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 42300)]
    internal void ObserveHappyWeiYanFirstEquipment(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent declared || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(declared.CardKind) || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveHappyWeiYanZigu) || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber <= 0 || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { Action: { Type: CardActionType.Use } action } use || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanFirstEquipmentEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.Actor == use.SourceSeat))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanFirstEquipmentEvent(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, use.SourceSeat, use.Id, action.ActionId));
    }

    internal CardUseFrame? HappyZiguFirstEquipment(int owner, ProgramSkillWindowContext context) => context.CardUse is { } info && _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } use && use.SourceSeat == owner && action.ActionId == info.CardActionId && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanFirstEquipmentEvent>().Count(e => e == new HappyWeiYanFirstEquipmentEvent(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, owner, use.Id, action.ActionId)) == 1 ? use : null;
    internal bool TryHappyWeiYanEquipmentCommitted(long id, Card equipment)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } action } use || !(_coreNativeSkillContextQueriesOperations.HasProgramSkill(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], HappyWeiYanComposition.Zigu) && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanFirstEquipmentEvent>().Any(e => e == new HappyWeiYanFirstEquipmentEvent(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, use.SourceSeat, id, action.ActionId))) && !(HasHappyWeiYanEzhiCapability && HappyBones(use.SourceSeat).Count > 0 && action.PhysicalCards.Count > 0) && use.HappyWeiYanCancellation is null)
            return false;
        var continuation = new ProgramSimpleCardContinuation(equipment.Id, SimpleCardUseEffect.EquipmentPlacement);
        return _coreCardUseLifecycleOperations.TryBeginCommittedCardUse(id, ProgramCardContinuation.CommittedSimpleCard, simple: continuation) || ContinueHappyWeiYanIneffectiveSimple(id, continuation);
    }
    internal int HappyZiguCounterpart(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.SourceSeat is not { } source || context.TargetSeat is not { } target || source == target || source != owner && target != owner || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.SourceSeat != source || attack.TargetSeat != target)
            return -1;
        return source == owner ? target : source;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42300, SkillProgramEffectOp.ResolveHappyWeiYanZigu)]
    internal bool HappyZiguEligible(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Id == "first-equipment")
            return HappyZiguFirstEquipment(candidate.OwnerSeat, context)is not null;
        var counterpart = HappyZiguCounterpart(candidate.OwnerSeat, context);
        return counterpart >= 0 && _coreMatchParticipantsPlayersAccess._players[counterpart].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0 && HappyBoneAvailableSlots(counterpart).Any();
    }

    internal SkillProgramStepOutcome BeginHappyWeiYan(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (operation != SkillProgramEffectOp.ResolveHappyWeiYanZigu)
            return BeginHappyWeiYanAdditional(frame, operation);
        if (frame.HappyWeiYan is not null || frame.InstructionIndex != 1 || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Happy Wei Yan requires his original unpaid native instruction.");
        var use = frame.TriggerId == "first-equipment" ? HappyZiguFirstEquipment(frame.OwnerSeat, context) : null;
        var target = use is not null ? frame.OwnerSeat : HappyZiguCounterpart(frame.OwnerSeat, context);
        if (target < 0)
            throw new InvalidOperationException("Zigu lost the actual sourced damage counterpart.");
        var receipt = new HappyWeiYanReceipt(HappyWeiYanSource(frame), frame.GameplayHash, operation, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
        {
            Target = target,
            UseId = use?.Id ?? 0,
            ActionId = use?.Action!.ActionId ?? 0,
            Stage = use is null ? HappyWeiYanStage.Hand : HappyWeiYanStage.DamageTarget,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, operation, receipt.ParentId, receipt.ActualTurn, target, receipt.UseId, receipt.ActionId));
        ContinueHappyWeiYan(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainHappyWeiYan(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 42300)]
    internal bool ContinueHappyWeiYan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyWeiYan: not null } frame || frame.Id != id)
            return false;
        if (frame.HappyWeiYan.Operation == SkillProgramEffectOp.ResolveHappyWeiYanEzhi)
            return ContinueHappyEzhi(frame);
        if (frame.HappyWeiYan.Operation == SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn)
            return ContinueHappyBoneReturn(frame);
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            var receipt = frame.HappyWeiYan!;
            if (receipt.Stage is HappyWeiYanStage.Replacing or HappyWeiYanStage.Placing or HappyWeiYanStage.GainChildren)
            {
                if (DrainHappyWeiYan(frame))
                    return true;
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                receipt = frame.HappyWeiYan!;
                var stage = receipt.Stage == HappyWeiYanStage.Replacing ? HappyWeiYanStage.Slot : receipt.Operation == SkillProgramEffectOp.ResolveHappyWeiYanGujing ? HappyWeiYanStage.GujingSelection : HappyWeiYanStage.Finished;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, HappyWeiYan = receipt = receipt with { Stage = stage } });
                if (stage == HappyWeiYanStage.Slot)
                {
                    PlaceHappyBone(frame);
                    return true;
                }
            }

            if (receipt.Operation == SkillProgramEffectOp.ResolveHappyWeiYanGujing && receipt.Stage == HappyWeiYanStage.GujingDiscardChildren)
                return ContinueHappyGujingPayment(frame);
            if (receipt.Stage == HappyWeiYanStage.Damage)
            {
                if (frame.AttackAttempt is not null)
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt = receipt with { Stage = HappyWeiYanStage.DamageCard } });
            }

            if (receipt.Stage == HappyWeiYanStage.DamageCard)
            {
                var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(c.Kind) || global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(c.Kind) || c.Kind == CardKind.Lightning).ToArray();
                if (pool.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
                {
                    FinishHappyWeiYan(frame);
                    return true;
                }

                var card = pool[_randomOperations._random.Next(pool.Length)];
                PayHappyWeiYanMove(frame, HappyWeiYanStage.GainChildren, card, CardLocation.DrawPile, CardLocation.Hand(frame.OwnerSeat), HappyZiguGain);
                continue;
            }

            if (receipt.Stage == HappyWeiYanStage.Finished || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishHappyWeiYan(frame);
                return true;
            }

            var choices = HappyWeiYanChoices(frame);
            if (choices.Count == 0)
            {
                FinishHappyWeiYan(frame);
                return true;
            }

            var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                TargetSeat = receipt.Target,
                Choices = Array.AsReadOnly(choices.ToArray()),
                SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return true;
    }

    internal IReadOnlyList<PromptChoice> HappyWeiYanChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyWeiYan!;
        if (receipt.Operation is SkillProgramEffectOp.ResolveHappyWeiYanEzhi or SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn || receipt.Stage == HappyWeiYanStage.GujingSelection)
            return HappyWeiYanAdditionalChoices(frame);
        PromptChoice Choice(string option, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"happy-wei-yan.{(frame.Id)}.{(receipt.Stage)}.{(option)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "happy-wei-yan", ["option"] = option, ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) });
        if (receipt.Stage == HappyWeiYanStage.Hand)
            return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(c => Choice("card:" + c.Id, "置入“骨”：" + c.DisplayName, [c.Id])).ToArray();
        if (receipt.Stage == HappyWeiYanStage.Slot)
            return HappyBoneAvailableSlots(receipt.Target).Select(s => Choice("slot:" + s, EquipmentCatalog.GetSlotName(s), targets: [receipt.Target])).ToArray();
        if (receipt.Stage == HappyWeiYanStage.DamageTarget)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, p.Seat)).Select(p => Choice("target:" + p.Seat, "对“" + p.Name + "”造成1点伤害", targets: [p.Seat])).ToArray();
        return[];
    }

    [ProgramAnswer("happy-wei-yan")]
    internal void AnswerHappyWeiYan(PromptChoice chosen)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Happy Wei Yan lost his original choice frame.");
        if (!HappyWeiYanChoices(frame).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [chosen])))
            throw new InvalidOperationException("Happy Wei Yan requires the exact current published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var receipt = frame.HappyWeiYan!;
        if (receipt.Operation is SkillProgramEffectOp.ResolveHappyWeiYanEzhi or SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn || receipt.Stage == HappyWeiYanStage.GujingSelection)
        {
            AnswerHappyWeiYanAdditional(frame, chosen);
            return;
        }

        if (receipt.Stage == HappyWeiYanStage.Hand)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyWeiYan = receipt with { CardId = chosen.Cards.Single(), Stage = HappyWeiYanStage.Slot } });
            ContinueHappyWeiYan(frame.Id);
            return;
        }

        if (receipt.Stage == HappyWeiYanStage.Slot)
        {
            var slot = Enum.Parse<EquipmentSlot>(chosen.Parameters["option"][5..]);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Slot = slot } });
            var occupied = _corePhysicalOwnedCardQueriesOperations.GetEquipment2(receipt.Target).Where(c => (HappyBoneSlot(c) ?? _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) == slot).ToArray();
            if (occupied.Length >= _coreMatchParticipantsPlayersAccess._players[receipt.Target].EquipmentSlotCapacity(slot))
            {
                PayHappyWeiYanMove(frame, HappyWeiYanStage.Replacing, occupied[0], CardLocation.Equipment(receipt.Target), CardLocation.DiscardPile, HappyBoneReplace);
                ContinueHappyWeiYan(frame.Id);
                return;
            }

            PlaceHappyBone(frame);
            return;
        }

        var target = chosen.Targets.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Stage = HappyWeiYanStage.Damage, Target = target } });
        _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, 1);
    }

    internal void PlaceHappyBone(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyWeiYan!;
        if (receipt.Slot is not { } slot || !_coreMatchParticipantsPlayersAccess._players[receipt.Target].IsAlive || !HappyBoneAvailableSlots(receipt.Target).Contains(slot) || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Any(c => c.Id == receipt.CardId))
        {
            FinishHappyWeiYan(frame);
            return;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(receipt.CardId);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null), HappyWeiYan = receipt with { Stage = HappyWeiYanStage.Placing, Before = before, After = before, Paid = [] } });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(frame.OwnerSeat), CardLocation.Equipment(receipt.Target), new(HappyBoneEnter), movement =>
        {
            if (movement.To != CardLocation.Equipment(receipt.Target))
                throw new InvalidOperationException("A bone placement must enter its selected real equipment slot.");
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            owner.HappyWeiYanBones = Array.AsReadOnly(owner.HappyWeiYanBones.Append(new(card.Id, card.Kind, receipt.Target, slot, receipt.Source, receipt.GameplayHash, frame.Id, movement.Sequence)).ToArray());
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanBonePlacedEvent(frame.Id, receipt.Source, receipt.Target, slot, before, movement.Sequence));
            SaveHappyWeiYanMove(frame.Id, HappyWeiYanStage.Placing, before);
        }, disclosure: CardMovementDisclosure.FaceDown);
        ContinueHappyWeiYan(frame.Id);
    }

    internal void PayHappyWeiYanMove(ProgramSkillFrame frame, HappyWeiYanStage stage, Card card, CardLocation from, CardLocation to, string reason)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null), HappyWeiYan = frame.HappyWeiYan!with { Stage = stage, Before = before, After = before, Paid = [] } });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, new(reason), _ => SaveHappyWeiYanMove(frame.Id, stage, before));
    }

    internal void SaveHappyWeiYanMove(long id, HappyWeiYanStage stage, int before)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyWeiYan = frame.HappyWeiYan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanMovementEvent(id, stage, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid.Length));
    }

    internal void FinishHappyWeiYan(ProgramSkillFrame frame)
    {
        if (frame.PendingMovementContinuation is not null || frame.AttackAttempt is not null)
            throw new InvalidOperationException("Happy Wei Yan cannot retire a paid native child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = frame.HappyWeiYan!with { Stage = HappyWeiYanStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanCompletedEvent(frame.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42300)]
    internal bool ReturnHappyWeiYanMovement(ProgramSkillFrame frame) => frame.HappyWeiYan is not null && ContinueHappyWeiYan(frame.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyWeiYanZigu)]
    internal PromptChoice HappyZiguAi(PendingDecision decision) => decision.Choices.First();
}
