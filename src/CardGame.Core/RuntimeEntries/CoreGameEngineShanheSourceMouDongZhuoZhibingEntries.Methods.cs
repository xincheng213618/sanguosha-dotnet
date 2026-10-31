#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoZhibingEntries
{
    internal void ApplySourceMouThreshold(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplySourceMouThreshold(f);
    internal bool HasSourceMouZhixi(int seat) => _resolve().HasSourceMouZhixi(seat);
    internal void RecordSourceMouActualUse(global::CardGame.Core.IGameEvent payload) => _resolve().RecordSourceMouActualUse(payload);
    internal (global::CardGame.Core.ShanheSourceMouBlackUseEvent Use, int Ordinal)? SourceMouThreshold(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().SourceMouThreshold(owner, context);
    internal bool SourceMouZhixiNativeUseForbidden(global::CardGame.Core.ProgramNativeResponseQuery query) => _resolve().SourceMouZhixiNativeUseForbidden(query);
    internal bool SourceMouZhixiUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().SourceMouZhixiUseForbidden(query);
}
