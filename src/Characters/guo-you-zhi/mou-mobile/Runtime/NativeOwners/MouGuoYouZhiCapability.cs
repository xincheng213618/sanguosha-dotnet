#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoYouZhiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuoYouZhiMouMobileRuntime>> getCharacterGuoYouZhiMouMobileRuntime) : IMouGuoYouZhiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoYouZhiMouMobileRuntime().Value.BeginMouGyz(f, op);
}
