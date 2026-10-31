#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class XuRongProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries> getCharacterXuRongOlRuntime) : IXuRongProgramHost
{
    public SkillProgramStepOutcome ExecuteXuRong(SkillProgramEffectOp op, ProgramSkillFrame frame) => getCharacterXuRongOlRuntime().ExecuteXuRong(op, frame);
}
