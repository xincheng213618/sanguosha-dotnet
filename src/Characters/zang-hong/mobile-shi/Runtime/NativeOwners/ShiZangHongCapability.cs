#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZangHongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZangHongMobileShiRuntime>> getCharacterZangHongMobileShiRuntime) : IShiZangHongHost
{
    public SkillProgramStepOutcome ResolveLiezhi(ProgramSkillFrame frame) => getCharacterZangHongMobileShiRuntime().Value.BeginShiZhLiezhi(frame);
    public SkillProgramStepOutcome ResolveJuegu(ProgramSkillFrame frame) => getCharacterZangHongMobileShiRuntime().Value.BeginShiZhJuegu(frame);
}
