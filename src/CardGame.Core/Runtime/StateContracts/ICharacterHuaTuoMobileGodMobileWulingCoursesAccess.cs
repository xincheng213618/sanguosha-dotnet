#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHuaTuoMobileGodMobileWulingCoursesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<long, MobileWulingCourse> _mobileWulingCourses { get; }
}
