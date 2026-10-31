#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class KenshangCapability(Func<RuntimeEntries.CharacterMaXiuMaTieOlEntries> getCharacterMaXiuMaTieOlRuntime) : IKenshangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaXiuMaTieOlRuntime().BeginKenshang(f, op);
}
