#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShenwuEquipmentRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 40)]
    [ProgramTargetRule(ProgramTargetRulePoint.DeclaredGlobalProhibition, 20)]
    internal bool ShenwuRobeDirectedTargetProhibited(ProgramDirectedTargetQuery query) => IsShenwuRobeTargetProhibited(query.ActorSeat, query.TargetSeat, query.Kind);
    internal string? ShenwuEquipmentSkillId(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.GetSkillBinding(kind)is { } binding && binding.SkillId.StartsWith("shenwu:", StringComparison.Ordinal) ? binding.SkillId : null;
    internal string? ShenwuEquipmentSkillGrantId(Card card) => ShenwuEquipmentSkillId(card.Kind)is null ? null : _coreEquipmentSkillsOperations.EquipmentSkillGrantId(card);
    internal bool HasShenwuArmor(CharacterState player, CardKind kind) => player.IsAlive && !_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(player) && _characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => card.Kind == kind);
    internal bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, CardKind kind) => actorSeat != targetSeat && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && HasShenwuArmor(_coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.NationalStyleRobe);
    internal bool TryApplyShenwuQimen(NativeOwners.CardAttackHandle attack)
    {
        if (attack.CardUserSeat == attack.TargetSeat || attack.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(attack.ResolutionId, attack.TargetSeat) || !HasShenwuArmor(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], CardKind.QimenEightTrigrams))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorEffectAppliedEvent(attack.ResolutionId, CardKind.QimenEightTrigrams, attack.SourceSeat, attack.TargetSeat, attack.EffectiveCardKind ?? CardKind.Slash));
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        return true;
    }

    internal DamageNature ShenwuDamageNature(GameEngine.IDamageAttempt attack, DamageNature original) => !attack.IsSourceLess && (_coreShenwuNpcProgramsOperations.ShenwuEnabled(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], "shenwu:zhaohuo") || _coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], CardKind.CrimsonSoulZither)) ? DamageNature.Fire : original;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 240)]
    internal int ShenwuShuraDamageBonus(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && !attack.IsSourceLess && attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], CardKind.ShuraInfernoHalberd) ? 1 : 0;
    internal int ShenwuDustlessDistanceBonus(CharacterState source, CharacterState target) => _coreMatchParticipantsPlayersAccess._players.Count(owner => owner.IsAlive && owner.Seat != target.Seat && owner.TeamId is { } team && team == target.TeamId && source.TeamId != team && _characterHuCheErMouOperations.KongwuEffectiveEquipment(owner).Any(card => card.Kind == CardKind.DustlessGoldenSpear));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27800)]
    internal bool AllowsShenwuShuraRecovery(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "shenwu:shura-inferno-halberd")
            return true;
        return _corePhysicalOwnedCardQueriesOperations.GetEquipment(gate.Owner).Any(card => card.Kind == CardKind.ShuraInfernoHalberd && ShenwuEquipmentSkillGrantId(card) == gate.Candidate.SkillInstanceId) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsChainPropagation: false, IsSourceLess: false } attack && attack.SourceSeat == gate.OwnerSeat && attack.CardUserSeat == gate.OwnerSeat && attack.EffectiveCardKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreAdvancedWeaponsOperations.HasWeaponAbility(gate.Owner, CardKind.ShuraInfernoHalberd) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.EffectiveDesignatedTargetSeats.Contains(attack.TargetSeat) == true;
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 20)]
    internal void AddShenwuDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms)
    {
        var distance = ShenwuDustlessDistanceBonus(query.Source, query.Target);
        if (distance != 0)
            terms.Add(new("shenwu:dustless-allied-distance", distance));
    }
}
