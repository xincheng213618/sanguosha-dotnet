#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TangTangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTangTangOlRuntime>> getCharacterTangTangOlRuntime) : ITangTangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterTangTangOlRuntime().Value.BeginTangTang(f, op);
}
