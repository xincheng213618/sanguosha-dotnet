#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BaoXinCapability(Func<RuntimeEntries.CharacterBaoXinOlEntries> getCharacterBaoXinOlRuntime) : IBaoXinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterBaoXinOlRuntime().BeginBaoXin(f, op);
}
