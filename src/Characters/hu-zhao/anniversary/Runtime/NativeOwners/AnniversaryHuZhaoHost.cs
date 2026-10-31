#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHuZhaoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuZhaoAnniversaryRuntime>> getCharacterHuZhaoAnniversaryRuntime) : IAnniversaryHuZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterHuZhaoAnniversaryRuntime().Value.BeginHuZhaoMidu(frame);
}
