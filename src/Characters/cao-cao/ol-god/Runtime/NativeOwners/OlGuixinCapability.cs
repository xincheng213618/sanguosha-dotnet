#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OlGuixinCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoCaoOlGodRuntime>> getCharacterCaoCaoOlGodRuntime) : IOlGuixinHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCaoCaoOlGodRuntime().Value.BeginOlGuixin(f);
}
