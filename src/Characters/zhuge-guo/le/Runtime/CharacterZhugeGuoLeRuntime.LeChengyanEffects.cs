#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoLeRuntime
{
    internal static CardKind LeChengyanNativeEffectKind(CardUseFrame use) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhuge_guo_le_LeChengyanEffects.LeChengyanNativeEffectKind(use);
    internal static IReadOnlyList<int> LeChengyanNativeTargets(CardUseFrame use) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhuge_guo_le_LeChengyanEffects.LeChengyanNativeTargets(use);
    internal bool IsLeChengyanEffectUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { LeChengyanEffect: { Started: true, Finished: false } } u && ValidLeChengyanEffect(u);
    internal bool ValidLeChengyanEffect(CardUseFrame u) => u.LeChengyanEffect is { } p && u.Action is { Type: CardActionType.Use } a && a.ActionId == p.ActionId && a.ActorSeat == p.Source.OwnerSeat && u.SourceSeat == a.ActorSeat && (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind)) && p.EffectKind != CardKind.Nullification && (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectKind) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(p.EffectKind)) && _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash == p.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<LeChengyanEffectIssuedEvent>().Count(e => e == new LeChengyanEffectIssuedEvent(p.ProgramFrameId, u.Id, p.ActionId, p.Source, p.ShownCardId, p.EffectKind)) == 1;
    internal void AssertLeChengyanEffects()
    {
        foreach (var u in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.LeChengyanEffect is not null))
            if (!ValidLeChengyanEffect(u) || u.LeChengyanEffect!.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)))
                throw new InvalidOperationException("Chengyan effect lost its original action or one issued shown-card receipt.");
    }

    internal bool TryStartLeChengyanEffect(ProgramCardTriggerWindowFrame window)
    {
        if (global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseTargetsFinalized || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { LeChengyanEffect: { Started: false } p } u)
            return false;
        if (!ValidLeChengyanEffect(u) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != u.Id)
            throw new InvalidOperationException("Chengyan must return to its paid original native Use.");
        // Auxiliary Borrowed Sword victims are not designated card targets.
        var targets = (u.CardKind == CardKind.BorrowedSword ? u.TargetSeats.Where((_, i) => i % 2 == 0) : u.TargetSeats).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(u = u with { LeChengyanEffect = p with { Started = true, Targets = targets }, TargetIndex = 0, SequentialTrick = null, FireAttackSelection = null, CardAttack = null, PreparedTargetAttacks = null, Continuations = u.Continuations with { FangtianHalberd = null, GroupCard = null, BorrowedSword = null } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeChengyanEffectStartedEvent(u.Id, p.ActionId, u.CardKind, p.EffectKind, targets.Length));
        StartLeChengyanNativeEffect(u);
        return true;
    }

    internal void StartLeChengyanNativeEffect(CardUseFrame u)
    {
        var p = u.LeChengyanEffect!;
        if (p.Targets.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(u.Id, LeChengyanOriginalCard(u), u.CardKind);
            return;
        }

        var kind = p.EffectKind;
        if (kind == CardKind.BorrowedSword)
        {
            BeginLeChengyanBorrowedWithAuxiliary(u);
            return;
        }

        var action = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? LegalActionKind.Slash : Enum.Parse<LegalActionKind>(kind.ToString());
        CardKind? required = kind == CardKind.BarbarianAssault ? CardKind.Slash : kind == CardKind.ArrowBarrage ? CardKind.Dodge : null;
        _coreNativeNullificationOperations.BeginNullificationWindow(u.Id, LeChengyanOriginalCard(u), u.SourceSeat, p.Targets, action, requiredCardKind: required, playedCardKind: u.CardKind);
    }

    internal Card LeChengyanOriginalCard(CardUseFrame u) => _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(u.Id, u.CardId);
    internal bool TryBypassLeChengyanNullification(long id, Card card, IReadOnlyList<int> targets, LegalActionKind action, int? targetCard, CardKind? required)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { LeChengyanEffect.Started: true } u || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) || !ValidLeChengyanEffect(u))
            return false;
        _coreNativeNullificationOperations.ResolveNullifiableEffect(new NullificationWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, id, u.SourceSeat, Array.AsReadOnly(targets.ToArray()), card.Id, u.CardKind, action, targetCard, required, []));
        return true;
    }

    internal bool TryResolveLeChengyanNativeEffect(NullificationWindowFrame pending)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)is not { LeChengyanEffect: { Started: true, Finished: false } p } u || p.TranslatedWindowId == pending.Id && p.TranslatedTargetIndex == u.TargetIndex || !ValidLeChengyanEffect(u))
            return false;
        if (pending.EffectCardKind != u.CardKind || pending.SourceSeat != u.SourceSeat || pending.EffectCardId != u.CardId)
            throw new InvalidOperationException("Chengyan effect must retain its paid card, actor and original counterspell eligibility.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { LeChengyanEffect = p with { TranslatedWindowId = pending.Id, TranslatedTargetIndex = u.TargetIndex } });
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectKind))
        {
            BeginLeChengyanSlash(u);
            return true;
        }

        _coreNativeNullificationOperations.ResolveNullifiableEffectCore(pending with { EffectCardKind = p.EffectKind });
        return true;
    }

    internal void BeginLeChengyanSlash(CardUseFrame u)
    {
        var p = u.LeChengyanEffect!;
        var index = p.SlashCursor;
        while (index < p.Targets.Count && (!_coreMatchParticipantsPlayersAccess._players[p.Targets[index]].IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(u.Id, p.Targets[index])))
            index++;
        if (index >= p.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { LeChengyanEffect = p with { Finished = true } });
            _coreNativeCardUseCompletionOperations.FinishCardUse(u.Id, LeChengyanOriginalCard(u), u.CardKind);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { TargetIndex = index, LeChengyanEffect = p with { SlashCursor = index } });
        var target = p.Targets[index];
        var card = LeChengyanOriginalCard(u);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(u.Id, u.SourceSeat, target, card.Id == 0 ? null : card, damageAmount: 1 + (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) ? u.ConsumedAlcoholBonus ?? (u.ConsumedAlcoholBoost ? 1 : 0) : 0), playedCardKind: p.EffectKind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], _coreMatchParticipantsPlayersAccess._players[target], u.CardKind), physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(u.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeSlashUseOperations.ContinueSlashAfterProgramTargetEffects(attack);
    }

    [ProgramDamageHook(ProgramDamagePhase.AttackCompletionPrelude, 10)]
    internal bool FinishLeChengyanAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { LeChengyanEffect: { Started: true, Finished: false } p } u || !ValidLeChengyanEffect(u))
            return false;
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.EffectKind))
        {
            if (attack.EffectiveCardKind != p.EffectKind || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null)
                return false;
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeCardUseCompletionOperations.FinishCardUse(u.Id, LeChengyanOriginalCard(u), u.CardKind);
            paused = true;
            return true;
        }

        if (attack.TargetSeat != p.Targets[p.SlashCursor] || attack.EffectiveCardKind != p.EffectKind)
            throw new InvalidOperationException("Chengyan Slash lost its current original target.");
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        u = _coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { LeChengyanEffect = p with { SlashCursor = p.SlashCursor + 1 }, CausedDamage = u.CausedDamage || attack.DamageWasApplied });
        BeginLeChengyanSlash(u);
        paused = true;
        return true;
    }
}
