#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDyingProgramsRuntime
{
    internal void RevealProgramUniqueRankForDying(ProgramSkillFrame frame, CardZoneKind zone, int rescueHp)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("The unique-rank rescue has no dying occurrence.");
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dying.FrameId || dying.VictimSeat != active.OwnerSeat || dying.ResponderSeat != active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Hp > 0)
            throw new InvalidOperationException("The unique-rank rescue lost its dying owner.");
        if (!_corePhysicalDrawFacadesOperations.EnsureDrawPile())
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var location = new CardLocation(zone, owner.Seat);
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
        var unique = _cardZonesOperations._cardZones.CardsAt(location).All(existing => existing.Rank != card.Rank);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, unique ? location : CardLocation.DiscardPile, new CardMoveReason(unique ? "skill-program.dying-rank.retain" : "skill-program.dying-rank.duplicate"));
        if (unique)
            owner.Hp = Math.Min(owner.MaxHp, rescueHp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramUniqueRankDyingResolvedEvent(dying.FrameId, active.SkillId, owner.Seat, zone, card.Id, card.Rank, unique, Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(location).Select(item => item.Id).ToArray())));
        _coreMatchLogOperations.AddLog("SkillTriggered", unique ? $"{(owner.Name)} 亮出点数 {(card.Rank)}，与已有牌不同，回复至 {(owner.Hp)} 点体力。" : $"{(owner.Name)} 亮出重复点数 {(card.Rank)}，该牌置入弃牌堆并继续濒死结算。", owner.Seat);
    }

    internal void UseProgramBoundCardAsDyingAlcohol(ProgramSkillFrame frame, string sourceBind, CardMoveReason reason)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("The configured rescue has no dying occurrence.");
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.DyingResponse } context || context.ParentFrameId != dying.FrameId || context.TargetSeat != dying.VictimSeat || dying.ResponderSeat != active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0)
            throw new InvalidOperationException("The configured rescue lost its responder or dying victim.");
        if (!_coreAlcoholIdentityPoliciesOperations.CanUseProgramDyingAlcohol(active, SkillProgramEffectOp.UseBoundCardAsDyingAlcohol))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "当前实际回合禁止该濒死角色使用酒，未支付牌。 ");
            return;
        }

        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(active, sourceBind);
        if (binding.CardIds.Count != 1 || binding.SourceLocations.Count != 1)
            throw new InvalidOperationException("A configured dying rescue requires exactly one bound card.");
        var location = binding.SourceLocations[0];
        if (location.OwnerSeat != active.OwnerSeat || location.Zone is not (CardZoneKind.WoodenOxGrain or CardZoneKind.BuquWound or CardZoneKind.Authority or CardZoneKind.Chunlao) || _cardZonesOperations._cardZones.GetLocation(binding.CardIds[0]) != location)
            throw new InvalidOperationException("The configured rescue card left its owner pile.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == binding.CardIds[0]);
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        var source = new CardConversionSource(active.SkillId, active.TriggerId ?? active.ActivationId, active.OwnerSeat, active.SkillInstanceId);
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, victim.Seat, [victim.Seat], CardKind.Alcohol, conversionSource: source);
        _coreBoundDyingAlcoholReturnsOperations.IssueBoundDyingAlcoholReturn(resolutionId, active, dying, card, location, reason);
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.Processing, reason);
        _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
        var recoveryAmount = _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(resolutionId, victim.Seat, victim.Seat, 1);
        if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(resolutionId, victim.Seat, victim.Seat, recoveryAmount, new(RecoveryAttemptProducer.DyingBoundAlcohol, CardId: card.Id, CardKind: CardKind.Alcohol, MoveReason: reason, ProgramFrameId: active.Id, DyingFrameId: dying.FrameId, SkillId: active.SkillId, SkillOwnerSeat: active.OwnerSeat)))
        {
            _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(resolutionId, PostEventContinuation.RecoveryProducer);
            return;
        }

        var recoveryFrameId = _coreNativeRecoveryFramesOperations.BeginRecovery(resolutionId, victim.Seat, victim.Seat, recoveryAmount);
        try
        {
            victim.Hp = Math.Min(victim.MaxHp, victim.Hp + recoveryAmount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(victim.Seat, victim.Seat, recoveryAmount, victim.Hp));
        }
        finally
        {
            _coreRuntimeOperations.PopResolutionFrame(recoveryFrameId, ResolutionFrameKind.Recovery);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, reason);
        _coreBoundDyingAlcoholReturnsOperations.RecordBoundDyingAlcoholPayment(resolutionId, recoveryAmount);
        _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, card, CardKind.Alcohol);
    }

    internal void UseProgramVirtualDyingAlcohol(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("The configured rescue has no dying occurrence.");
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dying.FrameId || context.TargetSeat != dying.VictimSeat || dying.VictimSeat != active.OwnerSeat || dying.ResponderSeat != active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].Hp > 0)
            throw new InvalidOperationException("The configured rescue lost its dying owner.");
        if (!_coreAlcoholIdentityPoliciesOperations.CanUseProgramDyingAlcohol(active, SkillProgramEffectOp.UseVirtualDyingAlcohol))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "当前实际回合禁止该濒死角色使用酒，未支付成本。 ");
            return;
        }

        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, victim.Seat, 0, CardKind.Alcohol, Array.AsReadOnly(new[] { victim.Seat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>())));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, CardKind.Alcohol, victim.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, Array.AsReadOnly(new[] { victim.Seat })));
        _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
        if (_characterLiuYanOlOperations.TryBeginLiuYanLegacyAlcoholUse(resolutionId, active.Id, dying.FrameId))
            return;
        if (_characterCuiYanOlOperations.TryBeginCuiYanLegacyAlcoholUse(resolutionId, active.Id, dying.FrameId))
            return;
        if (_characterZhuJunOlOperations.TryBeginZhuJunLegacyAlcoholUse(resolutionId, active.Id, dying.FrameId))
            return;
        if (_characterHuangYueyingOlMouOperations.TryBeginMouBingcaiLegacyAlcoholUse(resolutionId, active.Id, dying.FrameId))
            return;
        ResumeProgramVirtualDyingAlcoholRecovery(resolutionId, active.Id, dying.FrameId);
    }

    internal void ResumeProgramVirtualDyingAlcoholRecovery(long resolutionId, long programParentId, long dyingId)
    {
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == resolutionId);
        if (useIndex < 1 || useIndex != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[useIndex] is not CardUseFrame { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0 } use || _resolutionStackOperations._resolutionStack[useIndex - 1] is not ProgramSkillFrame active || active.Id != programParentId || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.FrameId != dyingId || dying.VictimSeat != use.SourceSeat || active.OwnerSeat != use.SourceSeat || active.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != dyingId || context.TargetSeat != use.SourceSeat)
            throw new InvalidOperationException("Legacy virtual Alcohol recovery lost its exact accepted use and dying producer.");
        var victim = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        if (!victim.IsAlive)
        {
            if (!_coreActualTurnLegacyCompletionOperations.FinishActualTurnLegacyDyingAlcohol(resolutionId, active.Id))
                _coreNativeCardUseCompletionOperations.PopFinishedCardUse(resolutionId);
            return;
        }

        var recoveryAmount = _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(resolutionId, victim.Seat, victim.Seat, 1);
        if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(resolutionId, victim.Seat, victim.Seat, recoveryAmount, new(RecoveryAttemptProducer.DyingVirtualAlcohol, CardId: 0, CardKind: CardKind.Alcohol, ProgramFrameId: active.Id, DyingFrameId: dying.FrameId, SkillId: active.SkillId, SkillOwnerSeat: active.OwnerSeat)))
        {
            _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(resolutionId, PostEventContinuation.RecoveryProducer);
            return;
        }

        var recoveryFrameId = _coreNativeRecoveryFramesOperations.BeginRecovery(resolutionId, victim.Seat, victim.Seat, recoveryAmount);
        try
        {
            victim.Hp = Math.Min(victim.MaxHp, victim.Hp + recoveryAmount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(victim.Seat, victim.Seat, recoveryAmount, victim.Hp));
        }
        finally
        {
            _coreRuntimeOperations.PopResolutionFrame(recoveryFrameId, ResolutionFrameKind.Recovery);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDyingRescueEvent(dying.FrameId, active.SkillId, active.OwnerSeat, victim.Seat, 0, recoveryAmount, victim.Hp));
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(skill.Name)}】，视为使用【酒】并回复{(recoveryAmount)}点体力。", active.OwnerSeat, victim.Seat);
        if (!_coreActualTurnLegacyCompletionOperations.FinishActualTurnLegacyDyingAlcohol(resolutionId, active.Id))
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(resolutionId);
    }
}
