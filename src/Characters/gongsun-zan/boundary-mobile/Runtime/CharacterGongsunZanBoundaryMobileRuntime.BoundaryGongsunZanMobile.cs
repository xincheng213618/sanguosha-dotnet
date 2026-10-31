#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGongsunZanBoundaryMobileRuntime
{
    internal const string BoundaryMobileGszYicong = "boundary:yicong-mobile", BoundaryMobileGszQiaomeng = "boundary:qiaomeng-mobile";
    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 57700)]
    internal void BoundaryMobileGongsunZanDistance(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms)
    {
        if (query.Source.Seat == query.Target.Seat)
            return;
        if (query.Source.HasHpValue && _coreNationalSkillsOperations.HasRuntimeSkill(query.Source, BoundaryMobileGszYicong))
            terms.Add(new($"skill:{(query.Source.Seat)}:{(BoundaryMobileGszYicong)}:outgoing", checked(1 - query.Source.Hp)));
        if (query.Target.HasHpValue && _coreNationalSkillsOperations.HasRuntimeSkill(query.Target, BoundaryMobileGszYicong))
            terms.Add(new($"skill:{(query.Target.Seat)}:{(BoundaryMobileGszYicong)}:incoming", checked(query.Target.MaxHp - query.Target.Hp - 1)));
    }

    internal static IReadOnlyList<CardZoneKind> DamageTargetMountZonesFor(string? skillId) => skillId == BoundaryMobileGszQiaomeng ? CharacterGongsunZanBoundaryMobileDefinitions.BoundaryMobileGszEquipment : CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones;
    internal bool DamageTargetMountUseMatchesEdition(string? skillId, CardUseFrame? use)
    {
        if (skillId != BoundaryMobileGszQiaomeng)
            return true;
        if (use?.Action is not { Type: CardActionType.Use, EffectiveIsRed: false } action || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind))
            return false;
        if (action.EffectiveSuit is Suit.Spade or Suit.Club)
            return true;
        // Mixed-suit black material (such as two black Zhangba cards) retains
        // its captured color even when the combined virtual card has no suit.
        // A colorless cardless Slash or colorless physical entity is not black.
        return action.PhysicalCards.Count > 0 && action.PhysicalCards.All(cost => cost.EffectiveIsRed == false && _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId).Suit != Suit.None);
    }
}
