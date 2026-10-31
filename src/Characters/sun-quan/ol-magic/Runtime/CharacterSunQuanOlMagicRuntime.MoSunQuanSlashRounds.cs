#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool HasUnstartedMoQuanyuExtraRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MoQuanyuExtraRound: { Started: false }, Action: { Type: CardActionType.Use } } use && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use);
    internal bool IsMoQuanyuExtraRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MoQuanyuExtraRound is { Started: true };
    internal void CaptureMoQuanyuExtraAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MoQuanyuExtraRound: { Started: false } r, CardAttack: { } original } use || _characterDaQiaoLeOperations.IsZixiExtraEffectRound(use.Id) || _characterFeiYiOlOperations.IsFeiYiExtraEffectRound(use.Id) || use.ExtraCardEffect is { WholeGroupRepeated: true } || r.Native.OriginalAttacks.Any(s => s.TargetIndex == use.TargetIndex))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MoQuanyuExtraRound = r with { Native = r.Native with { OriginalAttacks = r.Native.OriginalAttacks.Append(new(use.TargetIndex, original)).ToArray(), Fangtian = current.Continuations.FangtianHalberd ?? r.Native.Fangtian } } });
    }

    internal void CompleteMoQuanyuExtraRound(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MoQuanyuExtraRound: { Active: true } r } use)
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MoQuanyuExtraRound = r with { Active = false } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuExtraRoundCompletedEvent(r.ProgramFrameId, use.Id, r.ActionId));
    }

    internal bool TryBeginMoQuanyuExtraRound(long id, bool afterAttack)
    {
        if (!HasUnstartedMoQuanyuExtraRound(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MoQuanyuExtraRound: { } r } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Quanyu must await the complete original native effect and its children.");
        var previous = _characterFeiYiOlOperations.NativeExtraEffectState(use);
        var native = r.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? r.Native.MaterialClaims
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { MoQuanyuExtraRound = r with { Started = true, Active = true, Native = native }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuExtraRoundStartedEvent(r.ProgramFrameId, id, r.ActionId));
        var first = native.OriginalAttacks.OrderBy(s => s.TargetIndex).FirstOrDefault(s => _coreMatchParticipantsPlayersAccess._players[s.Attack.TargetSeat].IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, s.Attack.TargetSeat));
        if (first is null)
        {
            CompleteMoQuanyuExtraRound(id);
            return false;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, r.ActionId, first.TargetIndex, first.Attack.TargetSeat, use.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        var prepared = use.PreparedTargetAttacks?.Select((state, index) => native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == index)?.Attack ?? state).ToArray();
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

    internal bool ContinueMoQuanyuFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MoQuanyuExtraRound: { Active: true } r } use)
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
}
