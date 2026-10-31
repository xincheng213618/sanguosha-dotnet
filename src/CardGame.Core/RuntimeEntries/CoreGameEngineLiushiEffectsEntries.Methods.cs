#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiushiEffectsEntries
{
    internal void ApplyProgramLiushiEffect(ProgramSkillFrame frame) => _resolve().ApplyProgramLiushiEffect(frame);
    internal bool CanOfferLiushiClear(SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferLiushiClear(trigger, context);
    internal void CaptureLiushiClearDrawPayment(ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced) => _resolve().CaptureLiushiClearDrawPayment(frame, requested, before, after, actual, attemptId, replaced);
    internal void ClearProgramLiushiEffect(ProgramSkillFrame frame) => _resolve().ClearProgramLiushiEffect(frame);
    internal bool ContinuePaidLiushiClear(ProgramSkillFrame f) => _resolve().ContinuePaidLiushiClear(f);
    internal bool HasPaidLiushiClear(ProgramSkillFrame f) => _resolve().HasPaidLiushiClear(f);
    internal bool IsLiushiClearDraw(ProgramSkillFrame frame) => _resolve().IsLiushiClearDraw(frame);
}
