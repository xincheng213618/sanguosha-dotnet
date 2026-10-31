#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyPangTongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterPangTongHappyGodRuntime>> getCharacterPangTongHappyGodRuntime) : IHappyPangTongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongHappyGodRuntime().Value.BeginHappyPangTong(f, op);
}
