#nullable enable
using System.Globalization;
using System.Numerics;

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHuZhaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuZhaoAnniversaryEntries> getCharacterHuZhaoAnniversaryRuntime) : IAnniversaryHuZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterHuZhaoAnniversaryRuntime().BeginHuZhaoMidu(frame);
}
