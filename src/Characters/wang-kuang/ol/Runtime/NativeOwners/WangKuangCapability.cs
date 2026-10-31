#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WangKuangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangKuangOlRuntime>> getCharacterWangKuangOlRuntime) : IWangKuangProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, bool ending) => getCharacterWangKuangOlRuntime().Value.BeginWangKuang(f, ending);
}
