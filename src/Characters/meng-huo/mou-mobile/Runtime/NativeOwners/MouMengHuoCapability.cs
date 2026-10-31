#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouMengHuoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMengHuoMouMobileEntries> getCharacterMengHuoMouMobileRuntime) : IMouMengHuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMengHuoMouMobileRuntime().ResolveMouMh(f, op);
}
