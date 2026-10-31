#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BaoXinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBaoXinOlEntries> getCharacterBaoXinOlRuntime) : IBaoXinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterBaoXinOlRuntime().BeginBaoXin(f, op);
}
