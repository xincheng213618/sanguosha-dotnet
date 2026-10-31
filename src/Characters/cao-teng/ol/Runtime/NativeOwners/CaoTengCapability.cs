#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaoTengCapability(Func<RuntimeEntries.CharacterCaoTengOlEntries> getCharacterCaoTengOlRuntime) : ICaoTengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoTengOlRuntime().BeginCaoTeng(f, op);
}
