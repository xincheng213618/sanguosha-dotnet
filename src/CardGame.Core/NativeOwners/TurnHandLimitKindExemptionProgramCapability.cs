#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnHandLimitKindExemptionProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandLimitKindExemptionsEntries> getCoreGameEngineTurnHandLimitKindExemptionsRuntime) : ITurnHandLimitKindExemptionProgramHost
{
    public void GrantTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds) => getCoreGameEngineTurnHandLimitKindExemptionsRuntime().GrantProgramTurnHandLimitCardKindExemption(frame, cardKinds);
}
