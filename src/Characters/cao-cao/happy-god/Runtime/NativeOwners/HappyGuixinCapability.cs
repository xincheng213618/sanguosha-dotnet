#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyGuixinCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoCaoHappyGodRuntime>> getCharacterCaoCaoHappyGodRuntime) : IHappyGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoHappyGodRuntime().Value.BeginHappyGuixin(f);
}
