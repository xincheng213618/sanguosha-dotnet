#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPilePreparationReceiptsEntries
{
    internal void AssertPublicPilePreparation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertPublicPilePreparation(f);
    internal bool HasPublicPilePreparationDamageObserver(long id) => _resolve().HasPublicPilePreparationDamageObserver(id);
    internal bool IsPublicPilePreparationMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPublicPilePreparationMovement(f, effect, pending);
    internal bool IsPublicPilePreparationProgramDying() => _resolve().IsPublicPilePreparationProgramDying();
    internal bool PublicPilePreparationFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().PublicPilePreparationFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? PublicPilePreparationObserverRoot() => _resolve().PublicPilePreparationObserverRoot();
    internal bool PublicPilePreparationPaymentDestination(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardLocation destination) => _resolve().PublicPilePreparationPaymentDestination(f, movement, destination);
    internal bool ValidPublicPilePreparation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPublicPilePreparation(f);
}
