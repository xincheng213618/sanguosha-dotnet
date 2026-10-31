#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuZhugeLiangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries> getCharacterZhugeLiangWumiaoRuntime) : IWuZhugeLiangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhugeLiangWumiaoRuntime().BeginWuZgl(f, op);
}
