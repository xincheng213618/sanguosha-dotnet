#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXingtianEntries
{
    internal void ApplyShanheXingtianSeal(global::CardGame.Core.ProgramSkillFrame input) => _resolve().ApplyShanheXingtianSeal(input);
    internal bool CanOfferShanheXingtian(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheXingtian(gate);
    internal bool ContinueShanheXingtianPaidCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShanheXingtianPaidCost(f);
    internal bool HasShanheXingtianPaidCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasShanheXingtianPaidCost(f);
    internal bool HasShanheXingtianSeal(int seat) => _resolve().HasShanheXingtianSeal(seat);
    internal bool ReturnShanheXingtianPaidMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShanheXingtianPaidMovement(f);
    internal bool ShanheXingtianCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().ShanheXingtianCardUseForbidden(query);
    internal bool ShanheXingtianNativeResponseForbidden(global::CardGame.Core.ProgramNativeResponseQuery query) => _resolve().ShanheXingtianNativeResponseForbidden(query);
}
