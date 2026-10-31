#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPilePreparationReceiptsEntries
{
    internal bool IsPublicPilePreparationProgramDying() => _resolve().IsPublicPilePreparationProgramDying();
    internal bool PublicPilePreparationFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().PublicPilePreparationFirstChild(f, child);
    internal ProgramSkillFrame? PublicPilePreparationObserverRoot() => _resolve().PublicPilePreparationObserverRoot();
    internal bool PublicPilePreparationPaymentDestination(ProgramSkillFrame f, CardMovementRecord movement, CardLocation destination) => _resolve().PublicPilePreparationPaymentDestination(f, movement, destination);
    internal bool ValidPublicPilePreparation(ProgramSkillFrame f) => _resolve().ValidPublicPilePreparation(f);
}
