#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangOriginalSkillHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangOriginalSkillsEntries> getCoreGameEngineWushuangOriginalSkillsRuntime) : IWushuangOriginalSkillHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineWushuangOriginalSkillsRuntime().BeginWushuangOriginalSkill(frame, operation);
}
