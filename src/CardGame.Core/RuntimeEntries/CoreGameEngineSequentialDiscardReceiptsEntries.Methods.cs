#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardReceiptsEntries
{
    internal global::CardGame.Core.ProgramSkillFrame FreezeSequentialDiscardTopPayment(global::CardGame.Core.ProgramSkillFrame f, string bind, string? except, global::CardGame.Core.SkillProgramCardDestination destination) => _resolve().FreezeSequentialDiscardTopPayment(f, bind, except, destination);
    internal bool HasSequentialDiscardTopPayment(global::CardGame.Core.ProgramSkillFrame f, string bind) => _resolve().HasSequentialDiscardTopPayment(f, bind);
    internal bool ValidSequentialDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSequentialDiscard(f);
    internal bool ValidSequentialDiscardPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSequentialDiscardDraft d, global::CardGame.Core.ProgramSequentialDiscardPayment p) => _resolve().ValidSequentialDiscardPayment(f, d, p);
}
