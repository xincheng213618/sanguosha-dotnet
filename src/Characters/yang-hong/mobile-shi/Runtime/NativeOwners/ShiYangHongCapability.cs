#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiYangHongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYangHongMobileShiRuntime>> getCharacterYangHongMobileShiRuntime) : IShiYangHongHost
{
    public SkillProgramStepOutcome ResolveJijian(ProgramSkillFrame frame) => getCharacterYangHongMobileShiRuntime().Value.BeginShiYhJijian(frame);
    public SkillProgramStepOutcome ResolveYuanmo(ProgramSkillFrame frame) => getCharacterYangHongMobileShiRuntime().Value.BeginShiYhYuanmo(frame);
}
