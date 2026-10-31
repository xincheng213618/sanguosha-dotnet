#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoOlRuntime
{
    internal bool IsGuimingTreatedWounded(CharacterState player)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat) || !player.IsAlive || !player.HasHpValue || player.IdentityConcealed || player.Seat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player) != "wu")
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat];
        if (!owner.IsAlive || owner.IdentityConcealed)
            return false;
        // Prefilter the raw capability before evaluating qualification. In
        // particular, evaluating unrelated Tang Zi population grants here would
        // recurse through IsRuleWounded while the owner's shard is being built.
        var candidates = owner.SkillGrants.Grants.Where(g => _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Program?.CardPolicies.Any(p => p.Kind == SkillProgramCardPolicyKind.OtherWuTreatedWoundedDuringOwnTurn) == true).ToArray();
        if (candidates.Length == 0 || !SunHaoActualTurnOpen)
            return false;
        var suppressors = _corePrivateGeneralLibrariesOperations.PrivateGeneralLibrarySuppressionInputs(owner);
        return candidates.Any(g => _corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, g) && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, g) && _coreLordSkillProjectionsOperations.IsProjectedGrantQualified(owner, g) && _corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, g) && _coreHpConditionalSkillGrantsOperations.IsHpConditionalGrantQualified(owner, g) && _characterHeQiOlOperations.IsEquipmentSuitGrantQualified(owner, g) && (suppressors.Count == 0 || suppressors.Contains(g.SkillId) || g.SourceId.StartsWith("equipment:", StringComparison.Ordinal)) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, g.SkillId, g.SkillInstanceId, Role.Lord));
    }
}
