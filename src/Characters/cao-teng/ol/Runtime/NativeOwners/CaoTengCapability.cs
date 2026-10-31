#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoTengCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoTengOlEntries> getCharacterCaoTengOlRuntime) : ICaoTengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoTengOlRuntime().BeginCaoTeng(f, op);
}
