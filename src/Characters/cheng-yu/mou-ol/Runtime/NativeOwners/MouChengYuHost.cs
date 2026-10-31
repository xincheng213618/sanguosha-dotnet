#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouChengYuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengYuMouOlEntries> getCharacterChengYuMouOlRuntime) : IMouChengYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouChengYuLidan ? getCharacterChengYuMouOlRuntime().MouLidan.Resolve(f) : getCharacterChengYuMouOlRuntime().MouDanchi.Resolve(f);
}
