#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheLeizhenRuntime
{
    internal const string ShanheLeizhenSkill = "shanhe:leizhen";
    internal bool HasShanheLeizhenCapability => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheLeizhenUse) == true;

    internal ShanheLeizhenChargeEarnedEvent? PendingShanheLeizhenCharge(int owner)
    {
        var spent = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLeizhenChargeSpentEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.EarningActionId).ToHashSet();
        var earned = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLeizhenChargeEarnedEvent>().Where(e => e.OwnerSeat == owner && !spent.Contains(e.ActionId)).ToArray();
        if (earned.Length > 1)
            throw new InvalidOperationException("Leizhen's already-paid next-Slash benefit cannot stack.");
        return earned.SingleOrDefault();
    }

    internal int ShanheLeizhenPublicActionBonus(int actorSeat, CardKind? effectiveKind) => HasShanheLeizhenCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) && _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && effectiveKind is { } kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && PendingShanheLeizhenCharge(actorSeat)is not null ? 1 : 0;
    internal void RecordShanheLeizhenCommittedUse(CardActionContext action)
    {
        if (!HasShanheLeizhenCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || action.Type == CardActionType.Response && (action.EffectiveKind is not (CardKind.Dodge or CardKind.Nullification) || action.ProviderSeat != action.ActorSeat || action.RequesterSeat is not null))
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var count = action.PhysicalCards.Count(c => c.From == CardLocation.Hand(owner.Seat));
        if (count == 0 || !owner.IsAlive || !_coreAdvancedWeaponsOperations.HasWeaponAbility(owner, CardKind.Leizhen))
            return;
        foreach (var weapon in _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Where(c => c.Kind == CardKind.Leizhen).OrderBy(c => c.Id))
        {
            // Native equipment commitment follows its actual entry. The newly
            // installed weapon did not own the earlier Hand use that installs it.
            if (action.EffectiveKind == CardKind.Leizhen && action.PhysicalCards.Any(c => c.CardId == weapon.Id))
                continue;
            var instance = _coreEquipmentSkillsOperations.EquipmentSkillGrantId(weapon)!;
            if (!_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner.Seat, ShanheLeizhenSkill, instance, weapon))
                continue;
            var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == weapon.Id && m.To == CardLocation.Equipment(owner.Seat));
            if (entry is null)
                throw new InvalidOperationException("Leizhen counting requires a real equipped entity and residence origin.");
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLeizhenHandUseCountedEvent>().Any(e => e.ActionId == action.ActionId && e.WeaponCardId == weapon.Id))
                continue;
            var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheLeizhenHandUseCountedEvent>().LastOrDefault(e => e.OwnerSeat == owner.Seat && e.WeaponCardId == weapon.Id && e.EquipmentEntrySequence == entry.Sequence)?.RemainderAfter ?? 0;
            var total = checked(previous + count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheLeizhenHandUseCountedEvent(action.ActionId, owner.Seat, weapon.Id, entry.Sequence, count, previous, total % 3));
            if (total >= 3 && PendingShanheLeizhenCharge(owner.Seat)is null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheLeizhenChargeEarnedEvent(action.ActionId, owner.Seat, weapon.Id, entry.Sequence));
        }
    }

    internal void ResolveShanheLeizhenUse(ProgramSkillFrame f)
    {
        if (f.SkillId != ShanheLeizhenSkill || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } context } || context.ActorSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == f.WindowContext.ParentFrameId)is not { } window)
            throw new InvalidOperationException("Leizhen requires the original genuine committed-use actor window.");
        RecordShanheLeizhenCommittedUse(window.Action);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 59402)]
    internal void IssueShanheLeizhenSlash(CardUseDeclaredEvent declared)
    {
        if (!HasShanheLeizhenCapability || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(declared.CardKind) || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { } use || use.SourceSeat != declared.SourceSeat || use.CardKind != declared.CardKind || use.ShanheLeizhen is not null || PendingShanheLeizhenCharge(use.SourceSeat)is not { } charge)
            return;
        var receipt = new ShanheLeizhenSlashReceipt(charge.ActionId, use.SourceSeat, charge.WeaponCardId, use.Id);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShanheLeizhen = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheLeizhenChargeSpentEvent(charge.ActionId, use.SourceSeat, charge.WeaponCardId, use.Id));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 59403)]
    internal void ApplyShanheLeizhenNativeBase(CardUsedEvent fact)
    {
        if (!HasShanheLeizhenCapability || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(fact.CardKind) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ShanheLeizhen: { BaseApplied: false } receipt } use || fact.SourceSeat != use.SourceSeat || fact.TargetSeat != attack.TargetSeat || receipt.OwnerSeat != use.SourceSeat)
            return;
        var state = _coreCardAttackStateOperations.GetCardAttackState(use.Id);
        if (state.DamageAmountFinalized || state.DamageWasApplied || state.IsChainPropagation)
            throw new InvalidOperationException("Leizhen must add its already-earned benefit to the initial native Slash base.");
        var before = state.DamageAmount;
        var after = checked(before + 1);
        _coreCardAttackStateOperations.UpdateCardAttackState(use.Id, s => s!with { DamageAmount = after });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShanheLeizhen = receipt with { BaseApplied = true, BaseBefore = before, BaseAfter = after }, ProgramAdjustedSlashBaseDamage = current.ProgramAdjustedSlashBaseDamage is { } original ? checked(original + 1) : null, PreparedTargetAttacks = current.PreparedTargetAttacks is { } prepared ? Array.AsReadOnly(prepared.Select(s => s with { DamageAmount = checked(s.DamageAmount + 1) }).ToArray()) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheLeizhenBaseAppliedEvent(use.Id, use.SourceSeat, before, after));
    }
}
