#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAdvancedPoliciesRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 170)]
    internal void ObserveAdvancedLifecycleEvent(IGameEvent payload)
    {
        if (payload is TurnStartedEvent)
        {
            _coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess._nextUnlimitedCardOwners.Clear();
        }

        (int Seat, string Skill)? activated = payload switch
        {
            ProgramSkillResolvedEvent { Completed: true } item => (item.OwnerSeat, item.SkillId),
            ProgramBindingResolvedEvent { Activated: true } item => (item.OwnerSeat, item.SkillId),
            ProgramViewAsConvertedEvent item => (item.OwnerSeat, item.SkillId),
            PindianTopSourceChosenEvent item => (item.OwnerSeat, item.Source.SkillId),
            _ => null
        };
        if (activated is { } activation && !_contentRegistryOperations._contentRegistry.GetSkill(activation.Skill).Tags.HasFlag(SkillTag.Locked) && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[activation.Seat], SkillProgramCardPolicyKind.NextCardUnlimitedAfterNonLockedSkill).Any())
            _coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess._nextUnlimitedCardOwners.Add(activation.Seat);
        if (payload is CardUseDeclaredEvent declared)
        {
            var friendUnlimited = _coreGameEngineFriendTrioFriendQihuiUnlimitedAccess._friendQihuiUnlimited.Remove(declared.SourceSeat);
            if (_coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess._nextUnlimitedCardOwners.Remove(declared.SourceSeat) || friendUnlimited)
                _coreCardUseLifecycleOperations.TryMarkUnlimitedUse(declared.ResolutionId);
            if (friendUnlimited)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(declared.ResolutionId, u => u with { QuotaOnlyUseLimitExemption = true });
        }

        if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } response && _coreFriendTrioOperations.IsFriendActualCardUse(response.Action))
            _coreGameEngineFriendTrioFriendQihuiUnlimitedAccess._friendQihuiUnlimited.Remove(response.Action.ActorSeat);
    }

    internal bool HasNextUnlimitedCard(CharacterState owner) => _coreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess._nextUnlimitedCardOwners.Contains(owner.Seat) && _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.NextCardUnlimitedAfterNonLockedSkill).Any();
    internal bool HasFriendQihuiAllowance(CharacterState owner) => _coreGameEngineFriendTrioFriendQihuiUnlimitedAccess._friendQihuiUnlimited.Contains(owner.Seat);
    internal int RedBladeDamageBonus(NativeOwners.CardAttackHandle attack)
    {
        if (attack.IsChainPropagation || attack.CardUserSeat != attack.SourceSeat || attack.EffectiveCardKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash))
            return 0;
        var owner = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var applies = _coreAdvancedWeaponsOperations.HasWeaponAbility(owner, CardKind.RedBloodBlade) && _coreAdvancedLifecycleOperations.IsFarthestInRange(owner, _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, "equipment:red-blood-blade", "farthest-damage", SkillUsageScope.Turn) == 0;
        attack.PendingRedBladeDamageBonus = applies;
        return applies ? 1 : 0;
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 200)]
    internal int RedBladeActualDamageBonus(GameEngine.IDamageAttempt attack) => attack is NativeOwners.CardAttackHandle card ? RedBladeDamageBonus(card) : 0;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 160)]
    internal bool DistanceCardUseExemption160HasNextUnlimitedCard(ProgramCardDistanceQuery q) => HasNextUnlimitedCard(q.Actor);
}
