#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoLeRuntime
{
    internal void IssueZixiExtraEffect(ProgramSkillFrame f, CardUseFrame use, ProgramCardTriggerWindowFrame window)
    {
        var r = f.ZixiEffects!;
        var old = use.ZixiEffectRounds;
        if (r.JudgmentCount != 1 || window.Continuation == ProgramCardContinuation.Slash && !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || window.Continuation == ProgramCardContinuation.FinalizedTrick && window.TrickContinuation is null || window.Continuation == ProgramCardContinuation.FinalizedSimpleCard && window.SimpleContinuation is null || old is not null && (old.ActionId != r.ActionId || old.Started > 0 || old.Grants.Any(g => g.Source.OwnerSeat == f.OwnerSeat)))
            throw new InvalidOperationException("Zixi cannot issue another effect without the original accepted native continuation.");
        var simple = window.SimpleContinuation is { } s ? s with
        {
            RecoveryPolicySources = s.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
        }

        : null;
        var native = old?.Native ?? new ExtraCardEffectState
        {
            Source = r.Source,
            ProgramFrameId = f.Id,
            ActionId = r.ActionId,
            PaidCardId = 0,
            Trick = window.TrickContinuation,
            Simple = simple
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ZixiEffectRounds = new(r.ActionId, native) { Grants = (old?.Grants ?? []).Append(new(f.Id, r.Source, r.GameplayHash, r.WindowId, r.TargetSeat, 1)).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiExtraEffectIssuedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.TargetSeat, 1));
    }

    internal bool HasUnstartedZixiExtraEffect(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ZixiEffectRounds: { } r, Action: { } a } use && a.ActionId == r.ActionId && r.Started < r.Grants.Count && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
    internal bool IsZixiExtraEffectRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.ZixiEffectRounds is { Started: > 0 };
    internal bool IsTargetScopedZixiEffectRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ZixiEffectRounds: { Active: true, Started: > 0 } r, Action: { } a } && r.ActionId == a.ActionId;
    internal void CompleteZixiExtraEffectRound(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { ZixiEffectRounds: { Active: true, Started: > 0 } r, Action: { } a } use)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || use.NativeDraw is not null || use.NativeCardEffectDraw is not null || use.WumiaoFlood is not null || use.DragonBoatCardEffect is not null || use.GuanduCardEffect is not null)
            throw new InvalidOperationException("Zixi cannot retire its target-local effect before the actual native children finish.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { ZixiEffectRounds = r with { Active = false }, TargetIndex = current.TargetSeats.Count, SequentialTrick = null, AdjustedSimpleContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiExtraEffectRoundCompletedEvent(id, a.ActionId, r.Grants[r.Started - 1].FrameId, r.Started, r.ActiveTargetSeat));
    }

    internal void RecordZixiExtraEffectRedirect(ProgramSkillFrame f, long useId, int previous, int target)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { ZixiEffectRounds: { } rounds, Action: { } a } use)
            return;
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } id } context || id.ParentCardUseFrameId != use.Id || id.CardActionId != a.ActionId || f.OwnerSeat != previous || context.TargetSeat != previous || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.RedirectCurrentAttack || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.TargetSeats[use.TargetIndex] != target)
            throw new InvalidOperationException("Zixi's retained target may follow only this actual native Slash redirect.");
        var affected = rounds.Grants.Where(g => g.CurrentTargetSeat == previous).ToArray();
        if (affected.Length == 0)
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, current => current with { ZixiEffectRounds = rounds with { Grants = rounds.Grants.Select(g => g.CurrentTargetSeat == previous ? g with { CurrentTargetSeat = target } : g).ToArray() } });
        foreach (var grant in affected)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiExtraEffectRedirectedEvent(useId, a.ActionId, grant.FrameId, f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, use.TargetIndex, previous, target));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 28720)]
    internal void FollowZixiSoleTargetSubstitution(IGameEvent payload)
    {
        if (payload is not SoleTargetReplacedEvent e || _coreCardUseLifecycleOperations.LifecycleCardUse(e.CardUseFrameId)is not { ZixiEffectRounds: { Started: 0 } r, Action: { } a } use || e.ActionId != a.ActionId || !_coreSoleTargetQuarantineOperations.IsSoleTargetReplacementFact(use, e, e.BeforeTargets))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ZixiEffectRounds = r with { Grants = r.Grants.Select(g => g.CurrentTargetSeat == e.PreviousTargetSeat ? g with { CurrentTargetSeat = e.NewTargetSeat } : g).ToArray() } });
    }
    internal void CaptureZixiExtraEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ZixiEffectRounds: { Started: 0 } rounds, CardAttack: { } original } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || rounds.Native.OriginalAttacks.Any(s => s.TargetIndex == use.TargetIndex))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ZixiEffectRounds = rounds with { Native = rounds.Native with { OriginalAttacks = rounds.Native.OriginalAttacks.Append(new(use.TargetIndex, original)).ToArray(), Fangtian = current.Continuations.FangtianHalberd ?? rounds.Native.Fangtian } } });
    }

    internal bool TryBeginZixiExtraEffectRound(long id, bool afterAttack)
    {
        if (!HasUnstartedZixiExtraEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { ZixiEffectRounds: { } r, Action: { } a } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || r.Native.Trick is not null && _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || r.Native.Simple is not null && _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Zixi cannot repeat an effect before the whole native child returns.");
        var previous = _characterFeiYiOlOperations.NativeExtraEffectState(use);
        var grant = r.Grants[r.Started];
        var targetIndex = Array.IndexOf(use.TargetSeats.ToArray(), grant.CurrentTargetSeat);
        if (use.CardKind == CardKind.BorrowedSword && targetIndex % 2 != 0)
            targetIndex = -1;
        var native = r.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? r.Native.MaterialClaims,
            FrozenGroupDamageSourceSeat = previous?.FrozenGroupDamageSourceSeat ?? r.Native.FrozenGroupDamageSourceSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { ZixiEffectRounds = r with { Started = r.Started + 1, Native = native, Active = true, ActiveTargetIndex = targetIndex, ActiveTargetSeat = grant.CurrentTargetSeat }, TargetIndex = targetIndex < 0 ? use.TargetSeats.Count : targetIndex, Step = ResolutionFrameStep.ResolvingEffect, SequentialTrick = null, AdjustedSimpleContinuation = null, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZixiExtraEffectRoundStartedEvent(id, a.ActionId, grant.FrameId, r.Started + 1, grant.CurrentTargetSeat));
        if (targetIndex < 0 || !_coreMatchParticipantsPlayersAccess._players[grant.CurrentTargetSeat].IsAlive)
        {
            CompleteZixiExtraEffectRound(id);
            return false;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, a.ActionId, targetIndex, grant.CurrentTargetSeat, use.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
        {
            var first = native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == targetIndex && s.Attack.TargetSeat == grant.CurrentTargetSeat && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, s.Attack.TargetSeat));
            // A redirected native target stays redirected; the original issuer,
            // judgment count and physical payment remain this same one Use.
            if (first is null)
            {
                CompleteZixiExtraEffectRound(id);
                return false;
            }

            var prepared = use.PreparedTargetAttacks?.Select((state, index) => native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == index)?.Attack ?? state).ToArray();
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { TargetIndex = first.TargetIndex, PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared) });
            // This one target's extra effect must not restart a Fangtian/full
            // target walker belonging to the card's ordinary resolution.
            _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = null;
            _coreCardAttackStateOperations.UpdateCardAttackState(id, _ => global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(first.Attack));
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
            return true;
        }

        if (native.Trick is { } trick)
        {
            _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
            _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true);
            IReadOnlyList<int> targets = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Skip(targetIndex).Take(2).ToArray() : [grant.CurrentTargetSeat];
            _coreNativeNullificationOperations.BeginNullificationWindow(id, card, use.SourceSeat, targets, trick.ActionKind, null, trick.RequiredCardKind, use.CardKind);
            return true;
        }

        if (native.Simple is { } simple)
        {
            _coreCardUseLifecycleOperations.ContinueSimpleCardUse(id, simple);
            return true;
        }

        throw new InvalidOperationException("Zixi lost the original native card effect continuation.");
    }
}
