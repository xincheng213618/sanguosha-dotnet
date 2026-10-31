#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDuanhunEntries
{
    internal bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, global::System.Collections.Generic.IReadOnlySet<string>? visibleSkillIds = default) => _resolve().DuanhunBypassesNativeDamageArmor(sourceSeat, sourceLess, visibleSkillIds);
    internal bool IsActiveDuanhunPolicySource(int ownerSeat, global::CardGame.Core.IndexedSkillProgramInstance source) => _resolve().IsActiveDuanhunPolicySource(ownerSeat, source);
}
