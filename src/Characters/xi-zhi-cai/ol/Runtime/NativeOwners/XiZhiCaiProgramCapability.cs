#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class XiZhiCaiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiZhiCaiOlEntries> getCharacterXiZhiCaiOlRuntime) : IXiZhiCaiProgramHost
{
    public SkillProgramStepOutcome ArmXianfuRelation(ProgramSkillFrame f, int t) => getCharacterXiZhiCaiOlRuntime().ArmXianfuRelation(f, t);
    public SkillProgramStepOutcome SettleXianfuNativeEcho(ProgramSkillFrame f) => getCharacterXiZhiCaiOlRuntime().SettleXianfuNativeEcho(f);
    public SkillProgramStepOutcome StartChouceDamagePointJudgment(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterXiZhiCaiOlRuntime().StartChouceDamagePointJudgment(f, e);
    public SkillProgramStepOutcome ResolveChouceFrozenJudgment(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterXiZhiCaiOlRuntime().ResolveChouceFrozenJudgment(f, e);
}
