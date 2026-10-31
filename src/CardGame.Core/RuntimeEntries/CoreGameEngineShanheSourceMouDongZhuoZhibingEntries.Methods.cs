#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoZhibingEntries
{
    internal void ApplySourceMouThreshold(ProgramSkillFrame f) => _resolve().ApplySourceMouThreshold(f);
    internal bool HasSourceMouZhixi(int seat) => _resolve().HasSourceMouZhixi(seat);
    internal void RecordSourceMouActualUse(IGameEvent payload) => _resolve().RecordSourceMouActualUse(payload);
    internal (ShanheSourceMouBlackUseEvent Use, int Ordinal)? SourceMouThreshold(int owner, ProgramSkillWindowContext context) => _resolve().SourceMouThreshold(owner, context);
    internal bool SourceMouZhixiNativeUseForbidden(ProgramNativeResponseQuery query) => _resolve().SourceMouZhixiNativeUseForbidden(query);
    internal bool SourceMouZhixiUseForbidden(ProgramCardUseQuery query) => _resolve().SourceMouZhixiUseForbidden(query);
}
