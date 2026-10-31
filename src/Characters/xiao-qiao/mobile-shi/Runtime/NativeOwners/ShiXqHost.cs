#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXqHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXiaoQiaoMobileShiRuntime>> getCharacterXiaoQiaoMobileShiRuntime) : IShiXqHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => getCharacterXiaoQiaoMobileShiRuntime().Value.BeginShiXq(f, operation);
}
