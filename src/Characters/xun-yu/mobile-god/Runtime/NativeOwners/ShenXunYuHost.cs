#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenXunYuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries> getCharacterXunYuMobileGodRuntime) : IShenXunYuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterXunYuMobileGodRuntime().ResolveShenXunYu(frame, op);
}
