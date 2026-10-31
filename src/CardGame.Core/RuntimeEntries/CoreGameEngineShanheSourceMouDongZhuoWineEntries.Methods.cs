#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoWineEntries
{
    internal bool BeginShanheSourceMouWine(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginShanheSourceMouWine(f);
    internal bool ContinueShanheSourceMouWine(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueShanheSourceMouWine(id, continuation);
    internal bool IsShanheSourceMouWine(long id) => _resolve().IsShanheSourceMouWine(id);
    internal void ReturnSourceMouWine(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnSourceMouWine(use);
    internal bool SourceMouWineQuotaExemption(long useId, int actorSeat) => _resolve().SourceMouWineQuotaExemption(useId, actorSeat);
    internal void ValidateSourceMouWine(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateSourceMouWine(use);
}
