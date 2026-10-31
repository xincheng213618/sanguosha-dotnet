#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangYiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYangYiOlRuntime>> getCharacterYangYiOlRuntime) : IYangYiProgramHost
{
    public SkillProgramStepOutcome JuanxiaDeclareTricks(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangYiOlRuntime().Value.JuanxiaProgramDeclareTricks(frame, effect);
    public SkillProgramStepOutcome JuanxiaRetaliation(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangYiOlRuntime().Value.JuanxiaProgramRetaliation(frame, effect);
}
