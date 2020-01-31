using System;
using Regulus.Utility;
namespace Regulus.Game
{
    public delegate void OnQuit();
    public delegate void OnNewUser(System.Guid id);
    public interface IUser : IUpdatable 
    {
        event OnQuit QuitEvent;
        event OnNewUser VerifySuccessEvent;
        void OnKick(Guid id);

    }
}
