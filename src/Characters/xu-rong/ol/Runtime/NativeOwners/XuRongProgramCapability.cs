#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class XuRongProgramCapability(Func<RuntimeEntries.CharacterXuRongOlEntries> getCharacterXuRongOlRuntime) : IXuRongProgramHost
{
    public SkillProgramStepOutcome ExecuteXuRong(SkillProgramEffectOp op, ProgramSkillFrame frame) => getCharacterXuRongOlRuntime().ExecuteXuRong(op, frame);
}
