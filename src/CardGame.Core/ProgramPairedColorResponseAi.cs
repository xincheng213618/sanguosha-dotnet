namespace CardGame.Core;

internal sealed partial class ProgramAiEstimateContext
{
    internal void PairedColorDispositionValue(SkillProgramEffect effect) =>
        _otherAdjustment += _publicContext.PairedColorDispositionScore;
}
