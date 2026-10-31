#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhangJiaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime>> getCharacterZhangJiaoAnniversaryGodRuntime) : IAnniversaryZhangJiaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterZhangJiaoAnniversaryGodRuntime().Value.BeginAnniversaryZj(f, e);
}
