namespace CardGame.Core;

public sealed record ShenwuEquipmentInitializedEvent(int SelectedGodCount) : IGameEvent;

public sealed partial class GameEngine
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 40)]
    [ProgramTargetRule(ProgramTargetRulePoint.DeclaredGlobalProhibition, 20)]
    private bool ShenwuRobeDirectedTargetProhibited(ProgramDirectedTargetQuery query) =>
        IsShenwuRobeTargetProhibited(query.ActorSeat, query.TargetSeat, query.Kind);

    private string? ShenwuEquipmentSkillId(CardKind kind) =>
        EquipmentRules.GetSkillBinding(kind) is { } binding && binding.SkillId.StartsWith("shenwu:", StringComparison.Ordinal)
            ? binding.SkillId : null;
    private string? ShenwuEquipmentSkillGrantId(Card card) =>
        ShenwuEquipmentSkillId(card.Kind) is null ? null : EquipmentSkillGrantId(card);
    private bool HasShenwuArmor(CharacterState player, CardKind kind) =>
        player.IsAlive && !IsArmorIneffectiveForTurn(player) && KongwuEffectiveEquipment(player).Any(card => card.Kind == kind);
    private bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, CardKind kind) =>
        actorSeat != targetSeat && IsOrdinaryTrick(kind) &&
        HasShenwuArmor(_players[targetSeat], CardKind.NationalStyleRobe);
    private bool TryApplyShenwuQimen(CardAttackHandle attack)
    {
        if (attack.CardUserSeat == attack.TargetSeat || attack.IgnoresArmor ||
            HasDirectedCardArmorBypass(attack.ResolutionId, attack.TargetSeat) ||
            !HasShenwuArmor(_players[attack.TargetSeat], CardKind.QimenEightTrigrams)) return false;
        SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        AdvanceEventRulesAndQueueFact(new ArmorEffectAppliedEvent(attack.ResolutionId,
            CardKind.QimenEightTrigrams, attack.SourceSeat, attack.TargetSeat, attack.EffectiveCardKind ?? CardKind.Slash));
        CompleteAttack(attack);
        return true;
    }
    private DamageNature ShenwuDamageNature(IDamageAttempt attack, DamageNature original) =>
        !attack.IsSourceLess && (ShenwuEnabled(_players[attack.SourceSeat], "shenwu:zhaohuo") || HasWeaponAbility(_players[attack.SourceSeat], CardKind.CrimsonSoulZither))
            ? DamageNature.Fire : original;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 240)]
    private int ShenwuShuraDamageBonus(IDamageAttempt attack) =>
        !attack.IsChainPropagation && !attack.IsSourceLess && attack.CardUserSeat == attack.SourceSeat &&
        attack.EffectiveCardKind is { } kind && IsSlashCard(kind) &&
        HasWeaponAbility(_players[attack.CardUserSeat], CardKind.ShuraInfernoHalberd) ? 1 : 0;
    private int ShenwuDustlessDistanceBonus(CharacterState source, CharacterState target) =>
        _players.Count(owner => owner.IsAlive && owner.Seat != target.Seat && owner.TeamId is { } team &&
            team == target.TeamId && source.TeamId != team &&
            KongwuEffectiveEquipment(owner).Any(card => card.Kind == CardKind.DustlessGoldenSpear));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27800)]
    private bool AllowsShenwuShuraRecovery(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != "shenwu:shura-inferno-halberd") return true;
        return GetEquipment(gate.Owner).Any(card => card.Kind == CardKind.ShuraInfernoHalberd &&
                ShenwuEquipmentSkillGrantId(card) == gate.Candidate.SkillInstanceId) &&
            CurrentDamageAttempt is { IsChainPropagation: false, IsSourceLess: false } attack &&
            attack.SourceSeat == gate.OwnerSeat && attack.CardUserSeat == gate.OwnerSeat &&
            attack.EffectiveCardKind is { } kind && IsSlashCard(kind) &&
            HasWeaponAbility(gate.Owner, CardKind.ShuraInfernoHalberd) &&
            LifecycleCardUse(attack.ResolutionId)?.Action?.EffectiveDesignatedTargetSeats.Contains(attack.TargetSeat) == true;
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 20)]
    private void AddShenwuDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms)
    {
        var distance = ShenwuDustlessDistanceBonus(query.Source, query.Target);
        if (distance != 0) terms.Add(new("shenwu:dustless-allied-distance", distance));
    }

}
