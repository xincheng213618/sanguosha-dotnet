#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal void IssueMouBingcaiExtraEffect(ProgramSkillFrame frame)
    {
        if (frame.MouBingcai is not { Paid: true, MatchesDamage: true } r || r.Stage != MouBingcaiStage.DrawChildren || !IsPaidMouBingcai(frame) || frame.WindowContext is not { } context || !TryGetMouBingcaiFirstBasic(context, out var use, out var window) || use.Id != r.UseId || window.Id != r.WindowId || use.Action!.ActionId != r.ActionId || r.MaterialId is not { } material || r.PrintedKind is not { } printed || r.EffectiveKind is not { } effective || r.From is not { } from || use.MouBingcaiRounds is { Started: > 0 })
            throw new InvalidOperationException("Bingcai requires its exact paid recast and original first-basic native Use.");
        var old = use.MouBingcaiRounds;
        if (old is not null && (old.ActionId != r.ActionId || old.Grants.Any(g => g.FrameId == frame.Id || g.Source.OwnerSeat == frame.OwnerSeat)))
            throw new InvalidOperationException("One owner may issue only one Bingcai extra effect for this real Use.");
        ProgramSimpleCardContinuation? simple = window.SimpleContinuation;
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && simple is null)
        {
            if (use.CardKind == CardKind.Peach || use.CardKind == CardKind.Alcohol && (use.DyingResponse is not null || _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use)))
                simple = new(use.CardId, SimpleCardUseEffect.Recovery);
            else if (use.CardKind == CardKind.Alcohol)
                simple = new(use.CardId, SimpleCardUseEffect.Alcohol);
            else
                throw new InvalidOperationException("Bingcai's basic effect lost its actual native continuation.");
        }

        var native = old?.Native ?? new ExtraCardEffectState
        {
            Source = r.Source,
            ProgramFrameId = frame.Id,
            ActionId = r.ActionId,
            PaidCardId = material,
            Simple = simple
        };
        var grant = new MouBingcaiExtraEffectGrant(frame.Id, r.Source, r.GameplayHash, r.WindowId, r.BasicKind, r.RecastInvocationId, material, printed, effective, from, r.BatchId, r.Before, r.After, r.MovementSequence);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MouBingcaiRounds = new(r.ActionId, native) { Grants = (old?.Grants ?? []).Append(grant).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(MouBingcaiIssuedEvent(use.Id, r.ActionId, grant));
    }

    internal static MouBingcaiExtraEffectIssuedEvent MouBingcaiIssuedEvent(long useId, long actionId, MouBingcaiExtraEffectGrant g) => global::CardGame.Core.RuntimeSkills.RuntimeRules_huang_yueying_ol_mou_MouHuangYueyingBingcai_Rounds.MouBingcaiIssuedEvent(useId, actionId, g);
    internal bool HasUnstartedMouBingcaiExtraEffect(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouBingcaiRounds: { } r, Action: { Type: CardActionType.Use } a } use && r.ActionId == a.ActionId && r.Started < r.Grants.Count && _coreMatchOutcomeWinnerAccess._winner == Winner.None && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use);
    internal bool IsMouBingcaiExtraRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouBingcaiRounds is { Active: true };
    internal void CaptureMouBingcaiExtraEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouBingcaiRounds: { Started: 0 } r, CardAttack: { } original } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(use.Id) || r.Native.OriginalAttacks.Any(seed => seed.TargetIndex == use.TargetIndex))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MouBingcaiRounds = r with { Native = r.Native with { OriginalAttacks = r.Native.OriginalAttacks.Append(new(use.TargetIndex, original)).ToArray(), Fangtian = current.Continuations.FangtianHalberd ?? r.Native.Fangtian } } });
    }

    internal void CompleteMouBingcaiExtraEffectRound(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouBingcaiRounds: { Active: true, Started: > 0 } r, Action: { } a } use)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || use.NativeDraw is not null || use.NativeCardEffectDraw is not null || use.WumiaoFlood is not null || use.GuanduCardEffect is not null || use.DragonBoatCardEffect is not null)
            throw new InvalidOperationException("Bingcai cannot finish its native round before the actual children return.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouBingcaiRounds = r with { Active = false } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiExtraEffectRoundCompletedEvent(id, a.ActionId, r.Grants[r.Started - 1].FrameId, r.Started));
    }

    internal bool TryBeginMouBingcaiExtraEffectRound(long id, bool afterAttack)
    {
        if (!HasUnstartedMouBingcaiExtraEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouBingcaiRounds: { } r, Action: { } action } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id) || _characterSunQuanOlMagicOperations.HasUnstartedMoQuanyuExtraRound(id) || _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || r.Active)
            throw new InvalidOperationException("Bingcai must await the previous whole native effect and its children.");
        var previous = _characterFeiYiOlOperations.NativeExtraEffectState(use);
        var grant = r.Grants[r.Started];
        var native = r.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? r.Native.MaterialClaims
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { MouBingcaiRounds = r with { Started = r.Started + 1, Active = true, Native = native }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, VirtualBasicEffectApplied = false, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouBingcaiExtraEffectRoundStartedEvent(id, action.ActionId, grant.FrameId, r.Started + 1));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
        {
            var first = native.OriginalAttacks.OrderBy(seed => seed.TargetIndex).FirstOrDefault(seed => _coreMatchParticipantsPlayersAccess._players[seed.Attack.TargetSeat].IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, seed.Attack.TargetSeat));
            if (first is null)
            {
                CompleteMouBingcaiExtraEffectRound(id);
                return TryBeginMouBingcaiExtraEffectRound(id, afterAttack);
            }

            var prepared = use.PreparedTargetAttacks?.Select((state, index) => native.OriginalAttacks.SingleOrDefault(seed => seed.TargetIndex == index)?.Attack ?? state).ToArray();
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { TargetIndex = first.TargetIndex, PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared) });
            if (native.Fangtian is { } fang)
            {
                _coreCardAttackStateOperations.UpdateCardContinuations(id, state => state with { FangtianHalberd = fang with { Active = true, CurrentAttackOwnerId = id } });
                _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id);
            }

            _coreCardAttackStateOperations.UpdateCardAttackState(id, _ => global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(first.Attack));
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
            return true;
        }

        if (native.Simple is not { } simple)
            throw new InvalidOperationException("Bingcai lost its frozen Basic native effect.");
        if (ContinueMouBingcaiLegacyRecoveryRound(id, simple))
            return true;
        if (use.TargetSeats.Count > 1 && simple.Effect is SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol or SimpleCardUseEffect.GuanduIntoxication)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { AdjustedSimpleContinuation = simple });
        _coreCardUseLifecycleOperations.ContinueSimpleCardUse(id, simple);
        return true;
    }

    internal bool ContinueMouBingcaiLegacyRecoveryRound(long id, ProgramSimpleCardContinuation simple)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Alcohol, LegacyDyingAlcoholReturn: { } returned } use || simple.CardId != 0 || simple.Effect != SimpleCardUseEffect.Recovery)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
        if (index < 1 || index != _resolutionStackOperations._resolutionStack.Count - 1 || use is not { PhysicalCardIds.Count: 0, Action: { Type: CardActionType.Use } action, MouBingcaiRounds: { Active: true, Started: > 0 } rounds } || rounds.ActionId != action.ActionId || rounds.Grants.Any(grant => !ValidMouBingcaiExtraGrant(use, grant)) || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use) || !use.TargetSeats.SequenceEqual([use.SourceSeat]) || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != returned.ProgramFrameId || parent.InstructionIndex != returned.InstructionIndex || parent.OwnerSeat != use.SourceSeat || returned.ProducerSource != new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent)?.Op != SkillProgramEffectOp.UseVirtualDyingAlcohol || parent.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.FrameId != context.ParentFrameId || dying.VictimSeat != use.SourceSeat)
            throw new InvalidOperationException("Bingcai's zero-material recovery round lost its original paid self-rescue use and typed program return.");
        // The original producer already recorded its rescue. Repeat only this
        // accepted card's recovery, retaining its native HP/replacement children.
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[use.SourceSeat], new Card(0, CardKind.Alcohol, Suit.None, 0), id, CardKind.Alcohol, simple.RecoveryAmount, simple.RecoveryPolicySources ?? []);
        return true;
    }

    internal bool ContinueMouBingcaiExtraFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouBingcaiRounds: { Active: true } r } use)
            return false;
        var seed = r.Native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == use.TargetIndex);
        if (seed is null)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return true;
        }

        _coreCardAttackStateOperations.UpdateCardAttackState(use.Id, _ => global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(seed.Attack));
        _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
        return true;
    }

    internal bool ValidMouBingcaiExtraGrant(CardUseFrame use, MouBingcaiExtraEffectGrant g)
    {
        if (use.Action is not { Type: CardActionType.Use } a || g.Source.SkillId != "mou-ol:bingcai" || g.Source.BindingId != "first-basic-recast" || string.IsNullOrWhiteSpace(g.Source.SkillInstanceId) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Source.OwnerSeat) || g.FrameId <= 0 || g.WindowId <= 0 || g.RecastInvocationId <= 0 || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(g.BasicKind) != SkillProgramCardCategory.Basic || _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash != g.GameplayHash || g.From.OwnerSeat != g.Source.OwnerSeat || g.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || g.Before < 0 || g.After < g.Before || g.MovementSequence <= g.Before || g.MovementSequence > g.After || _coreProgramEventHistoryOperations.ProgramEventHistory<MouBingcaiExtraEffectIssuedEvent>().Count(e => e == MouBingcaiIssuedEvent(use.Id, a.ActionId, g)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouBingcaiStartedEvent>().Count(e => e == new MouBingcaiStartedEvent(g.FrameId, g.Source, g.GameplayHash, g.WindowId, use.Id, a.ActionId, g.BasicKind)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouBingcaiRecastPaidEvent>().Count(e => e.FrameId == g.FrameId && e.RecastInvocationId == g.RecastInvocationId && e.BatchId == g.BatchId && e.OwnerSeat == g.Source.OwnerSeat && e.CardId == g.MaterialId && e.PrintedKind == g.PrintedKind && e.EffectiveKind == g.EffectiveKind && e.From == g.From && e.MovementSequence == g.MovementSequence && e.Before == g.Before && e.After == g.After && e.MatchesDamage) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeRecastInvocationRecordedEvent>().Count(e => e.InvocationId == g.RecastInvocationId && e.ParentFrameId == g.FrameId && e.InitiatorSeat == g.Source.OwnerSeat && e.ExecutorSeat == g.Source.OwnerSeat && e.PrimaryCount == 1 && e.ActualRecastCount == 1 && e.SequenceBefore <= g.Before && e.SequenceAfter >= g.After) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == g.MovementSequence && m.CardId == g.MaterialId && m.CardKind == g.PrintedKind && m.From == g.From && m.Reason == CardMoveReasons.RecastDiscard) != 1)
            return false;
        var first = _coreWholeActualTurnUsesOperations.WholeActualTurnUseHistory().FirstOrDefault(e => e.CardCategoryMask == 1 << (int)SkillProgramCardCategory.Basic);
        return first is { RoundActionType: CardActionType.Use } && first.OriginFrameId == use.Id && (first.CardActionId == a.ActionId || first.CardActionId is null && _coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(use));
    }
}
