#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiChenDaoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChenDaoMobileShiRuntime>> getCharacterChenDaoMobileShiRuntime) : IMobileShiChenDaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterChenDaoMobileShiRuntime().Value.ResolveShiCd(f, op);
}
