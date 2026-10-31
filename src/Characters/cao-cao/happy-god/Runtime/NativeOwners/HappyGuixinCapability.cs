#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyGuixinCapability(Func<RuntimeEntries.CharacterCaoCaoHappyGodEntries> getCharacterCaoCaoHappyGodRuntime) : IHappyGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoHappyGodRuntime().BeginHappyGuixin(f);
}
