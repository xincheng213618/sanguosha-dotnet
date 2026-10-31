#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CaiMaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiMaoOlEntries> getCharacterCaiMaoOlRuntime) : ICaiMaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaiMaoOlRuntime().BeginCaiMao(f, op);
}
