#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyGuixinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoCaoHappyGodEntries> getCharacterCaoCaoHappyGodRuntime) : IHappyGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoHappyGodRuntime().BeginHappyGuixin(f);
}
