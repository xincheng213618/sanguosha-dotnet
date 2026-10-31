#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangOriginalSkillHost(Func<RuntimeEntries.CoreGameEngineWushuangOriginalSkillsEntries> getCoreGameEngineWushuangOriginalSkillsRuntime) : IWushuangOriginalSkillHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineWushuangOriginalSkillsRuntime().BeginWushuangOriginalSkill(frame, operation);
}
