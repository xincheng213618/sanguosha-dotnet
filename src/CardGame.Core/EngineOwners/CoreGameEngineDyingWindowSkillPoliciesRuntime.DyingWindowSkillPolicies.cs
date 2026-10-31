#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDyingWindowSkillPoliciesRuntime
{
    internal bool HasBlackTrickTargetPolicy => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ProhibitBlackTrickTarget);
    internal bool HasDyingWindowQualificationPolicy => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.SuppressOthersNonLockedDuringDying);

    internal bool IsDyingWindowGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (!HasDyingWindowQualificationPolicy || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || owner.Seat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || owner.Seat == dying.VictimSeat || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive)
            return true;
        return !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.SuppressOthersNonLockedDuringDying);
    }

    internal long CaptureDyingWindowQualificationStamp(long underlying)
    {
        if (!HasDyingWindowQualificationPolicy)
            return underlying;
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying;
        var source = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat];
        var next = (underlying, dying?.FrameId ?? 0L, dying?.VictimSeat ?? -1, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, source.SkillGrants.Revision, source.Hp, source.IsAlive);
        if (_coreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationStamp == 0 || next != _coreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationDependencies)
        {
            _coreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationDependencies = next;
            _coreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationStamp++;
        }

        return -_coreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationStamp;
    }

    internal bool IsExclusiveTurnPeachUseForbidden(int actorSeat, CardKind kind, CardActionType type) => kind == CardKind.Peach && type == CardActionType.Use && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && actorSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && actorSeat != _coreDyingCursorRuntimeOperations.ActiveDying?.VictimSeat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.ExclusiveTurnPeachUse, CardKind.Peach);
    internal bool IsBlackTrickTargetProhibited(CharacterState target, CardKind kind, bool? effectiveIsRed) => effectiveIsRed == false && CardUseCategoryCatalog.Get(kind)is CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick && _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitBlackTrickTarget);
    internal static bool? ActualTargetPolicyColor(CardActionContext action) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DyingWindowSkillPolicies.ActualTargetPolicyColor(action);
}
