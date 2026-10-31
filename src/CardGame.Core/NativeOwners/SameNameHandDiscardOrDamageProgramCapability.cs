#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SameNameHandDiscardOrDamageProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageEntries> getCoreGameEngineSameNameHandDiscardOrDamageRuntime) : ISameNameHandDiscardOrDamageProgramHost
{
    public SkillProgramStepOutcome OfferSameNameHandDiscardOrDamage(ProgramSkillFrame f) => getCoreGameEngineSameNameHandDiscardOrDamageRuntime().BeginSameNameHandDiscardOrDamage(f);
}
