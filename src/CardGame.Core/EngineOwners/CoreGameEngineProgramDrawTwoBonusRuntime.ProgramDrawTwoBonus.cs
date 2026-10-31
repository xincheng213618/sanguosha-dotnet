#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDrawTwoBonusRuntime
{
    internal bool HasNativeDrawTwoCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.AdditionalDrawTwoCards);

    internal int NativeDrawTwoCount(int sourceSeat) => checked(_coreWushuangCombatTacticsOperations.WushuangDrawTwoCount(sourceSeat) + _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], SkillProgramCardPolicyKind.AdditionalDrawTwoCards, CardKind.DrawTwo).Sum(item => item.Policy.Value));
}
