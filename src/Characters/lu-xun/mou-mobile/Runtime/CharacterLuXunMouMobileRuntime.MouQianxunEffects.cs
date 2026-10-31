#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunMouMobileRuntime
{
    internal bool HasPerTargetTrickEffectBinding(ProgramCardTriggerCandidate c) => _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.Triggers.Single(t => t.Id == c.TriggerId).Effects.Any(e => ProgramOperationCatalog.Default.Resolve(e.Op) is IProgramPerTargetTrickEffectOperation);
    internal bool CanMouLuXunRecordNativeTarget(int target, CardActionContext action) => action.ActorSeat != target && !MouLuXunRecordedNames(target).Contains(action.EffectiveKind) && _coreMatchParticipantsPlayersAccess._players[target].SkillGrants.Grants.Any(g => g.SkillId == MouLuXunComposition.Qianxun && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[target], g.SkillId, g.SkillInstanceId));
    internal bool TryBeginMouLuXunGroupEffect(NativeOwners.GroupCardHandle group)
    {
        if (!HasMouLuXun || group.TargetIndex >= group.TargetSeats.Count || _coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)?.Action is not { } action)
            return false;
        var target = group.TargetSeats[group.TargetIndex];
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(group.ResolutionId, target) || !CanMouLuXunRecordNativeTarget(target, action))
            return false;
        return _coreCardEffectProgramsOperations.BeginCardEffectPrograms(action, [target], null, null, new(group.ResolutionId, target, group.TargetIndex, group.Effect), perTargetOnly: true);
    }

    internal bool ValidNativeTrickTargetEffect(CardEffectBeforeApplyFrame window)
    {
        if (window.NativeTargetReturn is not { } r || window.ParentFrameId != (r.WenheProgramFrameId ?? r.UseId) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } action } use || action.ActionId != window.Action.ActionId || !window.FinalDesignatedTargetSeats.SequenceEqual([r.TargetSeat]) || use.PhysicalCardIds is not { } physical || physical.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing))
            return false;
        if (r.WenheProgramFrameId is not null)
            return ValidMouLuXunWenheTarget(r, action);
        if (r.TargetIndex != use.TargetIndex || r.HpAmount != 0)
            return false;
        if (r.GroupEffect is { } effect)
            return r.IronChainReturn is null && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group && group.ResolutionId == r.UseId && group.Effect == effect && group.TargetIndex == r.TargetIndex && group.TargetSeats.ElementAtOrDefault(r.TargetIndex) == r.TargetSeat;
        return r.IronChainReturn is { ActionKind: LegalActionKind.IronChain } pending && pending.ParentFrameId == r.UseId && use.NativeIronChainTargets is not null && pending.TargetSeats.ElementAtOrDefault(r.TargetIndex) == r.TargetSeat;
    }
    internal void ReturnNativeTrickTargetEffect(CardEffectBeforeApplyFrame window)
    {
        if (!ValidNativeTrickTargetEffect(window))
            throw new InvalidOperationException("The real trick target effect lost its exact native target cursor.");
        var r = window.NativeTargetReturn!;
        if (r.WenheProgramFrameId is not null)
        {
            ReturnMouLuXunWenheTarget(r);
            return;
        }

        if (r.IronChainReturn is { } chain)
        {
            _coreNativeTrickEffectsOperations.ResolveIronChainEffect(chain);
            return;
        }

        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard!;
        if (r.GroupEffect == GroupCardEffect.ResponseAttack)
            _coreNativeGroupAttackOperations.Value.ContinueGroupAttackResponseAfterEffect(group);
        else if (r.GroupEffect == GroupCardEffect.PublicDraft)
            _coreNativeHarvestOperations.Value.BeginHarvestSelection(group);
        else
            _coreNativeGroupRecoveryOperations.Value.RunOneGroupRecoveryStep();
    }

    internal bool ResolveMouLuXunMultiIronChain(NullificationWindowFrame pending)
    {
        if (!HasMouLuXun || pending.TargetSeats.Count < 2)
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)!;
        if (use.NativeIronChainTargets is null)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { NativeIronChainTargets = new(), TargetIndex = 0 });
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        }

        while (use.TargetIndex < pending.TargetSeats.Count)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeats[use.TargetIndex]];
            if (target.IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(use.Id, target.Seat))
            {
                if (CanMouLuXunRecordNativeTarget(target.Seat, use.Action!) && _coreCardEffectProgramsOperations.BeginCardEffectPrograms(use.Action!, [target.Seat], null, null, new(use.Id, target.Seat, use.TargetIndex, IronChainReturn: pending), perTargetOnly: true))
                    return true;
                var wasChained = target.IsChained;
                target.IsChained = _coreEquipmentSlotGroupProgramsOperations.ResolveEnteringChain(target, _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ForceChained) || !target.IsChained);
                if (!wasChained && target.IsChained)
                    _coreCharacterStateProgramsOperations.RecordCharacterStateChange(target.Seat, SkillProgramTriggerWindow.CharacterEnteredChain, use.Id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(use.Id, pending.SourceSeat, target.Seat, target.IsChained));
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { NativeIronChainTargets = u.NativeIronChainTargets!with { Changes = u.NativeIronChainTargets.Changes.Append($"{(target.Name)}{((target.IsChained ? "横置" : "重置"))}").ToArray() } });
            }

            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { TargetIndex = u.TargetIndex + 1 });
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        }

        _coreNativeTrickEffectsOperations.CompleteIronChainEffect(pending, use.NativeIronChainTargets!.Changes.ToList());
        return true;
    }
}
