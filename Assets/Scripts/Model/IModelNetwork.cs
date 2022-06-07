using System;
using Photon.Realtime;

namespace PingPong.Model
{
    public interface IModelNetwork : IModel, IOnEventCallback, IDisposable
    {
    
    }
}
