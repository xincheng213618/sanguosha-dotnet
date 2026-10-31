#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangYiProgramCapability(Func<RuntimeEntries.CharacterYangYiOlEntries> getCharacterYangYiOlRuntime) : IYangYiProgramHost
{
    public SkillProgramStepOutcome JuanxiaDeclareTricks(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangYiOlRuntime().JuanxiaProgramDeclareTricks(frame, effect);
    public SkillProgramStepOutcome JuanxiaRetaliation(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangYiOlRuntime().JuanxiaProgramRetaliation(frame, effect);
}
