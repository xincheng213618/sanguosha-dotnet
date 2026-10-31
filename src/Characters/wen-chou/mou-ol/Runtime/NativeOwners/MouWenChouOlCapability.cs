#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouWenChouOlCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWenChouMouOlEntries> getCharacterWenChouMouOlRuntime) : IMouWenChouOlHost, IMouWenChouOlSelection
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.UseMouWenChouLunzhan ? getCharacterWenChouMouOlRuntime().BeginMouWenChouLunzhan(f) : getCharacterWenChouMouOlRuntime().BeginMouWenChouTrigger(f, op);
    public bool Target(int owner, int target) => !getCharacterWenChouMouOlRuntime().MouWenChouTargetBanned(owner, target);
}
