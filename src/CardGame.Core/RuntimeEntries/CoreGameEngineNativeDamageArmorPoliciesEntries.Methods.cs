#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesEntries
{
    internal bool BypassesNativeDamageArmor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().BypassesNativeDamageArmor(attack);
    internal bool HasNativeDamageArmorPolicy(int sourceSeat, bool sourceLess, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => _resolve().HasNativeDamageArmorPolicy(sourceSeat, sourceLess, kind, visibleSkillIds);
    internal int NativeDamageArmorMultiplier(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().NativeDamageArmorMultiplier(attack);
}
