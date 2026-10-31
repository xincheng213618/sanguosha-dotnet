#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AvailableBoundSubsetProgramEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime) : IAvailableBoundSubsetProgramEffectHost
{
    public SkillProgramStepOutcome SelectAvailableBoundSubset(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineProgramLifecycleRuntime().SelectProgramCardSubset(frame.Id, frame.OwnerSeat, effect.SourceBind!, effect.ResultBind!, effect.MinimumCards, effect.MaximumCards, effect.MaximumRankSum, effect.AiOrder!.Value, effect.AllowFewerWhenInsufficient, effect.OnePerSuit, availableAtSourceOnly: true);
}
