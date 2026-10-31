#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangGuanCapability(Func<RuntimeEntries.CharacterWangGuanOlEntries> getCharacterWangGuanOlRuntime) : IWangGuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWangGuanOlRuntime().BeginWangGuan(f, op);
}
