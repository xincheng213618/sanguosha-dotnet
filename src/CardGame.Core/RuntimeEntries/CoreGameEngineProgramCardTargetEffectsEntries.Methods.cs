#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardTargetEffectsEntries
{
    internal void CompleteIneffectiveTrickTarget(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat) => _resolve().CompleteIneffectiveTrickTarget(pending, targetSeat);
    internal bool IsCardEffectIneffective(long resolutionId, int targetSeat) => _resolve().IsCardEffectIneffective(resolutionId, targetSeat);
    internal void MarkCardEffectIneffective(long resolutionId, int targetSeat) => _resolve().MarkCardEffectIneffective(resolutionId, targetSeat);
    internal void NullifyCurrentProgramCardEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NullifyCurrentProgramCardEffect(frame);
    internal void NullifySelectedProgramCardEffects(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NullifySelectedProgramCardEffects(frame);
}
