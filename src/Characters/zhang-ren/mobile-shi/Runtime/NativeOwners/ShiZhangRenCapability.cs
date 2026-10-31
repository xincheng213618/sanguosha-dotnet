#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhangRenCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangRenMobileShiRuntime>> getCharacterZhangRenMobileShiRuntime) : IShiZhangRenHost
{
    public SkillProgramStepOutcome ResolveFuan(ProgramSkillFrame frame) => getCharacterZhangRenMobileShiRuntime().Value.BeginShiZrFuan(frame);
    public SkillProgramStepOutcome ResolveFuanStrike(ProgramSkillFrame frame) => getCharacterZhangRenMobileShiRuntime().Value.BeginShiZrFuanStrike(frame);
    public SkillProgramStepOutcome ResolveYinXian(ProgramSkillFrame frame) => getCharacterZhangRenMobileShiRuntime().Value.ResolveShiZrYinXian(frame);
}
