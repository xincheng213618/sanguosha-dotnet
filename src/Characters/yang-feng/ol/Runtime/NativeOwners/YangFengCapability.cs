#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangFengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYangFengOlEntries> getCharacterYangFengOlRuntime) : IYangFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYangFengOlRuntime().BeginYangFeng(f, op);
}
