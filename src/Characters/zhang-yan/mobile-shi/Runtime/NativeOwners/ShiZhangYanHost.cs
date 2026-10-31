#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhangYanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangYanMobileShiRuntime>> getCharacterZhangYanMobileShiRuntime) : IShiZhangYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhangYanMobileShiRuntime().Value.BeginShiZy(f, op);
}
