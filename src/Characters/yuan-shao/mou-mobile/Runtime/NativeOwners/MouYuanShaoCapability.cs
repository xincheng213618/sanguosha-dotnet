#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouYuanShaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYuanShaoMouMobileRuntime>> getCharacterYuanShaoMouMobileRuntime) : IMouYuanShaoHost
{
    public SkillProgramStepOutcome Reserve(ProgramSkillFrame frame) => getCharacterYuanShaoMouMobileRuntime().Value.ReserveMouYuanShaoReward(frame);
}
