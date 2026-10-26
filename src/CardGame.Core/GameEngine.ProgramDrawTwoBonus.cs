namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Registering future stock enables the native resolved-effect owner, without
    // granting its quantity bonus to an owner who has not acquired the policy.
    private bool HasNativeDrawTwoCapability => HasNativeDrawAttemptCapability ||
        _contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.AdditionalDrawTwoCards);

    private int NativeDrawTwoCount(int sourceSeat) => checked(WushuangDrawTwoCount(sourceSeat) +
        CardPolicies(_players[sourceSeat], SkillProgramCardPolicyKind.AdditionalDrawTwoCards, CardKind.DrawTwo)
            .Sum(item => item.Policy.Value));
}
