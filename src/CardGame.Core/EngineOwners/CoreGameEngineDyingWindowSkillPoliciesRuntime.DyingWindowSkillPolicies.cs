#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDyingWindowSkillPoliciesRuntime
{
    internal bool HasBlackTrickTargetPolicy => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ProhibitBlackTrickTarget);
    internal bool IsExclusiveTurnPeachUseForbidden(int actorSeat, CardKind kind, CardActionType type) => kind == CardKind.Peach && type == CardActionType.Use && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && actorSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && actorSeat != _coreDyingCursorRuntimeOperations.Value.ActiveDying?.VictimSeat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.ExclusiveTurnPeachUse, CardKind.Peach);
    internal bool IsBlackTrickTargetProhibited(CharacterState target, CardKind kind, bool? effectiveIsRed) => effectiveIsRed == false && CardUseCategoryCatalog.Get(kind)is CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick && _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitBlackTrickTarget);
    internal static bool? ActualTargetPolicyColor(CardActionContext action) => RuntimeSkills.RuntimeRules_Core_DyingWindowSkillPolicies.ActualTargetPolicyColor(action);
}
