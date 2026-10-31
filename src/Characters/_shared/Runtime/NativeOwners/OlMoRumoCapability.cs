#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OlMoRumoCapability(Func<RuntimeEntries.CharacterSharedGameEngineOlMoRumoEntries> getCharacterSharedGameEngineOlMoRumoRuntime) : IOlMoRumoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSharedGameEngineOlMoRumoRuntime().ResolveOlMoRumoRoundPenalty(frame);
}
