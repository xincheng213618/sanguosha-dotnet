#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreAvailableBoundSubsetProgramsRuntime
{
    SkillProgramStepOutcome IAvailableBoundSubsetProgramEffectHost.SelectAvailableBoundSubset(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreProgramLifecycleOperations.SelectProgramCardSubset(frame.Id, frame.OwnerSeat, effect.SourceBind!, effect.ResultBind!, effect.MinimumCards, effect.MaximumCards, effect.MaximumRankSum, effect.AiOrder!.Value, effect.AllowFewerWhenInsufficient, effect.OnePerSuit, availableAtSourceOnly: true);
}
