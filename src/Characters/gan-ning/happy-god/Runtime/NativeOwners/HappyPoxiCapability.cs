#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyPoxiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGanNingHappyGodRuntime>> getCharacterGanNingHappyGodRuntime) : IHappyPoxiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, string bind) => getCharacterGanNingHappyGodRuntime().Value.BeginHappyPoxi(frame, bind);
}
