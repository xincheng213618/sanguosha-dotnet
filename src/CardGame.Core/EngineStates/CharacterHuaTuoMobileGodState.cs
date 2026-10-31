#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuaTuoMobileGodState : Contracts.ICharacterHuaTuoMobileGodMobileWulingCoursesAccess
{
    // Durable private courses, rather than pending choices. Commands rebuild
    // these immutable accepted orders on replay; public facts reveal no future animal.
    private Dictionary<long, MobileWulingCourse> _mobileWulingCourses = [];
    internal ref readonly Dictionary<long, MobileWulingCourse> ReferenceToMobileWulingCourses => ref _mobileWulingCourses;

    ref readonly Dictionary<long, MobileWulingCourse> Contracts.ICharacterHuaTuoMobileGodMobileWulingCoursesAccess._mobileWulingCourses => ref _mobileWulingCourses;
}
