#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesEntries
{
    internal bool BypassesNativeDamageArmor(GameEngine.IDamageAttempt attack) => _resolve().BypassesNativeDamageArmor(attack);
    internal bool HasNativeDamageArmorPolicy(int sourceSeat, bool sourceLess, SkillProgramCardPolicyKind kind, IReadOnlySet<string>? visibleSkillIds = default) => _resolve().HasNativeDamageArmorPolicy(sourceSeat, sourceLess, kind, visibleSkillIds);
    internal int NativeDamageArmorMultiplier(GameEngine.IDamageAttempt attack) => _resolve().NativeDamageArmorMultiplier(attack);
}
