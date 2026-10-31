#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PeiYuanShaoCapability(Func<RuntimeEntries.CharacterPeiYuanShaoOlEntries> getCharacterPeiYuanShaoOlRuntime) : IPeiYuanShaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPeiYuanShaoOlRuntime().BeginFulue(f, op);
}
