#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LiWanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiWanOlEntries> getCharacterLiWanOlRuntime) : ILiWanProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterLiWanOlRuntime().BeginLiWan(frame, op);
}
