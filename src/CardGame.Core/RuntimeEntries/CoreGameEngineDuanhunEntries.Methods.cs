#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDuanhunEntries
{
    internal bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, IReadOnlySet<string>? visibleSkillIds = default) => _resolve().DuanhunBypassesNativeDamageArmor(sourceSeat, sourceLess, visibleSkillIds);
    internal bool IsActiveDuanhunPolicySource(int ownerSeat, IndexedSkillProgramInstance source) => _resolve().IsActiveDuanhunPolicySource(ownerSeat, source);
}
