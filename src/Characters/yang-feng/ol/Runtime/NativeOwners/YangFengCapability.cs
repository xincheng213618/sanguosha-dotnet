#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangFengCapability(Func<RuntimeEntries.CharacterYangFengOlEntries> getCharacterYangFengOlRuntime) : IYangFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYangFengOlRuntime().BeginYangFeng(f, op);
}
