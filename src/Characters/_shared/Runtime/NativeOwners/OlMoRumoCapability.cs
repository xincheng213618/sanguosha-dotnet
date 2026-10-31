#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class OlMoRumoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSharedGameEngineOlMoRumoEntries> getCharacterSharedGameEngineOlMoRumoRuntime) : IOlMoRumoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSharedGameEngineOlMoRumoRuntime().ResolveOlMoRumoRoundPenalty(frame);
}
