#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiushiEffectsEntries
{
    internal void ApplyProgramLiushiEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ApplyProgramLiushiEffect(frame);
    internal void AssertLiushiClearPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLiushiClearPayment(f);
    internal bool CanOfferLiushiClear(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLiushiClear(trigger, context);
    internal void CaptureLiushiClearDrawPayment(global::CardGame.Core.ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced) => _resolve().CaptureLiushiClearDrawPayment(frame, requested, before, after, actual, attemptId, replaced);
    internal void ClearProgramLiushiEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ClearProgramLiushiEffect(frame);
    internal bool ContinuePaidLiushiClear(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidLiushiClear(f);
    internal bool HasPaidLiushiClear(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasPaidLiushiClear(f);
    internal bool IsLiushiClearDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsLiushiClearDraw(frame);
}
