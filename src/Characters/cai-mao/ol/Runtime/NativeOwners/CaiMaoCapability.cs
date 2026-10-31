#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaiMaoCapability(Func<RuntimeEntries.CharacterCaiMaoOlEntries> getCharacterCaiMaoOlRuntime) : ICaiMaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaiMaoOlRuntime().BeginCaiMao(f, op);
}
