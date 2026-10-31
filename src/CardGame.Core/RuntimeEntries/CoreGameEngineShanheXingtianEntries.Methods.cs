#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXingtianEntries
{
    internal void ApplyShanheXingtianSeal(ProgramSkillFrame input) => _resolve().ApplyShanheXingtianSeal(input);
    internal bool CanOfferShanheXingtian(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheXingtian(gate);
    internal bool ContinueShanheXingtianPaidCost(ProgramSkillFrame f) => _resolve().ContinueShanheXingtianPaidCost(f);
    internal bool HasShanheXingtianPaidCost(ProgramSkillFrame f) => _resolve().HasShanheXingtianPaidCost(f);
    internal bool HasShanheXingtianSeal(int seat) => _resolve().HasShanheXingtianSeal(seat);
    internal bool ReturnShanheXingtianPaidMovement(ProgramSkillFrame f) => _resolve().ReturnShanheXingtianPaidMovement(f);
    internal bool ShanheXingtianCardUseForbidden(ProgramCardUseQuery query) => _resolve().ShanheXingtianCardUseForbidden(query);
    internal bool ShanheXingtianNativeResponseForbidden(ProgramNativeResponseQuery query) => _resolve().ShanheXingtianNativeResponseForbidden(query);
}
