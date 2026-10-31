#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PeiYuanShaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPeiYuanShaoOlEntries> getCharacterPeiYuanShaoOlRuntime) : IPeiYuanShaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPeiYuanShaoOlRuntime().BeginFulue(f, op);
}
