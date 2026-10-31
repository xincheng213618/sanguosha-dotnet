#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LastDamageSourceReciprocityProgramCapability(Func<RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries> getCoreGameEngineLastDamageSourceReciprocityRuntime) : ILastDamageSourceReciprocityProgramHost
{
    public SkillProgramStepOutcome LastDamageSourceReciprocity(ProgramSkillFrame f, string stateId) => getCoreGameEngineLastDamageSourceReciprocityRuntime().BeginLastDamageSourceReciprocity(f, stateId);
}
