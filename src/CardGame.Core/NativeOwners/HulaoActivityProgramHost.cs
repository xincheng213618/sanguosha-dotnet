#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class HulaoActivityProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHulaoActivityProgramsEntries> getCoreHulaoActivityProgramsRuntime) : IHulaoActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteHulaoActivity(ProgramSkillFrame f, SkillProgramEffect e) => getCoreHulaoActivityProgramsRuntime().ExecuteHulaoActivity(f, e);
}
