#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShenJwHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterJiangWeiAnniversaryGodRuntime>> getCharacterJiangWeiAnniversaryGodRuntime) : IShenJiangWeiHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterJiangWeiAnniversaryGodRuntime().Value.BeginShenJw(f, op);
}
