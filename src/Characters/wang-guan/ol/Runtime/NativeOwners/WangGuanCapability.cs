#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangGuanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangGuanOlEntries> getCharacterWangGuanOlRuntime) : IWangGuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWangGuanOlRuntime().BeginWangGuan(f, op);
}
